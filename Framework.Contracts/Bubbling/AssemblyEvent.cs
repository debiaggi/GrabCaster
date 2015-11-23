// --------------------------------------------------------------------------------------------------
// <copyright file = "AssemblyEvent.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Contracts.Bubbling
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The assembly event.
    /// </summary>
    [DataContract]
    [Serializable]
    public class AssemblyEvent
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        [DataMember]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the path file name.
        /// </summary>
        [DataMember]
        public string PathFileName { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the assembly content.
        /// </summary>
        [DataMember]
        public byte[] AssemblyContent { get; set; }
    }
}