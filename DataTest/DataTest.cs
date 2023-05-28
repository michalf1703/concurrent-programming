using Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTest
{
    public class DataTest
    {
        [Test]
        public void CreateBallsTest()
        {
            int number = 5;

            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataApi();
            DataAPI.createBalls(number);
            Assert.AreEqual(DataAPI.getBallsAmount(), 5);

        }


        [Test]
        public void GetBoardSizeTest()
        {

            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataApi();
            DataAPI.getBoardSize();
            Assert.AreEqual(DataAPI.getBoardSize(), 515);
        }

        [Test]
        public void GetBall() {
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataApi();
            DataAPI.createBalls(5);
            Assert.AreEqual(DataAPI.getBallsAmount(), 5);

        }
    }
}
