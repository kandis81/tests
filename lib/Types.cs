using System;
using System.Collections.Generic;

namespace Log
{
   public enum Eloquences { eloError = 0, eloInfo, eloDebug, eloAll };

   class Global
   {
      public class EloData
      {
         public string Prefix { get; set; }
         public ConsoleColor Color { get; set; }

         public EloData(string p, ConsoleColor c) { Prefix= p; Color= c; }
      };

      private static readonly Dictionary<Eloquences, EloData> edata = new Dictionary<Eloquences, EloData>()
      {
         { Eloquences.eloError, new EloData ("ERROR", ConsoleColor.Red)   },
         { Eloquences.eloInfo,  new EloData ("INFO",  ConsoleColor.Green) },
         { Eloquences.eloDebug, new EloData ("DEBUG", ConsoleColor.Gray)  }
      };

      public static Dictionary<Eloquences, EloData> EData { get { return edata; } }
   };

   public interface MessageSerializer
   {
      string serialize(Eloquences elo, string message, System.DateTime stamp);
   }

   public interface Logger : IDisposable
   {
      // public interfaces to members
      MessageSerializer Serializer { get; set; }
      long MaxLineSize { get; set; }
      Eloquences MaxLogLevel { get; set; }

      // public functionality
      void write(Eloquences elo, string message);
   }
}
