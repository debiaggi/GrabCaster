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