// --------------------------------------------------------------------------------------------------
// <copyright file = "SyncPointFile.cs" company="Nino Crudele">
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
//    
//    This work is registered with the UK Copyright Service.
//    Registration No:284695248  
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Contracts.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Configuration;

    /// <summary>
    /// The sync point file.
    /// </summary>
    [DataContract]
    [Serializable]
    public class SyncPointFile
    {
        /// <summary>
        /// Gets or sets the bubbling event type.
        /// </summary>
        [DataMember]
        public BubblingEventType BubblingEventType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        [DataMember]
        public Version Version { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shared.
        /// </summary>
        [DataMember]
        public bool Shared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether polling required.
        /// </summary>
        [DataMember]
        public bool PollingRequired { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        [DataMember]
        public List<Property> Properties { get; set; }

        /// <summary>
        /// Gets or sets the base actions.
        /// </summary>
        [DataMember]
        public List<BaseAction> BaseActions { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        [DataMember]
        public List<Event> Events { get; set; }
    }
}