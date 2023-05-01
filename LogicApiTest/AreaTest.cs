using NUnit.Framework;
using Logic;

namespace LogicTest
{
    internal class AreaTest
    {
        LogicAbstractApi test = LogicAbstractApi.CreateApi();
        [SetUp]
        public void Setup()
        {
            Area area = new Area(700);        
            area.addBalls(1);
            Assert.AreEqual(area.balls.Count, 1);
            Assert.NotNull(test);
            test.createBalls(3);
            Assert.AreEqual(test.GetBalls().Count, 3);
        }

        [Test]
        public void CreateBallsTest()
        {
            Area region = new Area(800);
            region.addBalls(3);

            double pX1 = region.balls[0].x;
            double pY1 = region.balls[0].y;

            double pX2 = region.balls[1].x;
            double pY2 = region.balls[1].y;

            region.MoveBall();

            Assert.AreNotEqual(region.balls[0].x, pX1);
            Assert.AreNotEqual(region.balls[0].y, pY1);
            Assert.AreNotEqual(region.balls[1].x, pX2);
            Assert.AreNotEqual(region.balls[1].y, pY2);
        }

       
    }
}