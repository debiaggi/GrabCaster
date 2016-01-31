namespace GrabCaster.Framework.Library.Azure
{
    using System;
    using System.Diagnostics;
    using System.Text;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Messaging;

    using Newtonsoft.Json;

    //Receiver side
    //class Program
    //{

    //    private static MessageIngestor.SetEventActionEventEmbedded setEventActionEventEmbedded;

    //    static void Main(string[] args)
    //    {

    //        EventsDownStream eventsDownStream = new EventsDownStream();
    //        setEventActionEventEmbedded += SetEventOnRampMessageReceivedMessage;
    //        eventsDownStream.Run(setEventActionEventEmbedded);
    //    }

    //    private static void SetEventOnRampMessageReceivedMessage(byte[] message)
    //    {
    //        string stringValue = Encoding.UTF8.GetString(message);
    //        Console.WriteLine("---------------EVENT RECEIVED IN GRABCASTER LIBRARY---------------");
    //        Console.WriteLine(stringValue);
    //    }
    //}


    //Sender side
    class Program
    {

        private static SetEventActionEvent setEventActionEventEmbedded;

        static void Main(string[] args)
        {
            //setEventActionEventEmbedded = EventReceivedFromEmbedded;
            //GrabCaster.Framework.Library.Embedded.InitializeOffRampEmbedded(setEventActionEventEmbedded);

            //GrabCaster.Framework.Library.Embedded.ExecuteTrigger(
            //    "{82208FAA-272E-48A7-BB5C-4EACDEA538D2}",
            //    "{306DE168-1CEF-4D29-B280-225B5D0D76FD}",
            //    Encoding.UTF8.GetBytes("test"));
        }


        static void PrepareSkeletonMessage(byte[] content)
        {

            var messageId = Guid.NewGuid().ToString();
            SkeletonMessage data = new SkeletonMessage(null);

            // IF > 256kb then persist
            if (content.Length > 256000)
            {
                data.Body = Encoding.UTF8.GetBytes(messageId);
                GrabCaster.Framework.Storage.BlobDevicePersistentProvider storagePersistent = new Storage.BlobDevicePersistentProvider();
                storagePersistent.PersistEventToStorage(content, messageId);
                data.Properties.Add(Configuration.MessageDataProperty.Persisting.ToString(), true);
            }
            else
            {
                data.Body = content;
                data.Properties.Add(Configuration.MessageDataProperty.Persisting.ToString(), false);
            }

            data.Properties.Add(Configuration.MessageDataProperty.MessageId.ToString(), messageId);

            // Set main security subscription
            data.Properties.Add(Configuration.GrabCasterMessageTypeName, Configuration.GrabCasterMessageTypeValue);

            // Message context
            data.Properties.Add(
                Configuration.MessageDataProperty.Message.ToString(),
                Configuration.MessageDataProperty.Message.ToString());
            data.Properties.Add(Configuration.MessageDataProperty.MessageType.ToString(), Configuration.MessageDataProperty.Trigger.ToString());
            data.Properties.Add(Configuration.MessageDataProperty.SenderId.ToString(), Configuration.PointId());
            data.Properties.Add(Configuration.MessageDataProperty.SenderName.ToString(), Configuration.PointName());
            data.Properties.Add(
                Configuration.MessageDataProperty.SenderDescriprion.ToString(),
                Configuration.PointDescription());
            data.Properties.Add(Configuration.MessageDataProperty.ChannelId.ToString(), Configuration.ChannelId());
            data.Properties.Add(
                Configuration.MessageDataProperty.ChannelName.ToString(),
                Configuration.ChannelName());
            data.Properties.Add(
                Configuration.MessageDataProperty.ChannelDescription.ToString(),
                Configuration.ChannelDescription());
            data.Properties.Add(Configuration.MessageDataProperty.ReceiverChannelId.ToString(), "channelId");
            data.Properties.Add(Configuration.MessageDataProperty.ReceiverPointId.ToString(), "pointId");

            EventsUpStream eventsUpStream = new EventsUpStream();
            eventsUpStream.CreateEventUpStream();

            eventsUpStream.SendMessage(data);
        }
    }
}
