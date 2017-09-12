
#include <iostream>
#include <cstdio>
#include <cwchar>

#include <vigenereapp.hpp>

namespace Docler
{

VigenereApp::VigenereApp() :
   cbuf(maxSizeOfOText + TBlen),
   wbuf(maxSizeOfOText + TBlen),
   open_text(maxSizeOfOText + TBlen),
   key_text(maxSizeOfOText + TBlen),
   lscope("")
{
}

VigenereApp::~VigenereApp()
{
}

void VigenereApp::readInput(const wchar_t* name, InputString& str, size_t inputLen, size_t fkeysize)
{
   Logger::wwrite(INFO, L"Enter the %ls:\n", name);

   fgetws(wbuf.get(), wbuf.size() - 1, stdin);
   wbuf.len(wcslen(wbuf.get()));
   fflush(stdin);

   if (wbuf.len() > inputLen)
      throw std::length_error("Lenght of input string is too long!");

   str.set(wbuf, fkeysize);
}

void VigenereApp::run()
{
   readInput(L"open text", open_text, maxSizeOfOText);

   Logger::wwrite(INFO, L"%s\n", open_text.conv().data());

   readInput(L"key text",  key_text,  maxSizeOfKey, open_text.conv().len());

   Logger::wwrite(INFO, L"%s\n", key_text.conv().data());
}

}


