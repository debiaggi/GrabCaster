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
    using System.Text;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Engine.OffRamp;
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
    public static class MessageIngestor
    {
        public delegate void SetConsoleActionEventEmbedded(string DestinationConsolePointId, ISkeletonMessage skeletonMessage);
        /// <summary>
        /// Used internally by the embedded
        /// </summary>
        public static SetConsoleActionEventEmbedded setConsoleActionEventEmbedded { get; set; }


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
                    Constant.DefconOne,
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

            // ****************************IF MESSAGE TYPE = GRABCASTER*************************
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

                    //If using Guid pattern as string some system put an escape character like \ before the end brachet }

                    // Who receive the message
                    LogEngine.ConsoleWriteLine(
                        $"Event received from Sender {senderId} Sender description {senderDescription}",
                        ConsoleColor.DarkCyan);

                    // ****************************IF SAME SENDER*************************
                    //TODO DELETE the  + "debug"
                    if (senderId == Configuration.PointId() + "debug")
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
                    Constant.DefconOne,
                    Constant.TaskCategoriesError,
                    ex,
                    EventLogEntryType.Error);
                return;
            }

            try
            {


                // ****************************GET FROM STORAGE IF REQUIRED*************************
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

                //*******************************************************************
                //              3 IF TYPES EVENT - CONSOLE - REST
                // first area events, second console, third rest
                //*******************************************************************

                // ****************************IF EVENT TYPE*************************
                if (skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString()
                    == Configuration.MessageDataProperty.Event.ToString())
                {

                    // ****************************IF EMBEDED TYPE EXECUTE TRIGGER*************************
                    if (skeletonMessage.Properties[Configuration.MessageDataProperty.Embedded.ToString()].ToString()
                        == "true")
                    {
                        var recChannelId =
                            skeletonMessage.Properties[Configuration.MessageDataProperty.ReceiverChannelId.ToString()].ToString();
                        var recPointId =
                            skeletonMessage.Properties[Configuration.MessageDataProperty.ReceiverPointId.ToString()].ToString();

                        //If using Guid pattern as string some system put an escape character like \ before the end brachet }

                        var reqAvailable = (recChannelId.Contains(Configuration.ChannelId())
                                                && recPointId.Contains(Configuration.PointId()))
                                               || (recChannelId.Contains(Configuration.ChannelAll)
                                                   && recPointId.Contains(Configuration.PointId()))
                                               || (recChannelId.Contains(Configuration.ChannelId())
                                                   && recPointId.Contains(Configuration.ChannelAll))
                                               || (recChannelId.Contains(Configuration.ChannelAll)
                                                   && recPointId.Contains(Configuration.ChannelAll));

                        if (!reqAvailable)
                        {
                            return;
                        }
                        string idConfiguration =
                            skeletonMessage.Properties[Configuration.MessageDataProperty.IdConfiguration.ToString()].ToString();
                        string idComponent =
                            skeletonMessage.Properties[Configuration.MessageDataProperty.IdComponent.ToString()].ToString();

                        try
                        {
                            var triggerSingleInstance =
                                (from trigger in EventsEngine.BubblingTriggerConfigurationsSingleInstance
                                 where trigger.IdComponent == idComponent && trigger.IdConfiguration == idConfiguration
                                 select trigger).First();
                            var bubblingTriggerConfiguration = triggerSingleInstance;
                            LogEngine.ConsoleWriteLine($"Execute trigger idConfiguration {idConfiguration} and idComponent {idComponent}", ConsoleColor.Green);
                            EventsEngine.ExecuteTriggerConfiguration(bubblingTriggerConfiguration, skeletonMessage.Body);

                        }
                        catch (Exception ex)
                        {
                            LogEngine.WriteLog(Configuration.EngineName,
                                            $"Error in {MethodBase.GetCurrentMethod().Name} - ExecuteTriggerConfiguration Error - Missing the idConfiguration {idConfiguration} and idComponent {idComponent}",
                                            Constant.DefconOne,
                                            Constant.TaskCategoriesError,
                                            ex,
                                            EventLogEntryType.Error);
                        }


                        // ****************************IF EMBEDED RETURN HERE*************************
                        return;
                    }

                    // ****************************CAST TO BUBBLING EVENT*************************
                    var eventBubbling = (BubblingEvent)SerializationEngine.ByteArrayToObject(eventDataByte);

                    // ****************************PERSIST MESSAGE IN FOLDER*************************
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

                    // ****************************IF EXIST EVENT TO EXECUTE*************************
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
                        // ****************************NO EVENT RETURN*************************
                        return;
                    }

                    // ****************************EVENT EXIST EXECUTE*************************
                    EventsEngine.ExecuteBubblingActionEvent(
                        eventBubbling,
                        false,
                        skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString());
                    return;
                }



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
                    // ****************************NOT FOR ME*************************
                    return;
                }


                // **************************** SYNC AREA *************************
                //Save in a string to simplify the reading and code
                string OperationTypRequested =
                    skeletonMessage.Properties[Configuration.MessageDataProperty.MessageType.ToString()].ToString();

                //******************* OPERATION CONF BAG- ALL THE CONF FILES AND DLLS ****************************************************************
                //Receive the request to send the bubbling
                if (OperationTypRequested == Configuration.MessageDataProperty.ConsoleRequestSendBubblingBag.ToString())
                {

                    if (!Configuration.DisableExternalEventsStreamEngine())
                    {
                        //If I am console do nothing
                        if (Configuration.IamConsole())
                        {
                            OffRampEngineSending.SendMessageOnRamp(
                                          EventsEngine.bubblingBag,
                                          Configuration.MessageDataProperty.ConsoleBubblingBagToSyncronize,
                                          skeletonMessage.Properties[Configuration.MessageDataProperty.ChannelId.ToString()].ToString(),
                                          skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString(),
                                          null,
                                          null);
                        }

                    }
                    else
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName,
                            "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.",
                            Constant.DefconThree,
                            Constant.TaskCategoriesError,
                            null,
                            EventLogEntryType.Warning);
                    }
                }
                //Receive the request to send the bubbling
                if (OperationTypRequested == Configuration.MessageDataProperty.ConsoleBubblingBagToSyncronize.ToString())
                {

                    if (!Configuration.DisableExternalEventsStreamEngine())
                    {
                        //se non sono console 
                        //metto 
                        if (!Configuration.IamConsole())
                        {
                            byte[] bubblingContent = SerializationEngine.ObjectToByteArray(skeletonMessage.Body);
                            string currentSyncFolder = Configuration.SyncDirectorySyncIn();
                            GrabCaster.Framework.CompressionLibrary.Helpers.CreateFromBytearray(skeletonMessage.Body, currentSyncFolder);
                        }
                        else
                        {
                            setConsoleActionEventEmbedded(skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString(),
                                        skeletonMessage);
                        }
                    }
                    else
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName,
                            "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.",
                            Constant.DefconThree,
                            Constant.TaskCategoriesError,
                            null,
                            EventLogEntryType.Warning);
                    }
                }


            }
            catch (Exception ex)
            {


                LogEngine.WriteLog(Configuration.EngineName,
                                  $"Error in {MethodBase.GetCurrentMethod().Name}",
                                  Constant.DefconOne,
                                  Constant.TaskCategoriesError,
                                  ex,
                                  EventLogEntryType.Error);




            }
        }
    }
}