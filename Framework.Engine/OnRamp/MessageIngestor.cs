// --------------------------------------------------------------------------------------------------
// <copyright file = "MessageIngestor.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
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
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Log;
    using GrabCaster.Framework.Serialization;
    using GrabCaster.Framework.Serialization.Object;
    using GrabCaster.Framework.Storage;

    using Microsoft.Data.Edm;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json;

    /// <summary>
    /// Engine main message ingestor
    /// </summary>
    internal static class MessageIngestor
    {
        private static object classInstanceDpp;
        private static bool secondaryPersistProviderEnabled;
        private static int secondaryPersistProviderByteSize;
        private static MethodInfo methodPersistEventFromBlob;
        private static readonly object[] ParametersPersistEventFromBlob = { null };

        public static void Init()
        {
            try
            {

                secondaryPersistProviderEnabled = Configuration.SecondaryPersistProviderEnabled();
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
                    methodPersistEventFromBlob = assemblyClassDpp.GetMethod("PersistEventFromStorage");
                   
                }

                classInstanceDpp = Activator.CreateInstance(assemblyClassDpp, null);

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
        public static void IngestMessagge(object message)
        {
            string senderId;
            string senderDescription;
            byte[] eventDataByte = null;
            var skeletonMessage = (ISkeletonMessage)message;

            // ****************************CHECK MESSAGE TYPE*************************
            try
            {
                // Check message subscription, it must come from engine
                if (skeletonMessage.Properties[Configuration.GrabCasterMessageTypeName].ToString()
                    != Configuration.GrabCasterMessageTypeValue)
                {
                    LogEngine.ConsoleWriteLine(
                        "Not GrabCaster message type received -DISCARED-", 
                        ConsoleColor.DarkYellow);
                    return;
                }
                else
                {
                    // Who sent the message
                    senderId = skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString();
                    senderDescription =
                        skeletonMessage.Properties[Configuration.MessageDataProperty.SenderDescriprion.ToString()].ToString();

                    // Who receive the message
                    LogEngine.ConsoleWriteLine(
                        $"Event received from Sender {senderId} Sender description {senderDescription}", 
                        ConsoleColor.DarkCyan);

                    if (senderId == Configuration.PointId())
                    {
                        LogEngine.ConsoleWriteLine("Same sender ID event discared.", ConsoleColor.Green);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // If error then not message typeof (no property present.)
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name} - Not GrabCaster message type received (Missing GrabCaster_MessageType_Name properties.) -DISCARED-", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return;
            }

            // ****************************CHECK MESSAGE TYPE*************************
            // Check if >256, the restore or not
            if ((bool)skeletonMessage.Properties[Configuration.MessageDataProperty.Persisting.ToString()])
            {
                ParametersPersistEventFromBlob[0] = skeletonMessage.Properties[Configuration.MessageDataProperty.MessageId.ToString()];
                var ret = methodPersistEventFromBlob.Invoke(classInstanceDpp, ParametersPersistEventFromBlob);
                eventDataByte = (byte[])ret;
            }
            else
            {
                eventDataByte = skeletonMessage.Body;
            }

            // Message Type Event,
            if (skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.Event.ToString())
            {
                var eventBubbling = (BubblingEvent)SerializationEngine.ByteArrayToObject(eventDataByte);
                PersistentProvider.PersistMessage(eventBubbling, PersistentProvider.CommunicationDiretion.OffRamp);

                if (Configuration.LoggingVerbose())
                {
                    var serializedEvents = JsonConvert.SerializeObject(
                        eventBubbling.Events, 
                        Formatting.Indented, 
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    LogEngine.ConsoleWriteLine(
                        $"Event received from point id: {skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()]} -point name : {skeletonMessage.Properties[Configuration.MessageDataProperty.SenderDescriprion.ToString()]} - {serializedEvents}", 
                        ConsoleColor.Green);
                }

                var eventsAvailable = from eventbubble in eventBubbling.Events
                                      from channel in eventbubble.Channels
                                      from point in channel.Points
                                      where
                                          (channel.ChannelId == Configuration.ChannelId()
                                           && point.PointId == Configuration.PointId())
                                          || (channel.ChannelId == Configuration.ChannelAll
                                              && point.PointId == Configuration.PointId())
                                          || (channel.ChannelId == Configuration.ChannelId()
                                              && point.PointId == Configuration.PointAll)
                                          || (channel.ChannelId == Configuration.ChannelAll
                                              && point.PointId == Configuration.PointAll)
                                      select eventbubble;

                if (!eventsAvailable.Any())
                {
                    return;
                }

                EventsEngine.ExecuteBubblingActionEvent(
                    eventBubbling, 
                    false, 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString());
                return;
            }

            // *******************************Sync and service messages area**********************************************************
            var receiverChannelId =
                skeletonMessage.Properties[Configuration.MessageDataProperty.ReceiverChannelId.ToString()].ToString();
            var receiverPointId =
                skeletonMessage.Properties[Configuration.MessageDataProperty.ReceiverPointId.ToString()].ToString();

            var requestAvailable = (receiverChannelId == Configuration.ChannelId()
                                    && receiverPointId == Configuration.PointId())
                                   || (receiverChannelId == Configuration.ChannelAll
                                       && receiverPointId == Configuration.PointId())
                                   || (receiverChannelId == Configuration.ChannelId()
                                       && receiverPointId == Configuration.PointAll)
                                   || (receiverChannelId == Configuration.ChannelAll
                                       && receiverPointId == Configuration.PointAll);

            if (!requestAvailable)
            {
                return;
            }

            // Send the local bubbling configuration
            if (skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendBubblingConfiguration.ToString())
            {
                // eventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendBubblingConfiguration from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                var syncConfigurationFilelIst =
                    (List<SyncConfigurationFile>)SerializationEngine.ByteArrayToObject(eventDataByte);
                SyncProvider.SyncBubblingConfigurationFileList(
                    syncConfigurationFilelIst, 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.SenderName.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.SenderDescriprion.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.ChannelName.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.ChannelDescription.ToString()].ToString());
                return;
            }

            // Request to send the local configuration
            // Send the configuration
            if (skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendRequestBubblingConfiguration.ToString())
            {
                // EventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendBubblingConfiguration from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                SyncProvider.SyncSendBubblingConfiguration(
                    skeletonMessage.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString());
                return;
            }

            // Request to send the  configuration
            // Send the configuration
            if (skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendRequestConfiguration.ToString())
            {
                // EventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendRequestConfiguration from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                SyncProvider.SyncSendConfiguration(
                    skeletonMessage.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString());
                return;
            }

            // Send the  configuration
            // Send the configuration
            if (skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendConfiguration.ToString())
            {
                // EventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendConfiguration from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                SyncProvider.SyncWriteConfiguration(
                    skeletonMessage.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString(), 
                    eventDataByte);
                return;
            }

            // Send a configuration file to update in the official bubbling folder
            // Write the file in bubbling
            if (skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendFileBubblingConfiguration.ToString())
            {
                // EventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendFileBubblingConfiguration from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                var syncConfigurationFilelIst =
                    (List<SyncConfigurationFile>)SerializationEngine.ByteArrayToObject(eventDataByte);
                SyncProvider.SyncLocalBubblingConfigurationFile(syncConfigurationFilelIst);
                return;
            }

            // Request to send back a component
            if (skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendRequestComponent.ToString())
            {
                // EventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendRequestComponent from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                SyncProvider.SyncSendComponent(
                    skeletonMessage.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.IdComponent.ToString()].ToString());
                return;
            }

            // Request to update a component
            if (skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendComponent.ToString())
            {
                // EventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendComponent from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                var eventBubbling = (BubblingEvent)SerializationEngine.ByteArrayToObject(eventDataByte);
                SyncProvider.SyncUpdateComponent(
                    skeletonMessage.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString(), 
                    eventBubbling);
                return;
            }

            // Send a configuration file to update in the official bubbling folder
            // Write the file in bubbling
            if (skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                != Configuration.MessageDataProperty.SyncSendFileBubblingConfiguration.ToString())
            {
                return;
            }
            // eventDataByte = NULL
            LogEngine.ConsoleWriteLine(
                $"SyncSendFileBubblingConfiguration from - {senderId} - {senderDescription}", 
                ConsoleColor.Cyan);
            var syncConfigurationFileList =
                (List<SyncConfigurationFile>)SerializationEngine.ByteArrayToObject(eventDataByte);
            SyncProvider.SyncLocalBubblingConfigurationFile(syncConfigurationFileList);
        }
    }
}