using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataTypes
{
    public class Team
    {
        public int id;
        public Brush color;

        public Team(int id, Brush color)
        {
            this.id = id;
            this.color = color;
        }
    }
}
