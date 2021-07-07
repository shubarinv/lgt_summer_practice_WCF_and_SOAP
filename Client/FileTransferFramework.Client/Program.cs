using System;
using System.IO;
using Service;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (StreamClient streamClient = new StreamClient())
                {
                    streamClient.Open();

                    using (var fileStream = new FileStream("transfer/test_file.txt", FileMode.Create))
                    {
                        streamClient.GetLargeObject().CopyTo(fileStream);
                    }

                    streamClient.Close();
                }

                Console.WriteLine("Press any key to end");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}