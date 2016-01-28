// --------------------------------------------------------------------------------------------------
// <copyright file = "OnRampEngine.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    Info:   http://grabcaster.io/
// 
//    Unless explicitly acquired and licensed from Licensor under another
//    license, the contents of this file are subject to the Reciprocal Public
//    License ("RPL") Version 1.5, or subsequent versions as allowed by the RPL,
//    and You may not copy or use this file in either source code or executable
//    form, except in compliance with the terms and conditions of the RPL.
//    
//    All software distributed under the RPL is provided strictly on an "AS
//    IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND
//    LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
//    LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//    PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RPL for specific
//    language governing rights and limitations under the RPL. 
//    
//    The Reciprocal Public License 1.5 (RPL1.5) license is described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Engine.OnRamp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Log;

    /// <summary>
    /// The on ramp engine.
    /// </summary>
    public sealed class OnRampEngine : LockSlimQueueEngine<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnRampEngine"/> class.
        /// </summary>
        /// <param name="capLimit">
        /// The cap limit.
        /// </param>
        /// <param name="timeLimit">
        /// The time limit.
        /// </param>
        public OnRampEngine(int capLimit, int timeLimit)
        {
            this.CapLimit = capLimit;
            this.TimeLimit = timeLimit;
            this.InitTimer();
        }
    }

    /// <summary>
    /// The on ramp engine receiving.
    /// </summary>
    public class OnRampEngineReceiving
    {
        /// <summary>
        /// The parameters ret.
        /// </summary>
        private static readonly object[] ParametersRet = { null };

        /// <summary>
        /// The method run.
        /// </summary>
        private static MethodInfo methodRun;

        /// <summary>
        /// The class instance.
        /// </summary>
        private static object classInstance;

        /// <summary>
        /// Create the internal queue
        /// </summary>
        private readonly OnRampEngine onRampEngine;

        /// <summary>
        /// Delegate used to fire the event to enqueue the message.
        /// </summary>
        private SetEventOnRampMessageReceived receiveMessageOnRampDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnRampEngineReceiving"/> class.
        /// </summary>
        public OnRampEngineReceiving()
        {
            this.onRampEngine = new OnRampEngine(
                Configuration.ThrottlingOnRampIncomingRateNumber(), 
                Configuration.ThrottlingOnRampIncomingRateSeconds());
            this.onRampEngine.OnPublish += OnRampEngineOnPublish;
        }

        /// <summary>
        /// Initialize the onramp engine.
        /// </summary>
        /// <param name="offRampPatternComponent">
        /// The off ramp pattern component.
        /// </param>
        public void Init(string offRampPatternComponent)
        {
            if (Configuration.RunLocalOnly())
            {
                LogEngine.WriteLog(Configuration.EngineName,
                                    $"This GrabCaster point is configured for local only execution.",
                                    Constant.ErrorEventIdHighCritical,
                                    Constant.TaskCategoriesError,
                                    null,
                                    EventLogEntryType.Warning);
                return;
            }
            // Delegate event for ingestor where ReceiveMessageOnRamp is the event
            this.receiveMessageOnRampDelegate = this.ReceiveMessageOnRamp;

            LogEngine.WriteLog(
                Configuration.EngineName, 
                "Start On Ramp engine.", 
                Constant.ErrorEventIdHighCritical, 
                Constant.TaskCategoriesError, 
                null, 
                EventLogEntryType.Information);

            // Inizialize the MSPC

            // Load event up stream external component
            var eventsUpStreamComponent = Path.Combine(
                Configuration.DirectoryOperativeRootExeName(), 
                Configuration.EventsStreamComponent());

            // Create the reflection method cached 
            var assembly = Assembly.LoadFrom(eventsUpStreamComponent);

            // Main class logging
            var assemblyClass = (from t in assembly.GetTypes()
                                 let attributes = t.GetCustomAttributes(typeof(EventsDownStreamContract), true)
                                 where t.IsClass && attributes != null && attributes.Length > 0
                                 select t).First();

            var classAttributes = assemblyClass.GetCustomAttributes(typeof(EventsDownStreamContract), true);

            if (classAttributes.Length > 0)
            {
                Debug.WriteLine("EventsDownStreamContract - methodRun caller");
                methodRun = assemblyClass.GetMethod("Run");
            }

            classInstance = Activator.CreateInstance(assemblyClass, null);

            ParametersRet[0] = this.receiveMessageOnRampDelegate;
            methodRun.Invoke(classInstance, ParametersRet);
        }

        /// <summary>
        /// Send the message to the engine message.
        /// </summary>
        /// <param name="objects">
        /// The objects.
        /// </param>
        private static void OnRampEngineOnPublish(List<object> objects)
        {
            foreach (var message in objects)
            {
                // Sent message to the MSPC
                MessageIngestor.IngestMessagge(message);
            }
        }

        /// <summary>
        /// Event fired by the On Ramp engine.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private void ReceiveMessageOnRamp(object message)
        {
            lock (this.onRampEngine)
            {
                this.onRampEngine.Enqueue(message);
            }
        }
    }
}