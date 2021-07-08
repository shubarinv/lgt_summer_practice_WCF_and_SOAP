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

        public void UploadFile(FileUploadMessage request)
        {
            // parameters validation omitted for clarity

            try
            {
                string basePath = "transfer/";
                string serverFileName = Path.Combine(basePath, request.Metadata.RemoteFileName);

                using (FileStream outfile = new FileStream(serverFileName, FileMode.Create))
                {
                    const int bufferSize = 65536; // 64K


                    Byte[] buffer = new Byte[bufferSize];
                    int bytesRead = request.FileByteStream.Read(buffer, 0, bufferSize);

                    while (bytesRead > 0)
                    {
                        outfile.Write(buffer, 0, bytesRead);
                        bytesRead = request.FileByteStream.Read(buffer, 0, bufferSize);
                    }
                }
            }
            catch (IOException e)
            {
                throw;
            }
        }

        public FileDownloadReturnMessage DownloadFile(FileDownloadMessage request)
        {
            // parameters validation omitted for clarity

            string localFileName = request.FileMetaData.LocalFileName;

            try
            {
                string basePath = "transfer/";
                string serverFileName = Path.Combine(basePath, request.FileMetaData.RemoteFileName);

                Stream fs = new FileStream(serverFileName, FileMode.Open);

                return new FileDownloadReturnMessage(new FileMetaData(localFileName, serverFileName), fs);
            }
            catch (IOException e)
            {
                throw;
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