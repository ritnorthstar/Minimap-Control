using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class Wall : Impassable, IDrawable
    {
        public double x { get; set; }
        public double y { get; set; }
        public int z { get; set; }
        new public double width { get; set; }
        new public double height { get; set; }
        public string guid { get; set; }

        public Wall(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            guid = System.Guid.NewGuid().ToString();
        }

        public object GetDrawable()
        {
            throw new NotImplementedException();
        }
    }
}
