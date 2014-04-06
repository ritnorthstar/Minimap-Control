using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MapComponent : ICloneable
    {
        private readonly string id;
        public string Id { get { return id; } }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public MapComponent()
        {
            id = System.Guid.NewGuid().ToString();
        }

        protected MapComponent(MapComponent copy)
        {
            id = copy.Id;
            X = copy.X;
            Y = copy.Y;
            Z = copy.Z;
            Width = copy.Width;
            Height = copy.Height;
        }

        public virtual Object Clone()
        {
            return new MapComponent(this);
        }
    }
}
