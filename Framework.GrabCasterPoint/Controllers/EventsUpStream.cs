﻿// --------------------------------------------------------------------------------------------------
// <copyright file = "EventsUpStream.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    Info:   http://grabcaster.io/
// 
//    Unless explicitly acquired and licensed from Licensor under another
//    license, the contents of this file are subject to the Reciprocal Public
//    License ("RPL") Version 1.5, or subsequent versions as allowed by the RPL,
//    and You may not copy or use this file in either source code or executable
//    form, except in compliance with the terms and conditions of the RPL.
//    
//    All software distributed under the RPL is provided strictly on an "AS
//    IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND
//    LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
//    LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//    PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RPL for specific
//    language governing rights and limitations under the RPL. 
//    
//    The Reciprocal Public License 1.5 (RPL1.5) license is described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Library.Azure
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Messaging;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    ///     Send messages to EH
    /// </summary>
    [EventsUpStreamContract("{6FAEA018-C21B-423E-B860-3F8BAC0BC637}", "EventUpStream", "Event Hubs EventUpStream")]
    public class EventsUpStream
    {
        //EH variable

        private static string connectionString = "";

        private static string eventHubName = "";

        private static EventHubClient eventHubClient;
        
        public bool CreateEventUpStream(string AzureNameSpaceConnectionString, string GroupEventHubsName)
        {
            try
            {
                //EH Configuration
                connectionString = AzureNameSpaceConnectionString;
                eventHubName = GroupEventHubsName;

                LogEngine.TraceInformation($"Start GrabCaster UpStream - connectionString {connectionString}");
                LogEngine.TraceInformation($"Start GrabCaster UpStream - GroupEventHubsName {GroupEventHubsName}");

                var builder = new ServiceBusConnectionStringBuilder(connectionString)
                                  {
                                      TransportType =
                                          TransportType.Amqp
                                  };

                LogEngine.TraceInformation($"eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName)");
                eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);
                LogEngine.TraceInformation($"After EventHubClient.CreateFromConnectionString");

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
        public void SendMessage(SkeletonMessage message)
        {
            try
            {
                LogEngine.TraceInformation($"Enter in SendMessage");
                LogEngine.TraceInformation($"Enter in SendMessage body lenght - {message.Body.Count().ToString()}");
 
                LogEngine.TraceInformation($"Enter in SendMessage body text - {Encoding.UTF8.GetString(message.Body)}");
                byte[] byteArrayBytes = SkeletonMessage.SerializeMessage(message);
                LogEngine.TraceInformation($"Create EventData");

                EventData evtData = new EventData(byteArrayBytes);

                LogEngine.TraceInformation($"Send Message!");
                eventHubClient.Send(evtData);
                LogEngine.TraceInformation($"Message Sent!");
            }
            catch (Exception ex)
            {
                LogEngine.TraceError($"Error in {MethodBase.GetCurrentMethod().Name} - Error {ex.Message}");
            }
        }
    }
}