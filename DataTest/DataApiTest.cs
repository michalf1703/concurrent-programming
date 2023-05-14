using NUnit.Framework;
using Data;
using System.Collections.Generic;
using System;

namespace DataTest
{
    internal class DataApiTest
    {

        internal class FakeDataAPI : DataAbstractAPI
        {
            int ballRadius = 10;
            private int sceneHeight;
            private int sceneWidth;
            private bool isRunning;
            List<IBall> _ballList = new List<IBall>();

            public override void CreateBall(Point startPosistion)
            {
                Random random = new Random();
                int x = random.Next(ballRadius, this.GetSceneWidth() - ballRadius);
                int y = random.Next(ballRadius, this.GetSceneHeight() - ballRadius);

                _ballList.Add(IBall.CreateBall(x, y));
                do
                {
                    _ballList.Last().XMovement = random.Next(-10000, 10000) % 3;
                    _ballList.Last().YMovement = random.Next(-10000, 10000) % 3;
                } while (_ballList.Last().XMovement == 0 || _ballList.Last().YMovement == 0);

            }

            public override void CreateScene(int height, int width)
            {
                sceneHeight = height;
                sceneWidth = width;
            }

            public override List<IBall> GetAllBalls()
            {
                return _ballList;
            }

            public override int GetSceneHeight()
            {
                return sceneHeight;
            }

            public override int GetSceneWidth()
            {
                return sceneWidth;
            }

            public override bool IsRunning()
            {
                return isRunning;
            }

            public override void TurnOff()
            {
                isRunning = false;
            }

            public override void TurnOn()
            {
                isRunning = true;
            }



            [TestMethod]
            public void logicAPITurnOnTurnOffTest()
            {
                FakeDataAPI fakeDataAPI = new FakeDataAPI();
                AbstractLogicAPI logicAPI = AbstractLogicAPI.CreateAPI(fakeDataAPI);
                logicAPI.CreateField(400, 400);
                Assert.AreEqual(false, logicAPI.IsRunning());

                logicAPI.TurnOn();
                Assert.AreEqual(true, logicAPI.IsRunning());

                logicAPI.TurnOff();
                Assert.AreEqual(false, logicAPI.IsRunning());
            }

            [TestMethod]
            public void logicAPICreateBallsTest()
            {
                FakeDataAPI fakeDataAPI = new FakeDataAPI();
                AbstractLogicAPI logicAPI = AbstractLogicAPI.CreateAPI(fakeDataAPI);
                logicAPI.CreateField(400, 400);
                logicAPI.CreateBalls(10, 10);
                Assert.IsTrue(10 == logicAPI.GetAllBalls().Count);
            }
        }
        }
    }
}