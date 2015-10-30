// --------------------------------------------------------------------------------------------------
// <copyright file = "LogMessage.cs" company="Nino Crudele">
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
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Forms
{
    /// <summary>
    /// Holds the creation parameters.
    /// </summary>
    internal static class ServiceFormSettingsCreationParameters
    {
        /// <summary>
        /// Gets or sets the startup mode.
        /// </summary>
        /// <value>
        /// The startup mode.
        /// </value>
        public static StartupModeEnum StartupMode { get; set; }

        /// <summary>
        /// Gets or sets the creation mode.
        /// </summary>
        /// <value>
        /// The creation mode.
        /// </value>
        public static CreationModeEnum CreationMode { get; set; }
    } // ServiceFormSettingsCreationParameters
} // namespace