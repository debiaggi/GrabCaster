// --------------------------------------------------------------------------------------------------
// <copyright file = "Constant.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Copyright (c) 2013 - 2015 Nino Crudele
//    Blog: http://ninocrudele.me
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License. 
// </summary>
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