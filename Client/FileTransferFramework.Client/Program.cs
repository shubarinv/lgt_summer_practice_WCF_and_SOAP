using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
                    UploadFile("test_file100MB.txt", streamClient);
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

        private static void IssueDownloadRequest(string localFile, FileDownloadMessage request, StreamClient service)
        {
            try
            {
                using (FileDownloadReturnMessage response = service.DownloadFile(request))
                {
                    if (response != null && response.FileByteStream != null)
                    {
                        SaveFile(response.FileByteStream, localFile);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                // we expect the stream returned from the server to be closed by the

                // server itself so nothing to be done with it here. Just abort

                // the proxy if needed.
            }
        }

        private static void SaveFile(Stream saveFile, string localFilePath)
        {
            const int bufferSize = 65536; // 64K


            using (FileStream outfile = new FileStream(localFilePath, FileMode.Create))
            {
                byte[] buffer = new byte[bufferSize];
                int bytesRead = saveFile.Read(buffer, 0, bufferSize);

                while (bytesRead > 0)
                {
                    outfile.Write(buffer, 0, bytesRead);
                    bytesRead = saveFile.Read(buffer, 0, bufferSize);
                }
            }
        }

        public static void UploadFile(string localFileName, StreamClient service)
        {
            try
            {
                Console.WriteLine("Stated File transfer");
                using (Stream fileStream = new FileStream(localFileName, FileMode.Open, FileAccess.Read))
                {
                    var request = new FileUploadMessage();
                    string remoteFileName = null;

                    remoteFileName = Path.GetFileName(localFileName);

                    var fileMetadata = new FileMetaData
                    {
                        localFilename = localFileName, remoteFilename = remoteFileName
                    };

                    request.Metadata = fileMetadata;
                    request.FileByteStream = fileStream;

                    service.UploadFile(request);
                    Console.WriteLine("Ended File transfer");
                }
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
            }
        }

        private static string CalculateMd5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead("transfers/" + filename))
                {
                    return Encoding.Default.GetString(md5.ComputeHash(stream));
                }
            }
        }
    }
}