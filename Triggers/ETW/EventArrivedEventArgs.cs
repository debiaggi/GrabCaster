﻿// --------------------------------------------------------------------------------------------------
// <copyright file = "EventArrivedEventArgs.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.ETW
{
    using System;

    public sealed class EventArrivedEventArgs : EventArgs
    {
        // Keep this event small.
        /// <summary>
        /// Initializes a new instance of the <see cref="EventArrivedEventArgs"/> class.
        /// </summary>
        /// <param name="error">
        /// The error.
        /// </param>
        internal EventArrivedEventArgs(Exception error)
            : this(0 /*eventId*/, new PropertyBag())
        {
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArrivedEventArgs"/> class.
        /// </summary>
        /// <param name="eventId">
        /// The event id.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        internal EventArrivedEventArgs(ushort eventId, PropertyBag properties)
        {
            this.EventId = eventId;
            this.Properties = properties;
        }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        public ushort EventId { get; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public PropertyBag Properties { get; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Exception Error { get; }
    }
}