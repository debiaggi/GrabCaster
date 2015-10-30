// --------------------------------------------------------------------------------------------------
// <copyright file = "LogMessage.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog: http://ninocrudele.me
//    
//    By accessing GrabCaster code here, you are agreeing to the following licensing terms.
//    If you do not agree to these terms, do not access the GrabCaster code.
//    Your license to the GrabCaster source and/or binaries is governed by the 
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
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