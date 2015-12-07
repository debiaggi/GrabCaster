// --------------------------------------------------------------------------------------------------
// <copyright file = "SyncPointMessage.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Contracts.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Contracts.Points;

    /// <summary>
    /// The shell item type.
    /// </summary>
    public enum ShellItemType
    {
        /// <summary>
        /// The host.
        /// </summary>
        Host,

        /// <summary>
        /// The point.
        /// </summary>
        Point,

        /// <summary>
        /// The trigger dll.
        /// </summary>
        TriggerDll,

        /// <summary>
        /// The event dll.
        /// </summary>
        EventDll,

        /// <summary>
        /// The trigger configuration.
        /// </summary>
        TriggerConfiguration,

        /// <summary>
        /// The event configuration.
        /// </summary>
        EventConfiguration
    }

    /// <summary>
    /// The sync point message.
    /// </summary>
    [DataContract]
    [Serializable]
    public class SyncPointMessage
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        [DataMember]
        public Host Host { get; set; }

        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        [DataMember]
        public Point Point { get; set; }

        /// <summary>
        /// Gets or sets the bubbling triggers.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> BubblingTriggers { get; set; }

        /// <summary>
        /// Gets or sets the bubbling events.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> BubblingEvents { get; set; }

        /// <summary>
        /// Gets or sets the triggers.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> DllTriggers { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> DllEvents { get; set; }
    }

    /// <summary>
    /// The host.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Host
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        [DataMember]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}