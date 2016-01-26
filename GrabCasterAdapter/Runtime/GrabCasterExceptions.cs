using System;
using System.Runtime.Serialization;

namespace GrabCaster.Framework.BizTalk.Adapter
{
	internal class GrabCasterExceptions : ApplicationException
	{
		public static string UnhandledTransmit_Error = "The GrabCaster Adapter encounted an error transmitting a batch of messages.";

        public GrabCasterExceptions () { }

		public GrabCasterExceptions (string msg) : base(msg) { }

		public GrabCasterExceptions (Exception inner) : base(String.Empty, inner) { }

		public GrabCasterExceptions (string msg, Exception e) : base(msg, e) { }

		protected GrabCasterExceptions (SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}

