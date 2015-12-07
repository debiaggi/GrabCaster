// --------------------------------------------------------------------------------------------------
// <copyright file = "SqlServerTrigger.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.SqlServerTrigger
{
    using System;
    using System.Data.SqlClient;
    using System.Text;
    using System.Xml;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;

    /// <summary>
    /// The SQL server trigger.
    /// </summary>
    [TriggerContract("{7920EE0F-CAC8-4ABB-82C2-1C69351EDD28}", "Sql Server Trigger", "Execute a Sql query or stored procedure.",
        true, true, false)]
    public class SqlServerTrigger : ITriggerType
    {
        /// <summary>
        /// Gets or sets the SQL query.
        /// </summary>
        [TriggerPropertyContract("SqlQuery", "Select Command [Select * from or EXEC Stored precedure name]")]
        public string SqlQuery { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [TriggerPropertyContract("ConnectionString", "ConnectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public EventActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action trigger.
        /// </summary>
        public SetEventActionTrigger SetEventActionTrigger { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [TriggerPropertyContract("DataContext", "Trigger Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="setEventActionTrigger">
        /// The set event action trigger.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [TriggerActionContract("{7BA7B689-6A1D-4FF6-87B3-720F9A723FB8}", "Main action", "Main action description")]
        public void Execute(SetEventActionTrigger setEventActionTrigger, EventActionContext context)
        {
            try
            {
                this.Context = context;
                this.SetEventActionTrigger = setEventActionTrigger;

                using (var myConnection = new SqlConnection(this.ConnectionString))
                {
                    var selectCommand = new SqlCommand(this.SqlQuery, myConnection);
                    myConnection.Open();
                    XmlReader readerResult = null;
                    try
                    {
                        readerResult = selectCommand.ExecuteXmlReader();
                        readerResult.Read();
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    if (readerResult.EOF)
                    {
                        return;
                    }

                    var xdoc = new XmlDocument();
                    xdoc.Load(readerResult);
                    if (xdoc.OuterXml != string.Empty)
                    {
                        this.DataContext = Encoding.UTF8.GetBytes(xdoc.OuterXml);
                        myConnection.Close();
                        setEventActionTrigger(this, context);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}