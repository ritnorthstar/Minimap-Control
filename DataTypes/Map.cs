using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class Map
    {
        public static int nextId = 0;

        public int id;
        public string name;
        public HashSet<Wall> walls;
        public HashSet<TableRow> tables;
        public HashSet<Beacon> beacons;

        public Map() { }

        public Map(string name)
        {
            this.id = nextId++;
            this.name = name;
            walls = new HashSet<Wall>();
            tables = new HashSet<TableRow>();
            beacons = new HashSet<Beacon>();
        }
    }
}
