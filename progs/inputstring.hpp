
#include <buffer.hpp>

#ifndef INPUTSTRING_CLASS
#define INPUTSTRING_CLASS

namespace Docler
{

/*

 Special string class for input of Vigenere project

 This string class knows to base processes on any input
 string.

 Example:

  - conver ascii lowercase chars to uppercase
  - ignore not alphabet characters
  - convert hungarian accents from letter

 */

class InputString
{
   public:

      InputString(size_t size) : original(), converted(size) {}
      virtual ~InputString() {}

      virtual void set(const Buffer<wchar_t>& input, size_t fkeysize= 0);

      virtual const Buffer<wchar_t>& orig() const { return original; }
      virtual const Buffer<char>& conv() const { return converted; }

   protected:

      Buffer<wchar_t> original;
      Buffer<char> converted;
};

}

#endif

