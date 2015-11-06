// --------------------------------------------------------------------------------------------------
// <copyright file = "LogMessage.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Contracts.Log
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// The message level.
    /// </summary>
    public enum MessageLevel
    {
        /// <summary>
        /// The information.
        /// </summary>
        Information,

        /// <summary>
        /// The warning.
        /// </summary>
        Warning,

        /// <summary>
        /// The error.
        /// </summary>
        Error
    }

    /// <summary>
    /// The log message.
    /// </summary>
    [DataContract]
    [Serializable]
    public class LogMessage : TableEntity
    {
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        [DataMember]
        public EventLogEntryType Level { get; set; }

        /// <summary>
        /// Gets or sets the message id.
        /// </summary>
        [DataMember]
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        [DataMember]
        public string DateTime { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the channel id.
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the channel name.
        /// </summary>
        [DataMember]
        public string ChannelName { get; set; }

        /// <summary>
        /// Gets or sets the event id.
        /// </summary>
        [DataMember]
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the task category.
        /// </summary>
        [DataMember]
        public string TaskCategory { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception object.
        /// </summary>
        [DataMember]
        public string ExceptionObject { get; set; }
    }
}