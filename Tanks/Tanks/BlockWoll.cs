using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
    class BlockWall : GameItem
    {
        public BlockWall()
        {
            width = 64;
            height = 64;
        }
        public override void DrawItem(Graphics item, PictureBox pic)
        {
            Bitmap img = new Bitmap("wall.png");
            pic.Size = new Size(width, height);
            pic.Image = img;
        }
    }
}
