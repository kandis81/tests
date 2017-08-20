using System;

namespace Log
{
   public abstract class LoggerBase : Logger
   {
      private MessageSerializer serializer = new DefaultMessageSerializer();
      private long maxlinesize = 1000;
      private Eloquences maxloglevel = Eloquences.eloAll;

      // public interfaces to members
      public virtual MessageSerializer Serializer
      {
         get { return serializer; }
         set { if (value == null) throw new System.ArgumentNullException("Serializer", "Set of null serializer is not allowed"); serializer= value; }
      }

      public long MaxLineSize
      {
         get { return maxlinesize; }
         set { if (value < 1) throw new System.ArgumentOutOfRangeException("MaxLineSize", "Max line size should be more like 0"); maxlinesize= value; }
      }

      public Eloquences MaxLogLevel { get { return maxloglevel; } set { maxloglevel= value; } }

      // public functionality
      public void write(Eloquences elo, string message)
      {
         if (MaxLogLevel < elo)
            return ; // No error, just ignore logging

         if (message.Length > MaxLineSize)
            throw new System.ArgumentOutOfRangeException("message", $"Message should be less than {MaxLineSize}");

         internal_write(elo, message);
      }

      // protected interface, what we should to implement
      protected abstract void internal_write(Eloquences elo, string message);

      // Destructor
      public abstract void Dispose();
   }
}
