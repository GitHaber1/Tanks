using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
    class Bricks : GameItem
    {
        public Bricks()
        {
            width = 65;
            height = 64;
        }
        public override void DrawItem(Graphics item, PictureBox pic)
        {
            Bitmap img = new Bitmap("brick.png");
            pic.Size = new Size(width, height);
            pic.Image = img;
        }
    }
}
