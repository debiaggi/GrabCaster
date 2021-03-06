﻿// -----------------------------------------------------------------------------------
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
namespace GrabCaster.Framework.Contracts.Serialization
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    ///     Serialization engine class
    /// </summary>
    public static class Serialization
    {
        /// <summary>
        /// The object to byte array no compressed.
        /// </summary>
        /// <param name="objectData">
        /// The object data.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>byte[]</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static byte[] ObjectToByteArrayNoCompressed(object objectData)
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
        /// The byte array to objectold.
        /// </summary>
        /// <param name="bytesArray">
        /// The bytes array.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static object ByteArrayToObjectold(byte[] bytesArray)
        {
            if (bytesArray == null)
            {
                return Encoding.UTF8.GetBytes(string.Empty);
            }

            var memoryStream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            memoryStream.Write(bytesArray, 0, bytesArray.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var obj = binaryFormatter.Deserialize(memoryStream);
            return obj;
        }

        /// <summary>
        /// The object to byte array.
        /// </summary>
        /// <param name="objectData">
        /// The object data.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>byte[]</cref>
        ///     </see>
        ///     .
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static byte[] ObjectToByteArray(object objectData)
        {
            var objStream = new MemoryStream();
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();

            bf.Serialize(ms, objectData);

            var objDeflated = new DeflateStream(objStream, CompressionMode.Compress);

            objDeflated.Write(ms.GetBuffer(), 0, (int)ms.Length);
            objDeflated.Flush();
            objDeflated.Close();

            return objStream.ToArray();
        }

        /// <summary>
        /// The byte array to object.
        /// </summary>
        /// <param name="byteArray">
        /// The byte array.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ByteArrayToObject(byte[] byteArray)
        {
            var inMs = new MemoryStream(byteArray);
            inMs.Seek(0, 0);
            var zipStream = new DeflateStream(inMs, CompressionMode.Decompress, true);

            var outByt = ReadFullStream(zipStream);
            zipStream.Flush();
            zipStream.Close();

            var outMs = new MemoryStream(outByt);
            outMs.Seek(0, 0);

            var bf = new BinaryFormatter();

            object outObject = (DataTable)bf.Deserialize(outMs, null);

            return outObject;
        }

        /// <summary>
        /// The data table to byte array.
        /// </summary>
        /// <param name="dataTable">
        /// The data table.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>byte[]</cref>
        ///     </see>
        ///     .
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static byte[] DataTableToByteArray(DataTable dataTable)
        {
            var objStream = new MemoryStream();
            dataTable.RemotingFormat = SerializationFormat.Binary;
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();

            bf.Serialize(ms, dataTable);

            var objDeflated = new DeflateStream(objStream, CompressionMode.Compress);

            objDeflated.Write(ms.GetBuffer(), 0, (int)ms.Length);
            objDeflated.Flush();
            objDeflated.Close();

            return objStream.ToArray();
        }

        /// <summary>
        /// The byte array to data table.
        /// </summary>
        /// <param name="byteDataTable">
        /// The byte data table.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        public static DataTable ByteArrayToDataTable(byte[] byteDataTable)
        {
            var outDs = new DataTable();

            var inMs = new MemoryStream(byteDataTable);
            inMs.Seek(0, 0);
            var zipStream = new DeflateStream(inMs, CompressionMode.Decompress, true);

            var outByt = ReadFullStream(zipStream);
            zipStream.Flush();
            zipStream.Close();

            var outMs = new MemoryStream(outByt);
            outMs.Seek(0, 0);
            outDs.RemotingFormat = SerializationFormat.Binary;
            var bf = new BinaryFormatter();

            outDs = (DataTable)bf.Deserialize(outMs, null);

            return outDs;
        }

        /// <summary>
        /// The read full stream.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>byte[]</cref>
        ///     </see>
        ///     .
        /// </returns>
        private static byte[] ReadFullStream(Stream stream)
        {
            var buffer = new byte[32768];

            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    var read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                    {
                        return ms.ToArray();
                    }

                    ms.Write(buffer, 0, read);
                }
            }
        }
    }
}