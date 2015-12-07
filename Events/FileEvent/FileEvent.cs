// --------------------------------------------------------------------------------------------------
// <copyright file = "FileEvent.cs" company="Nino Crudele">
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
                File.WriteAllBytes(this.OutputDirectory + Guid.NewGuid() + ".txt", this.DataContext);
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