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
        protected class ComponentCollection<T> : KeyedCollection<string, T> where T:MapComponent
        {
            public ComponentCollection()
            {
                // do nothing
            }

            protected ComponentCollection(ComponentCollection<T> copy)
            {
                foreach (T t in copy)
                {
                    Add((T)t.Clone());
                }
            }

            protected override string GetKeyForItem(T t)
            {
                return t.Id;
            }

            public virtual ComponentCollection<T> Clone()
            {
                return new ComponentCollection<T>(this);
            }
        }

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

        public IEnumerable<MapComponent> Components { get { return Barriers.Concat(Tables).Concat(Beacons); } }

        public MapObject()
        {
            barriers = new ComponentCollection<MapComponent>();
            tables = new ComponentCollection<MapTables>();
            beacons = new ComponentCollection<MapBeacon>();
        }

        protected MapObject(MapObject copy) : base(copy)
        {
            Name = copy.Name;
            Width = copy.Width;
            Height = copy.Height;
            Tech = copy.Tech;
            barriers = ((ComponentCollection<MapComponent>)copy.Barriers).Clone();
            tables = ((ComponentCollection<MapTables>)copy.Tables).Clone();
            beacons = ((ComponentCollection<MapBeacon>)copy.Beacons).Clone();
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
    }
}
