﻿// --------------------------------------------------------------------------------------------------
// <copyright file = "Globals.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
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
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Contracts.Globals
{
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Contracts.Triggers;

    /// <summary>
    /// Global Action Trigger Delegate used by triggers.
    /// </summary>
    /// <param name="_this">
    /// The _this.
    /// </param>
    /// <param name="context">
    /// The context.
    /// </param>
    public delegate void SetEventActionTrigger(ITriggerType _this, EventActionContext context);

    /// <summary>
    /// Global Action Event Delegate used by events.
    /// </summary>
    /// <param name="_this">
    /// The _this.
    /// </param>
    /// <param name="context">
    /// The context.
    /// </param>
    public delegate void SetEventActionEvent(IEventType _this, EventActionContext context);

    /// <summary>
    /// Global On Ramp Delegate used by on ramp receiver.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    public delegate void SetEventOnRampMessageReceived(SkeletonMessage message);

    /// <summary>
    /// Global Off Ramp Delegate used by on ramp sender.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    public delegate void SetEventOnRampMessageSent(object message);

    /// <summary>
    /// The event action context.
    /// </summary>
    public class EventActionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventActionContext"/> class.
        /// </summary>
        /// <param name="bubblingConfiguration">
        /// The bubbling configuration.
        /// </param>
        public EventActionContext(BubblingEvent bubblingConfiguration)
        {
            this.BubblingConfiguration = bubblingConfiguration;
        }

        /// <summary>
        /// Gets or sets the bubbling configuration.
        /// </summary>
        public BubblingEvent BubblingConfiguration { get; set; }
    }
}