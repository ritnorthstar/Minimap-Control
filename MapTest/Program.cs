using Core.Data;
using DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map();
            map.Name = "Home";
            map.Width = 300;
            map.Height = 300;

            MapBeacon beacon1 = new MapBeacon();
            beacon1.BeaconId = "#1";
            beacon1.X = 10;
            beacon1.Y = 10;
            map.Beacons.Add(beacon1);

            MapBeacon beacon2 = new MapBeacon();
            beacon2.BeaconId = "#2";
            beacon2.X = 290;
            beacon2.Y = 10;
            map.Beacons.Add(beacon2);

            MapBeacon beacon3 = new MapBeacon();
            beacon3.BeaconId = "#3";
            beacon3.X = 290;
            beacon3.Y = 290;
            map.Beacons.Add(beacon3);

            MapBeacon beacon4 = new MapBeacon();
            beacon4.BeaconId = "#4";
            beacon4.X = 10;
            beacon4.Y = 290;
            map.Beacons.Add(beacon4);

            MapTables tables1 = new MapTables();
            tables1.X = 100;
            tables1.Y = 117;
            tables1.Width = 100;
            tables1.Height = 66;
            tables1.TablesTall = 2;
            tables1.TablesWide = 3;
            map.Tables.Add(tables1);

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            System.IO.File.WriteAllText(path + @"\Home.map", map.ToJson());
        }
    }
}
