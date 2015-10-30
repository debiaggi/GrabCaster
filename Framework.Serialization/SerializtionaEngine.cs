// --------------------------------------------------------------------------------------------------
// <copyright file = "LogMessage.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog: http://ninocrudele.me
//    
//    By accessing GrabCaster code here, you are agreeing to the following licensing terms.
//    If you do not agree to these terms, do not access the GrabCaster code.
//    Your license to the GrabCaster source and/or binaries is governed by the 
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Serialization
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    ///     Serialization engine class
    /// </summary>
    public static class SerializationEngine
    {
        /// <summary>
        /// serialize object to Array
        /// </summary>
        /// <param name="objectData">
        /// Object to serialize
        /// </param>
        /// <returns>
        /// Return a byte array
        /// </returns>
        public static byte[] ObjectToByteArray(object objectData)
        {
            if (objectData == null)
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, objectData);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Serialize Array to Object
        /// </summary>
        /// <param name="arrayBytes">
        /// Array byte to deserialize
        /// </param>
        /// <returns>
        /// Object deserialized
        /// </returns>
        public static object ByteArrayToObject(byte[] arrayBytes)
        {
            if (arrayBytes == null)
            {
                return Encoding.UTF8.GetBytes(string.Empty);
            }

            var memoryStream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            memoryStream.Write(arrayBytes, 0, arrayBytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var obj = binaryFormatter.Deserialize(memoryStream);
            return obj;
        }
    }
}