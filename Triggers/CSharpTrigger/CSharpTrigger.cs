// --------------------------------------------------------------------------------------------------
// <copyright file = "CSharpTrigger.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.CSharpTrigger
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;

    using Roslyn.Compilers;
    using Roslyn.Scripting.CSharp;

    /// <summary>
    /// The c sharp trigger.
    /// </summary>
    [TriggerContract("{928647A2-9BB3-4D9C-8C4D-C63181AC1686}", "CSharp Trigger",
        "Execute a trigger write in CSharp script", false, true, false)]
    public class CSharpTrigger : ITriggerType
    {
        /// <summary>
        /// Gets or sets the script.
        /// </summary>
        [TriggerPropertyContract("Script", "Script to execute")]
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the script file.
        /// </summary>
        [TriggerPropertyContract("ScriptFile", "Script from file")]
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
        /// Gets or sets the set event action trigger.
        /// </summary>
        public SetEventActionTrigger SetEventActionTrigger { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [TriggerPropertyContract("DataContext", "Trigger Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="setEventActionTrigger">
        /// The set event action trigger.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [TriggerActionContract("{00437935-DB38-426B-BF4D-A101BD64E96F}", "Main action", "Main action description")]
        public void Execute(SetEventActionTrigger setEventActionTrigger, EventActionContext context)
        {
            try
            {
                var script = string.Empty;
                var metaProvider = new MetadataFileProvider();
                metaProvider.GetReference(context.GetType().Assembly.Location);
                var scriptEngine = new ScriptEngine(metaProvider);

                var session = scriptEngine.CreateSession(context);

                session.AddReference(
                    @"C:\Users\ninoc\Documents\Visual Studio 2015\Projects\HybridIntegrationServices\Framework\bin\Debug\Framework.exe");
                session.AddReference(
                    @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Windows.Forms.dll");

                if (this.ScriptFile != null || this.ScriptFile != string.Empty)
                {
                    // TODO 1020
                    // ReSharper disable once AssignNullToNotNullAttribute
                    script = File.ReadAllText(this.ScriptFile);
                    session.ExecuteFile(script);
                }
                else
                {
                    session.Execute(this.Script);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}