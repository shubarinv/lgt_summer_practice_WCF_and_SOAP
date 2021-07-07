using System;
using System.IO;
using System.Threading.Tasks;
using Service;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using (StreamClient streamClient = new StreamClient())
                {
                    streamClient.Open();

                    await ReadStream(streamClient);
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

        static async Task ReadStream(StreamClient streamClient)
        {
            using (var fileStream = new FileStream("transfer/test_file.txt", FileMode.Create))
            {
                const int bufferSize = 65536; // 64K


                Byte[] buffer = new Byte[bufferSize];
                var bytesRead = streamClient.GetLargeObject().ReadAsync(buffer, 0, bufferSize);
                long transferred = bytesRead.Result;
                while (bytesRead.Result > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead.Result);
                    bytesRead = streamClient.GetLargeObject().ReadAsync(buffer, 0, bufferSize);
                    transferred += bytesRead.Result;
                    Console.WriteLine("Transferred: {0} Mb ({1} bytes)", transferred / 1024 / 1024, transferred);
                }
            }
        }
    }
}