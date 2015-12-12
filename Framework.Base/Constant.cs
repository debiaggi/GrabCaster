// --------------------------------------------------------------------------------------------------
// <copyright file = "Constant.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
// 
//    Unless explicitly acquired and licensed from Licensor under another
//    license, the contents of this file are subject to the Reciprocal Public
//    License ("RPL") Version 1.5, or subsequent versions as allowed by the RPL,
//    and You may not copy or use this file in either source code or executable
//    form, except in compliance with the terms and conditions of the RPL.
//    
//    All software distributed under the RPL is provided strictly on an "AS
//    IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND
//    LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
//    LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//    PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RPL for specific
//    language governing rights and limitations under the RPL. 
//    
//    The Reciprocal Public License 1.5 (RPL1.5) license is described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
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