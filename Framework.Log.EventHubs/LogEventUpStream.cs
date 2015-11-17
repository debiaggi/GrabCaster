// --------------------------------------------------------------------------------------------------
// <copyright file = "LogEventUpStream.cs" company="Nino Crudele">
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