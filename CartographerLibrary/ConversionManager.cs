using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTypes;
using Core.Data;
using System.Windows.Media;
using System.Windows;

namespace CartographerLibrary
{
    public class ConversionManager
    {
        public static void AddAll(Map map, VisualCollection graphicsList)
        {
            TypeSwitch ts = new TypeSwitch()
                .Case((GraphicsBeacon x) => map.Beacons.Add(Convert(x)))
                .Case((GraphicsTableBlock x) => map.Tables.Add(Convert(x)))
                .Case((GraphicsBarrier x) => map.Barriers.Add(Convert(x)));

            foreach (GraphicsBase mapObject in graphicsList)
            {
                ts.Switch(mapObject);
            }
        }

        private static MapBeacon Convert(GraphicsBeacon beacon)
        {
            MapBeacon output = new MapBeacon();

            output.BeaconId = String.Empty;

            Rect r = beacon.Rectangle;
            Point center = new Point((r.Left + r.Right) / 2.0, (r.Top + r.Bottom) / 2.0);
            output.X = center.X;
            output.Y = center.Y;

            return output;
        }

        private static MapTables Convert(GraphicsTableBlock tableBlock)
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

        private static MapComponent Convert(GraphicsBarrier barrier)
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
