// --------------------------------------------------------------------------------------------------
// <copyright file = "TriggerAttributes.cs" company="Nino Crudele">
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
    /// The trigger contract.
    /// </summary>
    [DataContract]
    [Serializable]

    [AttributeUsage(AttributeTargets.Class)] // Multiuse attribute.
    public class TriggerContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerContract"/> class.
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
        /// <param name="pollingRequired">
        /// The polling required.
        /// </param>
        /// <param name="shared">
        /// The shared.
        /// </param>
        /// <param name="nop">
        /// The no operation.
        /// </param>
        public TriggerContract(string id, string name, string description, bool pollingRequired, bool shared, bool nop)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Shared = shared;
            this.PollingRequired = pollingRequired;
            this.Nop = nop;
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
        /// Gets or sets a value indicating whether no operation.
        /// </summary>
        [DataMember]
        public bool Nop { get; set; }
    }

    /// <summary>
    /// The trigger property contract.
    /// </summary>
    [DataContract]
    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    public class TriggerPropertyContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerPropertyContract"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public TriggerPropertyContract(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        ///     Property name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }

    /// <summary>
    /// The trigger action contract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [DataContract]
    [Serializable]
    public class TriggerActionContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerActionContract"/> class.
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
        public TriggerActionContract(string id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        ///     Unique Action ID
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        ///     Method name
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