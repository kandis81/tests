// C/C++ : include
using System;
using Log;

// C/C++ : typedef
using LogClient = Log.Client;
using LogEloquence = Log.Eloquences;
using LogInterface = Log.Interfaces;

class Program
{
   private static LogClient log;

   // ***************************************
   // Print Usage
   // ***************************************

   private static void printUsage()
   {
      log.write(LogEloquence.eloInfo, "Usage of this tool: \n");
      log.write(LogEloquence.eloInfo, "\tdotnet run <args>\n");
      log.write(LogEloquence.eloInfo, "\t\tArguments:");
      log.write(LogEloquence.eloInfo, "\t\t\t--log-type  <type>   : Where is the way to logging (console,stream,file) [default: console]");
      log.write(LogEloquence.eloInfo, "\t\t\t--max-ls <number>    : Max line size of a comment () [default: 1000]");
      log.write(LogEloquence.eloInfo, "\t\t\t--log-level <number> : Log level (0-" + ((int)LogEloquence.eloAll).ToString() + ") [default: " + ((int)LogEloquence.eloAll).ToString() + "]");
      log.write(LogEloquence.eloInfo, "\t\t\t--log-path <path>    : Path of log files. Log filename is app.log [default: (null)]");
      log.write(LogEloquence.eloInfo, "");
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
//      log.write(LogEloquence.eloDebug, "Count of parameters : " + args.Length.ToString());

      for (int idx= 0; idx < args.Length; idx++)
      {
//         log.write(LogEloquence.eloDebug, "Parameter : " + args[idx]);

         switch (args[idx])
         {
            case "--log-type" :
               idx++; evalACheckNext(idx, args);

               switch (args[idx])
               {
                  case "console" : log.setTypeOfInterface(LogInterface.ifConsole); break;
                  case "stream"  : log.setTypeOfInterface(LogInterface.ifStream);  break;
                  case "file"    : log.setTypeOfInterface(LogInterface.ifFile);    break;

                  default:
                     throw new ArgumentException(args[idx] + " - Unknown log type");
               }

               break;

            case "--log-level" :
               idx++; evalACheckNext(idx, args);

               switch (Int32.Parse(args[idx]))
               {
                  case (int) LogEloquence.eloError :  log.setLogLevel(LogEloquence.eloError); break;
                  case (int) LogEloquence.eloInfo  :  log.setLogLevel(LogEloquence.eloInfo);  break;
                  case (int) LogEloquence.eloDebug :  log.setLogLevel(LogEloquence.eloDebug); break;
                  case (int) LogEloquence.eloAll   :  log.setLogLevel(LogEloquence.eloAll);   break;

                  default:
                     throw new ArgumentException(args[idx] + " - Unknown log level");
               }

               break;

            case "--log-path" :
               idx++; evalACheckNext(idx, args);

               try { log.setFilePath(args[idx]); }
               catch (Exception e)
               {
                  log.write(LogEloquence.eloError, e.ToString());
                  throw new ArgumentException(args[idx] + " - Invalid log path");
               }

               break;

            case "--max-ls" :
               idx++; evalACheckNext(idx, args);
               log.setMaxLineLength(Int32.Parse(args[idx]));
               break;

            case "--help-app" :
               printUsage();
               System.Environment.Exit(0);
               break;

            default:
               throw new ArgumentException(args[idx] + " - Invalid argument");
         }
      }
   }

   // ***************************************
   // First enter point of program
   // ***************************************

   static void Main(string[] args)
   { 
      log= new LogClient();
      string line;

      try
      { 
         evalArguments(args);
      }
      catch (Exception e)
      {
         log.write(LogEloquence.eloError, e.ToString());

         printUsage();
         return;
      }

      while ((line = Console.ReadLine()) != null)
      {
         switch (line[0])
         {
            case 'E' : log.write(LogEloquence.eloError, line); break;
            case 'D' : log.write(LogEloquence.eloDebug, line); break;
            default  : log.write(LogEloquence.eloInfo,  line); break;
         }
      }
   }
}
