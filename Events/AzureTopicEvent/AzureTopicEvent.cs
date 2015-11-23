// --------------------------------------------------------------------------------------------------
// <copyright file = "AzureTopicEvent.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog: http://ninocrudele.me
//    
//    By accessing GrabCaster code here, you are agreeing to the following licensing terms.
//    If you do not agree to these terms, do not access the GrabCaster code.
//    Your license to the GrabCaster source and/or binaries is governed by the 
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//    
//    This work is registered with the UK Copyright Service.
//    Registration No:284695248  
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.AzureTopicEvent
{
    using System;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// The azure topic event.
    /// </summary>
    [EventContract("{B2311010-B505-4F9F-A927-2035A7640BCB}", "AzureTopicEvent", "Send message to Azure Topic", true)]
    public class AzureTopicEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [EventPropertyContract("ConnectionString", "Azure ConnectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the topic path.
        /// </summary>
        [EventPropertyContract("TopicPath", "TopicPath")]
        public string TopicPath { get; set; }

        /// <summary>
        /// Gets or sets the message context properties.
        /// </summary>
        [EventPropertyContract("MessageContextProperties", "MessageContextProperties")]
        public string MessageContextProperties { get; set; }

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
        [EventActionContract("{D33251EF-7638-4C34-AD6B-B5CBE32F7056}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                this.Context = context;
                this.SetEventActionEvent = setEventActionEvent;

                var namespaceManager = NamespaceManager.CreateFromConnectionString(this.ConnectionString);

                if (!namespaceManager.TopicExists(this.TopicPath))
                {
                    namespaceManager.CreateTopic(this.TopicPath);
                }

                var client = TopicClient.CreateFromConnectionString(this.ConnectionString, this.TopicPath);
                var brokeredMessage = new BrokeredMessage(this.DataContext);

                var value = this.MessageContextProperties.Split('|');
                brokeredMessage.Properties[value[0]] = value[1];
                client.Send(brokeredMessage);
                setEventActionEvent(this, context);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}