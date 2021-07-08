using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Service
{
    [ServiceContract]
    public interface IStream
    {
        [OperationContract]
        Stream GetLargeObject();

        [OperationContract(IsOneWay = true)]
        void UploadFile(FileUploadMessage request);

        [OperationContract(IsOneWay = false)]
        FileDownloadReturnMessage DownloadFile(FileDownloadMessage request);
    }

    [MessageContract]
    public class FileUploadMessage
    {
        [MessageBodyMember(Order = 1)] public Stream FileByteStream;

        [MessageHeader(MustUnderstand = true)] public FileMetaData Metadata;
    }

    [MessageContract]
    public class FileDownloadMessage
    {
        [MessageHeader(MustUnderstand = true)] public FileMetaData FileMetaData;
    }

    [MessageContract]
    public class FileDownloadReturnMessage
    {
        [MessageHeader(MustUnderstand = true)] public FileMetaData DownloadedFileMetadata;

        [MessageBodyMember(Order = 1)] public Stream FileByteStream;

        public FileDownloadReturnMessage(FileMetaData metaData, Stream stream)
        {
            this.DownloadedFileMetadata = metaData;
            this.FileByteStream = stream;
        }
    }

    [DataContract]
    public class FileMetaData
    {
        [DataMember(Name = "localFilename", Order = 0, IsRequired = false)]
        public string LocalFileName;

        [DataMember(Name = "remoteFilename", Order = 1, IsRequired = false)]
        public string RemoteFileName;

        public FileMetaData(
            string localFileName,
            string remoteFileName)
        {
            this.LocalFileName = localFileName;
            this.RemoteFileName = remoteFileName;
        }
    }
}