
#include <inputstring.hpp>

#include <stdexcept>

namespace Docler
{

void InputString::set(const Buffer<wchar_t>& input)
{
   if (input.len() <= 0)
      throw std::invalid_argument("Could not set input string, due to no data in input parameter!");

TODO ....
}

}

