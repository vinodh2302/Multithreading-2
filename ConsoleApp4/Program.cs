using System;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace threading
{
    class Program
    {
        private static object lock1 = new object();
        private static object lock2 = new object();
        private static object locker = new object();
        static void Main(string[] args)
        {
            // Scenario 1
             //Creating a single thread check background case. 
            Console.WriteLine("Entered main "+ Thread.CurrentThread.ManagedThreadId);
            Thread t1 = new Thread(new ThreadStart(f1));
            t1.Start();
            t1.IsBackground = true;

            Console.WriteLine("thread completed " + Thread.CurrentThread.ManagedThreadId);

            // Scenario 2 However this can cause race condition
            multipleThreads();

            // Deadlocks scenario
             t1 = new Thread(Deadlock1);
            Thread t2 = new Thread(Deadlock2);
            t1.Start();
            t2.Start();

            // Mutex = same as lock
            // Semaphore allows more than 1 thread to enter Critical section
        }

        static void Deadlock1()
        {
            lock(lock1)
            {
                Console.WriteLine("Deadlock1 Thread id entered" + Thread.CurrentThread.ManagedThreadId);   
                Thread.Sleep(1000);
                lock (lock2)
                {
                    Console.WriteLine("Deadlock1  Thread id entered" + Thread.CurrentThread.ManagedThreadId);
                }
            }
        }

        static void Deadlock2()
        {
            lock (lock2)
            {
                Console.WriteLine("Deadlock2 Thread id entered" + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(1000);
                lock (lock1)
                {
                    Console.WriteLine(" Deadlock2 Thread id entered" + Thread.CurrentThread.ManagedThreadId);
                }
            }
        }

        static void multipleThreads()
        {
            Thread[] tr = new Thread[3];
            
            for(int j =0; j< 3; j++)
            {
                tr[j] = new Thread(new ThreadStart(testmultiplethreads));
                tr[j].Start();
            }
        }

        static void testmultiplethreads()
        {
            lock (locker)
            {
                Console.WriteLine("Enterd by thread " + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(3000);
                Console.WriteLine("Exited by thread " + Thread.CurrentThread.ManagedThreadId);
            }
        }



        static void f1()
        {
            Console.WriteLine("Entered f1 " + Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(3000);
            Console.WriteLine("-----------");
            Console.WriteLine("completed f1 " + Thread.CurrentThread.ManagedThreadId);
        }
    }
}