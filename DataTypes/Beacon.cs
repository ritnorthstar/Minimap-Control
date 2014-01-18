﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using ExtensionMethods;

namespace DataTypes
{
    public class Beacon : IDrawable
    {
        public string id;
        public double x { get; set; }
        public double y { get; set; }

        public Beacon(string id, double x, double y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        public object GetDrawable()
        {
            string type = "beacon";

            int innerRadius = 20, outerRadius = 30;

            return new { type, id, x, y, innerRadius, outerRadius };
        }

        public override string ToString()
        {
            return String.Format("{0} - ({1}, {2})", id, x, y);
        }
    }
}
