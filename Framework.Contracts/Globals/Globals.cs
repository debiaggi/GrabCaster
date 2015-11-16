// --------------------------------------------------------------------------------------------------
// <copyright file = "Globals.cs" company="Nino Crudele">
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