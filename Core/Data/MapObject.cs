using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MapObject : DataObject
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Tech { get; set; }

        public KeyedCollection<string, MapComponent> Barriers { get { return barriers; } }
        protected KeyedCollection<string, MapComponent> barriers;

        public KeyedCollection<string, MapTables> Tables { get { return tables; } }
        protected KeyedCollection<string, MapTables> tables;

        public KeyedCollection<string, MapBeacon> Beacons { get { return beacons; } }
        protected KeyedCollection<string, MapBeacon> beacons;

        public MapObject()
        {
            barriers = new MapComponentCollection<MapComponent>();
            tables = new MapComponentCollection<MapTables>();
            beacons = new MapComponentCollection<MapBeacon>();
        }

        protected MapObject(MapObject copy) : base(copy)
        {
            Name = copy.Name;
            Width = copy.Width;
            Height = copy.Height;
            Tech = copy.Tech;
            barriers = ((MapComponentCollection<MapComponent>)copy.Barriers).Clone();
            tables = ((MapComponentCollection<MapTables>)copy.Tables).Clone();
            beacons = ((MapComponentCollection<MapBeacon>)copy.Beacons).Clone();
        }

        public override Object Clone()
        {
            return new MapObject(this);
        }

        public MapComponent GetComponent(string id)
        {
            MapComponent component = null;
            
            if (Barriers.Contains(id))
            {
                component = Barriers[id];
            }
            else if (Tables.Contains(id))
            {
                component = Tables[id];
            }
            else if (Beacons.Contains(id))
            {
                component = Beacons[id];
            }

            return component;
        }

        public IEnumerable<MapComponent> GetComponents()
        {
            return Barriers.Concat(Tables).Concat(Beacons);
        }
    }
}
