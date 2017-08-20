using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Log;

namespace Tests
{
    [TestClass]
    public class FileTest : TestBase
    {
        [TestMethod]
        public void TestParamSerialize()
        {
           using (logger= new Log.FileLogger("./"))
           {
              base.TestParamSerialize(logger);
           }
        }

        [TestMethod]
        public void TestParamMaxLineSize()
        {
           using (logger= new Log.FileLogger("./"))
           {
              base.TestParamMaxLineSize(logger);
           }
        }

        [TestMethod]
        public void TestLongText()
        {
           using (logger= new Log.FileLogger("./"))
           {
              base.TestParamMaxLineSize(logger);
           }
        }

        // TestLogWrites: Write log messages without any problem. Unfortunatelly
        //    I have no clue yet, how can I check the output & color of output.
        [TestMethod]
        public void TestLogWrites()
        {
            Log.Logger logger = new Log.FileLogger("./");

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
