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
namespace GrabCaster.Framework.Engine
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Xml;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Log;

    public class RestEventsEngine : IRestEventsEngine
    {
        /// <summary>
        /// Send the all bubbling configuration
        /// </summary>
        /// <param name="channelId">
        /// The Channel ID.
        /// </param>
        /// <param name="pointId">
        /// The Point ID.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string SyncSendBubblingConfiguration(string channelId, string pointId)
        {
            try
            {
                if (Base.Configuration.DisableExternalEventsStreamEngine())
                {
                    LogEngine.WriteLog(
                        Base.Configuration.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.DefconThree, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Warning);

                    return
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
                }

                SyncProvider.SyncSendBubblingConfiguration(channelId, pointId);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.DefconOne, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return ex.Message;
            }

            return $"Syncronization Executed at {DateTime.Now}.";
        }

        // http://localhost:8000/GrabCaster/SyncSendFileBubblingConfiguration?ChannelID={047B6D1E-A991-4CB1-ACAB-E83C3BDC0097}&PointID={B0A46E60-443C-4E8A-A6ED-7F2CB34CF9E5}&FileName=Demo Get SimpleFile Remote.trg&MessageType=Trigger
        public string SyncSendFileBubblingConfiguration(
            string channelId, 
            string pointId, 
            string fileName, 
            Configuration.MessageDataProperty messageType)
        {
            try
            {
                if (Base.Configuration.DisableExternalEventsStreamEngine())
                {
                    LogEngine.WriteLog(
                        Base.Configuration.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.DefconThree, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Warning);

                    return
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
                }

                SyncProvider.SyncSendFileBubblingConfiguration(channelId, pointId, fileName, messageType);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.DefconOne, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return ex.Message;
            }

            return $"Syncronization Executed at {DateTime.Now}.";
        }

        // Ask all the bubbling configuration
        // http://localhost:8000/GrabCaster/SyncSendRequestBubblingConfiguration?ChannelID=*&PointID=*
        public string SyncSendRequestBubblingConfiguration(string channelId, string pointId)
        {
            try
            {
                if (Base.Configuration.DisableExternalEventsStreamEngine())
                {
                    LogEngine.WriteLog(
                        Base.Configuration.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.DefconThree, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Warning);

                    return
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
                }

                SyncProvider.SyncSendRequestBubblingConfiguration(channelId, pointId);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.DefconOne, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return ex.Message;
            }

            return $"Syncronization Executed at {DateTime.Now}.";
        }

        // Send component
        // http://localhost:8000/GrabCaster/SyncSendComponent?ChannelID=*&PointID=*&IDComponent={3C62B951-C353-4899-8670-C6687B6EAEFC}
        public string SyncSendComponent(string channelId, string pointId, string idComponent)
        {
            try
            {
                if (Base.Configuration.DisableExternalEventsStreamEngine())
                {
                    LogEngine.WriteLog(
                        Base.Configuration.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.DefconThree, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Warning);

                    return
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
                }

                SyncProvider.SyncSendComponent(channelId, pointId, idComponent);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.DefconOne, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return ex.Message;
            }

            return $"Syncronization Executed at {DateTime.Now}.";
        }

        // Send request component
        // http://localhost:8000/GrabCaster/SyncSendRequestComponent?ChannelID=*&PointID=*&IDComponent={3C62B951-C353-4899-8670-C6687B6EAEFC}
        public string SyncSendRequestComponent(string channelId, string pointId, string idComponent)
        {
            try
            {
                if (Base.Configuration.DisableExternalEventsStreamEngine())
                {
                    LogEngine.WriteLog(
                        Base.Configuration.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.DefconThree, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Warning);

                    return
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
                }

                SyncProvider.SyncSendRequestComponent(channelId, pointId, idComponent);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.DefconOne, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return ex.Message;
            }

            return $"Syncronization Executed at {DateTime.Now}.";
        }

        // Internal operations
        public string SyncSendRequestConfiguration(string channelId, string pointId)
        {
            try
            {
                if (Base.Configuration.DisableExternalEventsStreamEngine())
                {
                    LogEngine.WriteLog(
                        Base.Configuration.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.DefconThree, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Warning);

                    return
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
                }

                SyncProvider.SyncSendRequestConfiguration(channelId, pointId);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.DefconOne, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return ex.Message;
            }

            return $"Configuration request sent at ChanelID: {channelId} PoinrID {pointId}.";
        }

        /// <summary>
        /// Refresh the internal bubbling setting
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// http://localhost:8000/GrabCaster/RefreshBubblingSetting
        public string RefreshBubblingSetting()
        {
            try
            {
                SyncProvider.RefreshBubblingSetting();
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.DefconOne, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return ex.Message;
            }

            return $"Syncronization Executed at {DateTime.Now}.";
        }

        /// <summary>
        /// Execute an internal trigger
        /// </summary>
        /// <param name="triggerId">
        /// </param>
        /// <param name="configurationId"></param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// http://localhost:8000/GrabCaster/ExecuteTrigger?ConfigurationID={3C62B951-C353-4899-8670-C6687B6EAEFC}TriggerID={3C62B951-C353-4899-8670-C6687B6EAEFC}&value=text
        public string ExecuteTrigger(string configurationId,string triggerId,string value)
        {
            try
            {
                var executed = false;
                try
                {
                    var triggerSingleInstance = (from trigger in EventsEngine.BubblingTriggerConfigurationsSingleInstance
                                                 where trigger.IdComponent == triggerId && trigger.IdConfiguration == configurationId
                                                 select trigger).First();
                    var bubblingTriggerConfiguration = triggerSingleInstance;
                    byte[] content = Encoding.UTF8.GetBytes(value);
                    EventsEngine.ExecuteTriggerConfiguration(bubblingTriggerConfiguration,content);
                    executed = true;
                }
                catch
                {
                    // ignored
                }

                try
                {
                    var triggerPollingInstance = (from trigger in EventsEngine.BubblingTriggerConfigurationsPolling
                                                  where trigger.IdComponent == triggerId && trigger.IdConfiguration == configurationId
                                                  select trigger).First();
                    var bubblingTriggerConfiguration = triggerPollingInstance;
                    byte[] content = Encoding.UTF8.GetBytes(value);
                    EventsEngine.ExecuteTriggerConfiguration(bubblingTriggerConfiguration,content);
                    executed = true;
                }
                catch
                {
                    // ignored
                }

                return executed ? "Trigger executed." : "Trigger not executed check the Windows event viewer.";
            }
            catch (Exception ex)
            {
                return $"Error - {ex.Message} ";
            }
        }

        /// <summary>
        /// Return the complete configuration
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        /// http://localhost:8000/GrabCaster/Configuration
        public Stream Configuration()
        {
            try
            {
                return SyncProvider.GetConfiguration();
            }
            catch (Exception ex)
            {
                var docMain = new XmlDocument();
                var errorTemplate = docMain.CreateElement(string.Empty, "Error", string.Empty);
                var errorText = docMain.CreateTextNode(ex.Message);
                errorTemplate.AppendChild(errorText);

                var currentWebContext = WebOperationContext.Current;
                if (currentWebContext != null)
                {
                    currentWebContext.OutgoingResponse.ContentType = "text/xml";
                }
                return new MemoryStream(Encoding.UTF8.GetBytes(errorTemplate.OuterXml));
            }
        }
    }
}