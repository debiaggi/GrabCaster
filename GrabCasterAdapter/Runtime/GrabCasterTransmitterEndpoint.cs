using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Threading;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using GrabCaster.Framework.BizTalk.Adapter.Common;
using GrabCaster.Framework.Log;

namespace GrabCaster.Framework.BizTalk.Adapter
{
	/// <summary>
	/// There is one instance of HttpTransmitterEndpoint class for each every static send port.
	/// Messages will be forwarded to this class by AsyncTransmitterBatch
	/// </summary>
	internal class GrabCasterTransmitterEndpoint : AsyncTransmitterEndpoint
	{
		private AsyncTransmitter asyncTransmitter = null;
        private string propertyNamespace;
        private static int IO_BUFFER_SIZE = 4096;
        private const string PROP_REMOTEMESSAGEID = "RemoteMessageId";
        private const string PROP_IDCONFIGURATION = "idConfiguration";
        private const string PROP_IDCOMPONENT = "idComponent";
        private const string PROP_POINTNAME = "pointName";
        private const string PROP_NAMESPACE = "https://GrabCaster.BizTalk.Schemas.GrabCasterProperties";

        public GrabCasterTransmitterEndpoint(AsyncTransmitter asyncTransmitter) : base(asyncTransmitter)
		{
			this.asyncTransmitter = asyncTransmitter;
		}

        public override void Open(EndpointParameters endpointParameters, IPropertyBag handlerPropertyBag, string propertyNamespace)
        {
            GrabCaster.Framework.Library.Embedded.InitializeOffRampEmbedded();
            this.propertyNamespace = propertyNamespace;
        }

		/// <summary>
		/// Implementation for AsyncTransmitterEndpoint::ProcessMessage
		/// Transmit the message and optionally return the response message (for Request-Response support)
		/// </summary>
		public override IBaseMessage ProcessMessage(IBaseMessage message)
		{
           
            Stream source = message.BodyPart.Data;
            byte[] content = null;
            using (var memoryStream = new MemoryStream())
            {
                source.CopyTo(memoryStream);
                content = memoryStream.ToArray();
            }
            // build url
            GrabCasterTransmitProperties props = new GrabCasterTransmitProperties(message, propertyNamespace);

            var idComponent = message.Context.Read(PROP_IDCOMPONENT, PROP_NAMESPACE);
            var idConfiguration = message.Context.Read(PROP_IDCONFIGURATION, PROP_NAMESPACE);
            var pointName = message.Context.Read(PROP_POINTNAME, PROP_NAMESPACE);

            if(idComponent != null && idConfiguration != null)
            {
                GrabCaster.Framework.Library.Embedded.ExecuteTrigger(
                    idConfiguration.ToString(),
                    idComponent.ToString(),
                    content);
            }
            else
            {
                GrabCaster.Framework.Library.Embedded.ExecuteTrigger(
                    props.IdConfiguration,
                    props.IdComponent,
                    content);
            }

            //GrabCaster.Framework.Library.Embedded.ExecuteTrigger(
            //    "{82208FAA-272E-48A7-BB5C-4EACDEA538D2}",
            //    "{306DE168-1CEF-4D29-B280-225B5D0D76FD}",
            //    content);

            return null;
		}

	    private bool kEngineAvailable()
	    {

            while (!GrabCaster.Framework.Library.Embedded.engineLoaded)
            {
                ;
            }
	        return true;
	    }
	}
}
