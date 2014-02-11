using DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Northstar.Minimap.Control.Server.Models
{
    public class LocalMapRepository : IMapRepository
    {
        private List<Map> maps = new List<Map>();
        private int nextId = 1;

        public LocalMapRepository()
        {
            maps.Add(new Map("Clark Gym"));
            maps.Add(new Map("Gordon Field House"));
            maps.Add(new Map("Gracie's Field"));
            maps.Add(new Map("Global Village Plaza"));
        }

        public IEnumerable<Map> GetAll()
        {
            return maps;
        }

        public Map Get(int id)
        {
            return maps.Find(m => m.id == id);
        }

        public Map Add(Map map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }
            map.id = nextId++;
            maps.Add(map);
            return map;
        }

        public void Remove(int id)
        {
            maps.RemoveAll(m => m.id == id);
        }

        public bool Update(Map map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }
            int index = maps.FindIndex(m => m.id == map.id);
            if (index == -1)
            {
                return false;
            }
            maps.RemoveAt(index);
            maps.Add(map);
            return true;
        }
    }
}