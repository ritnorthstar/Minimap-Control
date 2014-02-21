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
    public class Map
    {
        public static int nextId = 0;

        public int id;
        public string name;
        public HashSet<Wall> walls;
        public HashSet<TableBlock> tables;
        public HashSet<Beacon> beacons;
        private Dictionary<string, IDrawable> idTable;

        public Map() { idTable = new Dictionary<string, IDrawable>(); }

        public Map(string name)
        {
            this.id = nextId++;
            this.name = name;
            walls = new HashSet<Wall>();
            tables = new HashSet<TableBlock>();
            beacons = new HashSet<Beacon>();
            idTable = new Dictionary<string, IDrawable>();
        }

        public void Add(IDrawable drawable)
        {
            if (drawable is Wall)
                walls.Add((Wall)drawable);
            else if (drawable is TableBlock)
                tables.Add((TableBlock)drawable);
            else if (drawable is Beacon)
                beacons.Add((Beacon)drawable);

            idTable[drawable.guid] = drawable;
        }

        public Dictionary<string, IDrawable> IdTable()
        {
            return idTable;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public IDrawable GetDrawable(string guid)
        {
            return idTable.ContainsKey(guid) ? idTable[guid] : null;
        }

        public void GetDrawable()
        {
            try
            {
                foreach (Wall w in walls) w.GetDrawable();
                foreach (Beacon b in beacons) b.GetDrawable();
                foreach (TableBlock t in tables) t.GetDrawable();
            }
            catch (NotImplementedException e) { throw e; }
        }

        public void DrawOn(DrawingItemsSource source)
        {
            foreach (Wall w in walls) source.AddChild(w);
            foreach (Beacon b in beacons) source.AddChild(b);
            foreach (TableBlock t in tables) source.AddChild(t);
        }

        public void UpdateGuidTable()
        {
            List<IDrawable> drawables = walls.ToList<IDrawable>();
            drawables.AddRange(beacons);
            drawables.AddRange(tables);

            foreach(IDrawable d in drawables)
            {
                if(! idTable.ContainsKey(d.guid))
                    idTable[d.guid] = d;
            }
        }

        public static Map FromJson(string json)
        {
            Map output = JsonConvert.DeserializeObject<Map>(json);
            output.UpdateGuidTable();
            return output;
        }

        public static Map FromFile(string filename)
        {
            Map output =  Map.FromJson(File.ReadAllText(filename));
            return output;
        }
    }
}
