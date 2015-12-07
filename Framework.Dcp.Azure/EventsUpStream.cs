// --------------------------------------------------------------------------------------------------
// <copyright file = "EventsUpStream.cs" company="Nino Crudele">
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
        public void SendMessage(SkeletonMessage message)
        {
            try
            {
                byte[] byteArrayBytes = SkeletonMessage.SerializeMessage(message);
                EventData evtData = new EventData(byteArrayBytes);
                eventHubClient.Send(evtData);
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