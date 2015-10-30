#region License
// -----------------------------------------------------------------------
// Copyright (c) Antonino Crudele.  All Rights Reserved.  
// This work is registered with the UK Copyright Service.
// Registration No:284695248
// Licensed under the Reciprocal Public License 1.5 (RPL1.5) 
// See License.txt in the project root for license information.
// -----------------------------------------------------------------------
#endregion
namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;
    using Base;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;

    /// <summary>
    ///     Send messages to EH
    /// </summary>
    internal static class LogEventUpStream
    {
        //EH variable

        private static string connectionString = "";
        private static string eventHubName = "";
        private static ServiceBusConnectionStringBuilder builder;
        private static EventHubClient eventHubClient;

        public static void CreateEventUpStream()
        {
            try
            {
                //EH Configuration
                connectionString = Configuration.AzureNameSpaceConnectionString();
                eventHubName = Configuration.GroupEventHubsLogName();

                builder = new ServiceBusConnectionStringBuilder(connectionString)
                {
                    TransportType = TransportType.Amqp
                };

                eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(Configuration.General_Source,
                    string.Format("Error in {0}", MethodBase.GetCurrentMethod().Name),
                    Constant.Error_EventID_High_Critical_,
                    Constant.TaskCategories_,
                    ex,
                    EventLogEntryType.Error);
            }
        }

        /// <summary>
        ///     Send a EventMessage message
        ///     invio importantissimo perche spedisce eventi e oggetti in array byte la dimensione e strategica
        /// </summary>
        /// <param name="message"></param>
        public static void SendMessage(LogMessage LogMessage, Dictionary<string, object> Properties)
        {
            try
            {
                //Meter and measuring purpose
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                //Create EH data message
                var jsonSerialized = JsonConvert.SerializeObject(LogMessage);
                var serializedMessage = Encoding.UTF8.GetBytes(jsonSerialized);


                var MessageID = Guid.NewGuid().ToString();

                EventData data = null;

                data = new EventData(serializedMessage);
                data.Properties.Add(Configuration.MessageDataProperty.Persisting.ToString(), false);

                //Load custom Properties
                if (Properties != null)
                {
                    foreach (var prop in Properties)
                    {
                        data.Properties.Add(prop.Key, prop.Value);
                    }
                }
                data.Properties.Add(Configuration.MessageDataProperty.MessageID.ToString(), MessageID);

                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                data.Properties.Add(Configuration.MessageDataProperty.OperationTime.ToString(), ts.Milliseconds);

                //Send the metric to Event Hub
                eventHubClient.Send(data);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(Configuration.General_Source,
                    string.Format("Error in {0}", MethodBase.GetCurrentMethod().Name),
                    Constant.Error_EventID_High_Critical_EH,
                    Constant.TaskCategories_EH,
                    ex,
                    EventLogEntryType.Error);

                if (Common.IsAzureConnectionProblem(ex))
                    Environment.Exit(0);
            }
        }
    }
}