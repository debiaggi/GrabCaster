// -----------------------------------------------------------------------------------
// 
// GRABCASTER LTD CONFIDENTIAL
// ___________________________
// 
// Copyright © 2013 - 2016 GrabCaster Ltd. All rights reserved.
// This work is registered with the UK Copyright Service: Registration No:284701085
// 
// 
// NOTICE:  All information contained herein is, and remains
// the property of GrabCaster Ltd and its suppliers,
// if any.  The intellectual and technical concepts contained
// herein are proprietary to GrabCaster Ltd
// and its suppliers and may be covered by UK and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from GrabCaster Ltd.
// 
// -----------------------------------------------------------------------------------
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
