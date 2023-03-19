namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            program.Calculator a = new program.Calculator();
            int y = a.add(2, 2);
            Assert.AreEqual(y, 4);
        }

        [TestMethod]
        public void TestMethod2()
        {
            program.Calculator b = new program.Calculator();
            int y = b.subtract(2, 2);
            Assert.AreEqual(y, 0);
        }
    }
}