using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tracksStackd;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;

namespace tracks.shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var listeningOn = args.Length == 0 ? "http://localhost:1337/" : args[0];
            var appHost = new AppHost();
            appHost.Init();
            appHost.InitDb();
            appHost.Start(listeningOn);

            Console.WriteLine("AppHost Created at {0}, listening on {1}", DateTime.Now, listeningOn);
            Console.ReadKey();
        }
    }
}
