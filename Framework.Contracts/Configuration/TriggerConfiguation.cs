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
namespace GrabCaster.Framework.Contracts.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Base;

    using Newtonsoft.Json;

    /// <summary>
    ///     Trigger configuration File, To create a configuration file trigger who able to activate a trigger action/s
    /// </summary>
    [DataContract]
    [Serializable]
    public class TriggerConfiguration
    {
        /// <summary>
        /// Gets or sets the trigger.
        /// </summary>
        [DataMember]
        public Trigger Trigger { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        [DataMember]
        public List<Event> Events { get; set; }

        /// <summary>
        /// The create trigger event.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed. Suppression is OK here.")]
        public void CreateTriggerEvent()
        {
            var serializedMessage = JsonConvert.SerializeObject(this);
            File.WriteAllText(
                Path.Combine(
                    Configuration.DirectoryBubblingTriggers(),
                    string.Concat(
                        this.Trigger.Name,
                        "_", 
                        Guid.NewGuid().ToString(), 
                        Configuration.BubblingTriggersExtension)), 
                serializedMessage);
        }
    }

    /// <summary>
    /// The trigger.
    /// </summary>
    [DataContract,Serializable]
    public class Trigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Trigger"/> class.
        /// </summary>
        /// <param name="idComponent">
        /// The id component.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public Trigger(string idComponent, string idConfiguration, string name, string description)
        {
            this.IdComponent = idComponent;
            this.IdConfiguration = idConfiguration;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the id component.
        /// </summary>
        [DataMember]
        public string IdComponent { get; set; }

        /// <summary>
        /// Get or set the id configuration
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
        /// Gets or sets the trigger properties.
        /// </summary>
        [DataMember]
        public List<TriggerProperty> TriggerProperties { get; set; }
    }

    /// <summary>
    /// The trigger property.
    /// </summary>
    [DataContract, Serializable]
    public class TriggerProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerProperty"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public TriggerProperty(string name, object value)
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
    /// The trigger action.
    /// </summary>
    [DataContract, Serializable]
    public class TriggerAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerAction"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public TriggerAction(string id, string name)
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