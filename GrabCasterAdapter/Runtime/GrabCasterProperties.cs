using System;
using System.IO;
using System.Xml;
using System.Net;
using Microsoft.BizTalk.Message.Interop;
using GrabCaster.Framework.BizTalk.Adapter.Common;

namespace GrabCaster.Framework.BizTalk.Adapter
{
    /// <summary>
    /// This class handles properties for a given Receive Location
    /// </summary>
    internal class GrabCasterReceiveProperties : ConfigProperties
    {
        // Handler properties
        private static int handlerMaximumNumberOfMessages = 0;

        // Endpoint properties
        private string jsonBag;
        private int maximumBatchSize;
        private int maximumNumberOfMessages;
        private int errorThreshold;
        private string workInProgress;

        public string JsonBag { get { return this.jsonBag; } }
        public int MaximumBatchSize { get { return maximumBatchSize; } }
        public int MaximumNumberOfMessages { get { return this.maximumNumberOfMessages; } }
        public int ErrorThreshold { get { return errorThreshold; } }

        public GrabCasterReceiveProperties() : base()
        {
            // establish defaults
            this.jsonBag = String.Empty;
            maximumBatchSize = 0;
            this.maximumNumberOfMessages = handlerMaximumNumberOfMessages; // default to handler value, override if set on the endpoint
            workInProgress = String.Empty;
        }

        /// <summary>
        /// Load the Configuration for the Receive Handler
        /// </summary>
        public static void ReceiveHandlerConfiguration(XmlDocument configDOM)
        {
            // Handler properties
            handlerMaximumNumberOfMessages = IfExistsExtractInt(configDOM, "/Config/maximumNumberOfMessages", handlerMaximumNumberOfMessages);
        }

        /// <summary>
        /// Load the Configuration for a Receive Location
        /// </summary>
        public void ReadLocationConfiguration(XmlDocument configDOM)
        {

            this.jsonBag = IfExistsExtract(configDOM, "/Config/jsonBag", string.Empty);
            this.maximumBatchSize = IfExistsExtractInt(configDOM, "/Config/maximumBatchSize", 0);
            this.maximumNumberOfMessages = IfExistsExtractInt(configDOM, "/Config/maximumNumberOfMessages", handlerMaximumNumberOfMessages);
            this.errorThreshold = ExtractInt(configDOM, "/Config/errorThreshold");
            this.workInProgress = IfExistsExtract(configDOM, "/Config/workInProgress", string.Empty);
        }

    }

    /// <summary>
	/// This class maintains send port properties associated with a message. These properties
	/// will be extracted from the message context for static send ports.
	/// </summary>
    internal class GrabCasterTransmitProperties : ConfigProperties
    {
        // Handler properties
        private static int handlerSendBatchSize = 20;
        private static int handlerbufferSize = 4096;
        private static int handlerthreadsPerCPU = 1;

        // Endpoint properties
        private string idConfiguration;
        private string idTrigger;

        public string IdConfiguration { get { return idConfiguration; } }
        public string IdTrigger { get { return idTrigger; } }
        public static int BatchSize { get { return handlerSendBatchSize; } }

        public GrabCasterTransmitProperties(IBaseMessage message, string propertyNamespace)
        {
            XmlDocument locationConfigDom = null;

            //  get the adapter configuration off the message
            IBaseMessageContext context = message.Context;
            string config = (string)context.Read("AdapterConfig", propertyNamespace);

            //  the config can be null all that means is that we are doing a dynamic send
            if (null != config)
            {
                locationConfigDom = new XmlDocument();
                locationConfigDom.LoadXml(config);

                this.ReadLocationConfiguration(locationConfigDom);
            }
        }

        /// <summary>
        /// Load the Transmit Handler configuration settings
        /// </summary>
        public static void ReadTransmitHandlerConfiguration(XmlDocument configDOM)
        {
            // Handler properties
            handlerSendBatchSize = ExtractInt(configDOM, "/Config/sendBatchSize");
            handlerbufferSize = ExtractInt(configDOM, "/Config/bufferSize");
            handlerthreadsPerCPU = ExtractInt(configDOM, "/Config/threadsPerCPU");
        }

        /// <summary>
        /// Load the configuration for the Message that is being transmitted
        /// </summary>
        /// <param name="configDOM"></param>
        public void ReadLocationConfiguration(XmlDocument configDOM)
        {

            this.idConfiguration = Extract(configDOM, "/Config/idConfiguration", string.Empty);
            this.idTrigger = Extract(configDOM, "/Config/idTrigger", string.Empty);
        }
    }
}

