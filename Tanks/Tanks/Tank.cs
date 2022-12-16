using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
    class Tank : GameItem // родительский класс для танков 
    {
        public Rectangle NextRadius { get; set; } // следующее положение танка
        public Rectangle Radius { get; set; } // положение танка
        public bool isDead { get; set; } // отслеживание, мертвый ли танк
        public Tuple<int, int> coordinates { get; set; } // отслеживает координаты танка
        public Bullet bullet { get; set; } // снаряд танка
        public int Mode { get; set; } // направление движения
        public int Speed { get; set; } // скорость танка
        public Tank() // конструктор
        {
            isDead = false;
            bullet = new Bullet();
            Speed = 2;
            Mode = 3;
            width = 60;
            height = 60;
        }
        public Tuple<int, int> GetFirePosition(PictureBox pic) // определяет координаты, в которых появится снаряд при выстреле
        {
            Tuple<int, int> dir;
            if (Mode == 1)
                dir = new Tuple<int, int>(pic.Location.X + pic.Width / 2 + 40, pic.Location.Y + pic.Width / 2 - 10);
            else if (Mode == 2)
                dir = new Tuple<int, int>(pic.Location.X - pic.Width / 2, pic.Location.Y + pic.Width / 2 - 10);
            else if (Mode == 3)
                dir = new Tuple<int, int>(pic.Location.X + pic.Width / 2 - 10, pic.Location.Y - pic.Width / 2);
            else
                dir = new Tuple<int, int>(pic.Location.X + pic.Width / 2 - 10, pic.Location.Y + pic.Width / 2 + 40);
            return dir;
        }
        public Tuple<int, int> GetDirection() // определяет направление движения пули
        {
            Tuple<int, int> dir;
            if (Mode == 1)
                dir = new Tuple<int, int>(1, 0);
            else if (Mode == 2)
                dir = new Tuple<int, int>(-1, 0);
            else if (Mode == 3)
                dir = new Tuple<int, int>(0, -1);
            else
                dir = new Tuple<int, int>(0, 1);
            return dir;
        }
        public bool isValidMove(Tuple<int, int> coord) // функция для того, чтобы танк не выел за рамки формы
        {
            if (coord.Item1 > 1120 || coord.Item2 > 700 || coord.Item1 < 1 || coord.Item2 < 1)
            {
                return false;
            }
            else
                return true;
        }
        public override void DrawItem(Graphics item, PictureBox pic) // отрисовка
        {
        }
    }
}
