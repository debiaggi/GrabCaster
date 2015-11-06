// --------------------------------------------------------------------------------------------------
// <copyright file = "OnRampEngine.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Copyright (c) 2013 - 2015 Nino Crudele
//    Blog: http://ninocrudele.me
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License. 
// </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Engine.OnRamp
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Engine.OnRamp.Azure;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Engine.OnRamp.Azure.Sync;
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
            // Delegate event for ingestor where ReceiveMessageOnRamp is the event
            this.receiveMessageOnRampDelegate = this.ReceiveMessageOnRamp;

            LogEngine.WriteLog(
                Configuration.EngineName,
                "Start On Ramp engine.",
                Constant.ErrorEventIdHighCritical,
                Constant.TaskCategoriesError,
                null,
                EventLogEntryType.Information);
            DirectEventsDownstream.Run(this.receiveMessageOnRampDelegate);
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