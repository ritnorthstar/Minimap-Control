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
        public IEnumerable<Wall> walls;
        public IEnumerable<TableRow> tables;
        public IEnumerable<Beacon> beacons;

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
