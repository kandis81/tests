using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Log
{
   public class FileLogger : StreamLogger
   {
      private static readonly string fname = "log";
      private static readonly string extension = "txt";
      private long counter = 0;
      private long maxfilesize = 5 * 1024;
      private long filelength = 0;
      private static readonly int sizeofnewline = 1;

      protected string FilePath { get; set; }
      protected long Counter { get { return counter++; } }
      public long MaxFileSize 
      {
         get { return maxfilesize; }
         set 
         {
            string text= Serializer.serialize(Eloquences.eloError, "0123456789", System.DateTime.Now);

            if (value < 1)
               throw new System.ArgumentOutOfRangeException("MaxFileSize", "Max file size should be more like 0");

            if (value < text.Length)
               throw new System.ArgumentOutOfRangeException("MaxFileSize", $"It should be longer like a minimal message {text.Length}");

            maxfilesize= value;
         }
      }
      public override MessageSerializer Serializer
      {
         get { return base.Serializer; }
         set
         {
            base.Serializer= value;
            string text= Serializer.serialize(Eloquences.eloError, "0123456789", System.DateTime.Now);

            if (text.Length < MaxFileSize)
               throw new System.ArgumentOutOfRangeException("Serializer", $"It should be less than maximal file size {MaxFileSize}");
         }
      }

      public FileLogger(string path) : base()
      {
         Regex apprx = new Regex($"{fname}\\.([0-9]*)\\.{extension}");

         FilePath= path;
         string filename = Path.Combine(FilePath, $"{fname}.{extension}");

         Writer = new StreamWriter(File.Open(filename, FileMode.Append));
         filelength = new System.IO.FileInfo(filename).Length;

         string[] files = Directory.GetFiles(path, $"{fname}.*.{extension}");

         // try to find the last counter
         foreach (string file in files)
         {
            Match matches = apprx.Match(file);

            if (matches.Success == false)
               continue;

            if (counter > Int32.Parse(matches.Groups[1].Value))
               continue;

            counter = Int32.Parse(matches.Groups[1].Value) + 1;
         }
      }

      protected override void internal_write(Eloquences elo, string message)
      {
         string text= Serializer.serialize(elo, message, System.DateTime.Now);
         long sizediff= maxfilesize - filelength - text.Length - sizeofnewline;

         if (sizediff < 0)
         {
            Writer.Dispose();
            File.Move(Path.Combine(FilePath, $"{fname}.{extension}"), Path.Combine(FilePath, $"{fname}.{Counter}.{extension}"));
            Writer = new StreamWriter(File.Open(Path.Combine(FilePath, $"{fname}.{extension}"), FileMode.Append));
            filelength = 0;
         }

         if (text.Length + sizeofnewline > MaxFileSize)
         {
            writeStream(text.Substring(0, (int) MaxFileSize - sizeofnewline));
            filelength = MaxFileSize;
            internal_write(elo, text.Substring((int) MaxFileSize - sizeofnewline));
         }
         else
         {
            writeStream(text);
            filelength += text.Length + sizeofnewline;
         }
      }
   }
}
