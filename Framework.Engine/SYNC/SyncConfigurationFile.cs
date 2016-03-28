// -----------------------------------------------------------------------------------
// 
// GRABCASTER LTD CONFIDENTIAL
// ___________________________
// 
// Copyright © 2013 - 2016 GrabCaster Ltd. All rights reserved.
// This work is registered with the UK Copyright Service: Registration No:284701085
// 
// 
// NOTICE:  All information contained herein is, and remains
// the property of GrabCaster Ltd and its suppliers,
// if any.  The intellectual and technical concepts contained
// herein are proprietary to GrabCaster Ltd
// and its suppliers and may be covered by UK and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from GrabCaster Ltd.
// 
// -----------------------------------------------------------------------------------
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