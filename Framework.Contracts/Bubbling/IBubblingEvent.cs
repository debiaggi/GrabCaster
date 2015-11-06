// --------------------------------------------------------------------------------------------------
// <copyright file = "IBubblingEvent.cs" company="Nino Crudele">
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
    using System.Reflection;
    using GrabCaster.Framework.Contracts.Configuration;

    /// <summary>
    /// The BubblingEvent interface.
    /// </summary>
    internal interface IBubblingEvent
    {
        /// <summary>
        ///     Trigger or Event
        /// </summary>
        BubblingEventType BubblingEventType { get; set; }

        /// <summary>
        ///     If trigger type and a file trigger event is present in the Bubbling directory this prop is true
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        ///     Endpoints destinations
        /// </summary>
        string IdComponent { get; set; }

        /// <summary>
        /// Gets or sets the id configuration.
        /// </summary>
        string IdConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Assembly object ready to invoke (performances)
        /// </summary>
        Assembly AssemblyObject { get; set; }

        /// <summary>
        ///     Internal class type to invoke
        /// </summary>
        Type AssemblyClassType { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shared.
        /// </summary>
        bool Shared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether polling required.
        /// </summary>
        bool PollingRequired { get; set; }

        /// <summary>
        /// Gets or sets the assembly file.
        /// </summary>
        string AssemblyFile { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        List<Property> Properties { get; set; }

        /// <summary>
        /// Gets or sets the base actions.
        /// </summary>
        List<BaseAction> BaseActions { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        List<Event> Events { get; set; }
    }
}