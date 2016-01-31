using Microsoft.Azure.AppService.ApiApps.Service;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Text;
using Newtonsoft.Json.Linq;
using TRex.Metadata;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace GrabCasterPointAPIApp.Controllers
{
    using GrabCaster.Framework.Library.Azure;

    /// <summary>
    /// ApiController for the Event Hub.  Includes methods for the Push trigger to push on new Event Hub Messages, and the Send method to send Event Hub.  Also contains classes
    /// for InMemoryTriggerStore to store the callback URL for the push triggers, and the data model for EventHub Messages
    /// </summary>
    public class TriggersController : ApiController
    {
        /// <summary>
        /// Main Event Hub Push trigger
        /// </summary>
        /// <param name="triggerId">Passed in via Logic Apps to identify the Logic App needing the push notification</param>
        /// <param name="triggerInput">Item that includes trigger inputs like Event Hub parameters</param>
        /// <returns></returns>
        [Trigger(TriggerType.Push, typeof(SkeletonMessage))]
        [Metadata("Receive GrabCaster Message")]
        [HttpPut, Route("{triggerId}")]
        public HttpResponseMessage EventHubPushTrigger(string triggerId, [FromBody]TriggerInput<EventHubInput, SkeletonMessage> triggerInput)
        {
            if (!InMemoryTriggerStore.Instance.GetStore().ContainsKey(triggerId))
            {
                HostingEnvironment.QueueBackgroundWorkItem(async ct => await InMemoryTriggerStore.Instance.RegisterTrigger(triggerId, triggerInput));
            }
            return this.Request.PushTriggerRegistered(triggerInput.GetCallback());
        }

        
        /// <summary>
        /// Send the message, her in going to receive a message from an external APi and send it to a GrabCaster Point
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Metadata("Send GrabCaster Message")]
        [Swashbuckle.Swagger.Annotations.SwaggerResponse(HttpStatusCode.BadRequest, "An exception occured", typeof(Exception))]
        [Swashbuckle.Swagger.Annotations.SwaggerResponse(System.Net.HttpStatusCode.Created)]
        [Route("SendString")]
        public HttpResponseMessage EventHubSend([Metadata("idConfiguration")]string idConfiguration,
                                                [Metadata("idComponent")]string idComponent,
                                                [Metadata("groupEventHubsStorageAccountName")]string groupEventHubsStorageAccountName,
                                                [Metadata("groupEventHubsStorageAccountKey")]string groupEventHubsStorageAccountKey,
                                                [Metadata("SenderId")]string SenderId,
                                                [Metadata("SenderName")]string SenderName,
                                                [Metadata("SenderDescriprion")]string SenderDescriprion,
                                                [Metadata("SenderDescriprion")]string AzureNameSpaceConnectionString,
                                                [Metadata("SenderDescriprion")]string GroupEventHubsName,
                                                [FromBody]SkeletonActionMessage input)
        {
            try
            {

                SendSkeletonMessage(Encoding.UTF8.GetBytes(input.message),
                                    idConfiguration,
                                    idComponent,
                                    groupEventHubsStorageAccountName,
                                    groupEventHubsStorageAccountKey,
                                    SenderId,
                                    SenderName,
                                    SenderDescriprion,
                                    AzureNameSpaceConnectionString,
                                    GroupEventHubsName);

                return this.Request.CreateResponse(System.Net.HttpStatusCode.Created);
            }
            catch(NullReferenceException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, @"The input recieved by the API was null.  This sometimes happens if the message in the Logic App is malformed.  Check the message to make sure there are no escape characters like '\'.", ex);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        /// <summary>
        /// Class for the InMemoryTriggerStore.  How the Logic App works, it will call the PUSH trigger endpoint with the triggerID and CallBackURL - it is the job
        /// of the API App to then call the CallbackURL when an event happens.  I utilize this InMemoryTriggerStore to store the triggers I am watching, so that I don't
        /// continue to spin up new listeners whenever the Logic App asks me to register a trigger.
        /// 
        /// If the API App was ever to reset or lose connection, I would know I need to register new Event Hub Listeners as the InMemoryTriggerStore would be empty.
        /// </summary>
        public class InMemoryTriggerStore
        {
            private static InMemoryTriggerStore instance;
            private IDictionary<string, bool> _store;


            private InMemoryTriggerStore()
            {
                _store = new Dictionary<string, bool>();
            }

            public IDictionary<string, bool> GetStore()
            {
                return _store;
            }

            public static InMemoryTriggerStore Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new InMemoryTriggerStore();
                    }
                    return instance;
                }
            }

            /// <summary>
            /// The method that registers Event Hub listeners and assigns them to a recieve event.  When I receive an event from the event hub listener, I trigger the callbackURL
            /// </summary>
            /// <param name="triggerId"></param>
            /// <param name="triggerInput"></param>
            /// <returns></returns>
            public async Task RegisterTrigger(string triggerId, TriggerInput<EventHubInput, SkeletonMessage> triggerInput)
            {
    
                GrabCasterListener grabCasterListener = new GrabCasterListener();

                //Register the event.  When I recieve a message, call the method to trigger the logic app
                grabCasterListener.MessageReceived += (sender, e) => sendTrigger(sender, e, Runtime.FromAppSettings(), triggerInput.GetCallback());
                grabCasterListener.Start();
                //Register the triggerID in my store, so on subsequent checks from the logic app I don't spin up a new set of listeners
                _store[triggerId] = true;
            }
        }

        /// <summary>
        /// Method to trigger the logic app.  Called when an event is recieved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="runtime"></param>
        /// <param name="clientTriggerCallback"></param>
        private static void sendTrigger(object sender, SkeletonMessage e, Runtime runtime, ClientTriggerCallback<SkeletonMessage> clientTriggerCallback)
        {
            clientTriggerCallback.InvokeAsync(runtime, e);
        }
        

        /// <summary>
        /// Event Hub Listener class.  Keeps track of the receiver, and converts the message to a JOBject when recieved
        /// </summary>
        public class GrabCasterListener
        {
            private EventHubReceiver reciever;
            private  MessageIngestor.SetEventActionEventEmbedded setEventActionEventEmbedded;

            public void Start()
            {

                EventsDownStream eventsDownStream = new EventsDownStream();
                setEventActionEventEmbedded += this.SetEventOnRampMessageReceivedMessage;
                eventsDownStream.Run(setEventActionEventEmbedded);
                this.reciever = reciever;
            }

            public event EventHandler<SkeletonMessage> MessageReceived;

            private void SetEventOnRampMessageReceivedMessage(byte[] message)
            {
                LogEngine.WriteLog("---------------EVENT RECEIVED IN GRABCASTER LIBRARY---------------");

                var msg = UnicodeEncoding.UTF8.GetString(message);
                Console.WriteLine(msg);
                MessageReceived(this, new SkeletonMessage(msg));
            }

   
        }

        /// <summary>
        /// Data model for objects used in app.
        /// </summary>
        public class SkeletonMessage
        {
            public string body;
            public SkeletonMessage(string message)
            {
                this.body = message;
            }
        }

        public class SkeletonActionMessage
        {
            [Metadata("Message", null)]
            [Required(AllowEmptyStrings = false)]
            public string message { get; set; }
        }



        public class EventHubInput
        {
            [Metadata("Input parameter")]
            public string inputparam { get; set; }
        }


        static void SendSkeletonMessage(byte[] content, 
                                        string idConfiguration, 
                                        string idComponent, 
                                        string groupEventHubsStorageAccountName, 
                                        string groupEventHubsStorageAccountKey, 
                                        string SenderId, 
                                        string SenderName, 
                                        string SenderDescriprion,
                                        string AzureNameSpaceConnectionString, 
                                        string GroupEventHubsName)
        {

            var messageId = Guid.NewGuid().ToString();
            GrabCaster.Framework.Contracts.Messaging.SkeletonMessage data = new GrabCaster.Framework.Contracts.Messaging.SkeletonMessage(null);

            // IF > 256kb then persist
            if (content.Length > 256000)
            {
                data.Body = Encoding.UTF8.GetBytes(messageId);
                BlobDevicePersistentProvider storagePersistent = new BlobDevicePersistentProvider();
                storagePersistent.PersistEventToStorage(content, messageId, groupEventHubsStorageAccountName, groupEventHubsStorageAccountKey);
                data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.Persisting.ToString(), true);
            }
            else
            {
                data.Body = content;
                data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.Persisting.ToString(), false);
            }

            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.MessageId.ToString(), messageId);

            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.IdConfiguration.ToString(), idConfiguration);
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.IdComponent.ToString(), idComponent);
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.Embedded.ToString(), "true");

            // Set main security subscription
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.GrabCasterMessageTypeName, GrabCaster.Framework.Base.Configuration.GrabCasterMessageTypeValue);

            // Message context
            data.Properties.Add(
                GrabCaster.Framework.Base.Configuration.MessageDataProperty.Message.ToString(),
                GrabCaster.Framework.Base.Configuration.MessageDataProperty.Message.ToString());
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.MessageType.ToString(), GrabCaster.Framework.Base.Configuration.MessageDataProperty.Event.ToString());
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.SenderId.ToString(), SenderId);
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.SenderName.ToString(), SenderName);
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.SenderDescriprion.ToString(), SenderDescriprion);
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.ChannelId.ToString(), "{BD48AFDD-846E-4AF2-A0E7-99E87C0214C8}");
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.ChannelName.ToString(), "Logic App Channel");
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.ChannelDescription.ToString(), "Logic App sender");
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.ReceiverChannelId.ToString(), "channelId");
            data.Properties.Add(GrabCaster.Framework.Base.Configuration.MessageDataProperty.ReceiverPointId.ToString(), "pointId");

            EventsUpStream eventsUpStream = new EventsUpStream();
            eventsUpStream.CreateEventUpStream(AzureNameSpaceConnectionString, GroupEventHubsName);

            eventsUpStream.SendMessage(data);
        }

    }


}
