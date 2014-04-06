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
            map.Name = "Gordon Field House";

            MapBeacon beacon1 = new MapBeacon();
            beacon1.BeaconId = "nw";
            beacon1.X = 125;
            beacon1.Y = 125;
            map.Beacons.Add(beacon1);

            MapBeacon beacon2 = new MapBeacon();
            beacon2.BeaconId = "n";
            beacon2.X = 375;
            beacon2.Y = 125;
            map.Beacons.Add(beacon2);

            MapBeacon beacon3 = new MapBeacon();
            beacon3.BeaconId = "ne";
            beacon3.X = 550;
            beacon3.Y = 125;
            map.Beacons.Add(beacon3);

            MapBeacon beacon4 = new MapBeacon();
            beacon4.BeaconId = "w";
            beacon4.X = 125;
            beacon4.Y = 275;
            map.Beacons.Add(beacon4);

            MapBeacon beacon5 = new MapBeacon();
            beacon5.BeaconId = "c";
            beacon5.X = 375;
            beacon5.Y = 275;
            map.Beacons.Add(beacon5);

            MapBeacon beacon6 = new MapBeacon();
            beacon6.BeaconId = "3";
            beacon6.X = 550;
            beacon6.Y = 275;
            map.Beacons.Add(beacon6);

            MapBeacon beacon7 = new MapBeacon();
            beacon7.BeaconId = "sw";
            beacon7.X = 125;
            beacon7.Y = 425;
            map.Beacons.Add(beacon7);

            MapBeacon beacon8 = new MapBeacon();
            beacon8.BeaconId = "s";
            beacon8.X = 375;
            beacon8.Y = 425;
            map.Beacons.Add(beacon8);

            MapBeacon beacon9 = new MapBeacon();
            beacon9.BeaconId = "se";
            beacon9.X = 550;
            beacon9.Y = 425;
            map.Beacons.Add(beacon9);

            MapTables tables1 = new MapTables();
            tables1.X = 100;
            tables1.Y = 100;
            tables1.Width = 300;
            tables1.Height = 50;
            tables1.TablesTall = 2;
            tables1.TablesWide = 4;
            map.Tables.Add(tables1);

            MapTables tables2 = new MapTables();
            tables2.X = 100;
            tables2.Y = 250;
            tables2.Width = 300;
            tables2.Height = 50;
            tables2.TablesTall = 2;
            tables2.TablesWide = 4;
            map.Tables.Add(tables2);

            MapTables tables3 = new MapTables();
            tables3.X = 100;
            tables3.Y = 400;
            tables3.Width = 300;
            tables3.Height = 50;
            tables3.TablesTall = 2;
            tables3.TablesWide = 4;
            map.Tables.Add(tables3);

            MapTables tables4 = new MapTables();
            tables4.X = 500;
            tables4.Y = 100;
            tables4.Width = 100;
            tables4.Height = 350;
            tables4.TablesTall = 4;
            tables4.TablesWide = 2;
            map.Tables.Add(tables4);

            System.IO.File.WriteAllText(@"C:\Users\Josh\Desktop\test2.map", map.ToJson());
        }
    }
}
