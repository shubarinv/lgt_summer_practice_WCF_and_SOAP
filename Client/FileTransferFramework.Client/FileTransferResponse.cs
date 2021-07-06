using System;
using System.Runtime.Serialization;

namespace FileTransferFramework.Client
{
    /// <summary>
    /// File Response Object
    /// </summary>    
    public class FileTransferResponse
    {
        [DataMember]
        /// <summary>
        /// Gets or sets File Name
        /// </summary>
        public string FileName { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets Created at
        /// </summary>
        public DateTime CreateAt { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets Message
        /// </summary>
        public string Message { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets Response Status
        /// </summary>               
        public string ResponseStatus { get; set; }
    }
}