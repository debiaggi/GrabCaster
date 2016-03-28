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
namespace GrabCaster.Framework.NTService
{
    using System.ServiceProcess;

    /// <summary>
    /// Properties for the Windows Service.
    /// </summary>
    internal static class NtSharedProperties
    {
        /// <summary>
        /// Gets or sets the account for the Windows Service.
        /// </summary>
        /// <value>
        /// The account for the Windows Service.
        /// </value>
        public static ServiceAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the password for the Windows Service.
        /// </summary>
        /// <value>
        /// The password for the Windows Service.
        /// </value>
        public static string Password { get; set; }

        /// <summary>
        /// Gets or sets the username for the Windows Service.
        /// </summary>
        /// <value>
        /// The username for the Windows Service.
        /// </value>
        public static string Username { get; set; }

        /// <summary>
        /// Gets or sets the name for the Windows Service.
        /// </summary>
        /// <value>
        /// The name for the Windows Service.
        /// </value>
        public static string WindowsNtServiceName { get; set; }

        /// <summary>
        /// Gets or sets the display name for the Windows Service.
        /// </summary>
        /// <value>
        /// The display name for the Windows Service.
        /// </value>
        public static string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description for the Windows Service.
        /// </summary>
        /// <value>
        /// The description for the Windows Service.
        /// </value>
        public static string Description { get; set; }
    } // NTSharedProperties
} // namespace