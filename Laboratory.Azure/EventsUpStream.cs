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
                connectionString = ConfigurationLibrary.AzureNameSpaceConnectionString();
                eventHubName = ConfigurationLibrary.GroupEventHubsName();

                LogEngine.TraceInformation($"Start GrabCaster UpStream - Point Id {ConfigurationLibrary.PointId()} - Point name {ConfigurationLibrary.PointName()} - Channel Id {ConfigurationLibrary.ChannelId()} - Channel name {ConfigurationLibrary.ChannelName()} ");

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
                LogEngine.TraceError($"Error in {MethodBase.GetCurrentMethod().Name} - Error {ex.Message}");
            }
        }
    }
}