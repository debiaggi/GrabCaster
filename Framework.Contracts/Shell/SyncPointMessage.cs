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