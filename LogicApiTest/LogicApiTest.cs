using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using Data;
/*
namespace LogicTest
{
    public class LogicAPITest
    {
        internal class FalseDataApi : DataAbstractAPI
        {
            public override void createBalls(int ballsAmount)
            {
                // Do nothing
            }

            public override double getBallMass(int ballId)
            {
                return 1.0;
            }

            public override double getBallPositionX(int ballId)
            {
                return 0.0;
            }

            public override double getBallPositionY(int ballId)
            {
                return 0.0;
            }

            public override int getBallRadius(int ballId)
            {
                return 15;
            }

            public override int getBallsAmount()
            {
                return 0;
            }

            public override double getBallSpeedX(int ballId)
            {
                return 0.0;
            }

            public override double getBallSpeedY(int ballId)
            {
                return 0.0;
            }

            public override int getBoardSize()
            {
                return 100;
            }

            public override void OnCompleted()
            {
                // Do nothing
            }

            public override void OnError(Exception error)
            {
                // Do nothing
            }

            public override void OnNext(int value)
            {
                // Do nothing
            }

            public override void setBallSpeed(int ballId, double speedX, double speedY)
            {
                // Do nothing
            }

            public override IDisposable Subscribe(IObserver<int> observer)
            {
                // Do nothing
                return null;
            }
        }

        private LogicAPI testLogicAPI;

        [SetUp]
        public void Setup()
        {
            FalseDataApi falseDataApi = new FalseDataApi();
            testLogicAPI = LogicAPI.CreateLayer(falseDataApi);
        }

        [Test]
        public void getBallRadiusTest()
        {
            testLogicAPI.AddBallsAndStart(1);
            Assert.AreEqual(testLogicAPI.getBallRadius(1), 15);
        }
    }
}*/