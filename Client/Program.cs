using System;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //FileTransferService client = new FileTransferService();
            try
            {
                //   Console.WriteLine(client.GetMessage("Vladimir"));
            }
            catch (EndpointNotFoundException e)
            {
                Console.WriteLine("Server not available... Did you even start it???");
            }
        }
    }
}