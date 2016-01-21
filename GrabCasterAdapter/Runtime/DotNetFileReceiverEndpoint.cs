//---------------------------------------------------------------------
// File: DotNetFileReceiverEndpoint.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
//
// Sample: Adapter framework runtime adapter.
//
//---------------------------------------------------------------------
// This file is part of the Microsoft BizTalk Server SDK
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// This source code is intended only as a supplement to Microsoft BizTalk
// Server release and/or on-line documentation. See these other
// materials for detailed information regarding Microsoft code samples.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, WHETHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
// PURPOSE.
//---------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using System.Security;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.Samples.BizTalk.Adapter.Common;


namespace Microsoft.BizTalk.SDKSamples.Adapters
{
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Library;

    /// <summary>
    /// This class corresponds to a Receive Location/URI.  It handles polling the
    /// given folder for new messages.
    /// </summary>
    internal class DotNetFileReceiverEndpoint : ReceiverEndpoint
    {
        // constants
        private const string MESSAGE_BODY = "body";
        private const string DOT_NET_FILE_PROP_REMOTEFILENAME = "FileName";

        public DotNetFileReceiverEndpoint ()
        {
        }

        /// <summary>
        /// This method is called when a Receive Location is enabled.
        /// </summary>
        public override void Open(
            string uri,
            IPropertyBag config,
            IPropertyBag bizTalkConfig,
            IPropertyBag handlerPropertyBag,
            IBTTransportProxy transportProxy,
            string transportType,
            string propertyNamespace,
            ControlledTermination control)
        {
            Trace.WriteLine("[DotNetFileReceiverEndpoint] Open called");
            this.errorCount = 0;

            this.properties = new GrabCasterReceiveProperties();
            //  Location properties - possibly override some Handler properties
            XmlDocument locationConfigDom = ConfigProperties.ExtractConfigDom(config);
            this.properties.ReadLocationConfiguration(locationConfigDom);

            //  this is our handle back to the EPM
            this.transportProxy = transportProxy;

            // used to create new messages / message parts etc.
            this.messageFactory = this.transportProxy.GetMessageFactory();

            //  used in the creation of messages
            this.transportType = transportType;

            //  used in the creation of messages
            this.propertyNamespace = propertyNamespace;

            // used to track inflight work for shutting down properly
            this.controlledTermination = control;

            //start the task
            Start();
        }

        /// <summary>
        /// This method is called when the configuration for this receive location is modified.
        /// </summary>
        public override void Update (IPropertyBag config, IPropertyBag bizTalkConfig, IPropertyBag handlerPropertyBag)
        {
            Trace.WriteLine("[DotNetFileReceiverEndpoint] Updated called");
            lock (this)
            {
                Stop();

                errorCount = 0;

                //  keep handles to these property bags until we are ready
                this.updatedConfig             = config;
                this.updatedBizTalkConfig      = bizTalkConfig;
                this.updatedHandlerPropertyBag = handlerPropertyBag;

                if (updatedConfig != null)
                {
                    XmlDocument locationConfigDom = ConfigProperties.ExtractConfigDom(this.updatedConfig);
                    this.properties.ReadLocationConfiguration(locationConfigDom);
                }

                //Schedule the polling event
                Start();
            }
        }

        public override void Dispose()
        {
            Trace.WriteLine("[DotNetFileReceiverEndpoint] Dispose called");
            //  stop the schedule
            Stop();
        }

        private void Start()
        {
            ControlledEndpointTask(null);
        }

        private void Stop()
        {
            this.timer.Dispose();
        }

        /// <summary>
        /// this method is called from the task scheduler when the polling interval has elapsed.
        /// </summary>
        public void ControlledEndpointTask (object val)
        {
            if (this.controlledTermination.Enter())
            {
                try
                {
                    lock (this)
                    {
                        this.EndpointTask();
                    }
                    GC.Collect();
                }
                finally
                {
                    this.controlledTermination.Leave();
                }
            }
        }

        /// <summary>
        /// The set event action event embedded.
        /// </summary>
        private static Embedded.SetEventActionEventEmbedded setEventActionEventEmbedded;

        /// <summary>
        /// Handle the work to be performed each polling interval
        /// </summary>
        private void EndpointTask () 
        {
            try 
            {
                setEventActionEventEmbedded = EventReceivedFromEmbedded;
                Embedded.setEventActionEventEmbedded = setEventActionEventEmbedded;
                Console.WriteLine("Start GrabCaster Embedded Library");
                GrabCaster.Framework.Library.Embedded.StartEngine();

                //Success, reset the error count
                errorCount = 0; 
            }
            catch (Exception e) 
            {
                transportProxy.SetErrorInfo(e);
                //Track number of failures
                errorCount++;
            }
            
        }

        /// <summary>
        /// The event received from embedded.
        /// </summary>
        /// <param name="eventType">
        /// The event type.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        private void EventReceivedFromEmbedded(IEventType eventType, EventActionContext context)
        {
            string stringValue = Encoding.UTF8.GetString(eventType.DataContext);
            System.Diagnostics.Debug.WriteLine("---------------EVENT RECEIVED FROM EMBEDDED LIBRARY---------------");

            PrepareMessageAndSubmit(eventType, context);
            Trace.WriteLine("---------------EVENT RECEIVED FROM EMBEDDED LIBRARY---------------");
            Trace.WriteLine(stringValue);
            CheckErrorThreshold();

        }
        private bool CheckErrorThreshold ()
        {
            if ((0 != this.properties.ErrorThreshold) && (this.errorCount > this.properties.ErrorThreshold))
            {
                this.transportProxy.ReceiverShuttingdown(this.properties.Uri, new ErrorThresholdExceeded());
                
                //Stop the timer.
                Stop();
                return false;
            }
            return true;
        }
        List<BatchMessage> batchMessages = new List<BatchMessage>();
        //  The algorithm implemented here splits the list of files according to the
        //  batch tuning parameters (number of bytes and number of files) because the
        //  list is randomly ordered it is possible to have non-optimal batches. It would
        //  be a slight optimization to order by increasing size and then cut the batches.
        private void PrepareMessageAndSubmit (IEventType eventType, EventActionContext context)
        {
            Trace.WriteLine("[DotNetFileReceiverEndpoint] PickupFilesAndSubmit called");
            
            int maxBatchSize     = this.properties.MaximumBatchSize;
            int maximumNumberOfMessages = this.properties.MaximumNumberOfMessages;

            
            long bytesInBatch = 0;
            
            // If we couldn't lock the file, just move onto the next file
            IBaseMessage msg = CreateMessage(eventType, context);
            if ( null == msg )
                return;
            else
                batchMessages.Add(new BatchMessage(msg, context.BubblingConfiguration.MessageId, BatchOperationType.Submit));            //  keep a running total for the current batch
            bytesInBatch += eventType.DataContext.Length;

            //  zero for the value means infinite 
            bool messagesCountExceeded = ((0 != maximumNumberOfMessages) && (batchMessages.Count >= maximumNumberOfMessages));
            bool byteCountExceeded = ((0 != maxBatchSize)     && (bytesInBatch    >  maxBatchSize));

            if (messagesCountExceeded || byteCountExceeded)
            {
                //  check if we have been asked to stop - if so don't start another batch
                if (this.controlledTermination.TerminateCalled)
                    return;

                //  execute the batch
                this.SubmitMessages(batchMessages);

                //  reset the running totals
                bytesInBatch = 0;
                batchMessages.Clear();
            }
            

            //  check if we have been asked to stop - if so don't start another batch
            if (this.controlledTermination.TerminateCalled)
                return;

            //  end of file list - one final batch to do
            if (batchMessages.Count > 0)
                this.SubmitMessages(batchMessages);
        }

        /// <summary>
        /// Given a List of Messages submit them to BizTalk for processing
        /// </summary>
        private void SubmitMessages(List<BatchMessage> items)
        {
            if (items == null || items.Count == 0) throw new ArgumentException("SubmitFiles was called with an empty list of Files");

            Trace.WriteLine(string.Format("[DotNetFileReceiverEndpoint] SubmitFiles called. Submitting a batch of {0} files.", items.Count));

            //This class is used to track the files associated with this ReceiveBatch
            BatchInfo batchInfo = new BatchInfo(items);
            using (ReceiveBatch batch = new ReceiveBatch(this.transportProxy, this.controlledTermination, batchInfo.OnBatchComplete, this.properties.MaximumNumberOfMessages))
            {
                foreach (BatchMessage item in items)
                {
                    // submit file to batch
                    batch.SubmitMessage(item.Message, item.UserData);
                }

                batch.Done(null);
            }
        }

        /// <summary>
        /// This class tracks a collection of files associated with a EPM Batch
        /// </summary>
        private class BatchInfo
        {
            BatchMessage[] messages;

            internal BatchInfo(List<BatchMessage> messageList)
            {
                this.messages = new BatchMessage[messageList.Count];
                messageList.CopyTo(messages);
            }

            /// <summary>
            /// Called when the BizTalk Batch has been submitted.  If all the messages were submitted (good or suspended)
            /// we delete the files from the folder
            /// </summary>
            /// <param name="overallStatus"></param>
            internal void OnBatchComplete(bool overallStatus)
            {
                Trace.WriteLine(string.Format("[DotNetFileReceiverEndpoint] OnBatchComplete called. overallStatus == {0}.", overallStatus));

                if (overallStatus == true) //Batch completed
                {
                    //Delete the files
                    foreach (BatchMessage batchMessage in messages)
                    {
                        //Close the stream so we can delete this file
                        batchMessage.Message.BodyPart.Data.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Create a BizTalk message given the name of a file on disk optionally renaming
        /// the file while the message is being submitted into BizTalk.
        /// </summary>
        /// <param name="srcFilePath">The File to create the message from</param>
        /// <param name="renamedFileName">Optional, if specified the file will be renamed to this value.</param>
        /// <returns>The message to be submitted to BizTalk.</returns>
        private IBaseMessage CreateMessage (IEventType eventType, EventActionContext contextItem)
        {
            Stream fs;
            fs = new MemoryStream(eventType.DataContext);
            IBaseMessagePart part = this.messageFactory.CreateMessagePart();
            part.Data = fs;
            IBaseMessage message = this.messageFactory.CreateMessage();
            message.AddPart(MESSAGE_BODY, part, true);

            SystemMessageContext context = new SystemMessageContext(message.Context);
            context.InboundTransportLocation = this.properties.Uri;
            context.InboundTransportType     = this.transportType;
            
            //Write/Promote any adapter specific properties on the message context
            message.Context.Write(DOT_NET_FILE_PROP_REMOTEFILENAME, this.propertyNamespace, "Test o minchia");

            return message;
        }

        //  properties
        private GrabCasterReceiveProperties properties;

        //  handle to the EPM
        private IBTTransportProxy transportProxy;

        // used to create new messages / message parts etc.
        private IBaseMessageFactory messageFactory;

        //  used in the creation of messages
        private string transportType;

        //  used in the creation of messages
        private string propertyNamespace;

        // used to track inflight work
        private ControlledTermination controlledTermination;

        //  error count for comparison with the error threshold
        int errorCount;

        private System.Threading.Timer timer = null;

        //  support for Update
        IPropertyBag updatedConfig;
        IPropertyBag updatedBizTalkConfig;
        IPropertyBag updatedHandlerPropertyBag;
    }
}
