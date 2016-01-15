// --------------------------------------------------------------------------------------------------
// <copyright file = "MessageIngestor.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    Info:   http://grabcaster.io/
// 
//    Unless explicitly acquired and licensed from Licensor under another
//    license, the contents of this file are subject to the Reciprocal Public
//    License ("RPL") Version 1.5, or subsequent versions as allowed by the RPL,
//    and You may not copy or use this file in either source code or executable
//    form, except in compliance with the terms and conditions of the RPL.
//    
//    All software distributed under the RPL is provided strictly on an "AS
//    IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND
//    LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
//    LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//    PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RPL for specific
//    language governing rights and limitations under the RPL. 
//    
//    The Reciprocal Public License 1.5 (RPL1.5) license is described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Library.Azure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Storage;

    /// <summary>
    /// Engine main message ingestor
    /// </summary>
    public static class MessageIngestor
    {
        private static object classInstanceDpp;
        private static bool secondaryPersistProviderEnabled;
        private static int secondaryPersistProviderByteSize;
        private static MethodInfo methodPersistEventFromBlob;
        private static readonly object[] ParametersPersistEventFromBlob = { null };


        public delegate void SetEventActionEventEmbedded(byte[] message);
        /// <summary>
        /// Used internally by the embedded
        /// </summary>
        public static SetEventActionEventEmbedded setEventActionEventEmbedded { get; set; }

        public static void Init(SetEventActionEventEmbedded setEventOnRampMessageReceived)
        {
            try
            {

                setEventActionEventEmbedded = setEventOnRampMessageReceived;

                secondaryPersistProviderEnabled = Configuration.SecondaryPersistProviderEnabled();
                secondaryPersistProviderByteSize = Configuration.SecondaryPersistProviderByteSize();

                System.Type assemblyClassDpp = null;

                // Create the reflection method cached 
                var assemblyPersist = new BlobDevicePersistentProvider();

                // Main class logging
                assemblyClassDpp = (from t in Assembly.GetAssembly(assemblyPersist.GetType()).GetTypes()
                                    let attributes = t.GetCustomAttributes(typeof(DevicePersistentProviderContract), true)
                                    where t.IsClass && attributes != null && attributes.Length > 0
                                    select t).First();


                var classAttributeDpp = assemblyClassDpp.GetCustomAttributes(typeof(DevicePersistentProviderContract), true);

                if (classAttributeDpp.Length > 0)
                {
                    Debug.WriteLine("DevicePersistentProviderContract - methodPersistEvent caller");
                    methodPersistEventFromBlob = assemblyClassDpp.GetMethod("PersistEventFromStorage");
                   
                }

                classInstanceDpp = Activator.CreateInstance(assemblyClassDpp, null);

            }
            catch (Exception ex)
            {

                LogEngine.WriteLog($"Error in {MethodBase.GetCurrentMethod().Name} - Error {ex.Message}");
            }
        }
        public static void IngestMessagge(object message)
        {
            string senderId;
            string senderDescription;
            byte[] eventDataByte = null;
            var skeletonMessage = (ISkeletonMessage)message;

            // ****************************CHECK MESSAGE TYPE*************************
            try
            {
                // Check message subscription, it must come from engine
                if (skeletonMessage.Properties[Configuration.GrabCasterMessageTypeName].ToString()
                    != Configuration.GrabCasterMessageTypeValue)
                {
                    LogEngine.WriteLog("Not GrabCaster message type received -DISCARED-");
                    return;
                }
                else
                {
                    // Who sent the message
                    senderId = skeletonMessage.Properties[Configuration.MessageDataProperty.SenderId.ToString()].ToString();
                    senderDescription =
                        skeletonMessage.Properties[Configuration.MessageDataProperty.SenderDescriprion.ToString()].ToString();

                    // Who receive the message
                    LogEngine.WriteLog($"Event received from Sender {senderId} Sender description {senderDescription}");

                    if (senderId == ConfigurationLibrary.PointId())
                    {
                        LogEngine.WriteLog("Same sender ID event discared.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // If error then not message typeof (no property present.)
                LogEngine.WriteLog($"Error in {MethodBase.GetCurrentMethod().Name} - Not GrabCaster message type received (Missing GrabCaster_MessageType_Name properties.) -DISCARED- Error {ex.Message}");
                return;
            }

            // ****************************CHECK MESSAGE TYPE*************************
            // Check if >256, the restore or not
            if ((bool)skeletonMessage.Properties[Configuration.MessageDataProperty.Persisting.ToString()])
            {
                ParametersPersistEventFromBlob[0] = skeletonMessage.Properties[Configuration.MessageDataProperty.MessageId.ToString()];
                var ret = methodPersistEventFromBlob.Invoke(classInstanceDpp, ParametersPersistEventFromBlob);
                eventDataByte = (byte[])ret;
            }
            else
            {
                eventDataByte = skeletonMessage.Body;
            }

            setEventActionEventEmbedded(eventDataByte);

        }
    }
}