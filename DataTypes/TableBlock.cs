using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace DataTypes
{
    public class TableBlock : Impassable, Drawable
    {
        public int numTablesWide;
        public int numTablesTall;

        public TableBlock(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            numTablesTall = 1;
            numTablesWide = 1;
        }

        public void DrawSelf(Canvas c, double zoomFactor)
        {
            Rectangle rect = new Rectangle();
            rect.Stroke = Brushes.Black;
            rect.Width = width * zoomFactor;
            rect.Height = height * zoomFactor;
            Canvas.SetLeft(rect, x1 * zoomFactor);
            Canvas.SetTop(rect, y1 * zoomFactor);

            for (int n = 1; n < numTablesTall; n++)
            {
                Line l = new Line();
                l.Stroke = Brushes.Black;
                l.X1 = x1 * zoomFactor;
                l.X2 = x2 * zoomFactor;
                l.Y1 = (y1 + (height * (float)n / numTablesTall)) * zoomFactor;
                l.Y2 = l.Y1;
                c.Children.Add(l);
            }

            for (int n = 1; n < numTablesWide; n++)
            {
                Line l = new Line();
                l.Stroke = Brushes.Black;
                l.X1 = (x1 + (width * (float)n / numTablesWide)) * zoomFactor;
                l.X2 = l.X1;
                l.Y1 = y1 * zoomFactor;
                l.Y2 = y2 * zoomFactor;
                c.Children.Add(l);
            }

                c.Children.Add(rect);
        }
    }
}
