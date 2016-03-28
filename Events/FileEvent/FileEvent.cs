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
namespace GrabCaster.Framework.FileEvent
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Serialization;

    /// <summary>
    /// The file event.
    /// </summary>
    [EventContract("{D438C746-5E75-4D59-B595-8300138FB1EA}", "Write File", "Write the content in a file in a specific folder.", true)]
    public class FileEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        [EventPropertyContract("OutputDirectory", "When the file has to be created")]
        public string OutputDirectory { get; set; }

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
        [EventActionContract("{1FBD0C6E-1A49-4BEF-8876-33A21B23C933}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                Debug.WriteLine("In FileEvent Event.");
                File.WriteAllBytes(this.OutputDirectory + Guid.NewGuid() + ".txt", this.DataContext == null?new byte[0]: this.DataContext);
                this.DataContext = Serialization.ObjectToByteArray(true);
                setEventActionEvent(this, context);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FileEvent error > " + ex.Message);
                setEventActionEvent(this, null);
            }
        }
    }
}