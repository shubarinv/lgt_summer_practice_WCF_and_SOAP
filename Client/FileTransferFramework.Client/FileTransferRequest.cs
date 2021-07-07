using System.Runtime.Serialization;

namespace FileTransferFramework.Client
{
    [DataContract]
    /// <summary>
    /// Transfer Request Object
    /// </summary>
    public class FileTransferRequest
    {
        [DataMember]
        /// <summary>
        /// Gets or sets File Name
        /// </summary>
        public string FileName { get; set; }

        [DataMember] public byte[] Content { get; set; }
        [DataMember] public string Hash { get; set; }
    }
}