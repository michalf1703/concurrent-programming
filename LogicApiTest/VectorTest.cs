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
        private Vector2 divideVector;


        [SetUp]
        public void Setup()
        {
            testVector = new Vector2(11, 1);
            testVector1 = new Vector2(11, 14);
            zeroVector = new Vector2(0, 0);
            testSameVector = new Vector2(11, 1);
            divideVector = new Vector2(10, 6);

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
            Assert.IsFalse(testVector1.IsZero());
            Assert.IsFalse(testSameVector.IsZero());

        }
        [Test]

        public void ToStringTest()
        {
            Assert.AreEqual(testVector.ToString(), "[11, 1]") ;
            Assert.AreEqual(testVector1.ToString(), "[11, 14]");
            Assert.AreEqual(zeroVector.ToString(), "[0, 0]");
            Assert.AreEqual(testSameVector.ToString(), "[11, 1]");
            Assert.AreEqual(divideVector.ToString(), "[10, 6]");
        }
        [Test]

        public void EqualsTest()
        {
            Assert.IsTrue(testVector.Equals(testSameVector));
            Assert.AreEqual(testVector.Equals(testVector), testVector.Equals(testSameVector));
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
        [Test]
        public void OperatorsTest()
        {


            Assert.AreEqual(testVector - zeroVector, testVector);
            Assert.AreEqual(testVector + zeroVector, testVector);
            Assert.AreEqual(zeroVector / testVector, zeroVector);
            Assert.AreEqual(testVector * testSameVector, new Vector2(121, 1));
            Assert.AreEqual(-testVector, -testSameVector);
            Assert.AreEqual(testVector * 5, new Vector2(55, 5));
            Assert.AreEqual(divideVector / 2, new Vector2(5, 3));
            Assert.IsTrue(testVector == testSameVector);
            Assert.IsFalse(testVector != testSameVector);
            Assert.IsTrue(divideVector != testVector1);

        }

    }
}
