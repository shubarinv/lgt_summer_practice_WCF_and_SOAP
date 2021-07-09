using System.IO;
using System.ServiceModel;

namespace Service
{
    [ServiceContract]
    public interface IStream
    {
        [OperationContract]
        Stream GetLargeObject(string filename);

        [OperationContract]
        string GetObjectHash(string filename);

        [OperationContract]
        bool WasFileTransferredSuccessfully(string filename, string hash);
    }
}