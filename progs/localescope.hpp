
#include <clocale>

#ifndef LOCALESCOPE_CLASS
#define LOCALESCOPE_CLASS

namespace Docler
{

/*

 Locale Scope

 I should to change the locale settings to system version, due
 to it occures many problems, when you like to read console &
 write it to out.

 It settings necessary for correct wchar_t type usage.

 */

class LocaleScope
{
   public:

      LocaleScope(const char* locale)
      {
         orig= setlocale(LC_ALL, NULL);
         setlocale(LC_ALL, locale);
      }

      virtual ~LocaleScope()
      {
         setlocale(LC_ALL, orig);
      }

   protected:

      const char* orig;
};

}

#endif

