// C/C++ : include
using System;
using Log;

class Program
{
   private static Logger logger;

   // ***************************************
   // Print Usage
   // ***************************************

   private static void printUsage()
   {
      Console.WriteLine("Usage of this tool: \n");
      Console.WriteLine("\tdotnet run <args>\n");
      Console.WriteLine("\t\tArguments:");
      Console.WriteLine("\t\t\t--log-type  <type>   : Where is the way to logging (console,stream,file) [default: console]");
      Console.WriteLine("\t\t\t--max-ls <number>    : Max line size of a comment () [default: 1000]");
      Console.WriteLine($"\t\t\t--log-level <number> : Log level (0-{Eloquences.eloAll}) [default: {Eloquences.eloAll}]");
      Console.WriteLine("\t\t\t--log-path <path>    : Path of log files. Log filename is app.log [default: (null)]");
      Console.WriteLine("");
   }

   // ***************************************
   // Evaluate arguments
   // ***************************************

   private static void evalACheckNext(int idx, string[] args)
   {
      if (idx >= args.Length)
         throw new ArgumentException(args[idx - 1] + " - Missing argument value");
   }

   private static void evalArguments(string[] args)
   {
      long maxlinesize = 1000;
      string logtype = null;
      Eloquences eloquence = Eloquences.eloAll;
      string logpath = null;

      for (int idx= 0; idx < args.Length; idx++)
      {
         switch (args[idx])
         {
            case "--log-type" : idx++; evalACheckNext(idx, args); logtype= args[idx]; break;

            case "--log-level" :
               idx++; evalACheckNext(idx, args);

               switch (Int32.Parse(args[idx]))
               {
                  case (int) Eloquences.eloError :  eloquence =Eloquences.eloError; break;
                  case (int) Eloquences.eloInfo  :  eloquence =Eloquences.eloInfo;  break;
                  case (int) Eloquences.eloDebug :  eloquence =Eloquences.eloDebug; break;
                  case (int) Eloquences.eloAll   :  eloquence =Eloquences.eloAll;   break;

                  default:
                     throw new ArgumentException(args[idx] + " - Unknown log level");
               }

               break;

            case "--log-path" : idx++; evalACheckNext(idx, args); logpath= args[idx];                  break;
            case "--max-ls"   : idx++; evalACheckNext(idx, args); maxlinesize= Int32.Parse(args[idx]); break;
            case "--help-app" : printUsage(); System.Environment.Exit(0);                              break;

            default:
               throw new ArgumentException(args[idx] + " - Invalid argument");
         }
      }

      // TODO: Is should to replace InvalidOperationException to something ConfigurationException,
      //       due to it should be better, just actully the System.Configuration is not available.

      switch (logtype)
      {
         case "console" :
            if (logpath != null)
               throw new System.InvalidOperationException("Invalid configuration - Log path is configured, but log type is configured to console!");

            logger = new ConsoleLogger();
            break;

         case null      :
            logger = new ConsoleLogger();
            break;

         case "stream"  :
            throw new ArgumentOutOfRangeException("log-type", "Stream usage is not avilable yet, due to it is equivalent with file mode (it is an FileStream)");

         case "file"    :
            if (logpath == null)
               throw new System.InvalidOperationException("Invalid configuration - Log type is configured to file, but file path is not added!");

            logger = new FileLogger(logpath);
            break;

         default        : throw new ArgumentOutOfRangeException("log-type", $"{logtype} is invalid type of log");
      }

      logger.MaxLineSize = maxlinesize;
      logger.MaxLogLevel = eloquence;
   }

   // ***************************************
   // First enter point of program
   // ***************************************

   static void Main(string[] args)
   { 
      string line;

      try
      { 
         evalArguments(args);
      }
      catch (Exception e)
      {
         Console.WriteLine(e.ToString());

         printUsage();
         return;
      }

      while ((line = Console.ReadLine()) != null)
      {
         switch (line[0])
         {
            case 'E' : logger.write(Eloquences.eloError, line); break;
            case 'D' : logger.write(Eloquences.eloDebug, line); break;
            default  : logger.write(Eloquences.eloInfo,  line); break;
         }
      }
   }
}
