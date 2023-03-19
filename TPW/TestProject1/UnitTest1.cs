using program;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Calculator a = new Calculator();
            int y = a.add(2, 2);
            Assert.AreEqual(y, 4);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Calculator b = new Calculator();
            int y = b.subtract(2, 2);
            Assert.AreEqual(y, 0);
        }

        [TestMethod]
        public void TestMultiplyMethod()
        {
            Calculator calc = new Calculator();
            int test = calc.multiply(3, 3);
            Assert.AreEqual(test, 9);
        }

        [TestMethod]
        public void TestDivideMethod()
        {
            Calculator calc = new Calculator();
            int test = calc.divide(6, 3);
            Assert.AreEqual(test, 2);

        }
    }
}