using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class TeamObject : DataObject
    {
        public string Name { get; set; }

        public TeamObject()
        {
            // do nothing
        }

        protected TeamObject(TeamObject copy) : base(copy)
        {
            Name = copy.Name;
        }

        public override Object Clone()
        {
            return new TeamObject(this);
        }
    }
}
