﻿// --------------------------------------------------------------------------------------------------
// <copyright file = "EventsUpStreamAttribute.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//    
//    Unless explicitly acquired and licensed from Licensor under another
//    license, the contents of this file are subject to the Reciprocal Public
//    License ("RPL") Version 1.5, or subsequent versions as allowed by the RPL,
//    and You may not copy or use this file in either source code or executable
//    form, except in compliance with the terms and conditions of the RPL.
//    
//    All software distributed under the RPL is provided strictly on an "AS
//    IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND
//    LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
//    LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//    PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RPL for specific
//    language governing rights and limitations under the RPL. 
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Contracts.Attributes
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The log contract.
    /// </summary>
    [DataContract]
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)] // Multiuse attribute.
    public class EventsUpStreamContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventUpStreamAttribute"/> class.
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
        public EventsUpStreamContract(string id, string name, string description)
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