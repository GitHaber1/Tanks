using System;
using System.Drawing;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tanks.Tests
{
    [TestFixture]
    public class PlayerTankTests
    {
        private List<Tank> tanks;
        private PlayerTank tank;
        [SetUp]
        public void Setup()
        {
            tanks = new List<Tank>(); // создаем список танков
            tank = new PlayerTank(); // создаем танк
            tanks.Add(tank);
            tank = new PlayerTank();
            tanks.Add(tank);
        }
        [Test]
        public void isNoBarrierTest() // тест метода isNoBarrier
        {
            tanks[0].NextRadius = new Rectangle(100, 100, 60, 60);
            tanks[1].Radius = new Rectangle(100, 100, 60, 60);            
            bool result = false;
            Assert.AreEqual(result, tank.isNoBarrier(tanks)); // проверяет результат, если танки сталкиваются
            result = true;
            tanks[1].Radius = new Rectangle(20, 30, 60, 60);
            Assert.AreEqual(result, tank.isNoBarrier(tanks)); // проверяет результат, если танки не сталкиваются
        }
    }
}