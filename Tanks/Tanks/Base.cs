using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
    class Base : GameItem // класс для базы
    {
        public bool isDestroyed { get; set; } // индикатор, показывающий, уничтожена ли база
        public Base() // конструктор
        {
            isDestroyed = false;
            width = 64;
            height = 59;
        }
        public override void DrawItem(Graphics item, PictureBox pic) // отрисовка
        {
            Bitmap img = new Bitmap("base.png");
            pic.Size = new Size(width, height);
            pic.Image = img;
        }
    }
}
