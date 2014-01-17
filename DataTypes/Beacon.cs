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
    public class Beacon : Drawable
    {
        public string id;
        public double x, y;
        public static  double size = 20;

        public Beacon(string id, double x, double y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        public void DrawSelf(Canvas c)
        {
            Ellipse outer = new Ellipse { Height = size, Width = size, Fill = Brushes.DodgerBlue };
            Ellipse inner = new Ellipse { Height = size / 2, Width = size / 2, Fill = Brushes.SkyBlue };
            c.AddChild(outer, x - size / 2, y - size / 2);
            c.AddChild(inner, x - size / 4, y - size / 4);
        }
    }
}
