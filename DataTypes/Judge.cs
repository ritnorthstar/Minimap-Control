using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using ExtensionMethods;

namespace DataTypes
{
    public class Judge : IDrawable
    {
        new public double x { get; set; }
        new public double y { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public int z { get; set; }
        public string guid { get; set; }
        const string type = "judge";
        public string id;
        public Team team;

        public Judge(double x, double y, string name)
        {
            this.x = x;
            this.y = y;
            width = 25;
            height = 25;
            z = 50;
            guid = System.Guid.NewGuid().ToString();
            id = name;
        }

        public void SetTeam(Team team)
        {
            this.team = team;
        }

        public Object GetDrawable()
        {
            Brush fill = team.color;
            return new { type, x, y, id, fill, z, guid };
        }

        public override string ToString()
        {
            return String.Format("{0} ({1}) @ {2}, {3}", id, team.color.ToString(), x, y);
        }
    }
}
