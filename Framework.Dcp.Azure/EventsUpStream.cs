// --------------------------------------------------------------------------------------------------
// <copyright file = "LockSlimEventUpStream.cs" company="Nino Crudele">
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
    using System.Text;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Log;
    using GrabCaster.Framework.Serialization;
    using GrabCaster.Framework.Storage;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    ///     Send messages to EH
    /// </summary>
    [EventsUpStreamContract("{6FAEA018-C21B-423E-B860-3F8BAC0BC637}", "EventUpStream", "Event Hubs EventUpStream")]
    public class EventsUpStream: IEventsUpStream
    {
        //EH variable

        private static string connectionString = "";

        private static string eventHubName = "";

        private static EventHubClient eventHubClient;
        
        public bool CreateEventUpStream()
        {
            try
            {
                //EH Configuration
                connectionString = Configuration.AzureNameSpaceConnectionString();
                eventHubName = Configuration.GroupEventHubsName();

                LogEngine.WriteLog(
                    Configuration.EngineName,
                    $"Event Hubs transfort Type: {Configuration.ServiceBusConnectivityMode()}",
                    Constant.ErrorEventIdHighCritical,
                    Constant.TaskCategoriesError,
                    null,
                    EventLogEntryType.Information);

                var builder = new ServiceBusConnectionStringBuilder(connectionString)
                                  {
                                      TransportType =
                                          TransportType.Amqp
                                  };

                eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Send a EventMessage message
        ///     invio importantissimo perche spedisce eventi e oggetti in array bytela dimensione e strategica
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(object message)
        {
            try
            {
                EventData data = (EventData)message;
                // Create EH data message
                byte[] byteMessage = (byte[])message;
                // IF > 256kb then persist
                var messageId = data.Properties[Configuration.MessageDataProperty.MessageId.ToString()].ToString();
                if (byteMessage.Length > 256000)
                {
                    data = new EventData(Encoding.UTF8.GetBytes(messageId));
                    PersistentProvider.PersistEventToBlob(byteMessage, messageId);
                    data.Properties.Add(Configuration.MessageDataProperty.Persisting.ToString(), true);
                }
                else
                {
                    data = new EventData(byteMessage);
                    data.Properties.Add(Configuration.MessageDataProperty.Persisting.ToString(), false);
                }
                eventHubClient.Send((EventData)message);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.ErrorEventIdHighCriticalEventHubs,
                    Constant.TaskCategoriesEventHubs,
                    ex,
                    EventLogEntryType.Error);
            }
        }
    }
}