// --------------------------------------------------------------------------------------------------
// <copyright file = "OffRampEngine.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Engine.OffRamp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Engine.OnRamp;
    using GrabCaster.Framework.Log;
    using GrabCaster.Framework.Serialization.Object;
    using Microsoft.ServiceBus.Messaging;

    using Newtonsoft.Json;

    /// <summary>
    /// Internal messaging Queue
    /// </summary>
    public sealed class OffRampEngine : LockSlimQueueEngine<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OffRampEngine"/> class.
        /// </summary>
        /// <param name="capLimit">
        /// TODO The cap limit.
        /// </param>
        /// <param name="timeLimit">
        /// TODO The time limit.
        /// </param>
        public OffRampEngine(int capLimit, int timeLimit)
        {
            this.CapLimit = capLimit;
            this.TimeLimit = timeLimit;
            this.InitTimer();
        }
    }

    /// <summary>
    /// Last line of receiving and first before the message ingestor
    /// </summary>
    public static class OffRampEngineSending
    {
        /// <summary>
        /// The off ramp engine.
        /// </summary>
        private static OffRampEngine offRampEngine;

        /// <summary>
        /// The method create event up stream.
        /// </summary>
        private static MethodInfo methodCreateEventUpStream;

        /// <summary>
        /// The method send message.
        /// </summary>
        private static MethodInfo methodSendMessage;

        /// <summary>
        /// The class instance.
        /// </summary>
        private static object classInstance;

        private static object classInstanceDpp;
        private static bool secondaryPersistProviderEnabled;
        private static int secondaryPersistProviderByteSize;
        private static MethodInfo methodPersistEventToBlob;
        
        /// <summary>
        /// The parameters ret.
        /// </summary>
        private static readonly object[] ParametersSendMessage = { null };

        /// <summary>
        /// The parameters ret.
        /// </summary>
        private static readonly object[] ParametersCreateEventUpStream = { null,null };

        /// <summary>
        /// Initialize the onramp engine the OffRampPatternComponent variable is for the next version
        /// </summary>
        /// <param name="offRampPatternComponent">
        /// The Off Ramp Pattern Component.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Init(string offRampPatternComponent)
        {
            try
            {
                if(Configuration.RunLocalOnly())
                {
                    LogEngine.WriteLog(Configuration.EngineName,
                                        $"This GrabCaster point is configured for local only execution.",
                                        Constant.ErrorEventIdHighCritical,
                                        Constant.TaskCategoriesError,
                                        null,
                                        EventLogEntryType.Warning);
                    return true;
                }
                LogEngine.ConsoleWriteLine("Initialize Abstract Event Up Stream Engine.", ConsoleColor.Yellow);

                // Load event up stream external component
                var eventsUpStreamComponent = Path.Combine(
                    Configuration.DirectoryOperativeRootExeName(), 
                    Configuration.EventsStreamComponent());

                // Create the reflection method cached 
                var assembly = Assembly.LoadFrom(eventsUpStreamComponent);

                // Main class logging
                var assemblyClass = (from t in assembly.GetTypes()
                                     let attributes = t.GetCustomAttributes(typeof(EventsUpStreamContract), true)
                                     where t.IsClass && attributes != null && attributes.Length > 0
                                     select t).First();

                var classAttributes = assemblyClass.GetCustomAttributes(typeof(EventsUpStreamContract), true);

                if (classAttributes.Length > 0)
                {
                    Debug.WriteLine("EventsUpStreamContract - methodCreateEventUpStream caller");
                    methodCreateEventUpStream = assemblyClass.GetMethod("CreateEventUpStream");
                    Debug.WriteLine("EventsUpStreamContract - methodSendMessage caller");
                    methodSendMessage = assemblyClass.GetMethod("SendMessage");
                }

                classInstance = Activator.CreateInstance(assemblyClass, null);

                offRampEngine = new OffRampEngine(
                    Configuration.ThrottlingOffRampIncomingRateNumber(), 
                    Configuration.ThrottlingOffRampIncomingRateSeconds());
                offRampEngine.OnPublish += OffRampEngineOnPublish;

                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    "Start Off Ramp Engine.", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesError, 
                    null, 
                    EventLogEntryType.Information);

                // Inizialize the Dpp
                LogEngine.ConsoleWriteLine("Initialize Abstract Storage Provider Engine.", ConsoleColor.Yellow);
                methodCreateEventUpStream.Invoke(classInstance, null);

                secondaryPersistProviderEnabled= Configuration.SecondaryPersistProviderEnabled();
                secondaryPersistProviderByteSize = Configuration.SecondaryPersistProviderByteSize();

                // Load the abrstracte persistent provider
                var devicePersistentProviderComponent = Path.Combine(
                                    Configuration.DirectoryOperativeRootExeName(),
                                    Configuration.PersistentProviderComponent());

                // Create the reflection method cached 
                var assemblyPersist = Assembly.LoadFrom(devicePersistentProviderComponent);

                // Main class logging
                var assemblyClassDpp = (from t in assemblyPersist.GetTypes()
                                     let attributes = t.GetCustomAttributes(typeof(DevicePersistentProviderContract), true)
                                     where t.IsClass && attributes != null && attributes.Length > 0
                                     select t).First();

                var classAttributeDpp = assemblyClassDpp.GetCustomAttributes(typeof(DevicePersistentProviderContract), true);

                if (classAttributeDpp.Length > 0)
                {
                    Debug.WriteLine("DevicePersistentProviderContract - methodPersistEvent caller");
                    methodPersistEventToBlob = assemblyClassDpp.GetMethod("PersistEventToStorage");
                }

                classInstanceDpp = Activator.CreateInstance(assemblyClassDpp, null);

                return true;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return false;
            }
        }

        /// <summary>
        /// TODO The send message on ramp.
        /// </summary>
        /// <param name="bubblingTriggerConfiguration">
        /// TODO The bubbling trigger configuration.
        /// </param>
        /// <param name="ehMessageType">
        /// TODO The eh message type.
        /// </param>
        /// <param name="channelId">
        /// TODO The channel id.
        /// </param>
        /// <param name="pointId">
        /// TODO The point id.
        /// </param>
        /// <param name="properties">
        /// TODO The properties.
        /// </param>
        public static void SendMessageOnRamp(
            object bubblingTriggerConfiguration, 
            Configuration.MessageDataProperty ehMessageType, 
            string channelId, 
            string pointId, 
            Dictionary<string, object> properties,
            string pointIdOverrided)
        {
            try
            {
                if (Configuration.RunLocalOnly())
                {
                    LogEngine.WriteLog(Configuration.EngineName,
                                        $"Impossible to send the message using a remote message storage provider, this GrabCaster point is configured for local only execution.",
                                        Constant.ErrorEventIdHighCritical,
                                        Constant.TaskCategoriesError,
                                        null,
                                        EventLogEntryType.Warning);
                    return;
                }

                // Meter and measuring purpose
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                byte[] serializedMessage = null;
                // Create EH data message
                if (ehMessageType != Configuration.MessageDataProperty.ByteArray)
                {
                    serializedMessage = SerializationEngine.ObjectToByteArray(bubblingTriggerConfiguration);
                }
                else
                {
                    serializedMessage = (byte[]) bubblingTriggerConfiguration;
                }

                var messageId = Guid.NewGuid().ToString();
                SkeletonMessage data = new SkeletonMessage(null);

                // IF > 256kb then persist

                if (serializedMessage.Length > secondaryPersistProviderByteSize && secondaryPersistProviderEnabled)
                {
                    data.Body = Encoding.UTF8.GetBytes(messageId);
                    ParametersCreateEventUpStream[0] = serializedMessage;
                    ParametersCreateEventUpStream[1] = messageId;
                    methodPersistEventToBlob.Invoke(classInstanceDpp, ParametersCreateEventUpStream);
                    data.Properties.Add(Configuration.MessageDataProperty.Persisting.ToString(), true);
                }
                else
                {
                    data.Body = serializedMessage;
                    data.Properties.Add(Configuration.MessageDataProperty.Persisting.ToString(), false);
                }
                // Load custome Properties
                if (properties != null)
                {
                    foreach (var prop in properties)
                    {
                        data.Properties.Add(prop.Key, prop.Value);
                    }
                }

                data.Properties.Add(Configuration.MessageDataProperty.MessageId.ToString(), messageId);

                // Set main security subscription
                data.Properties.Add(Configuration.GrabCasterMessageTypeName, Configuration.GrabCasterMessageTypeValue);

                // Message context
                data.Properties.Add(
                    Configuration.MessageDataProperty.Message.ToString(), 
                    Configuration.MessageDataProperty.Message.ToString());
                data.Properties.Add(Configuration.MessageDataProperty.MessageType.ToString(), ehMessageType.ToString());

                string senderid = pointIdOverrided != null? pointIdOverrided : Configuration.PointId();
                data.Properties.Add(Configuration.MessageDataProperty.SenderId.ToString(), senderid);

                data.Properties.Add(Configuration.MessageDataProperty.SenderName.ToString(), Configuration.PointName());
                data.Properties.Add(
                    Configuration.MessageDataProperty.SenderDescriprion.ToString(), 
                    Configuration.PointDescription());
                data.Properties.Add(Configuration.MessageDataProperty.ChannelId.ToString(), Configuration.ChannelId());
                data.Properties.Add(
                    Configuration.MessageDataProperty.ChannelName.ToString(), 
                    Configuration.ChannelName());
                data.Properties.Add(
                    Configuration.MessageDataProperty.ChannelDescription.ToString(), 
                    Configuration.ChannelDescription());

                data.Properties.Add(Configuration.MessageDataProperty.ReceiverChannelId.ToString(), channelId);
                data.Properties.Add(Configuration.MessageDataProperty.ReceiverPointId.ToString(), pointId);

                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                data.Properties.Add(Configuration.MessageDataProperty.OperationTime.ToString(), ts.Milliseconds);

                lock (offRampEngine)
                {
                    if (ehMessageType == Configuration.MessageDataProperty.Event
                        || ehMessageType == Configuration.MessageDataProperty.Trigger)
                    {
                        var bubblingEvent = (BubblingEvent)bubblingTriggerConfiguration;
                        if (Configuration.LoggingVerbose())
                        {
                            var serializedEvents = JsonConvert.SerializeObject(
                                bubblingEvent.Events, 
                                Formatting.Indented, 
                                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                            LogEngine.ConsoleWriteLine(
                                $"Sent Message Type {ehMessageType}sent - Endpoints: {serializedEvents}", 
                                ConsoleColor.Green);
                        }
                        else
                        {
                            LogEngine.ConsoleWriteLine($"Sent Message Type {ehMessageType}", ConsoleColor.Green);
                        }
                    }
                    else
                    {
                        LogEngine.ConsoleWriteLine($"Sent Message Type {ehMessageType}", ConsoleColor.Green);
                    }
                }
                offRampEngine.Enqueue(data);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.ErrorEventIdHighCriticalEventHubs, 
                    Constant.TaskCategoriesEventHubs, 
                    ex, 
                    EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Send a message service as Sync received or sync available in json format
        ///     e un messaggio di servizio, tipo, sync disponibile eccetera, non e importantissimo
        /// </summary>
        /// <param name="ehMessageType">
        /// The EH Message Type.
        /// </param>
        /// <param name="channelId">
        /// The Channel ID.
        /// </param>
        /// <param name="pointId">
        /// The Point ID.
        /// </param>
        /// <param name="idComponent">
        /// The ID Component.
        /// </param>
        /// <param name="subscriberId">
        /// The subscriber ID.
        /// </param>
        public static void SendNullMessageOnRamp(
            Configuration.MessageDataProperty ehMessageType, 
            string channelId, 
            string pointId, 
            string idComponent, 
            string subscriberId,
            string pointIdOverrided)
        {
            try
            {
                if (Configuration.RunLocalOnly())
                {
                    LogEngine.WriteLog(Configuration.EngineName,
                                        $"Impossible to send the message using a remote message storage provider, this GrabCaster point is configured for local only execution.",
                                        Constant.ErrorEventIdHighCritical,
                                        Constant.TaskCategoriesError,
                                        null,
                                        EventLogEntryType.Warning);
                    return;
                }

                // Meter and measuring purpose
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var data = new SkeletonMessage(Encoding.UTF8.GetBytes(string.Empty));
                data.Properties.Add(Configuration.MessageDataProperty.Persisting.ToString(), false);

                // Set main security subscription
                data.Properties.Add(Configuration.GrabCasterMessageTypeName, Configuration.GrabCasterMessageTypeValue);

                data.Properties.Add(Configuration.MessageDataProperty.MessageId.ToString(), Guid.NewGuid().ToString());
                data.Properties.Add(
                    Configuration.MessageDataProperty.Message.ToString(), 
                    Configuration.MessageDataProperty.Message.ToString());
                data.Properties.Add(Configuration.MessageDataProperty.SubscriberId.ToString(), subscriberId);
                data.Properties.Add(Configuration.MessageDataProperty.MessageType.ToString(), ehMessageType.ToString());

                string senderid = pointIdOverrided != null ? pointIdOverrided : Configuration.PointId();
                data.Properties.Add(Configuration.MessageDataProperty.SenderId.ToString(), senderid);
                data.Properties.Add(Configuration.MessageDataProperty.SenderName.ToString(), Configuration.PointName());
                data.Properties.Add(
                    Configuration.MessageDataProperty.SenderDescriprion.ToString(), 
                    Configuration.PointDescription());
                data.Properties.Add(Configuration.MessageDataProperty.ChannelId.ToString(), Configuration.ChannelId());
                data.Properties.Add(
                    Configuration.MessageDataProperty.ChannelName.ToString(), 
                    Configuration.ChannelName());
                data.Properties.Add(
                    Configuration.MessageDataProperty.ChannelDescription.ToString(), 
                    Configuration.ChannelDescription());
                data.Properties.Add(Configuration.MessageDataProperty.ReceiverChannelId.ToString(), channelId);
                data.Properties.Add(Configuration.MessageDataProperty.ReceiverPointId.ToString(), pointId);
                data.Properties.Add(Configuration.MessageDataProperty.IdComponent.ToString(), idComponent);

                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                data.Properties.Add(Configuration.MessageDataProperty.OperationTime.ToString(), ts.Milliseconds);

                // Queue the data
                lock (offRampEngine)
                {
                    offRampEngine.Enqueue(data);
                }

                LogEngine.ConsoleWriteLine(
                    $"Sent Message Type: {ehMessageType} - To ChannelID: {channelId} PointID: {pointId}", 
                    ConsoleColor.DarkMagenta);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.ErrorEventIdHighCriticalEventHubs, 
                    Constant.TaskCategoriesEventHubs, 
                    ex, 
                    EventLogEntryType.Error);
            }
        }



 
        /// <summary>
        /// TODO The off ramp engine on publish.
        /// </summary>
        /// <param name="objects">
        /// TODO The objects.
        /// </param>
        private static void OffRampEngineOnPublish(List<object> objects)
        {
            foreach (var message in objects)
            {
                // Send message to MSPC 
                ParametersSendMessage[0] = message;
                methodSendMessage.Invoke(classInstance, ParametersSendMessage);
            }
        }
    }
}