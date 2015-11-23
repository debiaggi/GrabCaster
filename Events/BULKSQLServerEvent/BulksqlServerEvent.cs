// --------------------------------------------------------------------------------------------------
// <copyright file = "BulksqlServerEvent.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog: http://ninocrudele.me
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