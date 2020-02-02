using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplicationGUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(8976);
            server.start();
        }
    }
}
