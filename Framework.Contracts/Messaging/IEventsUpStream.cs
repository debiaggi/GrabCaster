// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventsUpStream.cs" company="">
//   
// </copyright>
// <summary>
//   The EventsUpStream interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GrabCaster.Framework.Contracts.Messaging
{
    /// <summary>
    /// The EventsUpStream interface.
    /// </summary>
    public interface IEventsUpStream
    {
        /// <summary>
        /// The create event up stream.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool CreateEventUpStream();

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void SendMessage(SkeletonMessage message);
    }
}
