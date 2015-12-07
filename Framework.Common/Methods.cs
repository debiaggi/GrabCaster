// --------------------------------------------------------------------------------------------------
// <copyright file = "Methods.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Common
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The communication diretion.
    /// </summary>
    public enum CommunicationDiretion
    {
        /// <summary>
        /// The off ramp.
        /// </summary>
        OffRamp, 

        /// <summary>
        /// The on ramp.
        /// </summary>
        OnRamp
    }

    /// <summary>
    /// The common.
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// The last error point
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        public static void DirectEventViewerLog(Exception ex)
        {
            string msg =
                $"Critical error, possible causes. \r1) The current Windows user has not the grants. \r2) The GrabCaster configuration files are not correct.\rError message-{ex.Message}";
            Debug.WriteLine(msg);
            EventLog.WriteEntry("GrabCaster", msg, EventLogEntryType.Error, 0);
        }

        /// <summary>
        /// The last error point
        /// </summary>
        public static void DirectEventViewerLog(string message)
        {
            EventLog.WriteEntry("GrabCaster", message, EventLogEntryType.Information, 0);
        }
    }
}