using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class Minimap
    {
        public static bool MAPS_ALLOW_REMOTE_READ = true;
        public static bool MAPS_ALLOW_REMOTE_WRITE = true;
        public static bool TEAMS_ALLOW_REMOTE_READ = true;
        public static bool TEAMS_ALLOW_REMOTE_WRITE = true;

        private static DataManager<MapObject> mapManager = new DataManager<MapObject>();
        private static DataManager<TeamObject> teamManager = new DataManager<TeamObject>();

        /*
        public static void LoadSampleData()
        {
            MapObject sampleMap = new MapObject();
            sampleMap.Name = "Sample Map A";

            MapComponent barrierA = new MapComponent();
            barrierA.X = 100;
            barrierA.Y = 100;
            barrierA.Width = 10;
            barrierA.Height = 100;

            MapTables tablesA = new MapTables();
            tablesA.X = 50;
            tablesA.Y = 50;
            tablesA.Width = 20;
            tablesA.Height = 60;
            tablesA.TablesTall = 3;
            tablesA.TablesWide = 2;

            MapBeacon beaconA = new MapBeacon();
            beaconA.X = 75;
            beaconA.Y = 75;
            beaconA.BeaconId = "0001";

            sampleMap.Barriers.Add(barrierA);
            sampleMap.Tables.Add(tablesA);
            sampleMap.Beacons.Add(beaconA);

            mapManager.Add(sampleMap);
        }
        */

        /// <summary>
        /// Get the global map manager.</summary>
        /// <returns>
        /// Returns the global map manager instance.</returns>
        public static DataManager<MapObject> MapManager()
        {
            return mapManager;
        }

        /// <summary>
        /// Get the global team manager.</summary>
        /// <returns>
        /// Returns the global team manager instance.</returns>
        public static DataManager<TeamObject> TeamManager()
        {
            return teamManager;
        }
    }
}
