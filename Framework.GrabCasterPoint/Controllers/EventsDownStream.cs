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
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Messaging;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;



    /// <summary>
    ///     Main Downstream events receiving
    ///     It execute the main DownStream Instance
    /// </summary>
    public class EventsDownStream
    {
        private MessageIngestor.SetEventActionEventEmbedded SetEventActionEventEmbedded;

        public string groupEventHubsStorageAccountName { get; set; }

        public string groupEventHubsStorageAccountKey { get; set; }

        public void Run(MessageIngestor.SetEventActionEventEmbedded setEventActionEventEmbedded, 
                                        string groupEventHubsStorageAccountName,
                                        string groupEventHubsStorageAccountKey,
                                        string AzureNameSpaceConnectionString,
                                        string GroupEventHubsName,
                                        string ChannelId,
                                        string PointId)
        {
            try
            {
                MessageIngestor.GroupEventHubsStorageAccountName = groupEventHubsStorageAccountName;
                MessageIngestor.GroupEventHubsStorageAccountKey = groupEventHubsStorageAccountKey;
                MessageIngestor.GroupEventHubsName = GroupEventHubsName;
                MessageIngestor.ChannelId = ChannelId;
                MessageIngestor.PointId =PointId;

                SetEventActionEventEmbedded = setEventActionEventEmbedded;

                //Load message ingestor
                MessageIngestor.Init(SetEventActionEventEmbedded);
                // Assign the delegate 
           
                // Load vars
                var eventHubConnectionString = AzureNameSpaceConnectionString;
                var eventHubName = GroupEventHubsName;

                LogEngine.TraceInformation($"Start GrabCaster DownStream - Point Id {PointId} - Channel Id {ChannelId}");

                var builder = new ServiceBusConnectionStringBuilder(eventHubConnectionString)
                                  {
                                      TransportType =
                                          TransportType.Amqp
                                  };

                //If not exit it create one, drop brachets because Azure rules
                var eventHubConsumerGroup = string.Concat(GrabCasterPointAPIApp.Controllers.Constants.EngineName, "_", ChannelId.Replace("{", "").Replace("}", "").Replace("-", ""));

                var nsManager = NamespaceManager.CreateFromConnectionString(builder.ToString());

                LogEngine.TraceInformation($"Start DirectRegisterEventReceiving. - Initializing Group Name {eventHubConsumerGroup}");

                // Create Event Hubs
                var eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);
                // Create consumer
                nsManager.CreateConsumerGroupIfNotExists(eventHubName, eventHubConsumerGroup);

                var namespaceManager = NamespaceManager.CreateFromConnectionString(builder.ToString());
                var ehDescription = namespaceManager.GetEventHub(eventHubName);
                // Use the default consumer group

                foreach (var partitionId in ehDescription.PartitionIds)
                {
                    var myNewThread =
                        new Thread(() => ReceiveDirectFromPartition(eventHubClient, partitionId, eventHubConsumerGroup));
                    myNewThread.Start();
                }

                LogEngine.TraceInformation("After DirectRegisterEventReceiving Downstream running.");
            }
            catch (Exception ex)
            {
                LogEngine.TraceError($"Error in {MethodBase.GetCurrentMethod().Name} - Hint: Check if the firewall outbound port 5671 is opened. - Error {ex.Message}");
            }
        }

        private static void ReceiveDirectFromPartition(
            EventHubClient eventHubClient,
            string partitionId,
            string consumerGroup)
        {
            try
            {
                var group = eventHubClient.GetConsumerGroup(consumerGroup);
                EventHubReceiver receiver = null;
                receiver = group.CreateReceiver(partitionId, DateTime.UtcNow);
                LogEngine.TraceInformation($"Direct Receiver created. Partition {partitionId}");
                while (true)
                {
                    var message = receiver?.Receive();
                    if (message != null)
                    {
                        SkeletonMessage skeletonMessage = SkeletonMessage.DeserializeMessage(message.GetBytes());
                        MessageIngestor.IngestMessagge(skeletonMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                LogEngine.TraceError($"Error in {MethodBase.GetCurrentMethod().Name} - Error {ex.Message}");
            }
        }
    }
}