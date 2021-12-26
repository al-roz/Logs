using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerModel server = new ServerModel();
            server.StartServer();
        }
    }
}