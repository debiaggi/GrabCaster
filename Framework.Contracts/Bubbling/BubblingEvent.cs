// --------------------------------------------------------------------------------------------------
// <copyright file = "BubblingEvent.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
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
//    
//    The Reciprocal Public License 1.5 (RPL1.5) license is described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Contracts.Bubbling
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;
    using GrabCaster.Framework.Contracts.Configuration;

    /// <summary>
    /// The bubbling event type.
    /// </summary>
    public enum BubblingEventType
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
        /// The event.
        /// </summary>
        Event,

        /// <summary>
        /// The event configuration file.
        /// </summary>
        EventConfigurationFile,

        /// <summary>
        /// The trigger.
        /// </summary>
        Trigger,

        /// <summary>
        /// The trigger configuration file.
        /// </summary>
        TriggerConfigurationFile
    }

    /// <summary>
    ///     Trigger Bubbling
    /// </summary>
    [DataContract]
    [Serializable]
    public class BubblingEvent : IBubblingEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BubblingEvent"/> class.
        /// </summary>
        public BubblingEvent()
        {
            this.BaseActions = new List<BaseAction>();
            this.Events = new List<Event>();
            this.Properties = new List<Property>();
        }

        /// <summary>
        /// Gets or sets the message id.
        /// </summary>
        [DataMember]
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        [DataMember]
        public Version Version { get; set; }

        /// <summary>
        /// Gets or sets the assembly content.
        /// </summary>
        public byte[] AssemblyContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether no operation.
        /// </summary>
        [DataMember]
        public bool Nop { get; set; }

        /// <summary>
        /// Gets or sets the correlation.
        /// </summary>
        [DataMember]
        public Correlation Correlation { get; set; }

        /// <summary>
        /// Gets or sets the correlation override.
        /// </summary>
        [DataMember]
        public Correlation CorrelationOverride { get; set; }

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
        ///     Assembly object ready to invoke (performances)
        /// </summary>
        public Assembly AssemblyObject { get; set; }

        /// <summary>
        ///     Internal class type to invoke
        /// </summary>
        public Type AssemblyClassType { get; set; }

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
        /// Gets or sets the assembly file.
        /// </summary>
        [DataMember]
        public string AssemblyFile { get; set; }

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

        /// <summary>
        /// The get clone.
        /// </summary>
        /// <param name="bubblingEvent">
        /// The _ bubbling event.
        /// </param>
        /// <returns>
        /// The <see cref="BubblingEvent"/>.
        /// </returns>
        public BubblingEvent GetClone(BubblingEvent bubblingEvent)
        {
            var bubblingEventClone = new BubblingEvent
                                         {
                                             BubblingEventType = bubblingEvent.BubblingEventType,
                                             IsActive = bubblingEvent.IsActive,
                                             IdComponent = bubblingEvent.IdComponent,
                                             IdConfiguration = bubblingEvent.IdConfiguration,
                                             Version = bubblingEvent.Version,
                                             Name = bubblingEvent.Name,
                                             AssemblyObject = bubblingEvent.AssemblyObject,
                                             AssemblyContent = bubblingEvent.AssemblyContent,
                                             AssemblyClassType = bubblingEvent.AssemblyClassType,
                                             Description = bubblingEvent.Description,
                                             Shared = bubblingEvent.Shared,
                                             PollingRequired = bubblingEvent.PollingRequired,
                                             Nop = bubblingEvent.Nop,
                                             AssemblyFile = bubblingEvent.AssemblyFile
                                         };

            // Copy Properties memberwiseClone for actions,events and correlations
            var bubblingEventMemberwiseClone = (BubblingEvent)this.MemberwiseClone();

            bubblingEventClone.BaseActions = bubblingEventMemberwiseClone.BaseActions;
            bubblingEventClone.Correlation = bubblingEventMemberwiseClone.Correlation;
            bubblingEventClone.Events = bubblingEventMemberwiseClone.Events;

            // The properties are always different
            bubblingEventClone.Properties = new List<Property>();

            foreach (var property in bubblingEvent.Properties)
            {
                var internalProperty = new Property(
                    property.Name,
                    property.Description,
                    property.AssemblyPropertyInfo,
                    property.Type,
                    null);
                bubblingEventClone.Properties.Add(internalProperty);
            }

            return bubblingEventClone;
        }
    }
}