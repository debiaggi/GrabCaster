// --------------------------------------------------------------------------------------------------
// <copyright file = "AzureQueueTrigger.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    Info:   http://GrabCaster.io
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
namespace GrabCaster.Framework.AzureQueueTrigger
{
    using System;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// The azure queue trigger.
    /// </summary>
    [TriggerContract("{79F1CAB1-6E78-4BF9-8D2E-F15E87F605CA}", "Azure Queue Trigger", "Azure Queue Trigger", false, true,
        false)]
    public class AzureQueueTrigger : ITriggerType
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [TriggerPropertyContract("ConnectionString", "Azure ConnectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the queue path.
        /// </summary>
        [TriggerPropertyContract("QueuePath", "QueuePath")]
        public string QueuePath { get; set; }

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
        [TriggerActionContract("{647FE4E4-2FD0-4AF4-8FC2-B3019F0BA571}", "Main action", "Main action description")]
        public void Execute(SetEventActionTrigger setEventActionTrigger, EventActionContext context)
        {
            try
            {
                var namespaceManager = NamespaceManager.CreateFromConnectionString(this.ConnectionString);

                if (!namespaceManager.QueueExists(this.QueuePath))
                {
                    namespaceManager.CreateQueue(this.QueuePath);
                }

                var client = QueueClient.CreateFromConnectionString(this.ConnectionString, this.QueuePath);

                // Configure the callback options
                var options = new OnMessageOptions { AutoComplete = false, AutoRenewTimeout = TimeSpan.FromMinutes(1) };

                // Callback to handle received messages
                client.OnMessage(
                    message =>
                        {
                            try
                            {
                                // Remove message from queue
                                this.DataContext = message.GetBody<byte[]>();
                                message.Complete();
                                setEventActionTrigger(this, context);
                            }
                            catch (Exception)
                            {
                                // Indicates a problem, unlock message in queue
                                message.Abandon();
                            }
                        },
                    options);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}