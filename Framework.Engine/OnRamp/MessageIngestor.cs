// --------------------------------------------------------------------------------------------------
// <copyright file = "MessageIngestor.cs" company="Nino Crudele">
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Log;
    using GrabCaster.Framework.Serialization;
    using GrabCaster.Framework.Storage;

    using Microsoft.ServiceBus.Messaging;

    using Newtonsoft.Json;

    /// <summary>
    /// Engine main message ingestor
    /// </summary>
    internal static class MessageIngestor
    {
        public static void IngestMessagge(object message)
        {
            string senderId;
            string senderDescription;
            byte[] eventDataByte = null;
            var eventData = (EventData)message;

            // ****************************CHECK MESSAGE TYPE*************************
            try
            {
                // Check message subscription, it must come from engine
                if (eventData.Properties[Configuration.GrabCasterMessageTypeName].ToString()
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
                    senderId = eventData.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString();
                    senderDescription =
                        eventData.Properties[Configuration.MessageDataProperty.SenderDescriprion.ToString()].ToString();

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
            if ((bool)Common.GetMessageContextPropertyValue(eventData, Configuration.MessageDataProperty.Persisting))
            {
                eventDataByte =
                    PersistentProvider.PersistEventFromBlob(
                        Common.GetMessageContextPropertyValue(eventData, Configuration.MessageDataProperty.MessageId)
                            .ToString());
            }
            else
            {
                eventDataByte = eventData.GetBytes();
            }

            // Message Type Event,
            if (eventData.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
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
                        $"Event received from point id: {eventData.Properties[Configuration.MessageDataProperty.SenderId.ToString()]} -point name : {eventData.Properties[Configuration.MessageDataProperty.SenderDescriprion.ToString()]} - {serializedEvents}", 
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
                    eventData.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString());
                return;
            }

            // *******************************Sync and service messages area**********************************************************
            var receiverChannelId =
                eventData.Properties[Configuration.MessageDataProperty.ReceiverChannelId.ToString()].ToString();
            var receiverPointId =
                eventData.Properties[Configuration.MessageDataProperty.ReceiverPointId.ToString()].ToString();

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
            if (eventData.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendBubblingConfiguration.ToString())
            {
                // Arriva eventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendBubblingConfiguration from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                var syncConfigurationFilelIst =
                    (List<SyncConfigurationFile>)SerializationEngine.ByteArrayToObject(eventDataByte);
                SyncProvider.SyncBubblingConfigurationFileList(
                    syncConfigurationFilelIst, 
                    eventData.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.SenderName.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.SenderDescriprion.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.ChannelName.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.ChannelDescription.ToString()].ToString());
                return;
            }

            // Request to send the local configuration
            // Send the configuration
            if (eventData.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendRequestBubblingConfiguration.ToString())
            {
                // EventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendBubblingConfiguration from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                SyncProvider.SyncSendBubblingConfiguration(
                    eventData.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString());
                return;
            }

            // Request to send the  configuration
            // Send the configuration
            if (eventData.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendRequestConfiguration.ToString())
            {
                // EventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendRequestConfiguration from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                SyncProvider.SyncSendConfiguration(
                    eventData.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString());
                return;
            }

            // Send the  configuration
            // Send the configuration
            if (eventData.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendConfiguration.ToString())
            {
                // EventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendConfiguration from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                SyncProvider.SyncWriteConfiguration(
                    eventData.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString(), 
                    eventDataByte);
                return;
            }

            // Send a configuration file to update in the official bubbling folder
            // Write the file in bubbling
            if (eventData.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
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
            if (eventData.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendRequestComponent.ToString())
            {
                // EventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendRequestComponent from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                SyncProvider.SyncSendComponent(
                    eventData.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.IdComponent.ToString()].ToString());
                return;
            }

            // Request to update a component
            if (eventData.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                == Configuration.MessageDataProperty.SyncSendComponent.ToString())
            {
                // EventDataByte = NULL
                LogEngine.ConsoleWriteLine(
                    $"SyncSendComponent from - {senderId} - {senderDescription}", 
                    ConsoleColor.Cyan);
                var eventBubbling = (BubblingEvent)SerializationEngine.ByteArrayToObject(eventDataByte);
                SyncProvider.SyncUpdateComponent(
                    eventData.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(), 
                    eventData.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString(), 
                    eventBubbling);
                return;
            }

            // Send a configuration file to update in the official bubbling folder
            // Write the file in bubbling
            if (eventData.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                != Configuration.MessageDataProperty.SyncSendFileBubblingConfiguration.ToString())
            {
                return;
            }
            // Arriva eventDataByte = NULL
            LogEngine.ConsoleWriteLine(
                $"SyncSendFileBubblingConfiguration from - {senderId} - {senderDescription}", 
                ConsoleColor.Cyan);
            var syncConfigurationFileList =
                (List<SyncConfigurationFile>)SerializationEngine.ByteArrayToObject(eventDataByte);
            SyncProvider.SyncLocalBubblingConfigurationFile(syncConfigurationFileList);
        }
    }
}