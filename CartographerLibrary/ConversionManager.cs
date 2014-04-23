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
        public Point scale;

        public static GraphicsBase ConvertFromMM(MapComponent item)
        {
            GraphicsBase output = null;

            if (item is MapBeacon)
            {
                output = new GraphicsBeacon(
                    item.X - 10,
                    item.Y - 10,
                    item.X + 10,
                    item.Y + 10,
                    1, Colors.DodgerBlue, 1.0);
            }

            else if (item is MapTables)
            {
                GraphicsTableBlock o = new GraphicsTableBlock(
                item.X,
                item.Y,
                item.X + item.Width,
                item.Y + item.Height,
                1, Colors.Green, 1.0);

                o.NumTablesTall = (item as MapTables).TablesTall;
                o.NumTablesWide = (item as MapTables).TablesWide;
                output = o;
            }

            else if (item is MapComponent)
            {
                output = new GraphicsBarrier(
                item.X,
                item.Y,
                item.X + item.Width,
                item.Y + item.Height,
                1, Colors.Red, 1.0);
            }

            return output;
        }

        public static void AddToMinimap(Map map, VisualCollection graphicsList)
        {
            TypeSwitch ts = new TypeSwitch()
                .Case((GraphicsBeacon x) => map.Beacons.Add(ConvertToMM(x)))
                .Case((GraphicsTableBlock x) => map.Tables.Add(ConvertToMM(x)))
                .Case((GraphicsBarrier x) => map.Barriers.Add(ConvertToMM(x)));

            foreach (GraphicsBase mapObject in graphicsList)
            {
                ts.Switch(mapObject);
            }
        }

        private static MapBeacon ConvertToMM(GraphicsBeacon beacon)
        {
            MapBeacon output = new MapBeacon();

            if (beacon.Info != null)
            {
                output.DeviceLabel = beacon.Info.ShortID;
                output.DeviceId = beacon.Info.BluetoothID;
            }

            Rect r = beacon.Rectangle;
            Point center = new Point((r.Left + r.Right) / 2.0, (r.Top + r.Bottom) / 2.0);
            output.X = center.X;
            output.Y = center.Y;

            return output;
        }

        private static MapTables ConvertToMM(GraphicsTableBlock tableBlock)
        {
            MapTables output = new MapTables();

            Rect r = tableBlock.Rectangle;
            output.X = r.Left;
            output.Y = r.Top;
            output.Width = r.Width;
            output.Height = r.Height;
            output.TablesTall = tableBlock.NumTablesTall;
            output.TablesWide = tableBlock.NumTablesWide;

            return output;
        }

        private static MapComponent ConvertToMM(GraphicsBarrier barrier)
        {
            MapComponent output = new MapComponent();

            Rect r = barrier.Rectangle;
            output.X = r.Left;
            output.Y = r.Top;
            output.Width = r.Width;
            output.Height = r.Height;

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
