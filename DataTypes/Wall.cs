using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class Wall : Impassable, Drawable
    {
        public Wall(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public void DrawSelf(System.Windows.Controls.Canvas c)
        {
            throw new NotImplementedException();
        }
    }
}
