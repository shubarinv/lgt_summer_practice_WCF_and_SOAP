using System;
using System.IO;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace Service
{
    [ServiceBehavior]
    public class StreamService : IStream
    {
        public Stream GetLargeObject(string filename)
        {
            // Add path to a big file, this one is 2.5 gb
            string filePath = Path.Combine(Environment.CurrentDirectory, "transfer/" + filename);

            try
            {
                FileStream imageFile = File.OpenRead(filePath);
                return imageFile;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An exception was thrown while trying to open file {filePath}");
                Console.WriteLine("Exception is: ");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public string GetObjectHash(string filename)
        {
            return CalculateMd5("transfer/" + filename);
        }

        public bool WasFileTransferredSuccessfully(string filename, string hash)
        {
            return GetObjectHash(filename) == hash;
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