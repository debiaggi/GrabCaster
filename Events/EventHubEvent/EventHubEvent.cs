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
namespace GrabCaster.Framework.EventHubEvent
{
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// The event hub event.
    /// </summary>
    [EventContract("{F249290E-0231-44A9-A348-1CC7FCC33C7F}", "Event Hub Event", "Send a message to Azure Event Hub.", true)]
    public class EventHubEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the event hub name.
        /// </summary>
        [EventPropertyContract("EventHubName", "EventHubName")]
        public string EventHubName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [EventPropertyContract("ConnectionString", "Event Hub connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public EventActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action event.
        /// </summary>
        public SetEventActionEvent SetEventActionEvent { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="setEventActionEvent">
        /// The set event action event.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [EventActionContract("{FA452E1A-95E9-4076-A1EE-1B41E9561824}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                if (!InternalEventUpStream.InstanceLoaded)
                {
                    InternalEventUpStream.CreateEventUpStream(this.ConnectionString, this.EventHubName);
                    InternalEventUpStream.InstanceLoaded = true;
                }

                InternalEventUpStream.SendMessage(this.DataContext);
                setEventActionEvent(this, context);
            }
            catch
            {
                // ignored
            }
        }
    }

    /// <summary>
    /// The internal event up stream.
    /// </summary>
    internal static class InternalEventUpStream
    {
        /// <summary>
        /// The builder.
        /// </summary>
        private static ServiceBusConnectionStringBuilder builder;

        /// <summary>
        /// The event hub client.
        /// </summary>
        private static EventHubClient eventHubClient;

        /// <summary>
        /// The instance loaded.
        /// </summary>
        public static bool InstanceLoaded { get; set; }

        /// <summary>
        /// The create event up stream.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="eventHubName">
        /// The event hub name.
        /// </param>
        public static void CreateEventUpStream(string connectionString, string eventHubName)
        {
            try
            {
                builder = new ServiceBusConnectionStringBuilder(connectionString) { TransportType = TransportType.Amqp };
                eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void SendMessage(byte[] message)
        {
            try
            {
                var data = new EventData(message);
                eventHubClient.SendAsync(data);
            }
            catch
            {
                // ignored
            }
        }
    }
}