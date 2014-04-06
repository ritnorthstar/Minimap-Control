using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class DataObject : ICloneable
    {
        private readonly string id;
        public string Id { get { return id; } }

        protected DataObject()
        {
            id = System.Guid.NewGuid().ToString();
        }

        protected DataObject(DataObject copy)
        {
            id = copy.Id;
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
