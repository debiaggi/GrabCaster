// --------------------------------------------------------------------------------------------------
// <copyright file = "LogMessage.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    Info:   http://GrabCaster.io
// 
//    By accessing GrabCaster code here, you are agreeing to the following licensing terms.
//    If you do not agree to these terms, do not access the GrabCaster code.
//    Your license to the GrabCaster source and/or binaries is governed by the 
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//    
//    This work is registered with the UK Copyright Service.
//    Registration No:284695248  
//  </summary>
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