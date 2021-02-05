using System;
using System.Threading;
using System.Net;

namespace BelowAverage
{
    class Program
    {
        static void Main(string[] Arguments)
        {
            Console.WriteLine("Below Average - sFlow to Elastic Collector - v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("------------------------------------------");
            try
            {
                if(Arguments.Length == 4)
                {
                    Console.WriteLine("Elastic Auth   : Using Basic Auth.");
                    ElasticRelay.ELASTIC_USER = Arguments[2];
                    ElasticRelay.ELASTIC_PASS = Arguments[3];
                }
                Console.WriteLine("Elastic URI    : " + Arguments[0]);
                Console.WriteLine("Elastic Prefix : " + Arguments[1]);
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("Starting...");
                ElasticRelay.URI = Arguments[0];
                ElasticRelay.PREFIX = Arguments[1];
                ElasticRelay.Setup();
                new Listener(IPAddress.Any, 6343).Start();
                Console.WriteLine("Started.");
                Thread.Sleep(-1);
            }
            catch(Exception)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("    sFlowToElasticCollector \"http://elastic:9200\" \"sflow-index-prefix-\" [\"elastic.username\"] [\"elastic.password\"]");
                Console.WriteLine();
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }
    }
}