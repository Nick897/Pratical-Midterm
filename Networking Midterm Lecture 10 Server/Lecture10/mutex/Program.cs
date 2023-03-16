//Create a server that can handle multiple clients
/*
 * SERVER
 - Receive updates from multiple clients
 - Create an update msg with updates from all clients
 - Sends to all connected clients

 - Keeps track of all connected clients - arrays - lists 
 - Update interval (50ms - 100ms)

Establish Connection with TCP 
 - When a client connects
    - add the client to the list of connected clients
    - create a thread (to process receivefrom, or recv)
 - Server will send update to all clients
    - iterate through list of clients
    - send update to every single client
- - Problem!!
    - Memory Access (Shared buffer)
    - mutex (mutually exclusive)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace mutex
{
    internal class Program
    {
        private static Mutex _mutex = new Mutex();

        static void Main(string[] args)
        {
            for (int i = 1; i < 4; i++)
            {
                // the thread will execute tFunc()
                Thread t = new Thread(tFunc);
                t.Name = "T" + i;
                Console.WriteLine(t.Name + " has been created!");
                t.Start();
            }

            Console.ReadLine();
            _mutex.Dispose();
        }
        //threaded function
        private static void tFunc()
        {
            Console.WriteLine(Thread.CurrentThread.Name + " is waiting to use the protected resource");
            // threads not using resource wait in WaitOne()
            if(_mutex.WaitOne())
            {
                Console.WriteLine(Thread.CurrentThread.Name + " is using the resource");

                //where you process the shared resource
                Thread.Sleep(3000);

                _mutex.ReleaseMutex();

                Console.WriteLine(Thread.CurrentThread.Name + " has released the mutex resource");


            }
            else
            {
                Console.WriteLine(Thread.CurrentThread.Name + " could not get a hold of the mutex");
            }
        }
    }
}
