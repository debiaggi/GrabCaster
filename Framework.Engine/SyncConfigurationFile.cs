// --------------------------------------------------------------------------------------------------
// <copyright file = "SyncConfigurationFile.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Copyright (c) 2013 - 2015 Nino Crudele
//    Blog: http://ninocrudele.me
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License. 
// </summary>
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