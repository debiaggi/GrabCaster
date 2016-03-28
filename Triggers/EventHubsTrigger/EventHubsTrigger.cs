﻿// -----------------------------------------------------------------------------------
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
namespace GrabCaster.Framework.EventHubsTrigger
{
    using System;
    using System.Threading;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// The event hubs trigger.
    /// </summary>
    [TriggerContract("{AD270984-5695-4D1F-AB78-1E960AFBEE9D}", "Event Hubs Trigger", "Get messages from Event Hubs",
        false, true, false)]
    public class EventHubsTrigger : ITriggerType
    {
        /// <summary>
        /// Gets or sets the event hubs connection string.
        /// </summary>
        [TriggerPropertyContract("EventHubsConnectionString", "Event Hubs Connection String")]
        public string EventHubsConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the event hubs name.
        /// </summary>
        [TriggerPropertyContract("EventHubsName", "Event Hubs Name")]
        public string EventHubsName { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public EventActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action trigger.
        /// </summary>
        public SetEventActionTrigger SetEventActionTrigger { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [TriggerPropertyContract("DataContext", "Trigger Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="setEventActionTrigger">
        /// The set event action trigger.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [TriggerActionContract("{90EA497E-61AE-4664-A957-41AC588106FB}", "Main action", "Main action description")]
        public void Execute(SetEventActionTrigger setEventActionTrigger, EventActionContext context)
        {
            try
            {
                this.Context = context;
                this.SetEventActionTrigger = setEventActionTrigger;

                // Create the connection string
                var builder = new ServiceBusConnectionStringBuilder(this.EventHubsConnectionString)
                                  {
                                      TransportType =
                                          TransportType
                                          .Amqp
                                  };

                // Create the EH Client
                var eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), this.EventHubsName);

                // muli partition sample
                var namespaceManager = NamespaceManager.CreateFromConnectionString(builder.ToString());
                var eventHubDescription = namespaceManager.GetEventHub(this.EventHubsName);

                // Use the default consumer group
                foreach (var partitionId in eventHubDescription.PartitionIds)
                {
                    var myNewThread = new Thread(() => this.ReceiveDirectFromPartition(eventHubClient, partitionId));
                    myNewThread.Start();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// The receive direct from partition.
        /// </summary>
        /// <param name="eventHubClient">
        /// The event hub client.
        /// </param>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        private void ReceiveDirectFromPartition(EventHubClient eventHubClient, string partitionId)
        {
            var group = eventHubClient.GetDefaultConsumerGroup();
            var receiver = group.CreateReceiver(partitionId, DateTime.UtcNow);
            while (true)
            {
                var message = receiver.Receive();
                if (message != null)
                {
                    this.DataContext = message.GetBytes();
                    this.SetEventActionTrigger(this, this.Context);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}