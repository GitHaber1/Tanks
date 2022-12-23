using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace WpfTanks
{
    public class Base : GameItem
    {
        public bool isDestroyed { get; set; }
        public Base()
        {
            isDestroyed = false;
            width = 64;
            height = 59;
        }
        public override void DrawItem(Rectangle pic)
        {
            pic.Width = width;
            pic.Height = height;
            pic.Visibility = Visibility.Visible;
        }
    }
}