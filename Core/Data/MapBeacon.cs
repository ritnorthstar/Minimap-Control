using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MapBeacon : MapComponent
    {
        public string DeviceId { get; set; }
        public string DeviceLabel { get; set; }

        public MapBeacon()
        {
            // do nothing
        }

        protected MapBeacon(MapBeacon copy) : base(copy)
        {
            DeviceId = copy.DeviceId;
            DeviceLabel = copy.DeviceLabel;
        }

        public override String ToString()
        {
            return String.Format("Beacon {0} - ({1}, {2})", DeviceLabel, X, Y);
        }

        public override Object Clone()
        {
            return new MapBeacon(this);
        }
    }
}
