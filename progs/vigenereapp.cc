
#include <iostream>
#include <cstdio>
#include <cwchar>

#include <vigenereapp.hpp>

namespace Docler
{

VigenereApp::VigenereApp() :
   cbuf(),
   logbuf(2000),
   wbuf(maxSizeOfOText + TBlen),
   vbuf(),
   open_text(maxSizeOfOText + TBlen),
   key_text(maxSizeOfOText + TBlen),
   lscope(""),
   infile("vtabla.dat"),
   outfile("kodolt.dat")
{
}

VigenereApp::~VigenereApp()
{
   for (size_t i= 0; i < vbuf.len(); i++)
      delete (Buffer<char>*) *(vbuf.get(i));
}

void VigenereApp::loadMatrix()
{
   infile.read(cbuf);
   Buffer<char> line, *p;

//   Logger::wwrite(DEBUG, L"File:\n%s\n", cbuf.data());

   for (size_t i= 0; i < cbuf.len(); i++)
   {
      if (line.size() <= line.len())
         line.size(line.size() + 1);

      if (*(cbuf.data(i)) < 'A' || *(cbuf.data(i)) > 'Z')
      {
//         Logger::wwrite(DEBUG, L"Line: [%s]\n", line.data());

         if(line.len() > 0)
         {
            p= new Buffer<char>(line.len());
            *p= line;
            vbuf.size(vbuf.size() + 1);
            vbuf.len(vbuf.len() + 1);
            *(vbuf.get(vbuf.len() - 1)) = p;

            line.clear();
         }

         continue;
      }

      line.len(line.len() + 1);
      *(line.get(line.len() - 1)) = *(cbuf.data(i));
   }

   if (line.len() > 0)
   {
//      Logger::wwrite(DEBUG, L"Line: [%s]\n", line.data());

      p= new Buffer<char>(line.len());
      *p= line;
      vbuf.size(vbuf.size() + 1);
      vbuf.len(vbuf.len() + 1);
      *(vbuf.get(vbuf.len() - 1)) = p;
   }
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

void VigenereApp::encodeText()
{
   cbuf.clear();

   if (open_text.conv().len() > cbuf.size())
      cbuf.size(open_text.conv().len());

   cbuf.len(open_text.conv().len());

   for (size_t i= 0; i < open_text.conv().len(); i++)
   {
      Buffer<char>* p= (Buffer<char>*) *(vbuf.get());
      size_t x= 0;

      for (size_t j= 0; j < p->len(); j++)
      {
         if (*(p->get(j)) == *(key_text.conv().data(i)))
         {
            x= j;
            break;
         }
      }

      if (*(p->get(x)) != *(key_text.conv().data(i)))
      {
         snprintf(logbuf.get(), logbuf.len(), "'%c' key character not found in first line!", *(key_text.conv().data(i)));
         throw std::out_of_range(logbuf.data());
      }

      p= 0;

      for (size_t j= 0; !p && j < vbuf.len(); j++)
         if (*(((Buffer<char>*) *(vbuf.get(j)))->get()) == *(open_text.conv().data(i)))
            p= (Buffer<char>*) *(vbuf.get(j));

      if (!p)
      {
         snprintf(logbuf.get(), logbuf.len(), "'%c' open text character not found in first column!", *(open_text.conv().data(i)));
         throw std::out_of_range(logbuf.data());
      }

      *(cbuf.get(i))= *p->get(x);
   }
}

void VigenereApp::run()
{
   loadMatrix();

   readInput(L"open text", open_text, maxSizeOfOText);

   Logger::wwrite(INFO, L"%s\n", open_text.conv().data());

   readInput(L"key text",  key_text,  maxSizeOfKey, open_text.conv().len());

   Logger::wwrite(INFO, L"%s\n", key_text.conv().data());

   encodeText();

   Logger::wwrite(INFO, L"%s\n", cbuf.data());

   outfile.write(cbuf);
}

}


