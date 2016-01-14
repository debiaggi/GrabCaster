namespace GrabCaster.Framework.Library.Azure
{
    using System;
    using System.Text;

    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Messaging;

    class Program
    {

        private static MessageIngestor.SetEventActionEventEmbedded setEventActionEventEmbedded;

        static void Main(string[] args)
        {

            EventsDownStream eventsDownStream = new EventsDownStream();
            setEventActionEventEmbedded += SetEventOnRampMessageReceivedMessage;
            eventsDownStream.Run(setEventActionEventEmbedded);
        }

        private static void SetEventOnRampMessageReceivedMessage(byte[] message)
        {
            string stringValue = Encoding.UTF8.GetString(message);
            Console.WriteLine("---------------EVENT RECEIVED IN GRABCASTER LIBRARY---------------");
            Console.WriteLine(stringValue);
        }
    }
}
