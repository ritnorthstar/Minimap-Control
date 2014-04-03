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
    public class Beacon : IDrawable
    {
        public string id;
        public double x { get; set; }
        public double y { get; set; }
        public int z { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string guid { get; set; }
        const string type = "beacon";

        public static int innerRadius = 20, outerRadius = 30;

        public Beacon(string id, double x, double y)
        {
            this.id = id;
            this.x = x - outerRadius/2;
            this.y = y - outerRadius/2;
            this.z = 100;
            guid = System.Guid.NewGuid().ToString();
        }

        public object GetDrawable()
        {
            return new { type, id, x, y, innerRadius, outerRadius, z, guid };
        }

        public override string ToString()
        {
            return String.Format("{0} - ({1}, {2})", id, x, y);
        }
    }
}
