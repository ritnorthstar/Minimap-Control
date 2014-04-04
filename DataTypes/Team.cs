using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataTypes
{
    public class Team
    {
        public string name { get; set; }
        public Color color;
        public Color secondaryColor;

        public Team(string name, Color color, Color secondaryColor)
        {
            this.name = name;
            this.color = color;
            this.secondaryColor = secondaryColor;
        }

        public string ToString()
        {
            return String.Format("{0}({1})", name, color.ToString());
        }
    }
}
