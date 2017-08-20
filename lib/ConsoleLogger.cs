using System;

namespace Log
{
   public class ConsoleLogger : LoggerBase
   {
      protected override void internal_write(Eloquences elo, string message)
      {
         System.Console.ForegroundColor = Global.EData[elo].Color;
         System.Console.WriteLine(Serializer.serialize(elo, message, System.DateTime.Now));
         System.Console.ResetColor();
      }

      public override void Dispose() { }
   }
}
