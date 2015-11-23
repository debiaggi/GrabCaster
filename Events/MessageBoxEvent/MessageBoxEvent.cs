// --------------------------------------------------------------------------------------------------
// <copyright file = "MessageBoxEvent.cs" company="Nino Crudele">
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
//    
//    This work is registered with the UK Copyright Service.
//    Registration No:284695248  
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.MessageBoxEvent
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Windows.Forms;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The message box event.
    /// </summary>
    [EventContract("{DC78440A-E82B-4A5C-B4C5-C78CC40472BD}", "MessageBox Event", "Show a messagebox", true)]
    public class MessageBoxEvent : IEventType
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
        [EventActionContract("{1B28FEDB-3940-463D-BA80-223B4C40FE31}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                Debug.WriteLine("In MessageBoxEvent Event.");
                var message = Encoding.UTF8.GetString(this.DataContext);
                MessageBox.Show(message);
                setEventActionEvent(this, context);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MessageBoxEvent error > " + ex.Message);
                setEventActionEvent(this, null);
            }
        }
    }
}