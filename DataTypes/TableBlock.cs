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
    public class TableBlock : Impassable, IDrawable
    {
        public int numTablesWide;
        public int numTablesTall;

        public double x { get; set; }
        public double y { get; set; }
        public int z { get; set; }
        new public double width { get; set; }
        new public double height { get; set; }
        
        public TableBlock(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            z = 50;
            this.width = width;
            this.height = height;
            numTablesTall = 1;
            numTablesWide = 1;
        }

        public object GetDrawable()
        {
            /*
            for (int n = 1; n < numTablesTall; n++)
            {
                Line l = new Line();
                l.Stroke = Brushes.Black;
                l.X1 = x1;
                l.X2 = x2;
                l.Y1 = (y1 + (height * (float)n / numTablesTall));
                l.Y2 = l.Y1;
            }

            for (int n = 1; n < numTablesWide; n++)
            {
                Line l = new Line();
                l.Stroke = Brushes.Black;
                l.X1 = (x1 + width * (float)n / numTablesWide);
                l.X2 = l.X1;
                l.Y1 = y1;
                l.Y2 = y2;
            }*/

            string type = "tableBlock";

            return new { type, x, y, width, height, z };
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}) @ {2}w{3}h - {4}x{5}", x, y, width, height, numTablesWide, numTablesTall);
        }
    }
}
