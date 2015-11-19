using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Contracts.Messaging
{
    using System.Runtime.Serialization;

    [Serializable]
    public class SkeletonMessage: ISkeletonMessage
    {
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public IDictionary<string, object> Properties { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonMessage"/> class.
        /// </summary>
        /// <param name="body">
        /// The body.
        /// </param>
        public SkeletonMessage(byte[] body)
        {
            Properties = new Dictionary<string, object>();
            Body = body;
        }

        /// <summary>
        /// The serialize message.
        /// </summary>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] SerializeMessage(SkeletonMessage skeletonMessage)
        {
            return GrabCaster.Framework.Serialization.Object.SerializationEngine.ObjectToByteArray(skeletonMessage);
        }

        /// <summary>
        /// The serialize message.
        /// </summary>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static SkeletonMessage DeserializeMessage(byte[] byteArray)
        {
            return (SkeletonMessage) GrabCaster.Framework.Serialization.Object.SerializationEngine.ByteArrayToObject(byteArray);
        }

    }
}
