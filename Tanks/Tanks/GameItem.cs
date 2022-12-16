using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public abstract class GameItem // общий класс для игровых объектов
    {
        public abstract void DrawItem(Graphics item, PictureBox pic); // отрисовка объекта
        public int width { get; set; } // ширина объекта
        public int height { get; set; } // высота объекта
    }
}
