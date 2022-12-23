using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfTanks
{
    public abstract class GameItem
    {
        public abstract void DrawItem(Rectangle pic);
        public int width { get; set; }
        public int height { get; set; }
    }
}

