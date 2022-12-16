using System;
using System.Drawing;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tanks.Tests
{
    [TestFixture]
    public class BulletTests
    {
        private Bullet bul;
        [SetUp]
        public void Setup()
        {
            bul = new Bullet();
        }
        [Test]
        public void isVisibleTest() // тест метода isVisible
        {
            bool result = false;
            Assert.AreEqual(result, bul.isVisible(bul.coordinates)); // проверка результата, при отрицательных координатах снаряда
            result = true;
            bul.coordinates = new Tuple<int, int>(250, 100);
            Assert.AreEqual(result, bul.isVisible(bul.coordinates));
        }
    }
}