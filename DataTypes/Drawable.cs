using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataTypes
{
    interface Drawable
    {
        void DrawSelf(Canvas c, double zoomFactor);
    }
}
