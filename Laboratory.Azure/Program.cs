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

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Sending message...");
                SendSkeletonMessage(
                    Encoding.UTF8.GetBytes("test string"),
                    "{82208FAA-272E-48A7-BB5C-4EACDEA538D2}",
                    "{306DE168-1CEF-4D29-B280-225B5D0D76FD}",
                    "grabcastersa",
                    "Xmq9EjpObhOkzETSxFphF/diQFxb2RMGEAUwWvRvEAraCPFzZ+Alr4mHnwGbBAEYAYWQZ91yq5bLREUg9MImAA==",
                    "{E77F1E00-4225-4B0B-8A8C-A5BD413B7A5D}",
                    "SenderName",
                    "SenderDescriprion");
                Console.WriteLine("Message sent");
                Console.ReadLine();
            }

        }


        static void SendSkeletonMessage(byte[] content,string idConfiguration, string idComponent, string groupEventHubsStorageAccountName, string groupEventHubsStorageAccountKey, string SenderId, string SenderName, string SenderDescriprion)
        {

            var messageId = Guid.NewGuid().ToString();
            SkeletonMessage data = new SkeletonMessage(null);

            // IF > 256kb then persist
            if (content.Length > 256000)
            {
                data.Body = Encoding.UTF8.GetBytes(messageId);
                BlobDevicePersistentProvider storagePersistent = new BlobDevicePersistentProvider();
                storagePersistent.PersistEventToStorage(content, messageId, groupEventHubsStorageAccountName, groupEventHubsStorageAccountKey);
                data.Properties.Add(Configuration.MessageDataProperty.Persisting.ToString(), true);
            }
            else
            {
                data.Body = content;
                data.Properties.Add(Configuration.MessageDataProperty.Persisting.ToString(), false);
            }

            data.Properties.Add(Configuration.MessageDataProperty.MessageId.ToString(), messageId);

            data.Properties.Add(Configuration.MessageDataProperty.IdConfiguration.ToString(), idConfiguration);
            data.Properties.Add(Configuration.MessageDataProperty.IdComponent.ToString(), idComponent);
            data.Properties.Add(Configuration.MessageDataProperty.Embedded.ToString(), "true");

            // Set main security subscription
            data.Properties.Add(Configuration.GrabCasterMessageTypeName, Configuration.GrabCasterMessageTypeValue);

            // Message context
            data.Properties.Add(
                Configuration.MessageDataProperty.Message.ToString(),
                Configuration.MessageDataProperty.Message.ToString());
            data.Properties.Add(Configuration.MessageDataProperty.MessageType.ToString(), Configuration.MessageDataProperty.Event.ToString());
            data.Properties.Add(Configuration.MessageDataProperty.SenderId.ToString(), SenderId);
            data.Properties.Add(Configuration.MessageDataProperty.SenderName.ToString(), SenderName);
            data.Properties.Add(Configuration.MessageDataProperty.SenderDescriprion.ToString(), SenderDescriprion);
            data.Properties.Add(Configuration.MessageDataProperty.ChannelId.ToString(), "{BD48AFDD-846E-4AF2-A0E7-99E87C0214C8}");
            data.Properties.Add(Configuration.MessageDataProperty.ChannelName.ToString(),"Logic App Channel");
            data.Properties.Add(Configuration.MessageDataProperty.ChannelDescription.ToString(),"Logic App sender");
            data.Properties.Add(Configuration.MessageDataProperty.ReceiverChannelId.ToString(), "channelId");
            data.Properties.Add(Configuration.MessageDataProperty.ReceiverPointId.ToString(), "pointId");

            EventsUpStream eventsUpStream = new EventsUpStream();
            eventsUpStream.CreateEventUpStream();

            eventsUpStream.SendMessage(data);
        }
    }
}
