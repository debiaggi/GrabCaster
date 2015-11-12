namespace GrabCaster.Framework.Contracts.Messaging
{
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The EventsDownstream interface.
    /// </summary>
    public interface IEventsDownstream
    {
        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="setEventOnRampMessageReceived">
        /// The set event on ramp message received.
        /// </param>
        void Run(SetEventOnRampMessageReceived setEventOnRampMessageReceived);
    }
}
