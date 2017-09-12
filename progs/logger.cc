
#include <logger.hpp>

#include <cwchar>
#include <cstdio>
#include <cstdlib>

namespace Docler
{

// void Logger::write(LogLevel elo, const char* format, ...)
// {
//    va_list args;
// 
//    va_start(args, format);
//    vprintf(format, args);
//    va_end(args);
//    fflush(stdout);
// }

void Logger::wwrite(LogLevel elo, const wchar_t* format, ...)
{
   va_list args;

   va_start(args, format);
   vwprintf(format, args);
   va_end(args);
   fflush(stdout);
}

}

