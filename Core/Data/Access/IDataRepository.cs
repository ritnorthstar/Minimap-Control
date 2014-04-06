using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Access
{
    interface IDataRepository<T> where T:DataObject
    {
        /// <summary>
        /// Returns the entire list of data objects contained in the repository.</summary>
        /// <returns>
        /// Returns the entire list of data objects.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Returns the data object with the specified id.</summary>
        /// <param name="id">
        /// The id of the data object to retrieve</param>
        /// <returns>
        /// Returns the data object, or null if it could not be found.</returns>
        T Get(string id);

        /// <summary>
        /// Add the data object to the repository. If the object id already exists in the repository, the existing object is replaced.</summary>
        /// <param name="t">
        /// The data object to add</param>
        /// <returns>
        /// Returns true if the data object was added.</returns>
        bool Add(T t);

        /// <summary>
        /// Remove the data object with the specified id from the repository.</summary>
        /// <param name="id">
        /// The id of the data object to remove</param>
        /// <returns>
        /// Returns true if a data object was removed.</returns>
        bool Remove(string id);
    }
}
