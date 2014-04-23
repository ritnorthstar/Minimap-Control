using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class TeamObject : DataObject
    {
        public struct TeamColor
        {
            public byte A;
            public byte R;
            public byte G;
            public byte B;

            public TeamColor(byte a, byte r, byte g, byte b)
            {
                A = a;
                R = r;
                G = g;
                B = b;
            }
        }

        public string Name { get; set; }
        public string MapId { get; set; }

        public TeamColor PrimaryColor { get; set; }
        public TeamColor SecondaryColor { get; set; }

        public KeyedCollection<string, MapPoint> Points { get { return points; } }
        protected KeyedCollection<string, MapPoint> points;

        public TeamObject()
        {
            PrimaryColor = new TeamColor(255, 255, 255, 255);
            SecondaryColor = new TeamColor(255, 255, 255, 255);
            points = new MapComponentCollection<MapPoint>();
        }

        protected TeamObject(TeamObject copy) : base(copy)
        {
            Name = copy.Name;
            MapId = copy.MapId;
            PrimaryColor = copy.PrimaryColor;
            SecondaryColor = copy.SecondaryColor;
            points = ((MapComponentCollection<MapPoint>)copy.Points).Clone();
        }

        public override Object Clone()
        {
            return new TeamObject(this);
        }
    }
}
