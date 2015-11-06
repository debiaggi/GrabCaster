// --------------------------------------------------------------------------------------------------
// <copyright file = "Correlation.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Contracts.Bubbling
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Contracts.Configuration;

    /// <summary>
    /// The correlation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Correlation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Correlation"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="scriptRule">
        /// The script rule.
        /// </param>
        /// <param name="events">
        /// The events.
        /// </param>
        public Correlation(string name, string scriptRule, List<Event> events)
        {
            this.Name = name;
            this.ScriptRule = scriptRule;
            this.Events = events;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the script rule.
        /// </summary>
        [DataMember]
        public string ScriptRule { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        [DataMember]
        public List<Event> Events { get; set; }
    }
}