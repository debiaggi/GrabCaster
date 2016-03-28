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
namespace GrabCaster.Framework.Contracts.Events
{
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// Interface for all event action classes.
    /// </summary>
    public interface IEventType
    {
        /// <summary>
        /// Gets or sets the internal context passed to the event (some other event to execute) to use in delegates events.
        /// </summary>
        /// <returns>The internal context passed to the event.</returns>
        EventActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the internal delegate to use in delegates events.
        /// </summary>
        /// <value>
        /// The set event action event.
        /// </value>
        SetEventActionEvent SetEventActionEvent { get; set; }

        /// <summary>
        /// Gets or sets the main default data.
        /// </summary>
        /// <value>
        /// The main default data.
        /// </value>
        byte[] DataContext { get; set; }

        /// <summary>
        /// Performs the execution of the event.
        /// </summary>
        /// <param name="setEventActionEvent">The The internal delegate to use</param>
        /// <param name="context">The internal context passed to the event.</param>
        void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context);
    }
}