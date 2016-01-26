using System;
using System.Xml;
using Microsoft.BizTalk.Component.Interop;
using GrabCaster.Framework.BizTalk.Adapter.Common;

namespace GrabCaster.Framework.BizTalk.Adapter
{
    public class GrabCasterReceiver : Receiver 
    {
        public GrabCasterReceiver() : base(
            "GrabCaster Receive Adapter",
            "1.0",
            "Submits message from GrabCaster points into BizTalk",
            ".GRABCASTER",
            new Guid("3D4B599E-2202-4bbb-9FC6-7ACA3906E5DE"),
            "http://schemas.microsoft.com/BizTalk/2003/grabcaster-properties",
            typeof(GrabCasterReceiverEndpoint))
        {
        }
        /// <summary>
        /// This function is called when BizTalk runtime gives the handler properties to adapter.
        /// </summary>
        protected override void HandlerPropertyBagLoaded ()
        {
            IPropertyBag config = this.HandlerPropertyBag;
            if (null != config)
            {
                XmlDocument handlerConfigDom = ConfigProperties.IfExistsExtractConfigDom(config);
                if (null != handlerConfigDom)
                {
                    GrabCasterReceiveProperties.ReceiveHandlerConfiguration(handlerConfigDom);
                }
            }
        }
    }
}
