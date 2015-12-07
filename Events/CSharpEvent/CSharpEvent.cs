// --------------------------------------------------------------------------------------------------
// <copyright file = "CSharpEvent.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.CSharpEvent
{
    using System.Collections.Generic;
    using System.IO;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    using Roslyn.Compilers;
    using Roslyn.Scripting.CSharp;

    /// <summary>
    /// The c sharp event.
    /// </summary>
    [EventContract("{A54EDF55-52E9-4D03-B14B-F5D438AF43F1}", "Execute a CSharp Event", "Execute a CSharp Event", true)]
    public class CSharpEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the script.
        /// </summary>
        [EventPropertyContract("Script", "Script to execute")]
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the script file.
        /// </summary>
        [EventPropertyContract("ScriptFile", "Script from file")]
        public string ScriptFile { get; set; }

        /// <summary>
        /// Gets or sets the message properties.
        /// </summary>
        [EventPropertyContract("MessageProperties", "MessageProperties")]
        public Dictionary<string, object> MessageProperties { get; set; }

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
        [EventActionContract("{3855F421-9451-45BD-8379-980F93FBB587}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                this.Context = context;
                this.SetEventActionEvent = setEventActionEvent;
                var script = string.Empty;
                var metaProvider = new MetadataFileProvider();
                metaProvider.GetReference(context.GetType().Assembly.Location);
                var scriptEngine = new ScriptEngine(metaProvider);

                var session = scriptEngine.CreateSession(context);

                // TODO 1040
                session.AddReference(
                    @"C:\Users\ninoc\Documents\Visual Studio 2015\Projects\HybridIntegrationServices\Framework\bin\Debug\Framework.exe");
                session.AddReference(
                    @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Windows.Forms.dll");
                
                // TODO 1041
                if (this.ScriptFile != null || this.ScriptFile != string.Empty)
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    script = File.ReadAllText(this.ScriptFile);
                    session.ExecuteFile(script);
                }
                else
                {
                    session.Execute(this.Script);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}