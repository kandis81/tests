
#include <vigenereapp.hpp>

using namespace Docler;

int main(void)
{
   VigenereApp app;

   try
   {
      app.run();
   }
   catch (std::exception& e)
   {
      Logger::wwrite(ERROR, L"%s", e.what());
      return 1;
   }

   return 0;
}

