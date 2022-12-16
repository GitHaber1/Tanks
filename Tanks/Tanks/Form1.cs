using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;

namespace Tanks
{
    public partial class Form1 : Form // класс, в котором содержится логика для формы с игрой 
    {
        List<Point> startPos = new List<Point>(); // стартовые позиции танков
        Graphics gr; // средство для отрисовки
        int animationMode = 3; // отслеживает направление танка
        bool isPressedKey = false; // отслеживает, нажата ли клавиша
        List<PictureBox> tanks = new List<PictureBox>(); // список с информацией о танках на форме
        List<PictureBox> bricks = new List<PictureBox>(); // список с информацией о блоках на форме
        List<PictureBox> walls = new List<PictureBox>();// список с информацией о блоках на форме
        List<PictureBox> bases = new List<PictureBox>();// список с информацией о блоках на форме
        Bricks brick = new Bricks(); // элемент для добавления блоков
        BlockWall wall = new BlockWall(); // элемент для добавления блоков
        Base Base = new Base(); // элемент для добавления блоков
        PlayerTank player = new PlayerTank(); // танк игрока
        List<Tank> all = new List<Tank>(); // список танков
        string TanksControls = "TanksControls.json"; // файл для сериализации
        string BricksContorls = "BricksControls.json"; // файл для сериализации
        string WallsControls = "WallsControls.json"; // файл для сериализации
        string BaseControl = "BaseControl.json"; // файл для сериализации
        string PlayerTankInfo = "PlayerTankInfo.json"; // файл для сериализации
        string EnemyTanksInfo = "EnemyTankInfo.json"; // файл для сериализации

        public Form1() // инициализация компанентов формы
        {
            InitializeComponent();

            startPos.Add(pictureBox51.Location);
            startPos.Add(pictureBox52.Location);
            startPos.Add(pictureBox53.Location);
            startPos.Add(pictureBox54.Location);

            BackgroundImage = (Bitmap)Image.FromFile(Path.Combine(Application.StartupPath + "\\background.jpg"));
            gr = this.CreateGraphics();
            if (File.ReadAllText(TanksControls).Length > 0) // условие, которое проверяет, надо ли загружать данные
                ReDraw();
            else
            {
                DrawMap();
                player = new PlayerTank();
                all.Add(player);
                ComputerTank enemy;
                enemy = new ComputerTank();
                enemy.Mode = 1;
                all.Add(enemy);
                enemy = new ComputerTank();
                enemy.Mode = 1;
                all.Add(enemy);
                enemy = new ComputerTank();
                all.Add(enemy);
                tanks.Add(pictureBox51);
                tanks.Add(pictureBox52);
                tanks.Add(pictureBox53);
                tanks.Add(pictureBox54);
            }

            this.KeyDown += new KeyEventHandler(keyboard);
            this.KeyUp += new KeyEventHandler(freeKey);
            timer1.Interval = 1;
            timer1.Tick += new EventHandler(update);
            timer1.Start();

            PlayerTankAnimation();
            EnemyTankAnimation();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true); // для быстрой отрисовки объектов

        }
        private void ReDraw() // зарузка последней сохраненной игры
        {
            string json1 = File.ReadAllText(TanksControls);
            string json2 = File.ReadAllText(BricksContorls);
            string json3 = File.ReadAllText(WallsControls);
            string json4 = File.ReadAllText(BaseControl);
            string json5 = File.ReadAllText(PlayerTankInfo);
            string json6 = File.ReadAllText(EnemyTanksInfo);

            List<Point> tankscon = new List<Point>();
            List<Point> brickscon = new List<Point>();
            List<Point> wallscon = new List<Point>();
            List<Point> basecon = new List<Point>();

            if (json1.Length > 0)
                tankscon = JsonSerializer.Deserialize<List<Point>>(json1);
            if (json2.Length > 0)
                brickscon = JsonSerializer.Deserialize<List<Point>>(json2);
            if (json3.Length > 0)
                wallscon = JsonSerializer.Deserialize<List<Point>>(json3);
            if (json4.Length > 0)
                basecon = JsonSerializer.Deserialize<List<Point>>(json4);
            if (json5.Length > 0)
                all.Add(JsonSerializer.Deserialize<PlayerTank>(json5));
            if (json6.Length > 0)
            {
                all.AddRange(JsonSerializer.Deserialize<List<ComputerTank>>(json6));
            }

            pictureBox51.Location = tankscon[0];
            pictureBox52.Location = tankscon[1];
            pictureBox53.Location = tankscon[2];
            pictureBox54.Location = tankscon[3];
            tanks.Add(pictureBox51);
            tanks.Add(pictureBox52);
            tanks.Add(pictureBox53);
            tanks.Add(pictureBox54);
            player = (PlayerTank)all[0];
            player.bullet = new Bullet();
            all[1].bullet = new Bullet();
            all[2].bullet = new Bullet();
            all[3].bullet = new Bullet();

            foreach (var item in this.Controls)
            {
                if (item is PictureBox)
                {
                    if (((PictureBox)item).Tag.Equals("map") && brickscon.Contains(((PictureBox)item).Location))
                    {
                        brick.DrawItem(gr, ((PictureBox)item));
                        bricks.Add((PictureBox)item);
                    }
                    else if (((PictureBox)item).Tag.Equals("wall") && wallscon.Contains(((PictureBox)item).Location))
                    {
                        wall.DrawItem(gr, ((PictureBox)item));
                        walls.Add((PictureBox)item);
                    }
                    else if (((PictureBox)item).Tag.Equals("base") && basecon.Contains(((PictureBox)item).Location))
                    {
                        Base.DrawItem(gr, ((PictureBox)item));
                        bases.Add((PictureBox)item);
                    }
                    else if (!((PictureBox)item).Tag.Equals("playerTank") && !((PictureBox)item).Tag.Equals("Enemy"))
                        ((PictureBox)item).Visible = false;
                }
            }
            tankscon = null;
            brickscon = null;
            wallscon = null;
            basecon = null;
        }
        private void Start() // функция для перезапуска при поражении
        {
            Base.isDestroyed = false;
            pictureBox51.Location = startPos[0];
            pictureBox52.Location = startPos[1];
            pictureBox53.Location = startPos[2];
            pictureBox54.Location = startPos[3];
            tanks = new List<PictureBox>();
            bricks = new List<PictureBox>();
            walls = new List<PictureBox>();
            bases = new List<PictureBox>();
            all = new List<Tank>();
            DrawMap();
            player = new PlayerTank();
            all.Add(player);
            ComputerTank enemy;
            enemy = new ComputerTank();
            enemy.Mode = 1;
            all.Add(enemy);
            enemy = new ComputerTank();
            enemy.Mode = 1;
            all.Add(enemy);
            enemy = new ComputerTank();
            all.Add(enemy);
            tanks.Add(pictureBox51);
            tanks.Add(pictureBox52);
            tanks.Add(pictureBox53);
            tanks.Add(pictureBox54);
            PlayerTankAnimation();
            EnemyTankAnimation();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // сохранение игры при закрытии формы
        {
            List<Point> adder = new List<Point>();
            foreach (PictureBox pic in tanks)
            {
                adder.Add(pic.Location);
            }
            string json1 = JsonSerializer.Serialize<List<Point>>(adder);
            adder = new List<Point>();
            foreach (PictureBox pic in bricks)
            {
                adder.Add(pic.Location);
            }
            string json2 = JsonSerializer.Serialize<List<Point>>(adder);
            adder = new List<Point>();
            foreach (PictureBox pic in walls)
            {
                adder.Add(pic.Location);
            }
            string json3 = JsonSerializer.Serialize<List<Point>>(adder);
            adder = new List<Point>();
            foreach (PictureBox pic in bases)
            {
                adder.Add(pic.Location);
            }
            string json4 = JsonSerializer.Serialize<List<Point>>(adder);
            adder = new List<Point>();
            player = (PlayerTank)all[0];
            string json5 = JsonSerializer.Serialize<PlayerTank>(player);
            List<ComputerTank> comp = new List<ComputerTank>();
            comp.Add((ComputerTank)all[1]);
            comp.Add((ComputerTank)all[2]);
            comp.Add((ComputerTank)all[3]);
            string json6 = JsonSerializer.Serialize<List<ComputerTank>>(comp);
            File.WriteAllText(TanksControls, json1);
            File.WriteAllText(BricksContorls, json2);
            File.WriteAllText(WallsControls, json3);
            File.WriteAllText(BaseControl, json4);
            File.WriteAllText(PlayerTankInfo, json5);
            File.WriteAllText(EnemyTanksInfo, json6);
        }
        private Point GetEmptySpace(List<PictureBox> tanks) // алгоритм для корректного возраждения вражеского танка
        {
            Point tup = new Point(1, 1);
            bool flag = true;
            bool conflict;
            while (flag)
            {
                conflict = false;
                foreach (PictureBox tank in tanks)
                {
                    if (tank.Bounds.IntersectsWith(new Rectangle(tup, new Size(60, 60))))
                    {
                        tup = new Point(tup.X + 60, tup.Y);
                        conflict = true;
                    }
                }
                if (!conflict)
                    flag = false;
            }
            return tup;
        }
        private void freeKey(object sender, KeyEventArgs e) // функция, которая отслеживает, нажата ли клавиша
        {
            isPressedKey = false;
        }
        private void EnemyTankMovement() // алгоритм движения вражеских танков
        {
            if (((ComputerTank)all[1]).isStoped && !((ComputerTank)all[1]).isDead)
                ((ComputerTank)all[1]).Movement(pictureBox52, this.Controls);
            else if (!all[1].isDead)
                ((ComputerTank)all[1]).Movement(pictureBox52);

            if (((ComputerTank)all[2]).isStoped && !((ComputerTank)all[2]).isDead)
                ((ComputerTank)all[2]).Movement(pictureBox53, this.Controls);
            else if (!all[2].isDead)
                ((ComputerTank)all[2]).Movement(pictureBox53);

            if (((ComputerTank)all[3]).isStoped && !((ComputerTank)all[3]).isDead)
                ((ComputerTank)all[3]).Movement(pictureBox54, this.Controls);
            else if (!all[3].isDead)
                ((ComputerTank)all[3]).Movement(pictureBox54);
        }
        private void update(object sender, EventArgs e) // обновление формы (анимация объектов)
        {

            Invalidate();
            if (Base.isDestroyed == true || player.isDead) // проверяем, можно ли продолжать игру
            {
                Start();
                return;
            }
            player.Mode = animationMode;
            if (isPressedKey && player.isValidMove(player.coordinates)) // фиксируем нажатую пользователем кнопку для движения танка
            {
                switch (animationMode)
                {
                    case 1:
                        if (player.isNoBarrier(all))
                            pictureBox51.Location = new Point(pictureBox51.Location.X + player.Speed, pictureBox51.Location.Y);
                        break;
                    case 2:
                        if (player.isNoBarrier(all))
                            pictureBox51.Location = new Point(pictureBox51.Location.X - player.Speed, pictureBox51.Location.Y);
                        break;
                    case 3:
                        if (player.isNoBarrier(all))
                            pictureBox51.Location = new Point(pictureBox51.Location.X, pictureBox51.Location.Y - player.Speed);
                        break;
                    case 4:
                        if (player.isNoBarrier(all))
                            pictureBox51.Location = new Point(pictureBox51.Location.X, pictureBox51.Location.Y + player.Speed);
                        break;
                }
            }
            for (int i = 0; i < tanks.Count; i++) // проверка столкновений танков с внутриигровыми объектами
            {
                int mode = 0;
                if (tanks[i] == pictureBox52)
                    mode = all[1].Mode;
                else if (tanks[i] == pictureBox53)
                    mode = all[2].Mode;
                else if (tanks[i] == pictureBox54)
                    mode = all[3].Mode;
                else
                    mode = player.Mode;
                foreach (PictureBox br in bricks)
                {
                    if (br.Bounds.IntersectsWith(tanks[i].Bounds))
                    {
                        if (mode == 1)
                        {
                            tanks[i].Location = new Point(tanks[i].Location.X - 2, tanks[i].Location.Y);
                            if (all[i] is ComputerTank)
                                ((ComputerTank)all[i]).isStoped = true;
                            break;
                        }
                        if (mode == 2)
                        {
                            tanks[i].Location = new Point(tanks[i].Location.X + 2, tanks[i].Location.Y);
                            if (all[i] is ComputerTank)
                                ((ComputerTank)all[i]).isStoped = true;
                            break;
                        }
                        if (mode == 3)
                        {
                            tanks[i].Location = new Point(tanks[i].Location.X, tanks[i].Location.Y + 2);
                            if (all[i] is ComputerTank)
                                ((ComputerTank)all[i]).isStoped = true;
                            break;
                        }
                        if (mode == 4)
                        {
                            tanks[i].Location = new Point(tanks[i].Location.X, tanks[i].Location.Y - 2);
                            if (all[i] is ComputerTank)
                                ((ComputerTank)all[i]).isStoped = true;
                            break;
                        }
                    }
                }
                foreach (PictureBox wl in walls)
                {
                    if (wl.Bounds.IntersectsWith(tanks[i].Bounds))
                    {
                        if (mode == 1)
                        {
                            tanks[i].Location = new Point(tanks[i].Location.X - 2, tanks[i].Location.Y);
                            if (all[i] is ComputerTank)
                                ((ComputerTank)all[i]).isStoped = true;
                            break;
                        }
                        if (mode == 2)
                        {
                            tanks[i].Location = new Point(tanks[i].Location.X + 2, tanks[i].Location.Y);
                            if (all[i] is ComputerTank)
                                ((ComputerTank)all[i]).isStoped = true;
                            break;
                        }
                        if (mode == 3)
                        {
                            tanks[i].Location = new Point(tanks[i].Location.X, tanks[i].Location.Y + 2);
                            if (all[i] is ComputerTank)
                                ((ComputerTank)all[i]).isStoped = true;
                            break;
                        }
                        if (mode == 4)
                        {
                            tanks[i].Location = new Point(tanks[i].Location.X, tanks[i].Location.Y - 2);
                            if (all[i] is ComputerTank)
                                ((ComputerTank)all[i]).isStoped = true;
                            break;
                        }
                    }
                }
            }
            foreach (Tank t in all) // если пришло время для возрождения, алгоритм находит нужное место для появления танка
            {
                if (t is ComputerTank && t.isDead)
                    if (((ComputerTank)t).restoreTime > 399)
                        ((ComputerTank)t).restoreCoord = GetEmptySpace(tanks);
            }

            for (int i = 0; i < tanks.Count; i++) // регистрация столкновения между танками
            {
                Rectangle comparison = new Rectangle();
                for (int j = 0; j < tanks.Count; j++)
                {
                    if (all[j].isDead || all[i].isDead)
                        break;
                    if (all[j] is ComputerTank)
                        comparison = all[j].NextRadius;
                    else
                        comparison = all[j].Radius;
                    if (tanks[i] == tanks[j])
                        continue;
                    else if (!all[i].NextRadius.IntersectsWith(comparison))
                        continue;
                    else if (all[i] is ComputerTank)
                    {
                        ((ComputerTank)all[i]).isStoped = true;
                        if (all[j] is ComputerTank)
                        {
                            ((ComputerTank)all[j]).isStoped = true;
                        }
                    }
                    else
                    {
                        ((ComputerTank)all[j]).isStoped = true;
                    }
                    break;
                }
            }
            foreach (Tank item in all) // регистрация попадания снаряда 
            {
                if (item.bullet.coordinates != null && item.bullet.coordinates.Item1 > 0)
                {
                    if (item is PlayerTank)
                    {
                        item.bullet.DrawItem(gr, pictureBox51);
                        item.bullet.Movement();
                    }
                    foreach (PictureBox brick in bricks)
                    {
                        if (brick.Bounds.IntersectsWith(new Rectangle(new Point(item.bullet.coordinates.Item1, item.bullet.coordinates.Item2), new Size(23, 23))))
                        {
                            item.bullet.Destroy();
                            brick.Visible = false;
                            bricks.Remove(brick);
                            break;
                        }
                    }
                    foreach (PictureBox wall in walls)
                    {
                        if (wall.Bounds.IntersectsWith(new Rectangle(new Point(item.bullet.coordinates.Item1, item.bullet.coordinates.Item2), new Size(23, 23))))
                        {
                            item.bullet.Destroy();
                        }
                    }
                    foreach (PictureBox b in bases)
                    {
                        if (b.Bounds.IntersectsWith(new Rectangle(new Point(item.bullet.coordinates.Item1, item.bullet.coordinates.Item2), new Size(23, 23))))
                        {
                            item.bullet.Destroy();
                            b.Visible = false;
                            bases.Remove(b);
                            Base.isDestroyed = true;
                            break;
                        }
                    }
                    foreach (PictureBox en in tanks)
                    {
                        if (en.Bounds.IntersectsWith(new Rectangle(new Point(item.bullet.coordinates.Item1, item.bullet.coordinates.Item2), new Size(23, 23))))
                        {
                            if ((en == pictureBox52 || en == pictureBox53 || en == pictureBox54) && item is ComputerTank)
                            {
                                item.bullet.Destroy();
                                break;
                            }
                            item.bullet.Destroy();
                            en.Location = new Point(-60, -60);
                            break;
                        }
                    }
                    if (!item.bullet.isVisible(item.bullet.coordinates))
                        item.bullet.Destroy();
                }
            }
            EnemyTankAnimation();
            EnemyTankMovement();
            PlayerTankAnimation();
        }

        private void keyboard(object sender, KeyEventArgs e) // логика для клавиш
        {
            switch (e.KeyCode.ToString())
            {
                case "D":
                    animationMode = 1;
                    isPressedKey = true;
                    break;
                case "A":
                    animationMode = 2;
                    isPressedKey = true;
                    break;
                case "W":
                    animationMode = 3;
                    isPressedKey = true;
                    break;
                case "S":
                    animationMode = 4;
                    isPressedKey = true;
                    break;
                case "F":
                    if (player.bullet.coordinates.Item1 < 0)
                        player.bullet.Spawn(player.GetDirection(), player.GetFirePosition(pictureBox51.Location, pictureBox51.Width));
                    break;
            }
        }
        public void EnemyTankAnimation() // отрисовка вражеских танков
        {
            ((ComputerTank)all[1]).DrawItem(gr, pictureBox52);
            ((ComputerTank)all[2]).DrawItem(gr, pictureBox53);
            ((ComputerTank)all[3]).DrawItem(gr, pictureBox54);
        }
        public void PlayerTankAnimation() // отрисовка игрока
        {
            player.DrawItem(gr, pictureBox51);
        }

        public void DrawMap() // отрисовка карты
        {
            brick = new Bricks();
            wall = new BlockWall();
            foreach (var item in this.Controls)
                if (item is PictureBox)
                {
                    if (!((PictureBox)item).Visible)
                        ((PictureBox)item).Visible = true;
                    if (((PictureBox)item).Tag.Equals("map"))
                    {
                        brick.DrawItem(gr, ((PictureBox)item));
                        bricks.Add((PictureBox)item);
                    }
                    if (((PictureBox)item).Tag.Equals("wall"))
                    {
                        wall.DrawItem(gr, ((PictureBox)item));
                        walls.Add((PictureBox)item);
                    }
                    if (((PictureBox)item).Tag.Equals("base"))
                    {
                        Base.DrawItem(gr, ((PictureBox)item));
                        bases.Add((PictureBox)item);
                    }
                }
        }
    }
}
