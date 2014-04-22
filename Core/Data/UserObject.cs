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
        public string TeamId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public UserObject()
        {
            // do nothing
        }

        protected UserObject(UserObject copy) : base(copy)
        {
            Name = copy.Name;
            TeamId = copy.TeamId;
            X = copy.X;
            Y = copy.Y;
            Z = copy.Z;
        }

        public override Object Clone()
        {
            return new UserObject(this);
        }
    }
}
