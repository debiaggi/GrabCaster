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