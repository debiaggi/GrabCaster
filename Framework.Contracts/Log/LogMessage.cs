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