
#include <iostream>
#include <cstdio>
#include <cwchar>

#include <logger.hpp>
#include <localescope.hpp>
#include <buffer.hpp>

using namespace std;
using namespace Docler;

int main(void)
{
   LocaleScope lscope("");
   Buffer<char> cbuf(255 + 1);
   Buffer<wchar_t> wbuf(255 + 1);

   fgetws(wbuf.get(), wbuf.size() - 1, stdin);

   Logger::wwrite(INFO, L"Readed: %ls", wbuf.data());

   return 0;
}

