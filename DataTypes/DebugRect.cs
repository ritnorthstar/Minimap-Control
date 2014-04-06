using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using ExtensionMethods;
using System.Windows;

namespace DataTypes
{
    public class DebugRect : IDrawable
    {
        const string type = "rectangle";
        public string id;
        public double x { get; set; }
        public double y { get; set; }
        public int z { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string guid { get; set; }
        public Brush border;
        public Brush fill;

        public DebugRect(double x, double y, double width, double height, string text)
        {
            this.x = x;
            this.y = y;
            this.z = -1;
            this.width = width;
            this.height = height;
            id = text;
            border = Brushes.Goldenrod;
            guid = System.Guid.NewGuid().ToString();
            fill = Brushes.Transparent;
        }

        public DebugRect(double x, double y, double width, double height) : this(x, y, width, height, String.Empty) { }

        public object GetDrawable()
        {
            //return this;
            return new { type, id, x, y, width, height, border, fill, z, guid };
        }

        public Rect GetBounds()
        {
            return new Rect(x, y, width, height);
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}) - {2}w{3}h", x, y, width, height);
        }
    }
}
