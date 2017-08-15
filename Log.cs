using System;
//using System.Collections;

namespace Log
{
   class Client
   {

      // *** Log Level Begin ********************

      // Available log levels

      public enum Eloquences
      {
         eloInfo  = 0,  // 0
         eloDebug,      // 1
         eloError,      // 2

         eloCnt
      };

      // Configuration for all log level

      private struct EloData
      {
         private readonly string prefix;
         private readonly ConsoleColor color;

         public EloData(string p, ConsoleColor c) { prefix= p; color= c; }

         public string Prefix { get { return prefix; } }
         public ConsoleColor Color { get { return color; } }
      };

      static private readonly EloData[] edata = new EloData[(int)Eloquences.eloCnt] {
         new EloData ("INFO", ConsoleColor.Gray),
         new EloData ("DEBUG", ConsoleColor.Green),
         new EloData ("ERROR", ConsoleColor.Red)
      };

      // *** Log Level End **********************

      // *** Log Configuration Begin ************

      public struct Config
      {
         private readonly int MAXLEN;

         public Config(int max)
         {
            MAXLEN = max;
         }

         public int MaxLength { get { return MAXLEN; } }
      };

      static private Config cfg = new Config(1000);

      // *** Log Configuration End **************


      public void write(Eloquences elo, string msg)
      {
         System.DateTime curDt = System.DateTime.Now;
         System.Globalization.CultureInfo cult = new System.Globalization.CultureInfo("hu-HU");

         string logText = curDt.ToString(cult) + " " + edata[(int) elo].Prefix + " : " + msg;

         writeToConsole(edata[(int) elo].Color, logText);

         if (logText.Length > cfg.MaxLength)
            throw new System.ArgumentOutOfRangeException(logText, "Text of log is longer like " + cfg.MaxLength.ToString() + " characters!");
      }

      private void writeToConsole(ConsoleColor color, string text)
      {
         System.Console.ForegroundColor = color;
         System.Console.WriteLine(text);
         System.Console.ResetColor();
      }
   }
}
