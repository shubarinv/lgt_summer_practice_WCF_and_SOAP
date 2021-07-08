using System;
using System.ServiceModel;
using Service;

namespace FileTrainsferFramework.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var serviceHost = new ServiceHost(typeof(StreamService)))
            {
                serviceHost.Open();

                Console.WriteLine("Press Any Key to end");
                Console.ReadKey();
                serviceHost.Close();
            }
        }
    }
}