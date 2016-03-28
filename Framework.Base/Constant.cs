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
        public static int DefconOne { get; } = 1;
        public static int DefconTwo { get; } = 2;
        public static int DefconThree { get; } = 3;
        public static int DefconFour { get; } = 4;
        public static int DefconFive { get; } = 5;


        /// <summary>
        /// The task category error.
        /// </summary>
        public static string TaskCategoriesError { get; } = Configuration.EngineName;

        /// <summary>
        /// The task category for console.
        /// </summary>
        public static string TaskCategoriesConsole { get; } = "Console";

        public static string EmbeddedEventId { get; } = "{A31209D7-C989-4E5D-93DA-BD341D843870}";
        /// <summary>
        /// The task category for event hubs.
        /// </summary>
        public static string TaskCategoriesEventHubs { get; } = "Event Hub";
    } // Constant
} // namespace