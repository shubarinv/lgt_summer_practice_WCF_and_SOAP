using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorldClient
{
    class Program
    {
        static void Main(string[] args)
        {
            HelloWorldServiceClient client = new HelloWorldServiceClient();
            try
            {
                Console.WriteLine(client.GetMessage("Vladimir"));
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}",e.Message);
            }
        }
    }
}
