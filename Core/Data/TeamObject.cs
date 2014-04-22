using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class TeamObject : DataObject
    {
        public string Name { get; set; }

        public Color PrimaryColor { get { return primaryColor; } set { primaryColor = (value == null ? Color.White : value); } }
        protected Color primaryColor;

        public Color SecondaryColor { get { return SecondaryColor; } set { secondaryColor = (value == null ? Color.White : value); } }
        protected Color secondaryColor;

        public TeamObject()
        {
            primaryColor = Color.White;
            secondaryColor = Color.White;
        }

        protected TeamObject(TeamObject copy) : base(copy)
        {
            Name = copy.Name;
            primaryColor = copy.PrimaryColor;
            secondaryColor = copy.SecondaryColor;
        }

        public override Object Clone()
        {
            return new TeamObject(this);
        }
    }
}
