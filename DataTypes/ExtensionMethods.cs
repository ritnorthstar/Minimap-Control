using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace ExtensionMethods
{
    public static class CanvasExtensions
    {
        public static int AddChild(this Canvas canvas, System.Windows.UIElement element, double x, double y)
        {
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
            return canvas.Children.Add(element);
        }
    }
}
