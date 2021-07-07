using System;
using System.IO;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    [ServiceBehavior]
    public class StreamService : IStream
    {
        public Stream GetLargeObject()
        {
            // Add path to a big file, this one is 2.5 gb
            string filePath = Path.Combine(Environment.CurrentDirectory, "transfer/test_file.txt");

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

        public async Task GetChunkedObject(string fileName)
        {
            string filePath = "transfer/" + fileName;
            if (File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                var chunk = new ChunkMsg
                {
                    FileName = fileName,
                    FileSize = fileInfo.Length,
                    Md5Cache = CalculateMd5(filePath),
                };
                const int chunkSize = 64 * 1024;
                var fileBytes = File.ReadAllBytes(filePath);
                var fileChunk = new byte[chunkSize];
                var offset = 0;

                while (offset < fileBytes.Length)
                {
                    var length = Math.Min(chunkSize, fileBytes.Length - offset);
                    Buffer.BlockCopy(fileBytes, offset, fileChunk, 0, length);

                    offset += length;

                    chunk.ChunkSize = length;
                    chunk.Chunk = fileChunk;
                }
            }
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