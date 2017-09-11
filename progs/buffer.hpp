
#include <cstring>
#include <cstdlib>

#include <stdexcept>

#ifndef BUFFER_CLASS
#define BUFFER_CLASS

namespace Docler
{

/*

 Buffer object.

 I do not like to manage heap allocations if it is possible,
 so I create this class for manage the characther buffers.

 */

template <typename T>
class Buffer
{
   public:

      Buffer(size_t size= 0) { bsize= blen= 0; buf= 0; this->size(size); }
      virtual ~Buffer() { free(); }

      virtual T* get(size_t fpos= 0)
      {
         if (fpos >= size())
            throw std::out_of_range("Could not get back buffer out of range!");

         return buf + fpos;
      }

      virtual const T* data(size_t fpos= 0) const
      {
         if (fpos >= size())
            throw std::out_of_range("Could not get back data out of range!");

         return buf + fpos;
      }

      virtual size_t size(size_t size, bool forced= false)
      {
         size_t oldSize= this->size();

         if (size == this->size())
            return this->size();

         if (size <= this->size() && !forced)
            return this->size();

         if (size < 0)
            throw std::invalid_argument("Invalid new size of buffer parameter!");

         if (size == 0)
         {
            free();
            return this->size();
         }

         buf= (T*) realloc(buf, size * sizeof(T));
         bsize= size;

         if (size > oldSize)
            clear(oldSize);

         return this->size();
      }

      virtual size_t size() const { return bsize; }

      virtual size_t len(size_t len)
      {
         if (len > size())
            throw std::invalid_argument("Invalid length paramete, due to largest like buffer size!");

         blen= len;

         return this->len();
      }

      virtual size_t len() const { return blen; }

      virtual void clear(size_t fpos= 0)
      {
         if (fpos < 0 || fpos >= size())
            throw std::invalid_argument("Invalid from position at buffer clear!");

         if (buf)
            memset(buf + fpos, 0, size() * sizeof(T) - fpos * sizeof(T));

         len(fpos);
      }
      virtual void free() { ::free(buf); buf= 0; }

   private:

      T* buf;
      size_t bsize, blen;
};

}

#endif

