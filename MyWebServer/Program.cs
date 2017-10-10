using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Threading
            Thread thread = new Thread(Runnable);
            thread.Start();

            ThreadPool.QueueUserWorkItem(ThreadProc);

            Console.WriteLine("Operating System: " + Environment.OSVersion);

            // Exit Program
            Console.WriteLine("\nHit the any key to exit...");
            Console.ReadKey();
        }

        static void Runnable()
        {
            Console.WriteLine("From Thread");
        }

        static void ThreadProc(object state)
        {
            Console.WriteLine("From Thread Pool");
        }
    }
}
