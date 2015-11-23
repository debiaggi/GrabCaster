// --------------------------------------------------------------------------------------------------
// <copyright file = "EventConfiguration.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Contracts.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Channels;

    /// <summary>
    ///     Trigger event File
    /// </summary>
    [DataContract]
    [Serializable]
    public class EventConfiguration
    {
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        [DataMember]
        public Event Event { get; set; }
    }

    /// <summary>
    /// The event.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="idComponent">
        /// The id component.
        /// </param>
        /// <param name="idConfiguration">
        /// The id configuration.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public Event(string idComponent, string idConfiguration, string name, string description)
        {
            this.IdConfiguration = idConfiguration;
            this.IdComponent = idComponent;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the id component.
        /// </summary>
        [DataMember]
        public string IdComponent { get; set; }

        /// <summary>
        /// Gets or sets the id configuration.
        /// </summary>
        [DataMember]
        public string IdConfiguration { get; set; }

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
        /// Gets or sets the event properties.
        /// </summary>
        [DataMember]
        public List<EventProperty> EventProperties { get; set; }

        /// <summary>
        /// Gets or sets the channels.
        /// </summary>
        [DataMember]
        public List<Channel> Channels { get; set; }

        /// <summary>
        /// Gets or sets the correlation.
        /// </summary>
        [DataMember]
        public Correlation Correlation { get; set; }
    }

    /// <summary>
    /// The event property.
    /// </summary>
    [DataContract]
    [Serializable]
    public class EventProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventProperty"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public EventProperty(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        ///     Property name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [DataMember]
        public object Value { get; set; }
    }

    /// <summary>
    /// The event action.
    /// </summary>
    [DataContract]
    [Serializable]
    public class EventAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventAction"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public EventAction(string id, string name)
        {
            this.Id = id;
            this.Name = name;
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
    }
}