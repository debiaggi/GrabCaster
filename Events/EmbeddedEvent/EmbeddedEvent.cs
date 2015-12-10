
// --------------------------------------------------------------------------------------------------
// <copyright file = "NopEvent.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.EmbeddedEvent
{
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The no operation event.
    /// </summary>
    [EventContract("{A31209D7-C989-4E5D-93DA-BD341D843870}", "Embedded Event", "Event use in the enbedded components.", true)]
    public class EmbeddedEvent : IEventType
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
        [EventActionContract("{C783836A-73B8-41C2-B072-B37240C1A224}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            setEventActionEvent(this, context);
        }
    }
}