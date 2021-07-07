using System;
using System.IO;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace FileTransferFramework.Client
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class FileTransfer : IFileTransfer
    {
        /// <summary>
        /// Push File to the client
        /// </summary>
        /// <param name="fileToPush">file you want to Push</param>
        /// <returns>a file transfer Response</returns>
        public FileTransferResponse Put(FileTransferRequest fileToPush)
        {
            var fileTransferResponse = CheckFileTransferRequest(fileToPush);
            if (fileTransferResponse.ResponseStatus != "FileIsValid") return fileTransferResponse;
            try
            {
                SaveFileStream(
                    System.Configuration.ConfigurationManager.AppSettings["SavedLocation"] + "\\" +
                    fileToPush.FileName, new MemoryStream(fileToPush.Content));

                if (CalculateMd5(System.Configuration.ConfigurationManager.AppSettings["SavedLocation"] + "\\" +
                                 fileToPush.FileName) == fileToPush.Hash)
                {
                    Console.WriteLine("Caches converged!");
                    return new FileTransferResponse
                    {
                        CreateAt = DateTime.Now,
                        FileName = fileToPush.FileName,
                        Message = "File was transferred",
                        ResponseStatus = "Successful"
                    };
                }

                Console.WriteLine("Caches are different, file is invalid!");
                return new FileTransferResponse
                {
                    CreateAt = DateTime.Now,
                    FileName = fileToPush.FileName,
                    Message = "File broken",
                    ResponseStatus = "Wrong MD5"
                };
            }
            catch (Exception ex)
            {
                return new FileTransferResponse
                {
                    CreateAt = DateTime.Now,
                    FileName = fileToPush.FileName,
                    Message = ex.Message,
                    ResponseStatus = "Error"
                };
            }
        }

        /// <summary>
        /// Check From file Transfer Object is not null 
        /// and all properties are set
        /// </summary>
        /// <param name="fileToPush">file to check</param>
        /// <returns>File Transfer Response</returns>
        private FileTransferResponse CheckFileTransferRequest(FileTransferRequest fileToPush)
        {
            if (fileToPush == null)
                return new FileTransferResponse
                {
                    CreateAt = DateTime.Now,
                    FileName = "No Name",
                    Message = " File Can't be Null",
                    ResponseStatus = "Error"
                };
            if (string.IsNullOrEmpty(fileToPush.FileName))
                return new FileTransferResponse
                {
                    CreateAt = DateTime.Now,
                    FileName = "No Name",
                    Message = " File Name Can't be Null",
                    ResponseStatus = "Error"
                };
            if (fileToPush.Content != null)
            {
                return new FileTransferResponse
                {
                    CreateAt = DateTime.Now,
                    FileName = fileToPush.FileName,
                    Message = string.Empty,
                    ResponseStatus = "FileIsValid"
                };
            }

            return new FileTransferResponse
            {
                CreateAt = DateTime.Now,
                FileName = "No Name",
                Message = " File Content is null",
                ResponseStatus = "Error"
            };
        }

        /// <summary>
        /// Write the Stream in the hard drive
        /// </summary>
        /// <param name="filePath">path to write the file in</param>
        /// <param name="stream">stream to write</param>
        private static void SaveFileStream(string filePath, Stream stream)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                stream.CopyTo(fileStream);
                fileStream.Dispose();
            }
            catch (Exception ex)
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