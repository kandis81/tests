
#include <file.hpp>

namespace Docler
{

File::File(const char* path, unsigned char omode)
{
   char mode[4];

   modeToStr(omode, mode);

   if ((fp= fopen(path, mode)) == NULL)
      throw std::ios_base::failure(strerror(errno));
}

void File::modeToStr(unsigned char omode, char* mode)
{
   int b= 0;
   mode[b] = omode & ORead   ? 'r' :
             omode & OWrite  ? 'w' :
             omode & OAppend ? 'a' : 0;

   if (!mode[b])
      throw std::invalid_argument("Could not open files, due to could not identificate read, write or append!");

   b++;

   mode[b] = omode & OPlus ? '+' : 0;

   if (mode[b])
      b++;

   mode[b] = omode & OText   ? 't' :
             omode & OBinary ? 'b' : 0;

   if (!mode[b])
      throw std::invalid_argument("Could not open files, due to could not identificate binary or text!");

   mode[++b]= 0;
}

void File::read(Buffer<char>& buf, bool all)
{
   if (fp == NULL)
      throw std::ios_base::failure("Could not read line, due to file is not open!");

   size_t actpos= ftell(fp);
   fseek(fp, 0, SEEK_END);
   size_t endpos= ftell(fp);
   fseek(fp, actpos, SEEK_SET);

   size_t len= endpos - actpos; // left size
   size_t readed= 0;

   if (all)
   {
      if (len > buf.len())
         buf.size(len);
   }
   else
      len = len > buf.len() ? buf.len() : len;

   buf.clear();

   readed += fread(buf.get(), 1, len, fp);

   if (readed != len)
      throw std::ios_base::failure(strerror(errno));

   buf.len(len);
}

void File::write(const Buffer<char>& buf, size_t bytes)
{
   if (fp == NULL)
      throw std::ios_base::failure("Could not write line, due to file is not open!");

   bytes = bytes > 0 ? bytes : buf.len();

   size_t wrote = fwrite(buf.data(), 1, bytes, fp);

   if (wrote != bytes)
      throw std::ios_base::failure(strerror(errno));
}

}

