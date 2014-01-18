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
    public class DebugRect : IDrawable
    {
        public string id;
        public double x { get; set; }
        public double y { get; set; }
        public double width, height;

        public DebugRect(double x, double y, double width, double height)
        {
            this.id = "test";
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public object GetDrawable()
        {
            string type = "rectangle";
            Brush border = Brushes.Goldenrod;

            return new { type, id, x, y, width, height, border};
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}) - {2}w{3}h", x, y, width, height);
        }
    }
}
