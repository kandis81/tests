using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Log
{
   // *******************************************
   // Enumerations to identificate log levels
   // type of interfaces
   // *******************************************

   enum Interfaces { ifConsole, ifStream, ifFile };
   enum Eloquences { eloError = 0, eloInfo, eloDebug, eloAll };

   // *******************************************
   // Output interfaces
   // InterfaceBase    : basic for common call in 
   //                    Client class
   // ConsoleInterface : write log to console
   // FileInterface    : write log to file(s)
   // *******************************************

   interface InterfaceBase
   {
      void write(ConsoleColor color, string text);
   };

   class ConsoleInterface : InterfaceBase
   {
      public void write(ConsoleColor color, string text)
      {
         System.Console.ForegroundColor = color;
         System.Console.WriteLine(text);
         System.Console.ResetColor();
      }
   };

   class StreamInterface : InterfaceBase
   {
      protected StreamWriter sw;

      public StreamInterface() { sw = null; }
      public void inititalize(Stream stream) { sw = new StreamWriter(stream); }
      public void write(ConsoleColor color, string text) { writeStream(color, text); }

      public void writeStream(ConsoleColor color, string text)
      {
         if (sw == null)
            throw new Exception("Stream is not initialized - Ignored message: " + text);

         sw.WriteLine(text);
      }
   };

   class FileInterface : StreamInterface
   {
      private int Counter;
      private string Path;
      private long FileLength;
      private long MaxFileSize;

      public FileInterface()
      {
         Counter= 0;
         Path = null;
         FileLength = 0;
         MaxFileSize = 5 * 1024;
         sw = null;
      }

      private void openFile()
      {
         string filename = Path + "/app.log";
         FileLength= 0;

         if (File.Exists(filename))
         {
            sw = File.AppendText(filename);
            FileLength= new System.IO.FileInfo(filename).Length;
         }
         else
            sw = File.CreateText(filename);
      }

      public void initialize(string path, long maxFileSize)
      {
         Regex apprx = new Regex("app\\.([0-9]*)\\.log");

         // Check is it a directory
         if (Directory.Exists(path) == false)
            throw new Exception(path + " is not available. Please create it before");

         Path = path;
         MaxFileSize = maxFileSize;

         string[] files = Directory.GetFiles(path, "app.*.log");

         // try to find the last counter
         foreach (string file in files)
         {
            Match matches = apprx.Match(file);

            if (matches.Success == false)
               continue;

            if (Counter < Int32.Parse(matches.Groups[1].Value))
               continue;

            Counter = Int32.Parse(matches.Groups[1].Value);
         }

         openFile();
      }

      public void write(ConsoleColor color, string text)
      {
         // Note: the -1 & +1 things are talking about the size of termination byte
         if (FileLength > 0 && text.Length + FileLength + 1 > MaxFileSize)
         {
            sw.Flush();
// Notice : is it possible, that the compiler says, it is not available???            sw.Close();
            File.Move(Path + "/app.log", Path + "/app." + Counter.ToString() + ".log");
            openFile();
         }

         if (text.Length + 1 > MaxFileSize)
         {
            writeStream(color, text.Substring(0, (int) MaxFileSize -1));
            FileLength= MaxFileSize;
            write(color, text.Substring((int) MaxFileSize - 1));
            return ;
         }

         writeStream(color, text);
         FileLength += text.Length + 1;
      }
   };

   // *******************************************
   // This client is the interface for logging
   // *******************************************

   class Client
   {
      // *** Configuration Begin ****************

      // private configuration
      private int MaxLineLength;
      private Interfaces iface;
      private Eloquences logLevel;
      private string filePath;
      private InterfaceBase device;

      // Constructor to set default configuration
      public Client()
      {
         MaxLineLength = 1000;
         iface         = Interfaces.ifConsole;
         logLevel      = Eloquences.eloAll;
         filePath      = null;
         device        = new ConsoleInterface();
      }

      public void setMaxLineLength(int l) { this.MaxLineLength= l; }
      public void setTypeOfInterface(Interfaces i) { this.iface= i; }
      public void setLogLevel(Eloquences l) { this.logLevel= l; }
      public void setFilePath(string p) { this.filePath= p; }

      // *** Configuration End ******************

      // *** Log Level Begin ********************

      // Configuration for all log level
      private struct EloData
      {
         private readonly string prefix;
         private readonly ConsoleColor color;

         public EloData(string p, ConsoleColor c) { prefix= p; color= c; }

         public string Prefix { get { return prefix; } }
         public ConsoleColor Color { get { return color; } }
      };

      static private readonly EloData[] edata = new EloData[] {
         new EloData ("ERROR", ConsoleColor.Red),
         new EloData ("INFO", ConsoleColor.Gray),
         new EloData ("DEBUG", ConsoleColor.Green)
      };

      // *** Log Level End **********************


      // *** Logging Begin **********************

      public void write(Eloquences elo, string msg)
      {
         if (elo >= this.logLevel)
            return ;

         System.DateTime curDt = System.DateTime.Now;
         System.Globalization.CultureInfo cult = new System.Globalization.CultureInfo("hu-HU");

         string logText = curDt.ToString(cult) + " " + edata[(int) elo].Prefix + " : " + msg;

         if (device == null)
            device = new ConsoleInterface();

         device.write(edata[(int) elo].Color, logText);

//         switch (iface)
//         {
//            case Interfaces.ifStream:
//               writeToConsole(edata[(int) elo].Color, "--Stream-- " + logText);
//               break;
//
//            case Interfaces.ifFile:
//               writeToConsole(edata[(int) elo].Color, "--Stream-- " + logText);
//               break;
//
//            case Interfaces.ifConsole:
//            default:
//               writeToConsole(edata[(int) elo].Color, logText);
//               break;
//         }

         if (logText.Length > this.MaxLineLength)
            // TODO: not the right exception :(
            throw new System.ArgumentOutOfRangeException(logText, "Text of log is longer like " + this.MaxLineLength.ToString() + " characters!");
      }

//      private void writeToConsole(ConsoleColor color, string text)
//      {
//         System.Console.ForegroundColor = color;
//         System.Console.WriteLine(text);
//         System.Console.ResetColor();
//      }

      // *** Logging End ************************
   }
}
