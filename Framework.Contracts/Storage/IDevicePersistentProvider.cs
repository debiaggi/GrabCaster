// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventsUpStream.cs" company="">
//   
// </copyright>
// <summary>
//   The EventsUpStream interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GrabCaster.Framework.Contracts.Storage
{
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The EventsUpStream interface.
    /// </summary>
    public interface IDevicePersistentProvider
    {

        /// <summary>
        /// The persist event to blob.
        /// </summary>
        /// <param name="messageBody">
        /// The message body.
        /// </param>
        /// <param name="messageId">
        /// The message id.
        /// </param>
        void PersistEventToStorage(byte[] messageBody, string messageId);

        byte[] PersistEventFromStorage(string messageId);
    }
}
