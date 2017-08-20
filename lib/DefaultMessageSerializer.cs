using System;

namespace Log
{
   public class DefaultMessageSerializer : MessageSerializer
   {
      public string serialize(Eloquences elo, string message, System.DateTime stamp)
      {
         return $"{stamp},{stamp.Millisecond} [{Global.EData[elo].Prefix}] {message}";
      }
   }
}

