using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTypes;

namespace MMWebAPI.Models
{
    interface IMapRepository
    {
        IEnumerable<Map> GetAll();
        Map Get(int id);
        Map Add(Map map);
        void Remove(int id);
        bool Update(Map map);
    }
}
