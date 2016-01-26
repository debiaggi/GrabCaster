using System;
using System.Xml;
using Microsoft.BizTalk.Component.Interop;
using GrabCaster.Framework.BizTalk.Adapter.Common;
using Microsoft.BizTalk.TransportProxy.Interop;

namespace GrabCaster.Framework.BizTalk.Adapter
{
	/// <summary>
	/// This is a singleton class for GrabCaster send adapter. All the messages, going to various
	/// send ports of this adapter type, will go through this class.
	/// </summary>
	public class GrabCasterTransmitter : AsyncTransmitter
	{
		internal static string GRABCASTER_NAMESPACE = "http://schemas.microsoft.com/BizTalk/2003/SDK_Samples/Messaging/Transports/grabcaster-properties";

		public GrabCasterTransmitter() : base(
			"GrabCaster Transmit Adapter",
			"1.0",
			"Send messages form BizTalk to GrabCaster points",
            ".GRABCASTER",
			new Guid("024DB758-AAF9-415e-A121-4AC245DD49EC"),
            GRABCASTER_NAMESPACE,
			typeof(GrabCasterTransmitterEndpoint),
            GrabCasterTransmitProperties.BatchSize)
        {
		}
	
		protected override void HandlerPropertyBagLoaded ()
		{
			IPropertyBag config = this.HandlerPropertyBag;
			if (null != config)
			{
				XmlDocument handlerConfigDom = ConfigProperties.IfExistsExtractConfigDom(config);
				if (null != handlerConfigDom)
				{
					GrabCasterTransmitProperties.ReadTransmitHandlerConfiguration(handlerConfigDom);
				}
			}
		}
	}
}
