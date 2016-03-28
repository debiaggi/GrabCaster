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
        public static void DirectEventViewerLog(string message, EventLogEntryType eventLogEntryType)
        {
            EventLog.WriteEntry("GrabCaster", message, eventLogEntryType, 0);
        }
    }
}