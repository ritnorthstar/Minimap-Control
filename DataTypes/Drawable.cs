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

        public Object Subject { get { return subject; } }
        protected Object subject;

        protected Func<Object> getDrawable;
        protected Func<Rect> getBounds;

        public Drawable(Object subject)
        {
            this.subject = subject;

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
                x = beacon.X - (BEACON_OUTER_RADIUS / 2),
                y = beacon.Y - (BEACON_OUTER_RADIUS / 2),
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
                x = tables.X,
                y = tables.Y,
                z = tables.Z,
                width = tables.Width,
                height = tables.Height,
                fill = TABLES_FILL,
                tileRect = new Rect(0, 0, tables.Width / tables.TablesWide, tables.Height / tables.TablesTall)
            };
        }

        protected Object GetDrawableMapComponent()
        {
            MapComponent barrier = (MapComponent)Subject;
            return new
            {
                type = BARRIER_TYPE,
                guid = barrier.Id,
                x = barrier.X,
                y = barrier.Y,
                z = barrier.Z,
                width = barrier.Width,
                height = barrier.Height,
                rect = new Rect(0, 0, barrier.Width, barrier.Height)
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
    }
}
