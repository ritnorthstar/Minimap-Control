using Core;
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
        public static string BEACON_TYPE = "beacon";
        public static int BEACON_INNER_RADIUS = 20;
        public static int BEACON_OUTER_RADIUS = 30;

        public static string TABLES_TYPE = "tableBlock";
        public static Brush TABLES_FILL = Brushes.Transparent;

        public static string BARRIER_TYPE = "barrier";

        public static string USER_TYPE = "judge";
        public static int USER_WIDTH = 25;
        public static int USER_HEIGHT = 25;

        public static string POI_TYPE = "point";
        public static int POI_WIDTH = 25;
        public static int POI_HEIGHT = 25;
        public static double scalar = 5.0;

        public Object Subject { get { return subject; } }
        protected Object subject;

        protected Func<Object> getDrawable;
        protected Func<Rect> getBounds;

        public Drawable(Object subject, double s)
        {
            this.subject = subject;

            if(s > 0) // negative values for dynamic elements
                scalar = s;

            // Map Components
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
            else if (subject is MapComponent)
            {
                getDrawable = GetDrawableMapComponent;
                getBounds = GetBoundsMapComponent;
            }

            // Users
            else if (subject is User)
            {
                getDrawable = GetDrawableUser;
                getBounds = GetBoundsUser;
            }

            // Itinerary Points
            else if (subject is POI)
            {
                getDrawable = GetDrawablePOI;
                getBounds = GetBoundsPOI;
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

        public override String ToString()
        {
            return Subject.ToString();
        }

        protected Object GetDrawableMapBeacon()
        {
            MapBeacon beacon = (MapBeacon)Subject;
            return new
            {
                type = BEACON_TYPE,
                guid = beacon.Id,
                id = beacon.DeviceLabel,
                x = beacon.X * scalar - (BEACON_OUTER_RADIUS / 2),
                y = beacon.Y * scalar - (BEACON_OUTER_RADIUS / 2),
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
                type = TABLES_TYPE,
                guid = tables.Id,
                x = tables.X * scalar,
                y = tables.Y * scalar,
                z = tables.Z,
                width = tables.Width * scalar,
                height = tables.Height * scalar,
                fill = TABLES_FILL,
                tileRect = new Rect(0, 0, tables.Width * scalar / tables.TablesWide, tables.Height * scalar / tables.TablesTall)
            };
        }

        protected Object GetDrawableMapComponent()
        {
            MapComponent barrier = (MapComponent)Subject;
            return new
            {
                type = BARRIER_TYPE,
                guid = barrier.Id,
                x = barrier.X * scalar,
                y = barrier.Y * scalar,
                z = barrier.Z,
                width = barrier.Width * scalar,
                height = barrier.Height * scalar,
                rect = new Rect(0, 0, barrier.Width * scalar, barrier.Height * scalar)
            };
        }

        protected Object GetDrawableUser()
        {
            User user = (User)Subject;

            Brush fill = new SolidColorBrush();
            Brush border = new SolidColorBrush();

            Team team = new Team(Minimap.TeamManager().Get(user.TeamId));
            fill = new SolidColorBrush(team.PrimaryColor);
            border = new SolidColorBrush(team.SecondaryColor);

            return new
            {
                type = USER_TYPE,
                id = user.Name,
                guid = user.Id,
                x = user.X,
                y = user.Y,
                z = user.Z,
                fill = fill,
                border = border
            };
        }

        protected Object GetDrawablePOI()
        {
            throw new NotImplementedException();
        }

        protected Rect GetBoundsMapComponent()
        {
            MapComponent component = (MapComponent)Subject;
            return new Rect(component.X, component.Y, component.Width, component.Height);
        }

        protected Rect GetBoundsUser()
        {
            User user = (User)Subject;
            return new Rect(user.X, user.Y, USER_WIDTH, USER_HEIGHT);
        }

        protected Rect GetBoundsPOI()
        {
            POI point = (POI)Subject;
            return new Rect(point.X, point.Y, POI_WIDTH, POI_HEIGHT);
        }
    }
}
