using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    abstract class GameItem
    {
        public abstract void DrawItem(Graphics item, PictureBox pic);
        public int width { get; set; }
        public int height { get; set; }
    }
}
