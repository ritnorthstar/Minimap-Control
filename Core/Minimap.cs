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
        public static bool MAPS_ALLOW_REMOTE_WRITE = false;
        public static bool TEAMS_ALLOW_REMOTE_READ = true;
        public static bool TEAMS_ALLOW_REMOTE_WRITE = false;
        public static bool USERS_ALLOW_REMOTE_READ = true;
        public static bool USERS_ALLOW_REMOTE_WRITE = true;

        private static DataManager<MapObject> mapManager = new DataManager<MapObject>();
        private static DataManager<TeamObject> teamManager = new DataManager<TeamObject>();
        private static DataManager<UserObject> userManager = new DataManager<UserObject>(TimeSpan.FromMinutes(5));

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

        /// <summary>
        /// Get the global user manager.</summary>
        /// <returns>
        /// Returns the global user manager instance.</returns>
        public static DataManager<UserObject> UserManager()
        {
            return userManager;
        }
    }
}
