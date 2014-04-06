using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MapBeacon : MapComponent
    {
        public string BeaconId { get; set; }

        public MapBeacon()
        {
            // do nothing
        }

        protected MapBeacon(MapBeacon copy) : base(copy)
        {
            BeaconId = copy.BeaconId;
        }

        public override String ToString()
        {
            return String.Format("Beacon {0} - ({1}, {2})", BeaconId, X, Y);
        }

        public override Object Clone()
        {
            return new MapBeacon(this);
        }
    }
}
