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
                                    Constant.DefconOne,
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
                Constant.DefconOne, 
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