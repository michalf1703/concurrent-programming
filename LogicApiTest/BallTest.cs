using NUnit.Framework;
using Logic;

namespace LogicTest
{
    internal class BallTest
    {
        [SetUp]
        public void Setup()
        {
           
        }

        [Test]
        public void UpdatePositionTest()
        {
            Ball ball = new Ball();
            double positionX = ball.x;
            double positionY = ball.y;
            ball.updatePosition(530);
            Assert.AreEqual(ball.x, positionX + ball.xSpeed);
            Assert.AreEqual(ball.y, positionY + ball.ySpeed);
        }
    }
}
