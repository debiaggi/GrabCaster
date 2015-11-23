// --------------------------------------------------------------------------------------------------
// <copyright file = "EventArrivedEventArgs.cs" company="Nino Crudele">
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