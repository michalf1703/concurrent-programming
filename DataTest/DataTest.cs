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
           
            Assert.AreNotEqual(DataAPI.getBallsAmount, 5);
        }


        [Test]
        public void GetBoardSizeTest()
        {

            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataApi();
            DataAPI.getBoardSize();
            Assert.AreEqual(DataAPI.getBoardSize(), 515);
        }
    }
}
