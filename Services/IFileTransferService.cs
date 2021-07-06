using System;
using System.ServiceModel;

[ServiceContract]
public interface IFileTransferService
{
    [OperationContract]
    RemoteFileInfo DownloadFile(DownloadRequest request);

    [OperationContract]
    void UploadFile(RemoteFileInfo request);
}

[MessageContract]
public class DownloadRequest
{
    [MessageBodyMember] public string FileName;
}

[MessageContract]
public class RemoteFileInfo : IDisposable
{
    [MessageBodyMember(Order = 1)] public System.IO.Stream FileByteStream;
    [MessageHeader(MustUnderstand = true)] public string FileName;

    [MessageHeader(MustUnderstand = true)] public long Length;

    public void Dispose()
    {
        if (FileByteStream == null) return;
        FileByteStream.Close();
        FileByteStream = null;
    }
}