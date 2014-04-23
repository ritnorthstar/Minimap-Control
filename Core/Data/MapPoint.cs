using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MapPoint : MapComponent
    {
        public string Name { get; set; }

        public MapPoint()
        {
            // do nothing
        }

        protected MapPoint(MapPoint copy) : base(copy)
        {
            Name = copy.Name;
        }

        public override String ToString()
        {
            return String.Format("{0} - ({1}, {2})", Name, X, Y);
        }

        public override Object Clone()
        {
            return new MapPoint(this);
        }
    }
}
