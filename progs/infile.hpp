
#include <file.hpp>

#ifndef INFILE_CLASS
#define INFILE_CLASS

namespace Docler
{

/*

 In File object.

 */

class InFile : virtual public File
{
   public:

      InFile(const char* path, unsigned char omode= ORead | OText) : File(path, omode) {}

      virtual void read(Buffer<char>& buf, bool all= true) { File::read(buf, all); }
};

}

#endif

