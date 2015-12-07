// --------------------------------------------------------------------------------------------------
// <copyright file = "ConsoleSyncEventList.cs" company="Nino Crudele">
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
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Points;

    /// <summary>
    /// The console sync event list.
    /// </summary>
    [DataContract]
    public class ConsoleSyncEventList
    {
        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        [DataMember]
        public Point Point { get; set; }

        /// <summary>
        /// Gets or sets the trigger configuration list.
        /// </summary>
        [DataMember]
        public List<TriggerConfiguration> TriggerConfigurationList { get; set; }

        /// <summary>
        /// Gets or sets the event configuration list.
        /// </summary>
        [DataMember]
        public List<EventConfiguration> EventConfigurationList { get; set; }
    }
}