using System.ServiceModel;

namespace FileTransferFramework.Client
{
    [ServiceContract]
    public interface IFileTransfer
    {
        [OperationContract]
        FileTransferResponse Put(FileTransferRequest fileToPush);
    }
}