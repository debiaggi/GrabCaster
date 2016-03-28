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