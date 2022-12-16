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
            tank = new Tank(); // ������� ����
        }
        [Test]
        public void isValidMoveTest() // ���� ������ isValidMove
        {
            Tuple<int, int> tankCoordinates = new Tuple<int, int>(1200, -1);
            bool result = false;
            Assert.AreEqual(result, tank.isValidMove(tankCoordinates)); // ��������� ��������� ��� �������� �����������
            tankCoordinates = new Tuple<int, int>(1000, 100);
            result = true;
            Assert.AreEqual(result, tank.isValidMove(tankCoordinates)); // ��������� ��������� ��� ������ �����������
        }

        [Test]
        public void GetDirectionTest() // ���� ������ GetDirection
        {
            Tuple<int, int> result = new Tuple<int, int>(1,0);
            tank.Mode = 1;
            Assert.AreEqual(result, tank.GetDirection()); // ��������� ��������� ��� �������� ����� �����
            result = new Tuple<int, int>(0, 1);
            tank.Mode = 4;
            Assert.AreEqual(result, tank.GetDirection()); // ��������� ��������� ��� �������� ����� ����
        }
        [Test]
        public void GetFirePositionTest() // ���� ������ GetFirePosition
        {
            Point point = new Point(220,30);
            int Width = 60;
            Tuple<int, int> result = new Tuple<int, int>(point.X + Width / 2 + 40, point.Y + Width / 2 - 10);
            tank.Mode = 1;
            Assert.AreEqual(result, tank.GetFirePosition(point, Width)); // ��������� ������� ��������� �������
        }
    }
}