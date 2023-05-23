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

         

            public override int getBallsAmount()
            {
                return 0;
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


            public override void OnNext(IBall Ball)
            {
                throw new NotImplementedException();
            }



            public override IDisposable Subscribe(IObserver<IBall> observer)
            {
                throw new NotImplementedException();
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
            testLogicAPI.
            Assert.AreEqual(testLogicAPI.getBallRadius(1), 15);
        }
    }
}*/