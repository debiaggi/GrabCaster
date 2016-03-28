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
namespace GrabCaster.Framework.Engine
{
    /// <summary>
    /// The service states.
    /// </summary>
    public static class ServiceStates
    {
        /// <summary>
        /// Gets or sets a value indicating whether run polling.
        /// </summary>
        public static bool RunPolling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether restart needed.
        /// </summary>
        public static bool RestartNeeded { get; set; }
    }
}