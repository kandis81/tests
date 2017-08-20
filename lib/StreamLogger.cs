using System;
using System.IO;

namespace Log
{
   public class StreamLogger : LoggerBase
   {
      protected StreamWriter Writer { get; set; }

      protected StreamLogger() { }
      public StreamLogger(Stream stream) { Writer = new StreamWriter(stream); }

      protected override void internal_write(Eloquences elo, string message) { writeStream(Serializer.serialize(elo, message, System.DateTime.Now)); }

      protected virtual void writeStream(string text) { Writer.WriteLine(text); Writer.Flush(); }

      public override void Dispose() { if (Writer != null) Writer.Dispose(); }
   }
}
