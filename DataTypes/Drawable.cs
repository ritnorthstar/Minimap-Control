using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DataTypes
{
    public class Drawable : IDrawable
    {
        public static int BEACON_INNER_RADIUS = 20;
        public static int BEACON_OUTER_RADIUS = 30;

        public static Brush TABLES_FILL = Brushes.Transparent;

        public Object Subject { get { return subject; } }
        protected Object subject;

        protected Func<Object> getDrawable;
        protected Func<Rect> getBounds;

        public Drawable(Object subject)
        {
            this.subject = subject;

            if (subject is MapBeacon)
            {
                getDrawable = GetDrawableMapBeacon;
                getBounds = GetBoundsMapComponent;
            }
            else if (subject is MapTables)
            {
                getDrawable = GetDrawableMapTables;
                getBounds = GetBoundsMapComponent;
            }
        }

        public Object GetDrawable()
        {
            if (getDrawable != null)
            {
                return getDrawable();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public Rect GetBounds()
        {
            if (getBounds != null)
            {
                return getBounds();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected Object GetDrawableMapBeacon()
        {
            MapBeacon beacon = (MapBeacon)Subject;
            return new
            {
                type = "beacon",
                guid = beacon.Id,
                id = beacon.BeaconId,
                x = beacon.X,
                y = beacon.Y,
                z = beacon.Z,
                innerRadius = BEACON_INNER_RADIUS,
                outerRadius = BEACON_OUTER_RADIUS
            };
        }

        protected Object GetDrawableMapTables()
        {
            MapTables tables = (MapTables)Subject;
            return new
            {
                type = "tableBlock",
                guid = tables.Id,
                x = tables.X,
                y = tables.Y,
                z = tables.Z,
                width = tables.Width,
                height = tables.Height,
                fill = TABLES_FILL,
                tileRect = new Rect(0, 0, tables.Width / tables.TablesWide, tables.Height / tables.TablesTall)
            };
        }

        protected Rect GetBoundsMapComponent()
        {
            MapComponent component = (MapComponent)Subject;
            return new Rect(component.X, component.Y, component.Width, component.Height);
        }
    }
}
