
#include <inputstring.hpp>

#ifndef INPUTKEYSTRING_CLASS
#define INPUTKEYSTRING_CLASS

namespace Docler
{

/*

 Special string class for key input of Vigenere project

 This string class knows to prepare of key string.

 Example:

  - normal imput string conversion
  - expand with repeat of input string to size of open_text

 */

class InputKeyString : public InputString
{
   public:

      InputKeyString(size_t size) : InputString(size) {}

      virtual void set(const Buffer<wchar_t>& input, size_t fkeysize= 0)
      {
          InputString::set(input);

          // Equal is ok, nothing to do
          if (converted.len() == fkeysize)
             return;

          // longer??? okay, cut it (but it was not declarated in doc)
          if (converted.len() > fkeysize)
          {
             converted.clear(fkeysize);
             return;
          }

          size_t j= 0;

          for (size_t i= converted.len(); i < fkeysize; i++)
             *(converted.get(i)) = *(converted.get(j++));
      }

};

}

#endif

