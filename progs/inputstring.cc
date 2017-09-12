
#include <inputstring.hpp>

#include <stdexcept>

namespace Docler
{

void InputString::set(const Buffer<wchar_t>& input, size_t fkeysize)
{
   if (input.len() <= 0)
      throw std::invalid_argument("Could not set input string, due to no data in input parameter!");

   original= input;
   converted.clear();

   size_t j= 0;

   for (size_t i= 0; i < original.len(); i++)
   {
      if (*(original.get(i)) >= L'A' && *(original.get(i)) <= L'Z')
      {
         *(converted.get(j++)) =  (char) *(original.get(i));

         continue;
      }

      if (*(original.get(i)) >= L'a' && *(original.get(i)) <= L'z')
      {
         *(converted.get(j++)) =  (char) *(original.get(i)) - ('a' - 'A');

         continue;
      }

      char c= 0;

      // árvíztűrő tükörfúrógép
      switch (*(original.get(i)))
      {
         case L'á' : c= 'A'; break;
         case L'é' : c= 'E'; break;
         case L'í' : c= 'I'; break;
         case L'ó' :
         case L'ö' :
         case L'ő' : c= 'O'; break;
         case L'ü' :
         case L'ű' :
         case L'ú' : c= 'U'; break;

         default: continue;
      };

      *(converted.get(j++)) = c;

      if (j > converted.size() - 1)
         throw std::invalid_argument("Could not set input string, due to too long!");
   }

   converted.len(j);
}

}

