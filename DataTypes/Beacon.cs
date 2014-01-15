using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataTypes
{
    public class Beacon : Drawable
    {
        public string id;
        public double x, y;

        public Beacon(string id, double x, double y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        public void DrawSelf(Canvas c)
        {
            // TODO: implement this
        }
    }
}
