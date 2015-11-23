// --------------------------------------------------------------------------------------------------
// <copyright file = "Constant.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Base
{
    /// <summary>
    /// Holds the constants.
    /// </summary>
    public static class Constant
    {
        /// <summary>
        /// The ID for the high critical events (Engine).
        /// </summary>
        public static int ErrorEventIdHighCritical { get; } = 10001;

        /// <summary>
        /// The ID for the high critical events (Event Hub)
        /// </summary>
        public static int ErrorEventIdHighCriticalEventHubs { get; } = 11001;

        /// <summary>
        /// The task category error.
        /// </summary>
        public static string TaskCategoriesError { get; } = Configuration.EngineName;

        /// <summary>
        /// The task category for console.
        /// </summary>
        public static string TaskCategoriesConsole { get; } = "Console";

        /// <summary>
        /// The task category for event hubs.
        /// </summary>
        public static string TaskCategoriesEventHubs { get; } = "Event Hub";
    } // Constant
} // namespace