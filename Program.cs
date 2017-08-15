// C/C++ : include
using System;
using Log;

// C/C++ : typedef
using LogClient = Log.Client;
using LogEloquence = Log.Client.Eloquences;

class Program
{
   // ***************************************
   // First enter point of program
   // ***************************************

   static void Main(string[] args)
   { 
      LogClient log= new LogClient();

      log.write(LogEloquence.eloInfo, "Hello World!");
      log.write(LogEloquence.eloDebug, "Hello World!");
      log.write(LogEloquence.eloError, "Hello World!");
   }
}
