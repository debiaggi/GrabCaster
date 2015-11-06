// --------------------------------------------------------------------------------------------------
// <copyright file = "TriggerConfiguation.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Copyright (c) 2013 - 2015 Nino Crudele
//    Blog: http://ninocrudele.me
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License. 
// </summary>
// --------------------------------------------------------------------------------------------------
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
    [DataContract]
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
        public Trigger(string idComponent, string name, string description)
        {
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
    [DataContract]
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
    [DataContract]
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