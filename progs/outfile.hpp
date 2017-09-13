
#include <file.hpp>

#ifndef OUTFILE_CLASS
#define OUTFILE_CLASS

namespace Docler
{

/*

 Out File object.

 */

class OutFile : virtual public File
{
   public:

      OutFile(const char* path, unsigned char omode= OWrite | OText) : File(path, omode) {}

      virtual void write(const Buffer<char>& buf, size_t len= 0) { File::write(buf, len); }
};

}

#endif

