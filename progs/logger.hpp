
#include <iostream>
#include <cstdarg>

#ifndef LOGGER_CLASS
#define LOGGER_CLASS

namespace Docler
{

/*

 Logger object.

 It is not a thread safe logger, due to I would like to centralize
 logging first. But so, I centralized it, I have chance to write one
 in the future.

 First step, the LogLevel is just a sign to prefix of log

 */

enum LogLevel
{
   ALWAYS = 0x00000000,
   ERROR  = 0x00000001,
   INFO   = 0x00000002,
   DEBUG  = 0x00000004,

   LLALL  = 0xffffffff
};

class Logger
{
   public:

      static void write(LogLevel elo, const char* format, ...);
      static void wwrite(LogLevel elo, const wchar_t* format, ...);
};

}

#endif

