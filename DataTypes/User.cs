using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class User : UserObject
    {
        public User()
        {
            // do nothing
        }

        public User(string name, int x, int y, int z)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
        }

        public void DrawOn(DrawingItemsSource source)
        {
            source.AddChild(new Drawable(this, -1));
        }
    }
}
