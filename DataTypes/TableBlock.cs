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

        public TableBlock(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            numTablesTall = 1;
            numTablesWide = 1;
        }

        public object GetDrawable()
        {
            /*Rectangle rect = new Rectangle { Width = this.width, Height = this.height, Stroke = Brushes.Black};
            c.AddChild(rect, x1, y1);

            for (int n = 1; n < numTablesTall; n++)
            {
                Line l = new Line();
                l.Stroke = Brushes.Black;
                l.X1 = x1;
                l.X2 = x2;
                l.Y1 = (y1 + (height * (float)n / numTablesTall));
                l.Y2 = l.Y1;
                c.Children.Add(l);
            }

            for (int n = 1; n < numTablesWide; n++)
            {
                Line l = new Line();
                l.Stroke = Brushes.Black;
                l.X1 = (x1 + width * (float)n / numTablesWide);
                l.X2 = l.X1;
                l.Y1 = y1;
                l.Y2 = y2;
                c.Children.Add(l);
            }*/

            throw new NotImplementedException();
        }
    }
}
