using System;
using System.IO;

public class FileTransferService : IFileTransferService
{
    public RemoteFileInfo DownloadFile(DownloadRequest request)
    {
        var result = new RemoteFileInfo();
        try
        {
            var filePath = Path.Combine("download/", request.FileName);
            var fileInfo = new FileInfo(filePath);

            // check if exists
            if (!fileInfo.Exists)
                throw new FileNotFoundException("File not found",
                    request.FileName);

            // open stream
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            // return result 
            result.FileName = request.FileName;
            result.Length = fileInfo.Length;
            result.FileByteStream = stream;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in FileTransferService::DownloadFile: {0}", ex.Message);
        }

        return result;
    }

    public void UploadFile(RemoteFileInfo request)
    {
        FileStream targetStream = null;
        var sourceStream = request.FileByteStream;

        const string uploadFolder = "upload/";

        var filePath = Path.Combine(uploadFolder, request.FileName);

        using (targetStream = new FileStream(filePath, FileMode.Create,
            FileAccess.Write, FileShare.None))
        {
            //read from the input stream in 65000 byte chunks

            const int bufferLen = 65000;
            var buffer = new byte[bufferLen];
            var count = 0;
            while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
            {
                // save to output stream
                targetStream.Write(buffer, 0, count);
            }

            targetStream.Close();
            sourceStream.Close();
        }
    }
}