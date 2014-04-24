using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTypes;
using Core.Data;
using System.Windows.Media;
using System.Windows;
using Core.Data;

namespace CartographerLibrary
{
    public class ConversionManager
    {
        private static Point convertBack(double x, double y, Map map, Point canvasDimensions)
        {
            Point output = new Point();

            output.X = x * canvasDimensions.X / map.Width;
            output.Y = y * canvasDimensions.Y / map.Height;

            return output;
        }

        public static GraphicsBase ConvertFromMM(MapComponent item, Map map, Point canvasDimensions)
        {
            GraphicsBase output = null;

            if (item is MapBeacon)
            {
                Point center = convertBack(item.X, item.Y, map, canvasDimensions);

                output = new GraphicsBeacon(
                    center.X - 10, center.Y - 10,
                    center.X + 10, center.Y + 10,
                    1, Colors.DodgerBlue, 1.0);
            }

            else
            {
                Point loc = convertBack(item.X, item.Y, map, canvasDimensions);
                Point dim = convertBack(item.Width, item.Height, map, canvasDimensions);

                if (item is MapTables)
                {
                    GraphicsTableBlock o = new GraphicsTableBlock(
                    loc.X, loc.Y,
                    loc.X + dim.X, loc.Y + dim.Y,
                    1, Colors.Green, 1.0);

                    o.NumTablesTall = (item as MapTables).TablesTall;
                    o.NumTablesWide = (item as MapTables).TablesWide;
                    output = o;
                }

                else if (item is MapComponent)
                {
                    output = new GraphicsBarrier(
                    loc.X, loc.Y,
                    loc.X + dim.X, loc.Y + dim.Y,
                    1, Colors.Red, 1.0);
                }
            }

            return output;
        }

        private static Map map;
        private static Point canvasDimensions;

        public static void AddToMinimap(Map _map, VisualCollection graphicsList, Point _canvasDimensions)
        {
            map = _map;
            canvasDimensions = _canvasDimensions;

            TypeSwitch ts = new TypeSwitch()
                .Case((GraphicsBeacon x) => map.Beacons.Add(convertToMM(x)))
                .Case((GraphicsTableBlock x) => map.Tables.Add(convertToMM(x)))
                .Case((GraphicsBarrier x) => map.Barriers.Add(convertToMM(x)));

            foreach (GraphicsBase mapObject in graphicsList)
            {
                ts.Switch(mapObject);
            }
        }

        private static MapBeacon convertToMM(GraphicsBeacon beacon)
        {
            MapBeacon output = new MapBeacon();

            if (beacon.Info != null)
            {
                output.DeviceLabel = beacon.Info.ShortID;
                output.DeviceId = beacon.Info.BluetoothID;
            }

            Rect r = beacon.Rectangle;
            Point loc = convertCoords(new Point((r.Left + r.Right) / 2.0, (r.Top + r.Bottom) / 2.0));
            output.X = loc.X;
            output.Y = loc.Y;

            return output;
        }

        private static MapTables convertToMM(GraphicsTableBlock tableBlock)
        {
            MapTables output = new MapTables();

            Rect r = tableBlock.Rectangle;
            Point loc = convertCoords(new Point(r.Left, r.Top));
            Point dim = convertCoords(new Point(r.Width, r.Height));

            output.X = loc.X;
            output.Y = loc.Y;
            output.Width = dim.X;
            output.Height = dim.Y;
            output.TablesTall = tableBlock.NumTablesTall;
            output.TablesWide = tableBlock.NumTablesWide;

            return output;
        }

        private static MapComponent convertToMM(GraphicsBarrier barrier)
        {
            MapComponent output = new MapComponent();

            Rect r = barrier.Rectangle;
            Point loc = convertCoords(new Point(r.Left, r.Top));
            Point dim = convertCoords(new Point(r.Width, r.Height));

            output.X = loc.X;
            output.Y = loc.Y;
            output.Width = dim.X;
            output.Height = dim.Y;

            return output;
        }

        private static Point convertCoords(Point p)
        {
            Point output = new Point();

            output.X = p.X / canvasDimensions.X * map.Width;
            output.Y = p.Y / canvasDimensions.Y * map.Height;

            return output;
        }
    }

    class TypeSwitch
    {
        Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();
        public TypeSwitch Case<T>(Action<T> action) { matches.Add(typeof(T), (x) => action((T)x)); return this; }
        public void Switch(object x) { matches[x.GetType()](x); }
    }
}
