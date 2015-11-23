// --------------------------------------------------------------------------------------------------
// <copyright file = "PersistentProvider.cs" company="Nino Crudele">
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
//    
//    This work is registered with the UK Copyright Service.
//    Registration No:284695248  
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Storage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Log;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json;

    /// <summary>
    /// Main persistent provider.
    /// </summary>
    public static class PersistentProvider
    {
        public enum CommunicationDiretion
        {
            OffRamp,

            OnRamp
        }

        public static bool PersistMessage(EventActionContext eventActionContext)
        {
            if (Configuration.EnablePersistingMessaging() == false)
            {
                return true;
            }

            return PersistMessage(eventActionContext.BubblingConfiguration, CommunicationDiretion.OnRamp);
        }

        /// <summary>
        /// Persist the message in local file system
        /// </summary>
        /// <param name="bubblingEvent">
        /// </param>
        /// <param name="communicationDiretion">
        /// The communication Diretion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool PersistMessage(BubblingEvent bubblingEvent, CommunicationDiretion communicationDiretion)
        {
            try
            {
                if (!Configuration.EnablePersistingMessaging())
                {
                    return true;
                }

                var serializedMessage = JsonConvert.SerializeObject(bubblingEvent);
                var directoryDate = string.Concat(
                    DateTime.Now.Year, 
                    "\\", 
                    DateTime.Now.Month.ToString().PadLeft(2, '0'), 
                    "\\", 
                    DateTime.Now.Day.ToString().PadLeft(2, '0'), 
                    "\\", 
                    communicationDiretion.ToString());
                var datetimeFile = string.Concat(
                    DateTime.Now.Year, 
                    DateTime.Now.Month.ToString().PadLeft(2, '0'), 
                    DateTime.Now.Day.ToString().PadLeft(2, '0'), 
                    "-", 
                    DateTime.Now.Hour.ToString().PadLeft(2, '0'), 
                    "-", 
                    DateTime.Now.Minute.ToString().PadLeft(2, '0'), 
                    "-", 
                    DateTime.Now.Second.ToString().PadLeft(2, '0'));

                var persistingForlder = Path.Combine(Configuration.LocalStorageConnectionString(), directoryDate);
                Directory.CreateDirectory(persistingForlder);
                var filePersisted = Path.Combine(
                    persistingForlder, 
                    string.Concat(datetimeFile, "-", bubblingEvent.MessageId, Configuration.MessageFileStorageExtension));

                File.WriteAllText(filePersisted, serializedMessage);
                LogEngine.ConsoleWriteLine(
                    "Event persisted -  Consistency Transaction Point created.", 
                    ConsoleColor.DarkGreen);
                return true;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return false;
            }
        }
        public static void PersistEventToBlob(byte[] messageBody, string messageId)
        {
            try
            {
                var storageAccountName = Configuration.GroupEventHubsStorageAccountName();
                var storageAccountKey = Configuration.GroupEventHubsStorageAccountKey();
                var connectionString =
                    $"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={storageAccountKey}";
                var storageAccount = CloudStorageAccount.Parse(connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container. 
                var container = blobClient.GetContainerReference(Configuration.GroupEventHubsName());

                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();
                container.SetPermissions(
                    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                // Create the messageid reference
                var blockBlob = container.GetBlockBlobReference(messageId);
                blockBlob.UploadFromByteArray(messageBody, 0, messageBody.Length);
                LogEngine.ConsoleWriteLine(
                    "Event persisted -  Consistency Transaction Point created.", 
                    ConsoleColor.DarkGreen);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
            }
        }
        public static byte[] PersistEventFromBlob(string messageId)
        {
            try
            {
                var storageAccountName = Configuration.GroupEventHubsStorageAccountName();
                var storageAccountKey = Configuration.GroupEventHubsStorageAccountKey();
                var connectionString =
                    $"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={storageAccountKey}";
                var storageAccount = CloudStorageAccount.Parse(connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container. 
                var container = blobClient.GetContainerReference(Configuration.GroupEventHubsName());

                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();
                container.SetPermissions(
                    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                // Create the messageid reference
                var blockBlob = container.GetBlockBlobReference(messageId);

                blockBlob.FetchAttributes();
                var msgByteLength = blockBlob.Properties.Length;
                var msgContent = new byte[msgByteLength];
                for (var i = 0; i < msgByteLength; i++)
                {
                    msgContent[i] = 0x20;
                }

                blockBlob.DownloadToByteArray(msgContent, 0);

                LogEngine.ConsoleWriteLine(
                    "Event persisted recovered -  Consistency Transaction Point restored.", 
                    ConsoleColor.DarkGreen);

                return msgContent;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return null;
            }
        }
    }
}