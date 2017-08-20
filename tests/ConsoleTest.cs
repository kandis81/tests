using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Log;

namespace Tests
{
    [TestClass]
    public class ConsoleTest : TestBase
    {
        [TestMethod]
        public void TestParamSerialize() { base.TestParamSerialize(new Log.ConsoleLogger()); }

        [TestMethod]
        public void TestParamMaxLineSize() { base.TestParamMaxLineSize(new Log.ConsoleLogger()); }

        [TestMethod]
        public void TestLongText() { base.TestParamMaxLineSize(new Log.ConsoleLogger()); }

        // TestLogWrites: Write log messages without any problem. Unfortunatelly
        //    I have no clue yet, how can I check the output & color of output.
        [TestMethod]
        public void TestLogWrites()
        {
            Log.Logger logger = new Log.ConsoleLogger();

            try
            {
               logger.write(Eloquences.eloInfo, "Info message should be green");
               logger.write(Eloquences.eloError, "Error message should be red");
               logger.write(Eloquences.eloDebug, "Debug message should be gray");

               Assert.IsTrue(true);
            }
            catch (AssertFailedException e) { throw e; }
            catch (Exception e) { Console.WriteLine(e.ToString()); Assert.IsTrue(false); }
        }
    }
}
