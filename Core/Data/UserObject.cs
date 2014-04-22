using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class UserObject : DataObject
    {
        public string Name { get; set; }

        public UserObject()
        {
            // do nothing
        }

        protected UserObject(UserObject copy) : base(copy)
        {
            Name = copy.Name;
        }

        public override Object Clone()
        {
            return new UserObject(this);
        }
    }
}
