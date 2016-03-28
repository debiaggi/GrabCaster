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
namespace GrabCaster.Framework.Dcp.Azure
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Log;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    ///     Main Downstream events receiving
    ///     It execute the main DownStream Instance
    /// </summary>
    [EventsDownStreamContract("{B8ECF14B-2A9E-41C9-9E85-D8EA2D5C4E22}", "EventsDownStream", "Event Hubs EventsDownStream")]
    public class DirectEventsDownstream : IEventsDownstream
    {
        private static SetEventOnRampMessageReceived SetEventOnRampMessageReceived { get; set; }


        public void Run(SetEventOnRampMessageReceived setEventOnRampMessageReceived)
        {
            try
            {

                // Assign the delegate 
                SetEventOnRampMessageReceived = setEventOnRampMessageReceived;
                // Load vars
                var eventHubConnectionString = Configuration.AzureNameSpaceConnectionString();
                var eventHubName = Configuration.GroupEventHubsName();

                LogEngine.WriteLog(
                    Configuration.EngineName,
                    $"Event Hubs transfort Type: {Configuration.ServiceBusConnectivityMode()}",
                    Constant.DefconOne,
                    Constant.TaskCategoriesError,
                    null,
                    EventLogEntryType.Information);

                var builder = new ServiceBusConnectionStringBuilder(eventHubConnectionString)
                                  {
                                      TransportType =
                                          TransportType.Amqp
                                  };

                //If not exit it create one, drop brachets because Azure rules
                var eventHubConsumerGroup =
                    string.Concat(Configuration.EngineName, "_", Configuration.ChannelId())
                        .Replace("{", "")
                        .Replace("}", "")
                        .Replace("-", "");
                var nsManager = NamespaceManager.CreateFromConnectionString(builder.ToString());
                LogEngine.ConsoleWriteLine(
                    $"Initializing Group Name {eventHubConsumerGroup}",
                    ConsoleColor.White);

                LogEngine.ConsoleWriteLine("Start DirectRegisterEventReceiving.", ConsoleColor.White);

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

                LogEngine.ConsoleWriteLine(
                    "After DirectRegisterEventReceiving Downstream running.",
                    ConsoleColor.White);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name} - Hint: Check if the firewall outbound port 5671 is opened.",
                    Constant.DefconOne,
                    Constant.TaskCategoriesEventHubs,
                    ex,
                    EventLogEntryType.Error);
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

                var eventHubsStartingDateTimeReceiving =
                    DateTime.Parse(
                        Configuration.EventHubsStartingDateTimeReceiving() == "0"
                            // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                            ? DateTime.UtcNow.ToString()
                            : Configuration.EventHubsStartingDateTimeReceiving());
                var eventHubsEpoch = Configuration.EventHubsEpoch();

                EventHubReceiver receiver = null;

                switch (Configuration.EventHubsCheckPointPattern())
                {
                    case EventHubsCheckPointPattern.CheckPoint:
                        //Receiving from the last valid receiving point
                        receiver = group.CreateReceiver(partitionId, DateTime.UtcNow);
                        break;
                    case EventHubsCheckPointPattern.Dt:

                        receiver = group.CreateReceiver(partitionId, eventHubsStartingDateTimeReceiving);
                        break;
                    case EventHubsCheckPointPattern.Dtepoch:
                        receiver = group.CreateReceiver(partitionId, eventHubsStartingDateTimeReceiving, eventHubsEpoch);
                        break;
                    case EventHubsCheckPointPattern.Dtutcnow:
                        receiver = group.CreateReceiver(partitionId, DateTime.UtcNow);
                        break;
                    case EventHubsCheckPointPattern.Dtnow:
                        receiver = group.CreateReceiver(partitionId, DateTime.Now);
                        break;
                    case EventHubsCheckPointPattern.Dtutcnowepoch:
                        receiver = group.CreateReceiver(partitionId, DateTime.UtcNow, eventHubsEpoch);
                        break;
                    case EventHubsCheckPointPattern.Dtnowepoch:
                        receiver = group.CreateReceiver(partitionId, DateTime.Now, eventHubsEpoch);
                        break;
                }
                LogEngine.ConsoleWriteLine(
                    $"Direct Receiver created. Partition {partitionId}",
                    ConsoleColor.Yellow);
                while (true)
                {
                    var message = receiver?.Receive();
                    if (message != null)
                    {
                        SkeletonMessage skeletonMessage = SkeletonMessage.DeserializeMessage(message.GetBytes());
                        SetEventOnRampMessageReceived(skeletonMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.DefconOne,
                    Constant.TaskCategoriesError,
                    ex,
                    EventLogEntryType.Error);
            }
        }
    }
}