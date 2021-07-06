using System;
using System.ServiceModel;

namespace FileTransferFramework.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var serviceHost = new ServiceHost(typeof(FileTransfer)))
            {
                serviceHost.Open();
                Console.WriteLine("Client Started..");
                Console.ReadKey();
            }
        }
    }
}