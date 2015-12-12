// --------------------------------------------------------------------------------------------------
// <copyright file = "SyncProvider.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
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
namespace GrabCaster.Framework.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Xml;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Engine.OffRamp;
    using GrabCaster.Framework.Log;
    using GrabCaster.Framework.Serialization;
    using GrabCaster.Framework.Serialization.Xml;
    using Newtonsoft.Json;

    using Formatting = Newtonsoft.Json.Formatting;

    /// <summary>
    ///     class concerning the syncronization engine
    /// </summary>
    public static class SyncProvider
    {
        /// <summary>
        /// The on syncronization.
        /// </summary>
        public static bool OnSyncronization;

        /// <summary>
        ///     Start restart o send the message
        /// </summary>
        internal static void RestartBecauseSync()
        {
            // TODO to complete 
        }

        /// <summary>
        /// The sync write configuration.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <param name="configurationFile">
        /// The configuration file.
        /// </param>
        public static void SyncWriteConfiguration(string channelId, string pointId, byte[] configurationFile)
        {
            try
            {
                LogEngine.ConsoleWriteLine(
                    $"Received configuration from ChannelID: {channelId} PointID {pointId}", 
                    ConsoleColor.Green);
                var folder = string.Concat(Configuration.DirectoryGcPointsBubbling(), "\\", channelId, "\\", pointId);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                File.WriteAllBytes(Path.Combine(folder, "GCPointConfiguration.xml"), configurationFile);
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

        /// <summary>
        /// The sync local bubbling configuration file.
        /// </summary>
        /// <param name="syncConfigurationFile">
        /// The sync configuration file.
        /// </param>
        public static void SyncLocalBubblingConfigurationFile(SyncConfigurationFile syncConfigurationFile)
        {
            try
            {
                LogEngine.ConsoleWriteLine(
                    $"Syncronize configuration file.{syncConfigurationFile.Name}", 
                    ConsoleColor.Green);
                var filename = Path.GetFileName(syncConfigurationFile.Name);

                var configurationFile = string.Concat(
                    syncConfigurationFile.FileType == Configuration.MessageDataProperty.Trigger
                        ? Configuration.DirectoryBubblingTriggers()
                        : Configuration.DirectoryBubblingEvents(), 
                    "\\", 
                    filename);
                File.WriteAllBytes(configurationFile, syncConfigurationFile.FileContent);
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

        /// <summary>
        /// Sync a single configuration file
        /// </summary>
        /// <param name="syncConfigurationFile">
        /// The sync Configuration File.
        /// </param>
        /// <param name="messageType">
        /// The Message Type.
        /// </param>
        /// <param name="senderId">
        /// The Sender ID.
        /// </param>
        /// <param name="senderName">
        /// The Sender Name.
        /// </param>
        /// <param name="senderDescriprion">
        /// The Sender Descriprion.
        /// </param>
        /// <param name="channelId">
        /// The Channel ID.
        /// </param>
        /// <param name="channelName">
        /// The Channel Name.
        /// </param>
        /// <param name="channelDescription">
        /// The Channel Description.
        /// </param>
        public static void SyncBubblingConfigurationFile(
            SyncConfigurationFile syncConfigurationFile, 
            string messageType, 
            string senderId, 
            string senderName, 
            string senderDescriprion, 
            string channelId, 
            string channelName, 
            string channelDescription)
        {
            try
            {
                LogEngine.ConsoleWriteLine(
                    $"Syncronize configuration file. {syncConfigurationFile.Name} from PointID {senderId} ", 
                    ConsoleColor.Green);

                var gcPointInfo = string.Concat(
                    channelId, 
                    "^", 
                    channelName, 
                    "^", 
                    channelDescription, 
                    "^", 
                    senderId, 
                    "^", 
                    senderName, 
                    "^", 
                    senderDescriprion);
                var filename = Path.GetFileName(syncConfigurationFile.Name);

                var configurationFile = string.Concat(
                    Configuration.DirectoryGcPointsBubbling(), 
                    "\\", 
                    channelId, 
                    "\\", 
                    senderId, 
                    "\\", 
                    syncConfigurationFile.FileType == Configuration.MessageDataProperty.Trigger
                        ? Configuration.DirectoryNameTriggers
                        : Configuration.DirectoryNameEvents, 
                    "\\", 
                    filename);
                var gcPointInfoFile = string.Concat(
                    Configuration.DirectoryGcPointsBubbling(), 
                    "\\", 
                    channelId, 
                    "\\", 
                    senderId, 
                    "\\", 
                    Configuration.DirectoryFileNameTriggerEventInfo);
                var directory = Path.GetDirectoryName(configurationFile);
                if (directory != null && !Directory.Exists(directory))
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Directory.CreateDirectory(directory);
                }
                else
                {

                    LogEngine.WriteLog(Configuration.EngineName, 
                        $"Error in {MethodBase.GetCurrentMethod().Name} - Missing configuration file.", 
                        Constant.ErrorEventIdHighCritical, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Error);
                }

                File.WriteAllText(gcPointInfoFile, gcPointInfo);

                File.WriteAllBytes(configurationFile, syncConfigurationFile.FileContent);
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

        // Sync all the configuration  event and triggers files
        /// <summary>
        /// TODO The sync bubbling configuration file list.
        /// </summary>
        /// <param name="syncConfigurationFileList">
        /// TODO The sync configuration file list.
        /// </param>
        /// <param name="messageType">
        /// TODO The message type.
        /// </param>
        /// <param name="senderId">
        /// TODO The sender id.
        /// </param>
        /// <param name="senderName">
        /// TODO The sender name.
        /// </param>
        /// <param name="senderDescriprion">
        /// TODO The sender descriprion.
        /// </param>
        /// <param name="channelId">
        /// TODO The channel id.
        /// </param>
        /// <param name="channelName">
        /// TODO The channel name.
        /// </param>
        /// <param name="channelDescription">
        /// TODO The channel description.
        /// </param>
        public static void SyncBubblingConfigurationFileList(
            List<SyncConfigurationFile> syncConfigurationFileList, 
            string messageType, 
            string senderId, 
            string senderName, 
            string senderDescriprion, 
            string channelId, 
            string channelName, 
            string channelDescription)
        {
            try
            {
                foreach (var syncConfigurationFile in syncConfigurationFileList)
                {
                    SyncBubblingConfigurationFile(
                        syncConfigurationFile, 
                        messageType, 
                        senderId, 
                        senderName, 
                        senderDescriprion, 
                        channelId, 
                        channelName, 
                        channelDescription);
                }
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

        // Sync all the configuration  event and triggers files
        /// <summary>
        /// TODO The sync local bubbling configuration file.
        /// </summary>
        /// <param name="syncConfigurationFileList">
        /// TODO The sync configuration file list.
        /// </param>
        public static void SyncLocalBubblingConfigurationFile(List<SyncConfigurationFile> syncConfigurationFileList)
        {
            try
            {
                foreach (var syncConfigurationFile in syncConfigurationFileList)
                {
                    SyncLocalBubblingConfigurationFile(syncConfigurationFile);
                }
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

        /// <summary>
        /// Send all Bubbling configuration
        /// </summary>
        /// <param name="channelId">
        /// The Channel ID.
        /// </param>
        /// <param name="pointId">
        /// The Point ID.
        /// </param>
        public static void SyncSendBubblingConfiguration(string channelId, string pointId)
        {
            try
            {
                if (!Configuration.DisableDeviceProviderInterface())
                {
                    LogEngine.ConsoleWriteLine(
                        $"Syncronizing  Bubbling confuguration - Point ID {Configuration.PointId()}", 
                        ConsoleColor.Yellow);
                    OffRampEngineSending.SendMessageOnRamp(
                        EventsEngine.SyncConfigurationFileList, 
                        Configuration.MessageDataProperty.SyncSendBubblingConfiguration, 
                        channelId, 
                        pointId, 
                        null);
                }
                else
                {
                    LogEngine.WriteLog(
                        Configuration.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.ErrorEventIdHighCritical, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Warning);
                }
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

        /// <summary>
        /// Send all configuration
        /// </summary>
        /// <param name="channelId">
        /// The Channel ID.
        /// </param>
        /// <param name="pointId">
        /// The Point ID.
        /// </param>
        public static void SyncSendConfiguration(string channelId, string pointId)
        {
            try
            {
                if (!Configuration.DisableDeviceProviderInterface())
                {
                    LogEngine.ConsoleWriteLine(
                        $"Send all confuguration - Point ID {Configuration.PointId()}", 
                        ConsoleColor.Yellow);
                    var stream = GetConfiguration();

                    // byte[] data
                    // SerializationEngine.ObjectToByteArray(stream);
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        OffRampEngineSending.SendMessageOnRamp(
                            memoryStream.ToArray(), 
                            Configuration.MessageDataProperty.SyncSendConfiguration, 
                            channelId, 
                            pointId, 
                            null);
                    }
                }
                else
                {
                    LogEngine.WriteLog(
                        Configuration.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.ErrorEventIdHighCritical, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Warning);
                }
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

        /// <summary>
        /// TODO The sync send file bubbling configuration.
        /// </summary>
        /// <param name="channelId">
        /// TODO The channel id.
        /// </param>
        /// <param name="pointId">
        /// TODO The point id.
        /// </param>
        /// <param name="fileName">
        /// TODO The file name.
        /// </param>
        /// <param name="messageType">
        /// TODO The message type.
        /// </param>
        public static void SyncSendFileBubblingConfiguration(
            string channelId, 
            string pointId, 
            string fileName, 
            Configuration.MessageDataProperty messageType)
        {
            try
            {
                if (Configuration.DisableDeviceProviderInterface())
                {
                    LogEngine.WriteLog(
                        Configuration.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.ErrorEventIdHighCritical, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Warning);
                    return;
                }

                LogEngine.ConsoleWriteLine(
                    $"Syncronizing  Bubbling confuguration - Point ID {Configuration.PointId()}", 
                    ConsoleColor.Yellow);
                var triggerConfigurationList = new List<SyncConfigurationFile>();

                try
                {
                    var folder = Path.Combine(
                        Configuration.DirectoryGcPointsBubbling(), 
                        channelId, 
                        pointId, 
                        messageType.ToString() == "Trigger"
                            ? Configuration.DirectoryNameTriggers
                            : Configuration.DirectoryNameEvents);

                    var content = File.ReadAllBytes(Path.Combine(folder, fileName));

                    var syncConfigurationFile = new SyncConfigurationFile(
                        messageType, 
                        fileName, 
                        content, 
                        string.Empty);
                    triggerConfigurationList.Add(syncConfigurationFile);
                    OffRampEngineSending.SendMessageOnRamp(
                        triggerConfigurationList, 
                        Configuration.MessageDataProperty.SyncSendFileBubblingConfiguration, 
                        channelId, 
                        pointId, 
                        null);
                }
                catch (Exception ex)
                {
                    LogEngine.WriteLog(
                        Configuration.EngineName, 
                        $"Error in {MethodBase.GetCurrentMethod().Name} File {fileName}", 
                        Constant.ErrorEventIdHighCritical, 
                        Constant.TaskCategoriesError, 
                        ex, 
                        EventLogEntryType.Error);
                }
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

        /// <summary>
        /// Request all the bubbling configuration
        /// </summary>
        /// <param name="channelId">
        /// </param>
        /// <param name="pointId">
        /// </param>
        public static void SyncSendRequestBubblingConfiguration(string channelId, string pointId)
        {
            try
            {
                LogEngine.ConsoleWriteLine(
                    $"Syncronizing  Bubbling confuguration - Point ID {Configuration.PointId()}", 
                    ConsoleColor.Yellow);
                OffRampEngineSending.SendNullMessageOnRamp(
                    Configuration.MessageDataProperty.SyncSendRequestBubblingConfiguration, 
                    channelId, 
                    pointId, 
                    string.Empty, 
                    null);
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

        /// <summary>
        /// Request all the configuration
        /// </summary>
        /// <param name="channelId">
        /// </param>
        /// <param name="pointId">
        /// </param>
        public static void SyncSendRequestConfiguration(string channelId, string pointId)
        {
            try
            {
                LogEngine.ConsoleWriteLine(
                    $"Syncronizing  Bubbling confuguration - Point ID {Configuration.PointId()}", 
                    ConsoleColor.Yellow);
                OffRampEngineSending.SendNullMessageOnRamp(
                    Configuration.MessageDataProperty.SyncSendRequestConfiguration, 
                    channelId, 
                    pointId, 
                    string.Empty, 
                    null);
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

        /// <summary>
        /// The sync update component.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <param name="bubblingEvent">
        /// The bubbling event.
        /// </param>
        public static void SyncUpdateComponent(string channelId, string pointId, BubblingEvent bubblingEvent)
        {
            try
            {
                var assemblyToUpdate = Path.ChangeExtension(
                    bubblingEvent.AssemblyFile, 
                    Configuration.UpdateFileExtension);

                var path = string.Empty;
                if (bubblingEvent.BubblingEventType == BubblingEventType.Trigger)
                {
                    path = Path.Combine(Configuration.DirectoryTriggers(), assemblyToUpdate);
                }

                if (bubblingEvent.BubblingEventType == BubblingEventType.Event)
                {
                    path = Path.Combine(Configuration.DirectoryEvents(), assemblyToUpdate);
                }

                // Path.Combine(Configuration.DirectoryBubblingTriggers()
                File.WriteAllBytes(path, bubblingEvent.AssemblyContent);
                ServiceStates.RestartNeeded = true;
                LogEngine.ConsoleWriteLine(
                    $"Received IDComponent {bubblingEvent.IdComponent} from Point ID {pointId}", 
                    ConsoleColor.Yellow);
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

        /// <summary>
        /// The sync send component.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <param name="idComponent">
        /// The id component.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string SyncSendComponent(string channelId, string pointId, string idComponent)
        {
            try
            {
                LogEngine.ConsoleWriteLine(
                    $"Send IDComponent {idComponent} to Point ID {pointId}", 
                    ConsoleColor.Yellow);

                var component =
                    (from components in EventsEngine.GlobalEventListBaseDll
                     where components.IdComponent == idComponent
                     select components).First();
                string ret;
                if (component != null)
                {
                    OffRampEngineSending.SendMessageOnRamp(
                        component, 
                        Configuration.MessageDataProperty.SyncSendComponent, 
                        channelId, 
                        pointId, 
                        null);
                    ret = $"Sent IDComponent {idComponent} to Point ID {pointId}";
                }
                else
                {
                    ret = $"IDComponent {idComponent} not present.";
                }

                return ret;
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
                return ex.Message;
            }
        }

        /// <summary>
        /// Request all the bubbling configuration
        /// </summary>
        /// <param name="channelId">
        /// </param>
        /// <param name="pointId">
        /// </param>
        /// <param name="idComponent">
        /// The ID Component.
        /// </param>
        public static void SyncSendRequestComponent(string channelId, string pointId, string idComponent)
        {
            try
            {
                LogEngine.ConsoleWriteLine(
                    $"Send request for IDComponent {idComponent} to Point ID {pointId}", 
                    ConsoleColor.Yellow);
                OffRampEngineSending.SendNullMessageOnRamp(
                    Configuration.MessageDataProperty.SyncSendRequestComponent, 
                    channelId, 
                    pointId, 
                    idComponent, 
                    null);
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

        /// <summary>
        /// Return the complete configuration
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public static Stream GetConfiguration()
        {
            try
            {
                // TODO [Next version] Implement swagger definition too
                var docMain = new XmlDocument();

                // Configuration node
                var eConfiguration = docMain.CreateElement(string.Empty, "Configuration", string.Empty);

                // Configurtation root node
                // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                XmlHelpers.AddAttribute(docMain, eConfiguration, "DateTimeNow", DateTime.Now.ToString());

                // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                XmlHelpers.AddAttribute(docMain, eConfiguration, "DateTimeUtcNow", DateTime.UtcNow.ToString());
                XmlHelpers.AddAttribute(docMain, eConfiguration, "ChannelId", Configuration.ChannelId());
                XmlHelpers.AddAttribute(docMain, eConfiguration, "ChannelName", Configuration.ChannelName());
                XmlHelpers.AddAttribute(
                    docMain, 
                    eConfiguration, 
                    "ChannelDescription", 
                    Configuration.ChannelDescription());
                XmlHelpers.AddAttribute(docMain, eConfiguration, "PointId", Configuration.PointId());
                XmlHelpers.AddAttribute(docMain, eConfiguration, "PointName", Configuration.PointName());
                XmlHelpers.AddAttribute(docMain, eConfiguration, "PointDescription", Configuration.PointDescription());

                // Bubbling Triggers
                var bubbling = docMain.CreateElement(string.Empty, "Bubbling", string.Empty);
                eConfiguration.AppendChild(bubbling);

                var bubblingTriggers = docMain.CreateElement(string.Empty, "Triggers", string.Empty);
                bubbling.AppendChild(bubblingTriggers);

                foreach (var triggerConfiguration in EventsEngine.TriggerConfigurationList)
                {
                    var eTrigger = docMain.CreateElement(string.Empty, "Trigger", string.Empty);
                    bubblingTriggers.AppendChild(eTrigger);

                    XmlHelpers.AddAttribute(docMain, eTrigger, "IdComponent", triggerConfiguration.Trigger.IdComponent);
                    XmlHelpers.AddAttribute(docMain, eTrigger, "Name", triggerConfiguration.Trigger.Name);
                    XmlHelpers.AddAttribute(docMain, eTrigger, "Description", triggerConfiguration.Trigger.Description);

                    // Properties
                    var properties = docMain.CreateElement(string.Empty, "TriggerProperties", string.Empty);
                    eTrigger.AppendChild(properties);
                    if (triggerConfiguration.Trigger.TriggerProperties != null)
                    {
                        foreach (var property in triggerConfiguration.Trigger.TriggerProperties)
                        {
                            if (property.Name != "DataContext")
                            {
                                var xeProperty = docMain.CreateElement(string.Empty, "TriggerProperty", string.Empty);
                                properties.AppendChild(xeProperty);

                                XmlHelpers.AddAttribute(docMain, xeProperty, "Name", property.Name);
                                XmlHelpers.AddAttribute(docMain, xeProperty, "Value", property.Value.ToString());
                            }
                        }
                    }

                    // Events
                    // ReSharper disable once InconsistentNaming
                    var Events = docMain.CreateElement(string.Empty, "Events", string.Empty);
                    eTrigger.AppendChild(Events);
                    foreach (var _event in triggerConfiguration.Events)
                    {
                        var Event = docMain.CreateElement(string.Empty, "Event", string.Empty);
                        Events.AppendChild(Event);

                        XmlHelpers.AddAttribute(docMain, Event, "IdComponent", _event.IdComponent);
                        XmlHelpers.AddAttribute(docMain, Event, "IdConfiguration", _event.IdConfiguration);
                        XmlHelpers.AddAttribute(docMain, Event, "Name", _event.Name);
                        XmlHelpers.AddAttribute(docMain, Event, "Description", _event.Description);

                        // 1*
                        if (_event.Channels != null)
                        {
                            var channels = docMain.CreateElement(string.Empty, "Channels", string.Empty);
                            Event.AppendChild(channels);

                            // ReSharper disable once InconsistentNaming
                            foreach (var _channel in _event.Channels)
                            {
                                var channel = docMain.CreateElement(string.Empty, "Channel", string.Empty);
                                channels.AppendChild(channel);
                                XmlHelpers.AddAttribute(docMain, channel, "ChannelId", _channel.ChannelId);
                                XmlHelpers.AddAttribute(docMain, channel, "Name", _channel.ChannelName);
                                XmlHelpers.AddAttribute(docMain, channel, "Description", _channel.ChannelDescription);

                                // ReSharper disable once InconsistentNaming
                                foreach (var _point in _channel.Points)
                                {
                                    var point = docMain.CreateElement(string.Empty, "Point", string.Empty);
                                    channel.AppendChild(point);
                                    XmlHelpers.AddAttribute(docMain, point, "PointId", _point.PointId);
                                    XmlHelpers.AddAttribute(docMain, channel, "Name", _point.Name);
                                    XmlHelpers.AddAttribute(docMain, channel, "Description", _point.Description);
                                }
                            }
                        }

                        if (_event.Correlation != null)
                        {
                            var correlations = docMain.CreateElement(string.Empty, "Correlation", string.Empty);
                            Event.AppendChild(correlations);
                            var endPointsCorrelation = docMain.CreateElement(string.Empty, "EndPointIDs", string.Empty);
                            correlations.AppendChild(endPointsCorrelation);

                            var correlationChannels = docMain.CreateElement(string.Empty, "Channels", string.Empty);
                            Event.AppendChild(correlationChannels);

                            // ReSharper disable once InconsistentNaming
                            foreach (var _channel in _event.Channels)
                            {
                                var channel = docMain.CreateElement(string.Empty, "Channel", string.Empty);
                                correlationChannels.AppendChild(channel);
                                XmlHelpers.AddAttribute(docMain, channel, "ChannelId", _channel.ChannelId);
                                XmlHelpers.AddAttribute(docMain, channel, "Name", _channel.ChannelName);
                                XmlHelpers.AddAttribute(docMain, channel, "Description", _channel.ChannelDescription);

                                // ReSharper disable once InconsistentNaming
                                foreach (var _point in _channel.Points)
                                {
                                    var point = docMain.CreateElement(string.Empty, "Point", string.Empty);
                                    channel.AppendChild(channel);
                                    XmlHelpers.AddAttribute(docMain, point, "PointId", _point.PointId);
                                    XmlHelpers.AddAttribute(docMain, channel, "Name", _point.Name);
                                    XmlHelpers.AddAttribute(docMain, channel, "Description", _point.Description);
                                }
                            }

                            endPointsCorrelation.AppendChild(correlationChannels);

                            XmlHelpers.AddAttribute(docMain, Event, "Name", _event.Correlation.Name);
                            XmlHelpers.AddAttribute(docMain, Event, "ScriptRule", _event.Correlation.ScriptRule);

                            // ReSharper disable once UnusedVariable
                            foreach (var eventCorrelated in _event.Correlation.Events)
                            {
                                var eventCorrelation = docMain.CreateElement(string.Empty, "Events", string.Empty);
                                correlations.AppendChild(eventCorrelation);

                                XmlHelpers.AddAttribute(docMain, eventCorrelation, "IdComponent", _event.IdComponent);
                                XmlHelpers.AddAttribute(
                                    docMain, 
                                    eventCorrelation, 
                                    "IDConfiguration", 
                                    _event.IdConfiguration);
                                XmlHelpers.AddAttribute(docMain, eventCorrelation, "Name", _event.Name);
                                XmlHelpers.AddAttribute(docMain, eventCorrelation, "Description", _event.Description);
                            }
                        }
                    }
                    {
                        // json configuration
                        var jsonTemplate = docMain.CreateElement(string.Empty, "JSON", string.Empty);
                        eTrigger.AppendChild(jsonTemplate);
                        var jsonSerialization = JsonConvert.SerializeObject(
                            triggerConfiguration, 
                            Formatting.Indented, 
                            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                        var jsonText = docMain.CreateTextNode(jsonSerialization);
                        jsonTemplate.AppendChild(jsonText);
                    }
                }

                // Bubbling Triggers
                // ************************************************************

                // ************************************************************
                // Bubbling Events
                var bubblingEvents = docMain.CreateElement(string.Empty, "Events", string.Empty);
                bubbling.AppendChild(bubblingEvents);

                foreach (var eventConfiguration in EventsEngine.EventConfigurationList)
                {
                    var eEventBubbling = docMain.CreateElement(string.Empty, "Event", string.Empty);
                    bubblingEvents.AppendChild(eEventBubbling);

                    XmlHelpers.AddAttribute(
                        docMain, 
                        eEventBubbling, 
                        "IdConfiguration", 
                        eventConfiguration.Event.IdConfiguration);
                    XmlHelpers.AddAttribute(
                        docMain, 
                        eEventBubbling, 
                        "IdComponent", 
                        eventConfiguration.Event.IdComponent);
                    XmlHelpers.AddAttribute(docMain, eEventBubbling, "Name", eventConfiguration.Event.Name);
                    XmlHelpers.AddAttribute(
                        docMain, 
                        eEventBubbling, 
                        "Description", 
                        eventConfiguration.Event.Description);

                    if (eventConfiguration.Event.EventProperties != null)
                    {
                        // Properties
                        var propertiesEvent = docMain.CreateElement(string.Empty, "EventProperties", string.Empty);
                        eEventBubbling.AppendChild(propertiesEvent);
                        foreach (var property in eventConfiguration.Event.EventProperties)
                        {
                            if (property.Name != "DataContext")
                            {
                                var xeProperty = docMain.CreateElement(string.Empty, "EventProperty", string.Empty);
                                propertiesEvent.AppendChild(xeProperty);

                                XmlHelpers.AddAttribute(docMain, xeProperty, "Name", property.Name);
                                XmlHelpers.AddAttribute(docMain, xeProperty, "Value", property.Value.ToString());
                            }
                        }
                    }
                    {
                        // json Template
                        var jsonTemplate = docMain.CreateElement(string.Empty, "JSON", string.Empty);
                        bubblingEvents.AppendChild(jsonTemplate);
                        var jsonSerialization = JsonConvert.SerializeObject(
                            eventConfiguration, 
                            Formatting.Indented, 
                            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                        var jsonText = docMain.CreateTextNode(jsonSerialization);
                        jsonTemplate.AppendChild(jsonText);
                    }

                    // Bubbling Events
                    // ************************************************************
                }

                // ************************************************************
                // Triggers
                var triggers = from trigger in EventsEngine.GlobalEventListBaseDll
                               where trigger.BubblingEventType == BubblingEventType.Trigger
                               select trigger;

                var eTriggers = docMain.CreateElement(string.Empty, "Triggers", string.Empty);

                eConfiguration.AppendChild(eTriggers);

                foreach (var bubblingEvent in triggers)
                {
                    var eTrigger = docMain.CreateElement(string.Empty, "Trigger", string.Empty);
                    eTriggers.AppendChild(eTrigger);

                    XmlHelpers.AddAttribute(docMain, eTrigger, "IdComponent", bubblingEvent.IdComponent);
                    XmlHelpers.AddAttribute(docMain, eTrigger, "Name", bubblingEvent.Name);
                    XmlHelpers.AddAttribute(docMain, eTrigger, "Description", bubblingEvent.Description);
                    XmlHelpers.AddAttribute(docMain, eTrigger, "isActive", bubblingEvent.IsActive.ToString());
                    XmlHelpers.AddAttribute(docMain, eTrigger, "Version", bubblingEvent.Version.ToString());
                    XmlHelpers.AddAttribute(docMain, eTrigger, "Shared", bubblingEvent.Shared.ToString());
                    XmlHelpers.AddAttribute(
                        docMain, 
                        eTrigger, 
                        "PollingRequired", 
                        bubblingEvent.PollingRequired.ToString());
                    XmlHelpers.AddAttribute(docMain, eTrigger, "AssemblyFile", bubblingEvent.AssemblyFile);

                    // Actions
                    var properties = docMain.CreateElement(string.Empty, "Properties", string.Empty);
                    eTrigger.AppendChild(properties);
                    if (bubblingEvent.Properties.Count > 1)
                    {
                        foreach (var property in bubblingEvent.Properties)
                        {
                            if (property.Name != "DataContext")
                            {
                                var xeProperty = docMain.CreateElement(string.Empty, "Property", string.Empty);
                                properties.AppendChild(xeProperty);

                                XmlHelpers.AddAttribute(docMain, xeProperty, "Name", property.Name);
                                XmlHelpers.AddAttribute(docMain, xeProperty, "Description", property.Description);
                                XmlHelpers.AddAttribute(
                                    docMain, 
                                    xeProperty, 
                                    "Type", 
                                    property.AssemblyPropertyInfo.PropertyType.ToString());
                            }
                        }
                    }

                    // Actions
                    if (bubblingEvent.BaseActions.Count > 1)
                    {
                        var actions = docMain.CreateElement(string.Empty, "Actions", string.Empty);
                        eTrigger.AppendChild(actions);
                        foreach (var baseAction in bubblingEvent.BaseActions)
                        {
                            var action = docMain.CreateElement(string.Empty, "Action", string.Empty);
                            actions.AppendChild(action);

                            XmlHelpers.AddAttribute(docMain, action, "Id", baseAction.Id);
                            XmlHelpers.AddAttribute(docMain, action, "Name", baseAction.Name);
                            XmlHelpers.AddAttribute(docMain, action, "Description", baseAction.Description);

                            var parameters = docMain.CreateElement(string.Empty, "Parameters", string.Empty);

                            // ReSharper disable once InconsistentNaming
                            foreach (var Parameter in baseAction.Parameters)
                            {
                                var parameter = docMain.CreateElement(string.Empty, "Parameter", string.Empty);
                                parameters.AppendChild(parameter);
                                XmlHelpers.AddAttribute(docMain, parameter, "Name", parameter.Name);
                            }
                        }
                    }
                    {
                        // json Template
                        var jsonTemplate = docMain.CreateElement(string.Empty, "JSON", string.Empty);
                        eTrigger.AppendChild(jsonTemplate);
                        var jsonSerialization = SerializationHelper.CreteJsonTriggerConfigurationTemplate(bubblingEvent);
                        var jsonText = docMain.CreateTextNode(jsonSerialization);
                        jsonTemplate.AppendChild(jsonText);
                    }
                }

                // ************************************************************
                // Events
                var events = from Event in EventsEngine.GlobalEventListBaseDll
                             where Event.BubblingEventType == BubblingEventType.Event
                             select Event;

                var eEvents = docMain.CreateElement(string.Empty, "Events", string.Empty);

                eConfiguration.AppendChild(eEvents);

                foreach (var bubblingEvent in events)
                {
                    var eEvent = docMain.CreateElement(string.Empty, "Event", string.Empty);
                    eEvents.AppendChild(eEvent);

                    XmlHelpers.AddAttribute(docMain, eEvent, "IdComponent", bubblingEvent.IdComponent);
                    XmlHelpers.AddAttribute(docMain, eEvent, "Name", bubblingEvent.Name);
                    XmlHelpers.AddAttribute(docMain, eEvent, "Description", bubblingEvent.Description);
                    XmlHelpers.AddAttribute(docMain, eEvent, "isActive", bubblingEvent.IsActive.ToString());
                    XmlHelpers.AddAttribute(docMain, eEvent, "Version", bubblingEvent.Version.ToString());
                    XmlHelpers.AddAttribute(docMain, eEvent, "Shared", bubblingEvent.Shared.ToString());
                    XmlHelpers.AddAttribute(
                        docMain, 
                        eEvent, 
                        "PollingRequired", 
                        bubblingEvent.PollingRequired.ToString());
                    XmlHelpers.AddAttribute(docMain, eEvent, "AssemblyFile", bubblingEvent.AssemblyFile);

                    // Actions
                    var properties = docMain.CreateElement(string.Empty, "Properties", string.Empty);
                    eEvent.AppendChild(properties);
                    if (bubblingEvent.Properties.Count > 1)
                    {
                        foreach (var property in bubblingEvent.Properties)
                        {
                            if (property.Name != "DataContext")
                            {
                                var xeProperty = docMain.CreateElement(string.Empty, "Property", string.Empty);
                                properties.AppendChild(xeProperty);

                                XmlHelpers.AddAttribute(docMain, xeProperty, "Name", property.Name);
                                XmlHelpers.AddAttribute(docMain, xeProperty, "Description", property.Description);
                                XmlHelpers.AddAttribute(
                                    docMain, 
                                    xeProperty, 
                                    "Type", 
                                    property.AssemblyPropertyInfo.PropertyType.ToString());
                            }
                        }
                    }

                    // Actions
                    if (bubblingEvent.BaseActions.Count > 1)
                    {
                        var actions = docMain.CreateElement(string.Empty, "Actions", string.Empty);
                        eEvent.AppendChild(actions);
                        foreach (var baseAction in bubblingEvent.BaseActions)
                        {
                            var action = docMain.CreateElement(string.Empty, "Action", string.Empty);
                            actions.AppendChild(action);

                            XmlHelpers.AddAttribute(docMain, action, "Id", baseAction.Id);
                            XmlHelpers.AddAttribute(docMain, action, "Name", baseAction.Name);
                            XmlHelpers.AddAttribute(docMain, action, "Description", baseAction.Description);

                            var parameters = docMain.CreateElement(string.Empty, "Parameters", string.Empty);

                            // ReSharper disable once InconsistentNaming
                            foreach (var Parameter in baseAction.Parameters)
                            {
                                var parameter = docMain.CreateElement(string.Empty, "Parameter", string.Empty);
                                parameters.AppendChild(parameter);
                                XmlHelpers.AddAttribute(docMain, parameter, "Name", parameter.Name);
                            }
                        }
                    }
                    {
                        // json Template
                        var jsonTemplate = docMain.CreateElement(string.Empty, "JSON", string.Empty);
                        eEvent.AppendChild(jsonTemplate);
                        var jsonSerialization = SerializationHelper.CreteJsonEventConfigurationTemplate(bubblingEvent);
                        var jsonText = docMain.CreateTextNode(jsonSerialization);
                        jsonTemplate.AppendChild(jsonText);
                    }
                }

                // If called by web context
                if (WebOperationContext.Current != null)
                {
                    var currentWebContext = WebOperationContext.Current;
                    if (currentWebContext != null)
                    {
                        currentWebContext.OutgoingResponse.ContentType = "text/xml";
                    }
                }

                return new MemoryStream(Encoding.UTF8.GetBytes(eConfiguration.OuterXml));
            }
            catch (Exception ex)
            {
                var docMain = new XmlDocument();
                var errorTemplate = docMain.CreateElement(string.Empty, "Error", string.Empty);
                var errorText = docMain.CreateTextNode(ex.Message);
                errorTemplate.AppendChild(errorText);

                var currentWebContext = WebOperationContext.Current;
                currentWebContext.OutgoingResponse.ContentType = "text/xml";
                return new MemoryStream(Encoding.UTF8.GetBytes(errorTemplate.OuterXml));
            }
        }

        // *********************************************************************************************
        // Configuration files AREA
        // *********************************************************************************************
        /// <summary>
        /// TODO The refresh bubbling setting.
        /// </summary>
        public static void RefreshBubblingSetting()
        {
            ////Check if it's a simple event/trigger configuration update (NOT DLL), then just update the eventlistsetting
            LogEngine.ConsoleWriteLine("-!EVENTS CONFIGURATION ENGINE SYNC!-", ConsoleColor.Green);
            EventsEngine.RefreshBubblingSetting();
            Configuration.LoadConfiguration();
        }
    }
}