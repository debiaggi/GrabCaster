﻿// --------------------------------------------------------------------------------------------------
// <copyright file = "NopEvent.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    Info:   http://grabcaster.io/
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
namespace GrabCaster.Framework.NopEvent
{
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The no operation event.
    /// </summary>
    [EventContract("{D1EC2907-56A9-474B-B08A-750E72F0C29D}", "NOP Event", "No operation Event, used for testing purpose.", true)]
    public class NopEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public EventActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action event.
        /// </summary>
        public SetEventActionEvent SetEventActionEvent { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="setEventActionEvent">
        /// The set event action event.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [EventActionContract("{5A42100D-7D63-43AA-B141-C658F5BFFB59}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
        }
    }
}