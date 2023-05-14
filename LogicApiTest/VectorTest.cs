using Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicTest
{
    internal class VectorTest
    {
        private Vector2 testVector;
        private Vector2 testVector1;
        private Vector2 zeroVector;
        private Vector2 testSameVector;

        [SetUp]
        public void Setup()
        {
            testVector = new Vector2(11, 1);
            testVector1 = new Vector2(11, 14);
            zeroVector = new Vector2(0, 0);
            testSameVector = new Vector2(11, 1);

        }

        [Test]
        public void DistanceTest()
        {
            Assert.AreEqual(Vector2.Distance(testVector, testVector1), 13);
        }
        [Test]
        public void DistanceSquaredTest()
        {
            Assert.AreEqual(Vector2.DistanceSquared(testVector, testVector1), 169);
        }
        [Test]

        public void IsZeroTest()
        {
            Assert.IsTrue(zeroVector.IsZero());
            Assert.IsFalse(testVector.IsZero());
        }
        [Test]

        public void ToStringTest()
        {


            Assert.AreEqual(testVector.ToString(), "[11, 1]") ;
        }
        [Test]

        public void EqualsTest()
        {
            Assert.IsTrue(testVector.Equals(testSameVector));
            Assert.IsTrue(testVector.Equals(testVector));
            Assert.IsFalse(testVector.Equals(testVector1));
        }
        [Test]
        public void GetHashCodeTest()
        {


            Assert.AreEqual(testVector.GetHashCode(), -2000565629);
            Assert.AreEqual(testVector1.GetHashCode(), -1969108349);
            Assert.AreEqual(zeroVector.GetHashCode(), -393098621);
            Assert.AreEqual(testSameVector.GetHashCode(), -2000565629);
        }
 
    }
}
