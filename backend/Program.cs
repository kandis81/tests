using System;
using System.Collections.Generic;
using ExpergefioTests;

/*
 * Several info in line with this solution:
 *  - developed under kUbuntu Linux 64 bit
 *  - .NET : 1.0.1 prev4
 * 
 *  I solved the two question in two classes: Question1, Question2
 */


class Program
{
    /*
     * Class Reader
     *
     * This is the class what describes how to read the inout stream. It
     * is thread safe, when you like to show the actual results.
     *
     * This class is waiting the reader its constructor.
     *
     */

    class Reader : IDisposable
    {
        private ICharacterReader reader;
        private bool finished= false;
        private System.Threading.Mutex mmutex= new System.Threading.Mutex(); // mutex for mapping
        private static System.Threading.Mutex cmutex= new System.Threading.Mutex(); // mutex for all Show method (I do not want to see mixed console outputs)
        private Dictionary<string, int> map= new Dictionary<string, int>();

        // ***********************************************************
        // Finished : reading progress is running or not.
        // ***********************************************************

        public bool Finished { get { return finished; } }

        // ***********************************************************
        // Constructor - it wants a right reader
        // ***********************************************************

        public Reader(ICharacterReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException("reader", "Set of null reader is not allowed");
            this.reader = reader;
        }

        // ***********************************************************
        // Run - do the job
        // ***********************************************************

        public void Run(object prefix)
        {
            char c;
            string word = "";

            try 
            {
                while (true)
                {
                   c= reader.GetNextChar();

                   if (!Char.IsLetterOrDigit(c))
                   {
                      Put(word);
                      word = "";
                   }
                   else
                      word += Char.ToLower(c);
                }
            }
            catch (System.IO.EndOfStreamException)
            {
                // We are at nd of stream
                Put(word);
                Show(prefix.ToString(), ConsoleColor.Green);
            }
            catch  (System.Exception e)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                if (prefix != null)
                   System.Console.Write(prefix);
                System.Console.WriteLine("Error: " + e);
                System.Console.ResetColor();
            }

            finished = true;
        }

        // ***********************************************************
        // Show - the following struct & class are necessary for Show
        //        procedure. One time only one Show could run. They
        //        are blocking themself
        // ***********************************************************

        private struct SKey
        {
           public string Word { set; get; }
           public int Cntr { set; get; }
        }

        private class SComparer : IComparer<SKey>
        {
           public int Compare(SKey x, SKey y)
           {
              if (x.Cntr != y.Cntr)
                 return x.Cntr > y.Cntr ? -1 : 1;

              return x.Word.CompareTo(y.Word);
           }
        }

        public void Show(string prefix = null, ConsoleColor preafixcolor = ConsoleColor.Yellow)
        {
            SComparer comp= new SComparer();
            SortedDictionary<SKey, int> smap= new SortedDictionary<SKey, int>(comp);

            mmutex.WaitOne();

            foreach (KeyValuePair<string, int> kvp in map)
            {
                SKey k= new SKey();
                k.Word = kvp.Key;
                k.Cntr = kvp.Value;
                smap[k] = k.Cntr;
            }

            mmutex.ReleaseMutex();
            cmutex.WaitOne();

            if (prefix != null)
            {
                System.Console.ForegroundColor = preafixcolor;
                System.Console.Write(prefix);
                System.Console.ResetColor();
            }

            foreach (KeyValuePair<SKey, int> kvp in smap)
            {
                System.Console.Write(kvp.Key.Word + " - " + kvp.Key.Cntr + " ");
            }
            System.Console.WriteLine("");

            cmutex.ReleaseMutex();
        }

        // ***********************************************************
        // Put - increase a counter of any string in map. Mutexed for
        //       usage with Show in same time.
        // ***********************************************************

        private void Put(string word)
        {
            if (word.Length <= 0)
                return;

            mmutex.WaitOne();
            int cntr= 0;

            if (map.ContainsKey(word))
                cntr= map[word];

            cntr++;

            map[word] = cntr;

            mmutex.ReleaseMutex();
        }

        // ***********************************************************
        // Dispose - for future
        // ***********************************************************

        public void Dispose() {}
    }

    /*
     * Interface Question - interface class for use question classes
     */

    interface Question : IDisposable
    {
        void Run();
    }

    /*

     Question 1:

        Write an application that takes an ICharacterReader interface and outputs a list of word frequencies
     ordered by word count and then alphabetically. You should use the SimpleCharacterReader class as a test
     input, and send the output to the console. For example, if the stream returns It was the best of times,
     it was the worst of times then the output will be: 

        it - 2 of - 2 the – 2 times - 2 was - 2 best - 1 worst – 1

     */

    class Question1 : Question
    {
        // ***********************************************************
        // Dispose - for future
        // ***********************************************************

        public void Run()
        {
           using (ICharacterReader ir = new SimpleCharacterReader())
           {
               using (Reader r = new Reader(ir))
               {
                   r.Run("");
               }
           }
        }

        // ***********************************************************
        // Dispose - for future
        // ***********************************************************

        public void Dispose() { }
    }

    /*

     Question 2:

        Write an application that takes an array of ICharacterReader interfaces, accesses them in parallel,
     and produces a console output of combined word counts, split by word as in the first step, every 10
     seconds. Test this using the SlowCharacterReader class.

     */

    class Question2 : Question
    {
        private static readonly int MaxNumberOfICharacterReaders = 10;
        private static readonly int ShowFrequency = 10;

        // ***********************************************************
        // Run - start of stream processing & the Data contains
        //       other objects/varialbles, what necessary for task
        //       in case of one Thread. It stored a dictionary, where
        //       the key is the reader object.
        // ***********************************************************

        private struct Data
        {
            public string Prefix { set; get; }
            public Reader Reader { set; get; }
            public System.Threading.Thread Thread { set; get; }
        }

        public void Run()
        {
           ICharacterReader[] readers = new ICharacterReader[MaxNumberOfICharacterReaders];
           Dictionary<ICharacterReader, Data> map = new Dictionary<ICharacterReader, Data>();
           bool running = true;

           // initialize
           for (int i= 0; i<MaxNumberOfICharacterReaders; i++)
           {
              Data data= new Data();

              readers[i]  = new SlowCharacterReader();
              data.Reader = new Reader(readers[i]);
              data.Thread = new System.Threading.Thread(data.Reader.Run);
              data.Prefix = "Thread " + i + ": " ;

              map[readers[i]]= data;
           }

           // start
           foreach (KeyValuePair<ICharacterReader, Data> kvp in map)
           {
              kvp.Value.Thread.Start(kvp.Value.Prefix);
           }

           // check
           while (running)
           {
              System.Threading.Thread.Sleep(ShowFrequency * 1000);
              running= false;

              foreach (KeyValuePair<ICharacterReader, Data> kvp in map)
              {
                 if (kvp.Value.Reader.Finished)
                    continue;

                 kvp.Value.Reader.Show(kvp.Value.Prefix);
                 running= true;
              }
           }

           // exitting
           foreach (KeyValuePair<ICharacterReader, Data> kvp in map)
           {
              kvp.Value.Reader.Dispose();
              kvp.Key.Dispose();
           }
        }

        // ***********************************************************
        // Dispose - for future
        // ***********************************************************

        public void Dispose() { }
    }

    /*
     * Test starts here
     */

    static void Main(string[] args)
    {
        try
        {
           System.Console.ForegroundColor = ConsoleColor.Blue;
           System.Console.WriteLine("************************************");
           System.Console.WriteLine("* Run test 1:");
           System.Console.WriteLine("************************************");
           System.Console.ResetColor();
 
           using (Question question= new Question1())
               question.Run();

           System.Console.ForegroundColor = ConsoleColor.Blue;
           System.Console.WriteLine("************************************");
           System.Console.WriteLine("* Run test 2:");
           System.Console.WriteLine("************************************");
           System.Console.ResetColor();
 
           using (Question question= new Question2())
               question.Run();

           System.Console.ForegroundColor = ConsoleColor.Blue;
           System.Console.WriteLine("************************************");
           System.Console.WriteLine("* End of tests");
           System.Console.WriteLine("************************************");
           System.Console.ResetColor();
       }
       catch (System.Exception e)
       {
           System.Console.ForegroundColor = ConsoleColor.Red;
           System.Console.WriteLine("UNHANDLED Exception: " + e);
           System.Console.ResetColor();
       }
    }
}
