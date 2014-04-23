using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CartographerLibrary
{
    public class BeaconInfoManager
    {
        private static BeaconInfoManager manager;

        public static BeaconInfoManager Instance()
        {
            if (manager == null)
                manager = new BeaconInfoManager();
            return manager;
        }

        public BeaconInfoList beacons;

        private BeaconInfoManager()
        {
            beacons = new BeaconInfoList();
        }
    }

    public class BeaconInfoList :  System.Collections.ObjectModel.ObservableCollection<BeaconInfo>
    {
        public BeaconInfoList() : base() { }
    }

    [Serializable]
    public class BeaconInfo
    {
        private string shortId;
        private string bluetoothId;

        public string ShortID
        {
            get { return shortId; }
            set
            {
                shortId = value;
                OnPropertyChanged("ShortID");
            }
        }

        public string BluetoothID
        {
            get { return bluetoothId; }
            set
            {
                bluetoothId = value;
                OnPropertyChanged("BluetoothID");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public BeaconInfo(string shortId, string bluetoothId)
        {
            this.shortId = shortId;
            this.bluetoothId = bluetoothId;
        }

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public BeaconInfo Clone()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;

                return (BeaconInfo)formatter.Deserialize(ms);
            }
        }
    }
}
