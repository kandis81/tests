
#include <cstdio>
#include <cerrno>
#include <ios>

#include <buffer.hpp>

#ifndef FILE_CLASS
#define FILE_CLASS

int errno;

namespace Docler
{

/*

 File object.

 */

enum OpenMode
{
   ORead       = 0x01,
   OWrite      = 0x02,
   OAppend     = 0x04,
   OPlus       = 0x08,

   OText       = 0x10,
   OBinary     = 0x20,

   OReadPlus   = ORead   | OPlus,
   OWritePlus  = OWrite  | OPlus,
   OAppendPlus = OAppend | OPlus

};

class File
{
   public:

      virtual ~File() {};

   protected:

      File(const char* path, unsigned char omode);

      void modeToStr(unsigned char omode, char* mode);

      void read(Buffer<char>& buf, bool all= true);
      void write(const Buffer<char>& buf, size_t bytes= 0);

      FILE* fp;
};

}

#endif

