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
namespace GrabCaster.Framework.Log.EventHubs
{
    using System;
    using System.Diagnostics;
    using System.Text;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Log;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    using Newtonsoft.Json;

    /// <summary>
    ///     Send messages to EH
    /// </summary>
    internal static class LogEventUpStream
    {
        //EH variable

        private static string azureNameSpaceConnectionString = "";

        private static string eventHubName = "";

        private static EventHubClient eventHubClient;

        public static bool CreateEventUpStream()
        {
            try
            {
                if (Configuration.RunLocalOnly())
                {
                    EventLog.WriteEntry("Framework.Log.EventHubs", "The remote logging storage provider is not available, this GrabCaster point is configured for local only execution.", EventLogEntryType.Warning);
                    return true;
                }

                Debug.WriteLine("-------------- Engine LogEventUpStream --------------");
                Debug.WriteLine("LogEventUpStream - Get Configuration settings.");
                //Event Hub Configuration
                azureNameSpaceConnectionString = Configuration.AzureNameSpaceConnectionString();
                eventHubName = Configuration.LoggingComponentStorage();
                Debug.WriteLine($"LogEventUpStream - azureNameSpaceConnectionString={azureNameSpaceConnectionString}");
                Debug.WriteLine($"LogEventUpStream - eventHubName={eventHubName}");

                // TODO ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Https;

                var builder = new ServiceBusConnectionStringBuilder(azureNameSpaceConnectionString)
                {
                    TransportType =
                                          TransportType
                                          .Amqp
                };

                Debug.WriteLine("LogEventUpStream - Create the eventHubClient.");

                eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LogEventUpStream - error->{0}", ex.Message);
                EventLog.WriteEntry("Framework.Log.EventHubs", ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     Send a EventMessage message
        /// </summary>
        public static bool SendMessage(LogMessage logMessage)
        {
            try
            {
                if (Configuration.RunLocalOnly())
                {
                    EventLog.WriteEntry("Framework.Log.EventHubs", "The remote logging storage provider is not available, this GrabCaster point is configured for local only execution.", EventLogEntryType.Warning);
                    return true;
                }

                Debug.WriteLine("LogEventUpStream - serialize log message.");
                //Create EH data message
                var jsonSerialized = JsonConvert.SerializeObject(logMessage);
                var serializedMessage = Encoding.UTF8.GetBytes(jsonSerialized);

                var data = new EventData(serializedMessage);
                Debug.WriteLine("LogEventUpStream - send log message.");

                //Send the metric to Event Hub
                eventHubClient.Send(data);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LogEventUpStream - error->{0}", ex.Message);

                EventLog.WriteEntry("Framework.Log.EventHubs", ex.Message, EventLogEntryType.Error);
                return false;
            }
        }
    }
}