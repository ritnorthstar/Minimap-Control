using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataTypes
{
    public interface IDrawable
    {
        double x { get; set; }
        double y { get; set; }
        int z { get; set; }
        double width { get; set; }
        double height { get; set; }

        object GetDrawable();
    }
}
