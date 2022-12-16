using System;
using System.Drawing;
using NUnit.Framework;

namespace Tanks.Tests
{
    [TestFixture]
    public class TankTests
    {
        private Tank tank;
        [SetUp]
        public void Setup()
        {            
            tank = new Tank(); // создаем танк
        }
        [Test]
        public void isValidMoveTest() // тест метода isValidMove
        {
            Tuple<int, int> tankCoordinates = new Tuple<int, int>(1200, -1);
            bool result = false;
            Assert.AreEqual(result, tank.isValidMove(tankCoordinates)); // проверяем результат при неверных координатах
            tankCoordinates = new Tuple<int, int>(1000, 100);
            result = true;
            Assert.AreEqual(result, tank.isValidMove(tankCoordinates)); // проверяем результат при верных координатах
        }

        [Test]
        public void GetDirectionTest() // тест метода GetDirection
        {
            Tuple<int, int> result = new Tuple<int, int>(1,0);
            tank.Mode = 1;
            Assert.AreEqual(result, tank.GetDirection()); // проверяем результат при движении танка влево
            result = new Tuple<int, int>(0, 1);
            tank.Mode = 4;
            Assert.AreEqual(result, tank.GetDirection()); // проверяем результат при движении танка вниз
        }
        [Test]
        public void GetFirePositionTest() // тест метода GetFirePosition
        {
            Point point = new Point(220,30);
            int Width = 60;
            Tuple<int, int> result = new Tuple<int, int>(point.X + Width / 2 + 40, point.Y + Width / 2 - 10);
            tank.Mode = 1;
            Assert.AreEqual(result, tank.GetFirePosition(point, Width)); // проверяем позицию появления снаряда
        }
    }
}