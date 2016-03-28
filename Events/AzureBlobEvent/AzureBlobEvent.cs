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
namespace GrabCaster.Framework.AzureBlobEvent
{
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Handles the Azure Blob event.
    /// </summary>
    [EventContract("{C185F004-62E4-45A4-97B1-BD0D382FFE33}", "Azure Blob Event", "Show a messagebox", true)]
    public class AzureBlobEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the Azure storage account.
        /// </summary>
        /// <value>
        /// The Azure storage account.
        /// </value>
        [EventPropertyContract("StorageAccount", "Azure StorageAccount")]
        public string StorageAccount { get; set; }

        /// <summary>
        /// Gets or sets the Azure BLOB container.
        /// </summary>
        /// <value>
        /// The Azure BLOB container.
        /// </value>
        [EventPropertyContract("BlobContainer", "Azure Blob Container")]
        public string BlobContainer { get; set; }

        /// <summary>
        /// Gets or sets the Azure BLOB block reference.
        /// </summary>
        /// <value>
        /// The Azure BLOB block reference.
        /// </value>
        [EventPropertyContract("BlobBlockReference", "Azure Blob BlockReference")]
        public string BlobBlockReference { get; set; }

        /// <summary>
        /// Gets or sets the event context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public EventActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action event.
        /// </summary>
        /// <value>
        /// The set event action event.
        /// </value>
        public SetEventActionEvent SetEventActionEvent { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="setEventActionEvent">
        /// The set event action event.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [EventActionContract("{346FC437-8464-4566-8AD6-A7E4B29A7EBC}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                this.Context = context;
                this.SetEventActionEvent = setEventActionEvent;

                var storageAccount = CloudStorageAccount.Parse(this.StorageAccount);
                var blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container. 
                var container = blobClient.GetContainerReference(this.BlobContainer);

                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();
                container.SetPermissions(
                    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                var blockBlob = container.GetBlockBlobReference(this.BlobBlockReference);
                blockBlob.UploadFromByteArray(this.DataContext, 0, this.DataContext.Length);

                setEventActionEvent(this, context);
            }
            catch
            {
                // ignored
            }
        } // Execute
    } // AzureBlobEvent
} // namespace