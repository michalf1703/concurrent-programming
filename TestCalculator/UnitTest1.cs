using program;

namespace TestCalculator
{
    [TestClass]
    public class UnitTest1
    {
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