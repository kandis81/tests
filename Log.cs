using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Log
{
   class Global
   {
      // *******************************************
      // private logger for low level logging
      // Technically for log to console at the
      // messages of logger class
      // *******************************************

      private static Client deepLog = new Client("GLOBAL");

      public static Client Logger { get { return deepLog; } }
   };

   // *******************************************
   // Enumerations to identificate log levels
   // type of interfaces
   // *******************************************

   enum Devices    { devConsole, devStream, devFile };
   enum Eloquences { eloError = 0, eloInfo, eloDebug, eloAll };

   // *******************************************
   // Output Devices
   // Device        : basic for common call in 
   //                    Client class
   // ConsoleDevice : write log to console
   // FileDevice    : write log to file(s)
   // *******************************************

   interface Device
   {
      bool isDevice(Devices i);
      void write(ConsoleColor color, string text);
   };

   class ConsoleDevice : Device
   {
      public bool isDevice(Devices i) { return i == Devices.devConsole; }

      public void write(ConsoleColor color, string text)
      {
         System.Console.ForegroundColor = color;
         System.Console.WriteLine(text);
         System.Console.ResetColor();
      }
   };

   class StreamDevice : Device
   {
      protected StreamWriter sw;

      public StreamDevice() { sw = null; }
      public void initialize(Stream stream) { sw = new StreamWriter(stream); }
      public virtual void write(ConsoleColor color, string text) { writeStream(color, text); }

      public virtual bool isDevice(Devices i) { return i == Devices.devStream; }

      public void writeStream(ConsoleColor color, string text)
      {
         if (sw == null)
            throw new Exception("Stream is not initialized - Ignored message: " + text);

         sw.WriteLine(text);
         sw.Flush();
      }
   };

   class FileDevice : StreamDevice
   {
      private int Counter;
      private string Path;
      private long FileLength;
      private long MaxFileSize;

      public FileDevice()
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

         Global.Logger.write(Eloquences.eloDebug, "App Log : " + filename);

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

            if (Counter > Int32.Parse(matches.Groups[1].Value))
               continue;

            Counter = Int32.Parse(matches.Groups[1].Value) + 1;
         }

         openFile();

         Global.Logger.write(Eloquences.eloDebug, "File inited : " + Counter.ToString() + "/" + path);
      }

      public override bool isDevice(Devices i) { return i == Devices.devFile; }

      public override void write(ConsoleColor color, string text)
      {
         // Note: the -1 & +1 things are talking about the size of termination byte
         if (FileLength > 0 && text.Length + FileLength + 1 > MaxFileSize)
         {
            Global.Logger.write(Eloquences.eloDebug, "File Length : " + FileLength.ToString() +
                                                     ",Text Length : " + text.Length.ToString() +
                                                     ",Max File Size : " + MaxFileSize.ToString());

            sw.Flush();
// Notice : is it possible, that the compiler says, it is not available???            sw.Close();
            File.Move(Path + "/app.log", Path + "/app." + Counter.ToString() + ".log");
            Counter++;
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
      private Devices devType;
      private Eloquences logLevel;
      private string filePath;
      private Device device;
      private Stream stream;
      private string prefix;

      // Constructor to set default configuration
      public Client(string prefix= null)
      {
         this.MaxLineLength = 1000;
         this.devType       = Devices.devConsole;
         this.logLevel      = Eloquences.eloAll;
         this.filePath      = null;
         this.device        = new ConsoleDevice();
         this.stream        = null;
         this.prefix        = prefix;
      }

      public void setMaxLineLength(int l) { this.MaxLineLength= l; }
      public void setTypeOfDevice(Devices i) { this.devType= i; }
      public void setLogLevel(Eloquences l) { this.logLevel= l; }
      public void setFilePath(string p) { this.filePath= p; setTypeOfDevice(Devices.devFile); openDevice(); }
      public void setStream(Stream s) { this.stream= s; setTypeOfDevice(Devices.devStream); openDevice(); }

      private void openDevice()
      {
         if (device != null && device.isDevice(this.devType))
            return ;

         switch (this.devType)
         {
            case Devices.devStream :
               {
                  StreamDevice dev = new StreamDevice();
                  device = dev;
                  dev.initialize(stream);
               }
               break;

            case Devices.devFile :
               {
                  FileDevice dev = new FileDevice();
                  device = dev;
                  dev.initialize(this.filePath, 5 * 1024);
               }
               break;

            case Devices.devConsole :
            default:
               device = new ConsoleDevice();
               break;
         }
      }

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
         new EloData ("INFO", ConsoleColor.Green),
         new EloData ("DEBUG", ConsoleColor.Gray)
      };

      // *** Log Level End **********************


      // *** Logging Begin **********************

      public void write(Eloquences elo, string msg)
      {
         if (elo >= this.logLevel)
            return ;

         System.DateTime curDt = System.DateTime.Now;
         System.Globalization.CultureInfo cult = new System.Globalization.CultureInfo("hu-HU");

         string logText = curDt.ToString(cult) + " " + edata[(int) elo].Prefix + " : ";
         if (this.prefix != null) logText += "(" + this.prefix + ") ";
         logText += msg;

         if (device == null)
            device = new ConsoleDevice();

         device.write(edata[(int) elo].Color, logText);

         if (logText.Length > this.MaxLineLength)
            // TODO: not the right exception :(
            throw new System.ArgumentOutOfRangeException(logText, "Text of log is longer like " + this.MaxLineLength.ToString() + " characters!");
      }

      // *** Logging End ************************
   }
}
