using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Service
{
    [ServiceContract]
    public interface IStream
    {
        [OperationContract]
        Stream GetLargeObject();

        Task GetChunkedObject(string fileName);
    }

    [MessageContract]
    public class ChunkMsg
    {
        [MessageBodyMember(Order = 5)] public byte[] Chunk;
        [MessageBodyMember(Order = 4)] public int ChunkSize;
        [MessageBodyMember(Order = 1)] public string FileName;
        [MessageBodyMember(Order = 2)] public long FileSize;
        [MessageBodyMember(Order = 3)] public string Md5Cache;
    }
}