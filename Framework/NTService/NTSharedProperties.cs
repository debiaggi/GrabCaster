// --------------------------------------------------------------------------------------------------
// <copyright file = "NTSharedProperties.cs" company="Nino Crudele">
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