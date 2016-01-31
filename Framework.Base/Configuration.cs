// --------------------------------------------------------------------------------------------------
// <copyright file = "Configuration.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Base
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using Microsoft.ServiceBus;
    using Newtonsoft.Json;
    using GrabCaster.Framework.Common;

    public enum EhReceivePatternType
    {
        Direct,

        Abstract
    }

    public enum EventHubsCheckPointPattern
    {
        CheckPoint,

        Dt,

        Dtepoch,

        Dtutcnow,

        Dtnow,

        Dtutcnowepoch,

        Dtnowepoch
    }

    /// <summary>
    ///     Configuration
    /// </summary>
    public static class Configuration
    {
        //EH General properties Messange 
        public enum MessageDataProperty
        {
            SubscriberId,

            ByteArray,

            Data,

            ReceiverChannelId,

            ReceiverPointId,

            ChannelId,

            ChannelName,

            ChannelDescription,

            SenderId,

            SenderName,

            SenderDescriprion,

            OperationTime,

            IdComponent,

            IdConfiguration,

            Embedded,

            MessageType,

            MessageId,

            Persisting,

            Event,

            Trigger,
            //Log,
            //TriggerDLL,
            SyncSendLocalDll,

            SyncSendBubblingConfiguration, //Send the bubbling configuration , the receiver will put in gcpoints

            SyncSendRequestBubblingConfiguration,
            //Send request bubbling, the receiver send the bubbling configuration, the point will put in gcpoints

            SyncSendFileBubblingConfiguration,
            //Send a file trg or evn to update, te reiver will update the conf file in bubbling trg or evn

            SyncSendRequestConfiguration, //Send the request for all the configuration 

            SyncSendConfiguration, //Send the request for all the configuration 

            SyncSendComponent, //Send a component to sync

            SyncSendRequestComponent, //Send a request to receive a component to sync

            TriggerEventJson,

            EventPropertyJson,

            SyncRequest,

            SyncRequestConfirmed,

            SyncAvailable,

            Message,

            ConsoleSync,

            ConsoleSyncEhName,

            ConsoleSyncEhConnstring
        }

        //Configuration storagew
        //Abstraction layer for the configuration storage, now is using json file
        public static ConfigurationStorage ConfigurationStorage;

        //General vars
        public static string FlagFileNameSyncToDo = "Sync.todo";

        public static string GcEventsConfigurationFilesExtensions = @".evn|.trg|.off";

        public static string GcEventsFilesExtensions = @".dll|.evn|.trg|.off";

        public static string EngineName = "GrabCaster";

        public static string CloneFileExtension = "clone";

        public static string UpdateFileExtension = "update";

        public static string DllFileExtension = "dll";

        public static string BlobDllSyncPostfix = "components";

        //public static string General_RunTimeFile = "runtime";
        public static string ConfigurationFileExtension = ".cfg";

        public static string GrabCasterMessageTypeName = "{08608A61-1A85-4E37-B27F-3E77A22A3CA7}";

        public static string GrabCasterMessageTypeValue = "{08608A61-1A85-4E37-B27F-3E77A22A3CA7}";

        //Directories

        public static string DirectoryNameConfigurationRoot = "Root";

        public static string DirectoryFileNameTriggerEventInfo = "GCPoint.info";

        public static string DirectoryNameTriggers = "Triggers";

        public static string DirectoryNameEvents = "Events";

        public static string DirectoryNameBubbling = "Bubbling";

        public static string DirectoryNamePoints = "GCPoints";

        //Messages
        public static string MessageContextAll = "*";

        public static string MessageFileStorageExtension = @".gcm";

        //Channels
        public static string ChannelAll = "*";

        //Points
        public static string PointAll = "*";

        //Events
        public static string TriggersDllExtension = @".(dll)";

        public static string EventsDllExtension = @".(dll)";

        public static string TriggersUpdateExtension = @".(update)";

        public static string EventsUpdateExtension = @".(update)";

        public static string TriggersDllExtensionLookFor = "*.dll";

        public static string EventsDllExtensionLookFor = "*.dll";

        public static string TriggersUpdateExtensionLookFor = "*.update";

        public static string EventsUpdateExtensionLookFor = "*.update";

        public static string BubblingTriggersEventsDllExtension = @".dll";

        public static string BubblingTriggersExtension = @".trg";

        public static string BubblingEventsExtension = @".evn";

        public static string BubblingOffExtension = @".off";

        //Methods
        public static void LoadConfiguration()
        {
            //Get Exe name
            var filename = 
                Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName).Replace(".vshost", "");

            //Get the configuration file
            var configurationFile = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                string.Concat(filename.Replace(".vshost", ""), ConfigurationFileExtension));
            Debug.WriteLine("ConfigurationFile:", configurationFile);
            ConfigurationStorage =
                JsonConvert.DeserializeObject<ConfigurationStorage>(
                    Encoding.UTF8.GetString(File.ReadAllBytes(configurationFile)));

            string BaseDirectory;
            //Check Cluster configuration
            if (Configuration.Clustered())
            {
                BaseDirectory = Configuration.ClusterBaseFolder();
                if (!Directory.Exists(BaseDirectory))
                {
                    Methods.DirectEventViewerLog($"Missing the Cluster Base Folder Directory {BaseDirectory}.", EventLogEntryType.Error);
                    throw new NotImplementedException($"Missing the Cluster Base Folder Directory {BaseDirectory}.");
                }
            }
            else
            {
                BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            var rootDirConf = Path.Combine(
                BaseDirectory,
                string.Concat(DirectoryNameConfigurationRoot, "_", filename));

            if (!Directory.Exists(rootDirConf))
            {
                Methods.DirectEventViewerLog($"Missing the Configuration Directory {rootDirConf}.", EventLogEntryType.Error);
                throw new NotImplementedException($"Missing the Configuration Directory {rootDirConf}.");
            }

            //AppDomain.CurrentDomain.BaseDirectory
            ConfigurationStorage.DirectoryOperativeRootExeName = Path.Combine(
                BaseDirectory,
                string.Concat(DirectoryNameConfigurationRoot, "_", filename));
            ConfigurationStorage.DirectoryServiceExecutable = BaseDirectory;
        }

        public static void SaveConfgurtation(ConfigurationStorage configurationStorage)
        {
            var filename = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);
            var configurationFile = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                string.Concat(filename.Replace(".vshost", ""), ConfigurationFileExtension));
            Debug.WriteLine("ConfigurationFile:", configurationFile);
            var configurationStorageContent = JsonConvert.SerializeObject(configurationStorage);

            File.WriteAllText(configurationFile, configurationStorageContent);
        }
        public static bool Clustered()
        {
            return ConfigurationStorage.Clustered;
        }
        public static string ClusterBaseFolder()
        {
            return ConfigurationStorage.ClusterBaseFolder;
        }

        public static bool SecondaryPersistProviderEnabled()
        {
            return ConfigurationStorage.SecondaryPersistProviderEnabled;
        }
        public static int SecondaryPersistProviderByteSize()
        {
            return ConfigurationStorage.SecondaryPersistProviderByteSize;
        }

        public static string PersistentProviderComponent()
        {
            return ConfigurationStorage.PersistentProviderComponent;
        }

        /// <summary>
        ///     Event Up Stream component used to send messages (EventHubs/File/.)
        /// </summary>
        /// <returns></returns>
        public static string EventsStreamComponent()
        {
            return ConfigurationStorage.EventsStreamComponent;
        }
        /// <summary>
        ///     Log pattern to use (EventHubs/File/.)
        /// </summary>
        /// <returns></returns>
        public static string LoggingComponent()
        {
            return ConfigurationStorage.LoggingComponent;
        }

        /// <summary>
        ///     Log pattern to use (EventHubs/File/.)
        /// </summary>
        /// <returns></returns>
        public static string LoggingComponentStorage()
        {
            return ConfigurationStorage.LoggingComponentStorage;
        }

        /// <summary>
        ///     Activate the Verbose Logging mode (All the logging levels Information/Warming/Errors)
        /// </summary>
        /// <returns></returns>
        public static bool LoggingVerbose()
        {
            return ConfigurationStorage.LoggingVerbose;
        }

        /// <summary>
        ///     Event receiving pattern (Direct/Abstration)
        /// </summary>
        /// <returns></returns>
        public static EhReceivePatternType EventHubsReceivingPattern()
        {
            return ConfigurationStorage.EventHubsReceivingPattern;
        }

        /// <summary>
        ///     Engine WeAPI EndPoint
        /// </summary>
        /// <returns></returns>
        public static string WebApiEndPoint()
        {
            return ConfigurationStorage.WebApiEndPoint;
        }

        /// <summary>
        ///     Time needs wait before rstarting
        /// </summary>
        /// <returns></returns>
        public static int WaitTimeBeforeRestarting()
        {
            return ConfigurationStorage.WaitTimeBeforeRestarting;
        }

        /// <summary>
        ///     Enabling engine persists message
        /// </summary>
        /// <returns></returns>
        public static bool EnablePersistingMessaging()
        {
            return ConfigurationStorage.EnablePersistingMessaging;
        }

        /// <summary>
        ///     Service executable directory
        /// </summary>
        /// <returns></returns>
        public static string DirectoryServiceExecutable()
        {
            return ConfigurationStorage.DirectoryServiceExecutable;
        }

        /// <summary>
        ///     Directory containing bubbling directory and events + triggers directory (jitRoor_executablefilename)
        /// </summary>
        /// <returns></returns>
        public static string DirectoryOperativeRootExeName()
        {
            return ConfigurationStorage.DirectoryOperativeRootExeName;
        }

        /// <summary>
        ///     BUBBLING directory
        /// </summary>
        /// <returns></returns>
        public static string DirectoryBubbling()
        {
            return Path.Combine(ConfigurationStorage.DirectoryOperativeRootExeName, DirectoryNameBubbling);
        }

        /// <summary>
        ///     BUBBLING directory
        /// </summary>
        /// <returns></returns>
        public static string DirectoryLog()
        {
            return Path.Combine(ConfigurationStorage.DirectoryOperativeRootExeName, "Log");
        }
        /// <summary>
        ///     ENDPOINTS directory
        /// </summary>
        /// <returns></returns>
        public static string DirectoryEndPoints()
        {
            return Path.Combine(ConfigurationStorage.DirectoryOperativeRootExeName, DirectoryNamePoints);
        }

        /// <summary>
        ///     ENDPOINTS directory
        /// </summary>
        /// <returns></returns>
        public static string DirectoryGcPointsBubbling()
        {
            return Path.Combine(ConfigurationStorage.DirectoryOperativeRootExeName, DirectoryNamePoints);
        }

        /// <summary>
        ///     TRIGGERS directory
        /// </summary>
        /// <returns></returns>
        public static string DirectoryTriggers()
        {
            return Path.Combine(ConfigurationStorage.DirectoryOperativeRootExeName, DirectoryNameTriggers);
        }

        /// <summary>
        ///     EVENTS directory
        /// </summary>
        /// <returns></returns>
        public static string DirectoryEvents()
        {
            return Path.Combine(ConfigurationStorage.DirectoryOperativeRootExeName, DirectoryNameEvents);
        }

        /// <summary>
        ///     ROOT\BUBBLING\TRIGGERS
        /// </summary>
        /// <returns></returns>
        public static string DirectoryBubblingTriggers()
        {
            return Path.Combine(
                ConfigurationStorage.DirectoryOperativeRootExeName,
                DirectoryNameBubbling,
                DirectoryNameTriggers);
        }

        /// <summary>
        ///     BUBBLING\EVENTS direcotry
        /// </summary>
        /// <returns></returns>
        public static string DirectoryBubblingEvents()
        {
            return Path.Combine(
                ConfigurationStorage.DirectoryOperativeRootExeName,
                DirectoryNameBubbling,
                DirectoryNameEvents);
        }

        /// <summary>
        ///     EventHubs group connection string
        /// </summary>
        /// <returns></returns>
        public static string AzureNameSpaceConnectionString()
        {
            return ConfigurationStorage.AzureNameSpaceConnectionString;
        }

        public static string RedisConnectionString()
        {
            return ConfigurationStorage.RedisConnectionString;
        }
        /// <summary>
        ///     EH group name used by service
        /// </summary>
        /// <returns></returns>
        public static string GroupEventHubsName()
        {
            return ConfigurationStorage.GroupEventHubsName;
        }

        /// <summary>
        ///     EH group storage
        /// </summary>
        /// <returns></returns>
        public static string GroupEventHubsStorageAccountName()
        {
            //TODO if not exist the SSO key it must created
            return ConfigurationStorage.GroupEventHubsStorageAccountName;
        }

        /// <summary>
        ///     EH Group Storage Account key
        /// </summary>
        /// <returns></returns>
        public static string GroupEventHubsStorageAccountKey()
        {
            //TODO if not exist the SSO key it must created
            return ConfigurationStorage.GroupEventHubsStorageAccountKey;
        }

        /// <summary>
        ///     Local storage used by the engine
        /// </summary>
        /// <returns></returns>
        public static string LocalStorageProvider()
        {
            return ConfigurationStorage.LocalStorageProvider;
        }

        /// <summary>
        ///     Local storage used by the engine
        /// </summary>
        /// <returns></returns>
        public static string LocalStorageConnectionString()
        {
            return ConfigurationStorage.LocalStorageConnectionString;
        }

        /// <summary>
        ///     point ID
        /// </summary>
        /// <returns></returns>
        public static string PointId()
        {
            return ConfigurationStorage.PointId;
        }

        /// <summary>
        ///     unique point Name of the local service
        /// </summary>
        /// <returns></returns>
        public static string PointName()
        {
            return ConfigurationStorage.PointName;
        }

        /// <summary>
        ///     point description
        /// </summary>
        /// <returns></returns>
        public static string PointDescription()
        {
            return ConfigurationStorage.PointDescription;
        }

        /// <summary>
        ///     Channel ID
        /// </summary>
        /// <returns></returns>
        public static string ChannelId()
        {
            return ConfigurationStorage.ChannelId;
        }

        /// <summary>
        ///     unique ChannelName of the local service
        /// </summary>
        /// <returns></returns>
        public static string ChannelName()
        {
            return ConfigurationStorage.ChannelName;
        }

        /// <summary>
        ///     Channel description
        /// </summary>
        /// <returns></returns>
        public static string ChannelDescription()
        {
            return ConfigurationStorage.ChannelDescription;
        }

        /// <summary>
        ///     Enable log
        /// </summary>
        /// <returns></returns>
        public static bool LoggingEngineEnabled()
        {
            return ConfigurationStorage.LoggingEngineEnabled;
        }

        /// <summary>
        ///     Connection string of storage
        /// </summary>
        /// <returns></returns>
        public static string ReturnStorageConnectionString()
        {
            return
                $"DefaultEndpointsProtocol=https;AccountName={ConfigurationStorage.GroupEventHubsStorageAccountName};AccountKey={ConfigurationStorage.GroupEventHubsStorageAccountKey}";
        }

        /// <summary>
        ///     Trigger polling time (Milliseconds)
        /// </summary>
        /// <returns></returns>
        public static int EnginePollingTime()
        {
            return ConfigurationStorage.EnginePollingTime;
        }

        /// <summary>
        ///     Define the date time receiving time from Event Hubs
        ///     Values("26/07/2015 16:58:35")
        /// </summary>
        /// <returns></returns>
        public static string EventHubsStartingDateTimeReceiving()
        {
            return ConfigurationStorage.EventHubsStartingDateTimeReceiving;
        }

        /// <summary>
        ///     Define the Event Hubs epochs
        /// </summary>
        /// <returns></returns>
        public static long EventHubsEpoch()
        {
            return ConfigurationStorage.EventHubsEpoch;
        }

        /// <summary>
        ///     Define the Event Hubs Transport Type
        /// </summary>
        /// <returns></returns>
        public static ConnectivityMode ServiceBusConnectivityMode()
        {
            return ConfigurationStorage.ServiceBusConnectivityMode;
        }

        /// <summary>
        /// Number of message in queue before publish
        /// </summary>
        /// <returns></returns>
        public static bool DisableDeviceProviderInterface()
        {
            return ConfigurationStorage.DisableDeviceProviderInterface;
        }

        /// <summary>
        /// Number of message in queue before publish
        /// </summary>
        /// <returns></returns>
        public static int ThrottlingOnRampIncomingRateNumber()
        {
            return ConfigurationStorage.ThrottlingOnRampIncomingRateNumber;
        }

        /// <summary>
        /// Number of second in queue before publish
        /// </summary>
        /// <returns></returns>
        public static int ThrottlingOnRampIncomingRateSeconds()
        {
            return ConfigurationStorage.ThrottlingOnRampIncomingRateSeconds;
        }

        /// <summary>
        /// Number of message in queue before send
        /// </summary>
        /// <returns></returns>
        public static int ThrottlingOffRampIncomingRateNumber()
        {
            return ConfigurationStorage.ThrottlingOffRampIncomingRateNumber;
        }

        /// <summary>
        /// Number of seconds in queue before publish
        /// </summary>
        /// <returns></returns>
        public static int ThrottlingOffRampIncomingRateSeconds()
        {
            return ConfigurationStorage.ThrottlingOffRampIncomingRateSeconds;
        }

        /// <summary>
        /// Number of message in queue before writing in console
        /// </summary>
        /// <returns></returns>
        public static int ThrottlingConsoleLogIncomingRateNumber()
        {
            return ConfigurationStorage.ThrottlingConsoleLogIncomingRateNumber;
        }

        /// <summary>
        /// Number of seconds in queue before writing in console
        /// </summary>
        /// <returns></returns>
        public static int ThrottlingConsoleLogIncomingRateSeconds()
        {
            return ConfigurationStorage.ThrottlingConsoleLogIncomingRateSeconds;
        }

        /// <summary>
        /// Number of message in queue before writing send to log component provider
        /// </summary>
        /// <returns></returns>
        public static int ThrottlingLsiLogIncomingRateNumber()
        {
            return ConfigurationStorage.ThrottlingLsiLogIncomingRateNumber;
        }

        /// <summary>
        /// Number of seconds in queue before writing send to log component provider
        /// </summary>
        /// <returns></returns>
        public static int ThrottlingLsiLogIncomingRateSeconds()
        {
            return ConfigurationStorage.ThrottlingLsiLogIncomingRateSeconds;
        }

        /// <summary>
        /// Number of seconds in queue before writing send to log component provider
        /// </summary>
        /// <returns></returns>
        public static bool RunLocalOnly()
        {
            return ConfigurationStorage.RunLocalOnly;
        }

        /// <summary>
        ///     Define the Event Hubs checkpoint pattern
        /// </summary>
        /// <returns></returns>
        public static EventHubsCheckPointPattern EventHubsCheckPointPattern()
        {
            return ConfigurationStorage.EventHubsCheckPointPattern;
        }
    }

    [DataContract]
    [Serializable]
    public class ConfigurationStorage
    {
        [DataMember]
        public bool LoggingVerbose { get; set; }
        [DataMember]
        public bool Clustered { get; set; }
        [DataMember]
        public string ClusterBaseFolder { get; set; }
        [DataMember]
        public bool SecondaryPersistProviderEnabled { get; set; }
        [DataMember]
        public int SecondaryPersistProviderByteSize { get; set; }

        [DataMember]
        public string PersistentProviderComponent { get; set; }
        [DataMember]
        public string EventsStreamComponent { get; set; }

        [DataMember]
        public string LoggingComponent { get; set; }

        [DataMember]
        public string LoggingComponentStorage { get; set; }

        [DataMember]
        public EhReceivePatternType EventHubsReceivingPattern { get; set; }

        /// <summary>
        ///     Wait time before restart
        /// </summary>
        [DataMember]
        public int WaitTimeBeforeRestarting { get; set; }

        [DataMember]
        public string WebApiEndPoint { get; set; }

        /// <summary>
        ///     Enable persisting message engine
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public bool EnablePersistingMessaging { get; set; }

        public string DirectoryServiceExecutable { get; set; }

        public string DirectoryOperativeRootExeName { get; set; }

        //Main Azure connection string
        [DataMember]
        public string AzureNameSpaceConnectionString { get; set; }

        [DataMember]
        public string RedisConnectionString { get; set; }

        /// <summary>
        ///     EventHub name used by station
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string GroupEventHubsName { get; set; }

        /// <returns></returns>
        /// <summary>
        ///     Group Azure Storage Account name 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string GroupEventHubsStorageAccountName { get; set; }

        /// <summary>
        ///     Group Storage Account key
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string GroupEventHubsStorageAccountKey { get; set; }

        /// <summary>
        ///     Local storage pattern used by engine
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string LocalStorageProvider { get; set; }

        /// <summary>
        ///     Local storage used by engine
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string LocalStorageConnectionString { get; set; }

        /// <summary>
        ///     Unique GUID point of the local engine
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string PointId { get; set; }

        /// <summary>
        ///     point name
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string PointName { get; set; }

        /// <summary>
        ///     point Description
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string PointDescription { get; set; }

        /// <summary>
        ///     Unique GUID channel of the local engine
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string ChannelId { get; set; }

        /// <summary>
        ///     Channel name
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string ChannelName { get; set; }

        /// <summary>
        ///     Engine Description
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string ChannelDescription { get; set; }

        /// <summary>
        ///     Enable log
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public bool LoggingEngineEnabled { get; set; }

        /// <summary>
        ///     Global Trigger polling time (Milliseconds)
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int EnginePollingTime { get; set; }

        /// <summary>
        ///     Define the date time receiving time from Event Hubs
        ///     Values("26/07/2015 16:58:35")
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string EventHubsStartingDateTimeReceiving { get; set; }

        /// <summary>
        ///     Define the Event Hubs epochs
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public long EventHubsEpoch { get; set; }

        [DataMember]
        public ConnectivityMode ServiceBusConnectivityMode { get; set; }

        /// <summary>
        ///     Define the Event Hubs checkpoint pattern
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public EventHubsCheckPointPattern EventHubsCheckPointPattern { get; set; }

        [DataMember]
        public bool DisableDeviceProviderInterface { get; set; }

        [DataMember]
        public int ThrottlingOnRampIncomingRateNumber { get; set; }

        [DataMember]
        public int ThrottlingOnRampIncomingRateSeconds { get; set; }

        [DataMember]
        public int ThrottlingOffRampIncomingRateNumber { get; set; }

        [DataMember]
        public int ThrottlingOffRampIncomingRateSeconds { get; set; }

        [DataMember]
        public int ThrottlingConsoleLogIncomingRateNumber { get; set; }

        [DataMember]
        public int ThrottlingConsoleLogIncomingRateSeconds { get; set; }

        [DataMember]
        public int ThrottlingLsiLogIncomingRateNumber { get; set; }

        [DataMember]
        public int ThrottlingLsiLogIncomingRateSeconds { get; set; }
        [DataMember]
        public bool RunLocalOnly { get; set; }
        
    }
}