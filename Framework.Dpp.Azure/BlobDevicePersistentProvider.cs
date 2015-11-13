// --------------------------------------------------------------------------------------------------
// <copyright file = "PersistentProvider.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Copyright (c) 2013 - 2015 Nino Crudele
//    Blog: http://ninocrudele.me
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License. 
// </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Storage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Storage;
    using GrabCaster.Framework.Log;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json;

    /// <summary>
    /// Main persistent provider.
    /// </summary>
    [DevicePersistentProviderContract("{53158DA4-EAEA-4D8A-90C8-81A66F7A0F74}", "DevicePersistentProvider", "Device Persistent Provider for Azure")]
    public class BlobDevicePersistentProvider:IDevicePersistentProvider
    {
        public void PersistEventToStorage(byte[] messageBody, string messageId)
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

        public byte[] PersistEventFromStorage(string messageId)
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