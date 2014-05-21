using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class DataObject : ICloneable
    {
        // TODO - Make Id readonly (will cause issues with current JSON deserialization)
        public string Id;
        public DateTime LastModified;

        protected DataObject()
        {
            Id = System.Guid.NewGuid().ToString();
        }

        protected DataObject(DataObject copy)
        {
            Id = copy.Id;
            LastModified = copy.LastModified;
        }

        /// <summary>
        /// Returns a deep clone of the data object.</summary>
        /// <returns>
        /// Returns a deep clone of the data object.</returns>
        public virtual Object Clone()
        {
            return new DataObject(this);
        }
    }
}
