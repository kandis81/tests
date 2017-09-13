
#include <logger.hpp>
#include <localescope.hpp>
#include <buffer.hpp>
#include <inputstring.hpp>
#include <inputkeystring.hpp>
#include <infile.hpp>
#include <outfile.hpp>

#ifndef VIGENEREAPP_CLASS
#define VIGENEREAPP_CLASS

namespace Docler
{

/*

 Task class

 The center of the test app is this class.

 TODO: (related with whole app)

 */

class VigenereApp
{
   public:

      VigenereApp();
      ~VigenereApp();

      void run();

   private:

      void loadMatrix();
      void readInput(const wchar_t* name, InputString& str, size_t inputLen, size_t fkeysize= 0);
      void encodeText();

      enum Sizes
      {
         maxSizeOfOText = 255,   // Max size of inputs (after conversion)
         maxSizeOfKey   = 5,     // Max size of key in case of read from stdin
         TBlen          = 1      // len of termination byte
      };

      Buffer<char>    cbuf,      // character buffer
                      logbuf;    // log buffer
      Buffer<wchar_t> wbuf;      // wide character buffer
      Buffer<void*>   vbuf;      // void pointer collection (Buffers are inside)
      InputString     open_text; // Open Text
      InputKeyString  key_text;  // Key Text
      LocaleScope     lscope;    // locale is fixed under run of this clas
      InFile          infile;    // File class to load from vigenere matrix
      OutFile         outfile;   // File class to write out the result
};

}

#endif

