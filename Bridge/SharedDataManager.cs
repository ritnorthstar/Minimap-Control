using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge
{
    public class SharedDataManager
    {
        public static List<BeaconInfo> Beacons = new List<BeaconInfo>();
    }

        public class BeaconInfo
    {
        public string DeviceLabel;
        public string DeviceID;

        public BeaconInfo(string deviceLabel, string deviceId)
        {
            this.DeviceLabel = deviceLabel;
            this.DeviceID = deviceId;
        }
    }
}
