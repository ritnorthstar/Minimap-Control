using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class Beacon
    {
        public string id;
        public double x, y;

        public Beacon(string id, double x, double y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }
    }
}
