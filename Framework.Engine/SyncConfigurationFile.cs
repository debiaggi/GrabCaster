// --------------------------------------------------------------------------------------------------
// <copyright file = "LogMessage.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog: http://ninocrudele.me
//    
//    By accessing GrabCaster code here, you are agreeing to the following licensing terms.
//    If you do not agree to these terms, do not access the GrabCaster code.
//    Your license to the GrabCaster source and/or binaries is governed by the 
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Engine
{
    using System;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Base;

    /// <summary>
    /// The sync configuration file.
    /// </summary>
    [DataContract]
    [Serializable]
    public class SyncConfigurationFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncConfigurationFile"/> class.
        /// </summary>
        /// <param name="fileType">
        /// The file type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="fileContent">
        /// The file content.
        /// </param>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        public SyncConfigurationFile(
            Configuration.MessageDataProperty fileType,
            string name,
            byte[] fileContent,
            string channelId)
        {
            this.FileType = fileType;
            this.Name = name;
            this.FileContent = fileContent;
            this.ChannelId = channelId;
        }

        /// <summary>
        /// Gets or sets the file type.
        /// </summary>
        [DataMember]
        public Configuration.MessageDataProperty FileType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the file content.
        /// </summary>
        [DataMember]
        public byte[] FileContent { get; set; }

        /// <summary>
        /// Gets or sets the channel id.
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }
    }
}