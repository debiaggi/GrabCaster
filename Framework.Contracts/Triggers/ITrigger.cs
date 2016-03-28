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
namespace GrabCaster.Framework.Contracts.Triggers
{
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The TriggerType interface.
    /// </summary>
    public interface ITriggerType
    {
        /// <summary>
        /// Internal Trigger context.
        /// </summary>
        EventActionContext Context { get; set; }

        /// <summary>
        ///     internal delegate to use in delegates events
        /// </summary>
        SetEventActionTrigger SetEventActionTrigger { get; set; }

        /// <summary>
        ///     Main default data property
        /// </summary>
        byte[] DataContext { get; set; }

        /// <summary>
        /// Main default method
        /// </summary>
        /// <param name="setEventActionTrigger">
        /// The set Event Action Trigger.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        void Execute(SetEventActionTrigger setEventActionTrigger, EventActionContext context);
    }
}