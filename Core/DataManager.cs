using Core.Data;
using Core.Data.Access;
using Core.Data.Access.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Core
{
    public class DataManager<T> where T:DataObject
    {
        private IDataRepository<T> repository = new LocalDataRepository<T>();

        private TimeSpan expireTime;
        private Timer purgeTimer;

        public DataManager()
        {
            SetDataExpireTime(0);
        }

        /// <summary>
        /// Creates a data manager that purges unmodified data after the specified time span.</summary>
        /// <param name="expireTime">
        /// The amount of time that must pass before unmodified data is purged from the repository.</param>
        public DataManager(TimeSpan expireTime)
        {
            SetDataExpireTime(expireTime);
        }

        /// <summary>
        /// Returns the entire list of data objects contained in the managed repository.</summary>
        /// <returns>
        /// Returns the entire list of data objects.</returns>
        public IEnumerable<T> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Returns the data object with the specified id.</summary>
        /// <param name="id">
        /// The id of the data object to retrieve</param>
        /// <returns>
        /// Returns the data object, or null if it could not be found.</returns>
        public T Get(string id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Add the passed data object to the managed repository. If the object id already exists in the managed repository, the existing object is replaced.</summary>
        /// <param name="t">
        /// The data object to add</param>
        /// <returns>
        /// Returns true if the data object was added.</returns>
        public bool Add(T t)
        {
            return repository.Add(t);
        }

        /// <summary>
        /// Remove the stored data object with the specified id from the managed repository.</summary>
        /// <param name="id">
        /// The id of the data object to remove</param>
        /// <returns>
        /// Returns true if a data object was removed.</returns>
        public bool Remove(string id)
        {
            return repository.Remove(id);
        }

        /// <summary>
        /// Sets the amount of time that must pass before unmodified data is purged from the repository.</summary>
        /// <param name="expireTime">
        /// The amount of time that must pass before unmodified data is purged from the repository.</param>
        public void SetDataExpireTime(TimeSpan expireTime)
        {
            this.expireTime = expireTime;

            // enable purge thread
            if (expireTime.Ticks > 0)
            {
                // create new purge timer
                if (purgeTimer == null)
                {
                    purgeTimer = new Timer(new TimerCallback(purgeExpiredData), null, new TimeSpan(0), TimeSpan.FromSeconds(10));
                }

                // update purge timer
                else
                {
                    purgeTimer.Change(new TimeSpan(0), TimeSpan.FromSeconds(10));
                }
            }
            
            // disable purge thread
            else
            {
                if (purgeTimer != null)
                {
                    purgeTimer.Dispose();
                    purgeTimer = null;
                }
            }
        }

        /// <summary>
        /// Sets the number of ticks that must pass before unmodified data is purged from the repository.</summary>
        /// <param name="ticks">
        /// The number of ticks that must pass before unmodified data is purged from the repository.</param>
        public void SetDataExpireTime(long ticks)
        {
            SetDataExpireTime(new TimeSpan(ticks));
        }

        private void purgeExpiredData(object state)
        {
            DateTime currentDate = DateTime.Now;
            foreach (DataObject obj in GetAll())
            {
                if (expireTime.Ticks > 0 && (currentDate - obj.LastModified > expireTime))
                {
                    Remove(obj.Id);
                }
            }
        }
    }
}
