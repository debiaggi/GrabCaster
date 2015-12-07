// --------------------------------------------------------------------------------------------------
// <copyright file = "IBubblingEvent.cs" company="Nino Crudele">
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