using System;
using System.Threading;
using PENet;

namespace BinSocket
{
    static class Protocol
    {
        public static string IP = "127.0.0.1";
        public static int PORT = 1685;
        static void Main(string[] args)
        {
            Server server = new Server( );
          
            while (true)
                Thread.Sleep(1000);
        }
    }
}
