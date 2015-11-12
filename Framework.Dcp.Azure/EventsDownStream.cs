// --------------------------------------------------------------------------------------------------
// <copyright file = "DirectEventsDownstream.cs" company="Nino Crudele">
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
                    Constant.ErrorEventIdHighCritical,
                    Constant.TaskCategoriesError,
                    null,
                    EventLogEntryType.Information);

                var builder = new ServiceBusConnectionStringBuilder(eventHubConnectionString)
                                  {
                                      TransportType =
                                          TransportType.Amqp
                                  };

                //If not exit it create one
                var eventHubConsumerGroup =
                    string.Concat(Configuration.EngineName, "_", Configuration.ChannelId())
                        .Replace("{", "")
                        .Replace("}", "")
                        .Replace("-", "");
                var nsManager = NamespaceManager.CreateFromConnectionString(builder.ToString());
                LogEngine.ConsoleWriteLine(
                    $"Initializing GROUP NAME {eventHubConsumerGroup}",
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
                    Constant.ErrorEventIdHighCriticalEventHubs,
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
                        SetEventOnRampMessageReceived(message);
                    }
                }
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
            }
        }
    }
}