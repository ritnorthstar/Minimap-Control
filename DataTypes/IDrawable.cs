using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataTypes
{
    public interface IDrawable
    {
        Rect GetBounds();

        Object GetDrawable();
    }
}
