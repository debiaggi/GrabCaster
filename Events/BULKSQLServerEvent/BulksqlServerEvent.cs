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
namespace GrabCaster.Framework.BulksqlServerEvent
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Serialization;

    /// <summary>
    /// The bulksql server event.
    /// </summary>
    [EventContract("{767D579B-986B-47B1-ACDF-46738434043F}", "BulksqlServerEvent Event", "Receive a Sql Server recordset to perform a bulk insert.",
        true)]
    public class BulksqlServerEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the table name destination.
        /// </summary>
        [EventPropertyContract("TableNameDestination", "TableName")]
        public string TableNameDestination { get; set; }

        /// <summary>
        /// Gets or sets the bulk select query destination.
        /// </summary>
        [EventPropertyContract("BulkSelectQueryDestination", "BulkSelectQueryDestination")]
        public string BulkSelectQueryDestination { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [EventPropertyContract("ConnectionString", "ConnectionString")]
        public string ConnectionString { get; set; }

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
        [EventActionContract("{F469BD5B-B352-40D6-BD33-591EF96E8F6C}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                using (var destinationConnection = new SqlConnection(this.ConnectionString))
                {
                    destinationConnection.Open();
                    using (var bulkCopy = new SqlBulkCopy(this.ConnectionString))
                    {
                        bulkCopy.DestinationTableName = this.TableNameDestination;
                        try
                        {
                            object obj = Serialization.ByteArrayToDataTable(this.DataContext);
                            var dataTable = (DataTable)obj;
                            
                            // Write from the source to the destination.
                            bulkCopy.WriteToServer(dataTable);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }

                setEventActionEvent(this, context);
            }
            catch
            {
                // ignored
            }
        }
    }
}