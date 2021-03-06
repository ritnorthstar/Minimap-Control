﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MapTables : MapComponent
    {
        public int TablesWide { get; set; }
        public int TablesTall { get; set; }

        public MapTables()
        {
            // do nothing
        }

        protected MapTables(MapTables copy) : base(copy)
        {
            TablesWide = copy.TablesWide;
            TablesTall = copy.TablesTall;
        }

        public override String ToString()
        {
            return String.Format("Tables: ({0}, {1}), {2}x{3}, {4}x{5}", X, Y, Width, Height, TablesWide, TablesTall);
        }

        public override Object Clone()
        {
            return new MapTables(this);
        }
    }
}
