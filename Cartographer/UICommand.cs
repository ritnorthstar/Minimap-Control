using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cartographer
{
    public static class UICommand
    {
        public static readonly RoutedUICommand OpenMap = new RoutedUICommand("Open map data", "OpenMap", typeof(MainWindow));
        public static readonly RoutedUICommand OpenMapImage = new RoutedUICommand("Open map image", "OpenMapImage", typeof(MainWindow));
    }
}
