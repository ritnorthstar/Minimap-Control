using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MapComponent : ICloneable
    {
        // TODO - Make Id readonly (will cause issues with current JSON deserialization)
        public string Id;
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public MapComponent()
        {
            Id = System.Guid.NewGuid().ToString();
        }

        protected MapComponent(MapComponent copy)
        {
            Id = copy.Id;
            X = copy.X;
            Y = copy.Y;
            Z = copy.Z;
            Width = copy.Width;
            Height = copy.Height;
        }

        public override String ToString()
        {
            return String.Format("MapComponent");
        }

        public virtual Object Clone()
        {
            return new MapComponent(this);
        }
    }

    public class MapComponentCollection<T> : KeyedCollection<string, T> where T : MapComponent
    {
        public MapComponentCollection()
        {
            // do nothing
        }

        protected MapComponentCollection(MapComponentCollection<T> copy)
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

        public virtual MapComponentCollection<T> Clone()
        {
            return new MapComponentCollection<T>(this);
        }
    }
}
