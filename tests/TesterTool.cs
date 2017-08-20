using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Log;

namespace Tests
{
    public class ConsoleOutput : IDisposable
    {
       private StringWriter stringWriter;
       private TextWriter originalOutput;

       public ConsoleOutput()
       {
          stringWriter = new StringWriter();
          originalOutput = Console.Out;
          Console.SetOut(stringWriter);
       }

       public string GetOuput()
       {
          return stringWriter.ToString();
       }

       public void Dispose()
       {
          Console.SetOut(originalOutput);
          stringWriter.Dispose();
       }
   }

   public class TestBase
   {
      protected Logger logger;

      // TestParamSerialize : It is not allowed, that we set null Serializer,
      //     and if we do, it shoudl to throw correct exception.
      public void TestParamSerialize(Logger logger)
      {
          try
          {
             logger.Serializer = null;
             Assert.IsTrue(false);
          }
          catch (System.ArgumentNullException) { Assert.IsTrue(true); }
          catch (AssertFailedException e) { throw e; }
          catch (Exception e) { Console.WriteLine(e.ToString()); Assert.IsTrue(false); }
      }

      // TestParamMaxLineSize : Max line size should be more like the generic
      //    log header -->#{date} [#{info}] <-- . It should be throw a correct
      //    exception if the new value is lower like this size.
      public void TestParamMaxLineSize(Logger logger)
      {
         try
         {
            logger.MaxLineSize = 0;
            Assert.IsTrue(false);
         }
         catch (System.ArgumentOutOfRangeException) { Assert.IsTrue(true); }
         catch (AssertFailedException e) { throw e; }
         catch (Exception e) { Console.WriteLine(e.ToString()); Assert.IsTrue(false); }
      }

      // TestLongText : It has a message size controll, so the incoming message
      //    is more like allowed, it should throw a right exception.
      [TestMethod]
      public void TestLongText(Logger logger)
      {
          string text = "";
          for (int i= 0; i < logger.MaxLineSize + 1; i++)
             text+= "a";

          try
          {
             logger.write(Eloquences.eloInfo, text);
             Assert.IsTrue(false);
          }
          catch (System.ArgumentOutOfRangeException) { Assert.IsTrue(true); }
          catch (AssertFailedException e) { throw e; }
          catch (Exception e) { Console.WriteLine(e.ToString()); Assert.IsTrue(false); }
      }

   }
}

