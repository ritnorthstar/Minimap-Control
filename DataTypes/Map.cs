using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Controls;

namespace DataTypes
{
    public class Map : Drawable
    {
        public static int nextId = 0;

        public int id;
        public string name;
        public HashSet<Wall> walls;
        public HashSet<TableBlock> tables;
        public HashSet<Beacon> beacons;

        public Map() { }

        public Map(string name)
        {
            this.id = nextId++;
            this.name = name;
            walls = new HashSet<Wall>();
            tables = new HashSet<TableBlock>();
            beacons = new HashSet<Beacon>();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void DrawSelf(Canvas c)
        {
            try
            {
                foreach (Wall w in walls) w.DrawSelf(c);
                foreach (Beacon b in beacons) b.DrawSelf(c);
                foreach (TableBlock t in tables) t.DrawSelf(c);
            }
            catch (NotImplementedException) { }
        }

        public static Map FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Map>(json);
        }

        public static Map FromFile(string filename)
        {
            return Map.FromJson(File.ReadAllText(filename));
        }
    }
}
