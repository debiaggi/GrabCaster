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
namespace GrabCaster.Framework.ETW
{
    using System;

    public sealed class EventArrivedEventArgs : EventArgs
    {
        // Keep this event small.
        /// <summary>
        /// Initializes a new instance of the <see cref="EventArrivedEventArgs"/> class.
        /// </summary>
        /// <param name="error">
        /// The error.
        /// </param>
        internal EventArrivedEventArgs(Exception error)
            : this(0 /*eventId*/, new PropertyBag())
        {
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArrivedEventArgs"/> class.
        /// </summary>
        /// <param name="eventId">
        /// The event id.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        internal EventArrivedEventArgs(ushort eventId, PropertyBag properties)
        {
            this.EventId = eventId;
            this.Properties = properties;
        }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        public ushort EventId { get; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public PropertyBag Properties { get; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Exception Error { get; }
    }
}