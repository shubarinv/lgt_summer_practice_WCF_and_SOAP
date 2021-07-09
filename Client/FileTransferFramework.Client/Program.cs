using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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

                    RequestFile("test_file1GB.txt", streamClient);
                    RequestFile("test_file1GB.txt", streamClient);
                    RequestFile("test_file1GB.txt", streamClient);
                    RequestFile("test_file1GB.txt", streamClient);
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

        static void RequestFile(string filename, IStream streamClient)
        {
            var timer = new Stopwatch();
            timer.Start();


            Console.WriteLine("Started transfer of {0}", filename);
            using (var fileStream = new FileStream("transfer/" + filename, FileMode.Create))
            {
                streamClient.GetLargeObject(filename).CopyTo(fileStream);
            }

            timer.Stop();
            var timeTaken = timer.Elapsed;
            Console.WriteLine("File {0} was transferred", filename);
            Console.WriteLine("Time taken: " + timeTaken.ToString(@"G"));
            Console.WriteLine(
                streamClient.WasFileTransferredSuccessfully(filename, CalculateMd5("transfer/" + filename))
                    ? "Caches converged! File transfer was successful!"
                    : "Caches are different :( File is broken");
            Console.WriteLine("===============");
        }

        private static string CalculateMd5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return Encoding.Default.GetString(md5.ComputeHash(stream));
                }
            }
        }
    }
}