// --------------------------------------------------------------------------------------------------
// <copyright file = "EventAttributes.cs" company="Nino Crudele">
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

    /// <summary>
    /// The event contract.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class EventContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventContract"/> class.
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
        /// <param name="shared">
        /// The shared.
        /// </param>
        public EventContract(string id, string name, string description, bool shared)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Shared = shared;
        }

        /// <summary>
        ///     Unique Action ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Method name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shared.
        /// </summary>
        public bool Shared { get; set; }
    }

    /// <summary>
    /// The event property contract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EventPropertyContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventPropertyContract"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public EventPropertyContract(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        ///     Method name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// The event action contract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EventActionContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventActionContract"/> class.
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
        public EventActionContract(string id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        ///     Unique Action ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Method name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}