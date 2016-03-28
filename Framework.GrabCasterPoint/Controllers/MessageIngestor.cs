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
namespace GrabCaster.Framework.Library.Azure
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
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.EmbeddedEvent;
    using GrabCaster.Framework.Serialization.Object;
    using GrabCaster.Framework.Storage;

    /// <summary>
    /// Engine main message ingestor
    /// </summary>
    public static class MessageIngestor
    {


        public static string GroupEventHubsStorageAccountName { get; set; }

        public static string GroupEventHubsStorageAccountKey { get; set; }
        public static string GroupEventHubsName { get; set; }

        
        public static string ChannelId { get; set; }
        public static string PointId { get; set; }

        public delegate void SetEventActionEventEmbedded(byte[] message);
        /// <summary>
        /// Used internally by the embedded
        /// </summary>
        public static SetEventActionEventEmbedded setEventActionEventEmbedded { get; set; }

        public static void Init(SetEventActionEventEmbedded setEventOnRampMessageReceived)
        {
            try
            {

                setEventActionEventEmbedded = setEventOnRampMessageReceived;


            }
            catch (Exception ex)
            {

                LogEngine.TraceError($"Error in {MethodBase.GetCurrentMethod().Name} - Error {ex.Message}");
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
                LogEngine.TraceInformation("GrabCaster embedded message received!");
                // Check message subscription, it must come from engine
                if (skeletonMessage.Properties[Configuration.GrabCasterMessageTypeName].ToString()
                    != Configuration.GrabCasterMessageTypeValue)
                {
                    LogEngine.TraceInformation("Not GrabCaster message type received -DISCARED-");
                    return;
                }
                else
                {
                    // Who sent the message
                    senderId = skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString();
                    senderDescription =
                        skeletonMessage.Properties[Configuration.MessageDataProperty.SenderDescriprion.ToString()].ToString();

                    // Who receive the message
                    LogEngine.TraceInformation($"Event received step1 from Sender {senderId} Sender description {senderDescription}");

                    var receiverChannelId =
                        skeletonMessage.Properties[Configuration.MessageDataProperty.ReceiverChannelId.ToString()].ToString();
                    var receiverPointId =
                        skeletonMessage.Properties[Configuration.MessageDataProperty.ReceiverPointId.ToString()].ToString();
 
                    var requestAvailable = (receiverChannelId.Contains(ChannelId)
                                            && receiverPointId.Contains(PointId))
                                           || (receiverChannelId.Contains("*")
                                               && receiverPointId.Contains(PointId))
                                           || (receiverChannelId.Contains(ChannelId)
                                               && receiverPointId.Contains("*"))
                                           || (receiverChannelId.Contains("*")
                                               && receiverPointId.Contains("*"));
                    LogEngine.TraceInformation($"Event received step1.1 requestAvailable {requestAvailable.ToString()}");

                    if (!requestAvailable)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // If error then not message typeof (no property present.)
                LogEngine.TraceError($"Error in {MethodBase.GetCurrentMethod().Name} - Not GrabCaster message type received (Missing GrabCaster_MessageType_Name properties.) -DISCARED- Error {ex.Message}");
                return;
            }

            // ****************************CHECK MESSAGE TYPE*************************
            // Check if >256, the restore or not
            LogEngine.TraceInformation($"Event received step2 Check if > 256, the restore or not.");
            if ((bool)skeletonMessage.Properties[Configuration.MessageDataProperty.Persisting.ToString()])
            {
                LogEngine.TraceInformation($"Event received step3 it is > 256");
                string messageId = skeletonMessage.Properties[Configuration.MessageDataProperty.MessageId.ToString()].ToString();
                BlobDevicePersistentProvider storagePersistent = new BlobDevicePersistentProvider();

                var ret = storagePersistent.PersistEventFromStorage(
                    messageId,
                    GroupEventHubsStorageAccountName,
                    GroupEventHubsStorageAccountKey,
                    GroupEventHubsName);


                eventDataByte = (byte[])ret;
            }
            else
            {
                LogEngine.TraceInformation($"Event received step4 it is < 256");
                eventDataByte = skeletonMessage.Body;
            }

            LogEngine.TraceInformation($"Event received step5 sent to setEventActionEventEmbedded");
            setEventActionEventEmbedded(eventDataByte);

        }
    }
}