using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMWebAPI.Models
{
    public class LocalMapRepository : IMapRepository
    {
        private List<Map> maps = new List<Map>();
        private int _nextId = 1;

        public IEnumerable<Map> GetAll()
        {
            return maps;
        }

        public Map Get(int id)
        {
            return maps.Find(m => m.Id == id);
        }

        public Map Add(Map map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }
            map.Id = _nextId++;
            maps.Add(map);
            return map;
        }

        public void Remove(int id)
        {
            maps.RemoveAll(m => m.Id == id);
        }

        public bool Update(Map map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }
            int index = maps.FindIndex(m => m.Id == map.Id);
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
