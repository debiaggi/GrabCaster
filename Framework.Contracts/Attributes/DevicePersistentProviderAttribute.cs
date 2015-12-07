// --------------------------------------------------------------------------------------------------
// <copyright file = "DevicePersistentProviderAttribute.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Contracts.Attributes
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Dpp contract.
    /// </summary>
    [DataContract]
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)] // Multiuse attribute.
    public class DevicePersistentProviderContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DevicePersistentProviderContract"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public DevicePersistentProviderContract(string id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

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
    }
}