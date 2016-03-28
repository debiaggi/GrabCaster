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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Library.Azure
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public static class LogEngine
    {
        public static string storageAccountName { get; set; }
        public static string storageAccountKey { get; set; }
        public static bool azureLogEnabled { get; set; }

        public static void TraceInformation(string message)
        {
            string logMessage = $"{GrabCasterPointAPIApp.Controllers.Constants.EngineName} - {message}";
            WriteMessage(logMessage);
        }
        public static void TraceWarning(string message)
        {
            string logMessage = $"{GrabCasterPointAPIApp.Controllers.Constants.EngineName} - {message}";
            WriteMessage(logMessage);
        }
        public static void TraceError(string message)
        {
            string logMessage = $"{GrabCasterPointAPIApp.Controllers.Constants.EngineName} - {message}";
            WriteMessage(logMessage);
        }

        private static void WriteMessage(string message)
        {
            try
            {
                if(!azureLogEnabled) return;

                var connectionString =
                    $"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={storageAccountKey}";
                var storageAccount = CloudStorageAccount.Parse(connectionString);

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                // Create the table if it doesn't exist.
                CloudTable table = tableClient.GetTableReference("grabcasterpointlog");
                table.CreateIfNotExists();
                LogrEntity log = new LogrEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
                log.Message = message;
                log.TimeMessage = DateTime.Now;
                TableOperation insertOperation = TableOperation.Insert(log);
                // Execute the insert operation.
                table.Execute(insertOperation);
            }
            catch (Exception)
            {
                
        
            }

        }
    }
    public class LogrEntity : TableEntity
    {
        public LogrEntity(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public LogrEntity() { }

        public string Message { get; set; }

        public DateTime TimeMessage { get; set; }
    }
}
