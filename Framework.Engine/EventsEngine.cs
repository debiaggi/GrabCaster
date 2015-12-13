// --------------------------------------------------------------------------------------------------
// <copyright file = "EventsEngine.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;
    using GrabCaster.Framework.Engine.OffRamp;
    using GrabCaster.Framework.Log;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    using Newtonsoft.Json;

    using Roslyn.Scripting.CSharp;

    /// <summary>
    ///     This is engine class containing the most important methods
    /// </summary>
    public static class EventsEngine
    {
        // Global Action Triggers
        /// <summary>
        /// The delegate action trigger.
        /// </summary>
        private static SetEventActionTrigger delegateActionTrigger;

        // Global Action Events
        /// <summary>
        /// The delegate action event.
        /// </summary>
        private static SetEventActionEvent delegateActionEvent;

        // Global Events and Triggers List DLL
        /// <summary>
        /// The global event list base dll.
        /// </summary>
        public static List<BubblingEvent> GlobalEventListBaseDll;

        // Triggers and Triggers active and running
        /// <summary>
        /// The bubbling triggers events active.
        /// </summary>
        public static List<BubblingEvent> BubblingTriggersEventsActive;

        // Triggers and Events configured and running
        /// <summary>
        /// The bubbling trigger configurations polling.
        /// </summary>
        public static List<BubblingEvent> BubblingTriggerConfigurationsPolling;

        /// <summary>
        /// The bubbling trigger configurations single instance.
        /// </summary>
        public static List<BubblingEvent> BubblingTriggerConfigurationsSingleInstance;

        // All shared
        /// <summary>
        /// The assembly event list shared.
        /// </summary>
        public static List<AssemblyEvent> AssemblyEventListShared;

        // Triggers and Events files configurations
        /// <summary>
        /// The sync configuration file list.
        /// </summary>
        public static List<SyncConfigurationFile> SyncConfigurationFileList;

        // All trigger files configurations
        /// <summary>
        /// The trigger configuration list.
        /// </summary>
        public static List<TriggerConfiguration> TriggerConfigurationList;

        // All event files configurations
        /// <summary>
        /// The event configuration list.
        /// </summary>
        public static List<EventConfiguration> EventConfigurationList;

        /// <summary>
        /// The connection string.
        /// </summary>
        private static string connectionString = string.Empty;

        /// <summary>
        /// The event hub name.
        /// </summary>
        private static string eventHubName = string.Empty;

        /// <summary>
        /// Gets the hub client.
        /// </summary>
        public static EventHubClient HubClient { get; private set; }

        // ***********REST*************
        /// <summary>
        /// The engine host.
        /// </summary>
        private static WebServiceHost engineHost;

        // ***************************
        // ********************
        // Events Sync Watcher
        /// <summary>
        /// The fsw event folder.
        /// </summary>
        private static readonly FileSystemWatcher FswEventFolder = new FileSystemWatcher();

        /// <summary>
        ///     Trigger Delegate initialization
        /// </summary>
        public static void InitializeEventEngine(SetEventActionEvent delegateActionEventEmbedded)
        {
            // Start Web API interface
            LogEngine.ConsoleWriteLine(
                $"Start the Hub Web API Interface at {Configuration.WebApiEndPoint()}", 
                ConsoleColor.Yellow);
            engineHost = new WebServiceHost(typeof(RestEventsEngine), new Uri(Configuration.WebApiEndPoint()));
            engineHost.AddServiceEndpoint(typeof(IRestEventsEngine), new WebHttpBinding(), Configuration.EngineName);
            var stp = engineHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            stp.HttpHelpPageEnabled = false;
            engineHost.Open();
            LogEngine.ConsoleWriteLine("Hub Web API Interface Running.", ConsoleColor.Yellow);

            delegateActionTrigger = ActionTriggerReceived;
            if (delegateActionEventEmbedded != null)
            {
                //
                delegateActionEvent = delegateActionEventEmbedded;

            }
            else
            {
                delegateActionEvent = ActionEventReceived;

            }
        }

        /// <summary>
        /// The delegate event executed by a trigger
        /// </summary>
        /// <param name="trigger">
        /// </param>
        /// <param name="context">
        /// </param>
        public static void ActionTriggerReceived(ITriggerType trigger, EventActionContext context)
        {
            if (context == null)
            {
                return;
            }

            lock (context)
            {
                foreach (var propertyInfo in trigger.GetType().GetProperties())
                {
                    var propertyFound =
                        context.BubblingConfiguration.Properties.FirstOrDefault(obj => obj.Name == propertyInfo.Name);
                    if (propertyFound != null && propertyFound.Name != "context"
                        && propertyFound.Name != "SetEventActionTrigger")
                    {
                        propertyFound.Value = propertyInfo.GetValue(trigger);
                    }
                }

                // Events mapping
                // if channel null then local event
                var localEvents =
                    (from _event in context.BubblingConfiguration.Events where _event.Channels == null select _event)
                        .ToList();

                // Local event context
                if (localEvents.Count != 0)
                {
                    var eventsCloned = ObjectHelper.CloneObject(localEvents);
                    var bubblingEventClone = (BubblingEvent)ObjectHelper.CloneObject(context.BubblingConfiguration);
                    bubblingEventClone.Events = (List<Event>)eventsCloned;
                    ExecuteBubblingActionEvent(bubblingEventClone, true, Configuration.PointId());
                }

                var remoteEvents =
                    (from _event in context.BubblingConfiguration.Events where _event.Channels != null select _event)
                        .ToList();

                // Send to MSP
                if (remoteEvents.Count != 0 && !Configuration.DisableDeviceProviderInterface())
                {
                    var eventsCloned = ObjectHelper.CloneObject(remoteEvents);
                    var bubblingEventClone = (BubblingEvent)ObjectHelper.CloneObject(context.BubblingConfiguration);
                    bubblingEventClone.Events = (List<Event>)eventsCloned;
                    OffRampEngineSending.SendMessageOnRamp(
                        bubblingEventClone, 
                        Configuration.MessageDataProperty.Event, 
                        string.Empty, 
                        string.Empty, 
                        null);
                }
            }
        }

        /// <summary>
        /// The delegate event executed by a event
        /// </summary>
        /// <param name="eventType">
        /// </param>
        /// <param name="context">
        /// EventActionContext cosa deve essere esuito
        /// </param>
        private static void ActionEventReceived(IEventType eventType, EventActionContext context)
        {
            lock (context)
            {
                try
                {
                    // if exist correlation then execute
                    if (context.BubblingConfiguration.Correlation == null
                        && context.BubblingConfiguration.CorrelationOverride == null)
                    {
                        return;
                    }

                    // Correlation exist -> execute it
                    // execute the rule
                    // CSharp using rosling passing datacontext = propertyInfo.GetValue(IEventType) if true then execute

                    // Set correlation base
                    var correlation = context.BubblingConfiguration.Correlation;

                    // Check if overriding required
                    if (context.BubblingConfiguration.CorrelationOverride != null)
                    {
                        correlation = context.BubblingConfiguration.CorrelationOverride;
                    }

                    // TODO 1010
                    var propDataContext =
                        eventType.GetType().GetProperties().FirstOrDefault(obj => obj.Name == "DataContext");

                    // ReSharper disable once PossibleNullReferenceException, impossible to happen
                    var value = propDataContext.GetValue(eventType);

                    if (correlation != null && ExecuteRoslynRule(value, correlation.ScriptRule) == false)
                    {
                        context.BubblingConfiguration.CorrelationOverride = null;
                        return;
                    }

                    context.BubblingConfiguration.CorrelationOverride = null;
                    if (context.BubblingConfiguration.Correlation != null)
                    {
                        LogEngine.ConsoleWriteLine(
                            $"-!CORRELATION NAME {context.BubblingConfiguration.Correlation.Name} EVALUATE TRUE!-", 
                            ConsoleColor.Green);

                        context.BubblingConfiguration.Events = context.BubblingConfiguration.Correlation.Events;
                        context.BubblingConfiguration.Events = context.BubblingConfiguration.Correlation.Events;
                    }

                    // keep easy and discard the correlation, I don't need more
                    context.BubblingConfiguration.Correlation = null;
                    foreach (var propertyInfo in eventType.GetType().GetProperties())
                    {
                        var propertyFound =
                            context.BubblingConfiguration.Properties.FirstOrDefault(
                                obj => obj.Name == propertyInfo.Name);
                        if (propertyFound != null && propertyFound.Name != "context"
                            && propertyFound.Name != "SetEventActionTrigger")
                        {
                            propertyFound.Value = propertyInfo.GetValue(eventType);
                        }
                    }

                    // 1*
                    // Events mapping
                    // if channel null the local event
                    var localEvents = from levents in context.BubblingConfiguration.Events
                                      from channel in levents.Channels
                                      where channel == null
                                      select levents;

                    // Local event context
                    var levent = (List<Event>)localEvents;

                    // ReSharper disable once UseObjectOrCollectionInitializer
                    var contextLocal = new EventActionContext(context.BubblingConfiguration);
                    contextLocal.BubblingConfiguration.Events = levent;
                    ExecuteBubblingActionEvent(contextLocal.BubblingConfiguration, true, Configuration.ChannelId());

                    var remoteEvents = from levents in context.BubblingConfiguration.Events
                                       from channel in levents.Channels
                                       where channel != null
                                       select levent;

                    // Send to MSP
                    if (!Configuration.DisableDeviceProviderInterface() && remoteEvents.Any())
                    {
                        var remoteContext = new EventActionContext(context.BubblingConfiguration);
                        OffRampEngineSending.SendMessageOnRamp(
                            remoteContext.BubblingConfiguration, 
                            Configuration.MessageDataProperty.Event, 
                            string.Empty, 
                            string.Empty, 
                            null);
                    }
                }
                catch (Exception ex)
                {
                    context.BubblingConfiguration.CorrelationOverride = null;

                    LogEngine.WriteLog(
                        Configuration.EngineName, 
                        $"Error in {MethodBase.GetCurrentMethod().Name}", 
                        Constant.ErrorEventIdHighCritical, 
                        Constant.TaskCategoriesError, 
                        ex, 
                        EventLogEntryType.Error);
                }
            }
        }

        /// <summary>
        /// The execute roslyn rule.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="script">
        /// The script.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool ExecuteRoslynRule(object dataContext, string script)
        {
            try
            {
                // return true;
                var hostObject = new HostContext { DataContext = Encoding.UTF8.GetString((byte[])dataContext) };

                var roslynEngine = new ScriptEngine();
                var session = roslynEngine.CreateSession(hostObject);
                session.AddReference(hostObject.GetType().Assembly);
                session.ImportNamespace("System");
                session.ImportNamespace("System.Windows.Forms");
                session.ImportNamespace("Framework");
                session.ImportNamespace("System.Text");

                // TODO 1006
                session.AddReference(
                    @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Windows.Forms.dll");

                var bval = session.Execute(script);

                return (bool)bval;
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

        /// <summary>
        ///     Sync the assembly list event
        /// </summary>
        public static void UpdateAssemblyEventListShared()
        {
            try
            {
                // Load all assembly content
                AssemblyEventListShared = new List<AssemblyEvent>();
                var sharedEvents = from eventShared in GlobalEventListBaseDll
                                   where eventShared.Shared
                                   select eventShared;
                foreach (var sharedEvent in sharedEvents)
                {
                    var assemblyEvent = new AssemblyEvent
                                            {
                                                Id = sharedEvent.IdComponent, 
                                                FileName = Path.GetFileName(sharedEvent.AssemblyFile), 
                                                PathFileName = sharedEvent.AssemblyFile, 
                                                Type = sharedEvent.BubblingEventType.ToString(), 
                                                Version = sharedEvent.Version.ToString(), 
                                                AssemblyContent =
                                                    File.ReadAllBytes(sharedEvent.AssemblyFile)
                                            };
                    AssemblyEventListShared.Add(assemblyEvent);
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
        /// The check for file to update.
        /// </summary>
        public static void CheckForFileToUpdate()
        {
            try
            {
                // Load triggers bubbling path
                var triggersDirectory = Configuration.DirectoryTriggers();
                var regTriggers = new Regex(Configuration.TriggersUpdateExtension);
                var assemblyFilesTriggers =
                    Directory.GetFiles(triggersDirectory, Configuration.TriggersUpdateExtensionLookFor)
                        .Where(path => regTriggers.IsMatch(path))
                        .ToList();

                // Load event bubbling path
                var eventsDirectory = Configuration.DirectoryEvents();
                var regEvents = new Regex(Configuration.EventsUpdateExtension);
                var assemblyFilesEvents =
                    Directory.GetFiles(eventsDirectory, Configuration.EventsUpdateExtensionLookFor)
                        .Where(path => regEvents.IsMatch(path))
                        .ToList();

                // ****************************************************
                // Syncronize Triggers
                // ****************************************************
                foreach (var updateFile in assemblyFilesTriggers)
                {
                    // Sync assemblies events in root
                    try
                    {
                        var dllFile = Path.ChangeExtension(updateFile, Configuration.DllFileExtension);
                        LogEngine.ConsoleWriteLine(
                            $"[NEW TRIGGER UPDATE!]-[{Path.GetFileName(updateFile)}]", 
                            ConsoleColor.Green);
                        File.Copy(updateFile, dllFile, true);
                        File.Delete(updateFile);
                        LogEngine.ConsoleWriteLine(
                            $"[TRIGGER UPDATE DELETED!]-[{Path.GetFileName(updateFile)}]", 
                            ConsoleColor.Green);
                    }
                    catch (Exception ex)
                    {
                        LogEngine.ConsoleWriteLine($"[TRIGGER UPDATE]-[{updateFile}]-{ex.Message}", ConsoleColor.Red);
                    }
                }

                // ****************************************************
                // Syncronize events
                // ****************************************************
                foreach (var updateFile in assemblyFilesEvents)
                {
                    // Sync assemblies events in root
                    try
                    {
                        var dllFile = Path.ChangeExtension(updateFile, Configuration.DllFileExtension);
                        LogEngine.ConsoleWriteLine(
                            $"[NEW TRIGGER UPDATE!]-[{Path.GetFileName(updateFile)}]", 
                            ConsoleColor.Green);
                        File.Copy(updateFile, dllFile, true);
                        File.Delete(updateFile);
                        LogEngine.ConsoleWriteLine(
                            $"[TRIGGER UPDATE DELETED!]-[{Path.GetFileName(updateFile)}]", 
                            ConsoleColor.Green);
                    }
                    catch (Exception ex)
                    {
                        LogEngine.ConsoleWriteLine($"[EVENT UPDATE]-[{updateFile}]-{ex.Message}", ConsoleColor.Red);
                    }
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
        /// Load the events list from the directory
        /// </summary>
        /// <param name="numOfTriggers">
        /// The num Of Triggers.
        /// </param>
        /// <param name="numOfEvents">
        /// The num Of Events.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool LoadBubblingEventList(ref int numOfTriggers, ref int numOfEvents)
        {
            // Load all Triggers and Events DLLs in the main dictionary
            // Directory working:
            // Root\\Triggers
            // Root\\Events
            numOfTriggers = 0;
            numOfEvents = 0;
            var lastAssemblyFileLoaded = string.Empty;
            GlobalEventListBaseDll = new List<BubblingEvent>();

            // Load triggers bubbling path
            var triggersDirectory = Configuration.DirectoryTriggers();
            var regTriggers = new Regex(Configuration.TriggersDllExtension);
            var assemblyFilesTriggers =
                Directory.GetFiles(triggersDirectory, Configuration.TriggersDllExtensionLookFor)
                    .Where(path => regTriggers.IsMatch(path))
                    .ToList();

            // Load event bubbling path
            var eventsDirectory = Configuration.DirectoryEvents();
            var regEvents = new Regex(Configuration.EventsDllExtension);
            var assemblyFilesEvents =
                Directory.GetFiles(eventsDirectory, Configuration.EventsDllExtensionLookFor)
                    .Where(path => regEvents.IsMatch(path))
                    .ToList();

            try
            {
                // ****************************************************
                // Load Triggers
                // ****************************************************
                foreach (var assemblyFile in assemblyFilesTriggers)
                {
                    try
                    {
                        lastAssemblyFileLoaded = assemblyFile;

                        // TODO 1005
                        try
                        {
                            // TEST SYNC 
                            if (Path.GetFileName(assemblyFile) == "Framework.Contracts.dll")
                            {
                                continue;
                            }

                            var fileName = Path.GetFileName(assemblyFile);
                            if (fileName != null && fileName.Substring(0, 10) == "Microsoft.")
                            {
                                continue;
                            }

                            if (Path.GetFileName(assemblyFile).Substring(0, 7) == "System.")
                            {
                                continue;
                            }
                        }
                        catch
                        {
                            // ignored
                        }

                        // Get all classes with Attribute = Event
                        var assembly = Assembly.LoadFrom(assemblyFile);
                        var assemblyClasses = from t in assembly.GetTypes()
                                              let attributes = t.GetCustomAttributes(typeof(TriggerContract), false)
                                              where t.IsClass && attributes != null && attributes.Length > 0
                                              select t;

                        foreach (var assemblyClass in assemblyClasses)
                        {
                            var bubblingEvent = new BubblingEvent();
                            var classAttributes = assemblyClass.GetCustomAttributes(typeof(TriggerContract), true);
                            if (classAttributes.Length > 0)
                            {
                                ++numOfTriggers;
                                var trigger = (TriggerContract)classAttributes[0];

                                // Create event bubbling
                                bubblingEvent.AssemblyObject = assembly;
                                bubblingEvent.AssemblyFile = assemblyFile;
                                bubblingEvent.BubblingEventType = BubblingEventType.Trigger;
                                bubblingEvent.Description = trigger.Description;
                                bubblingEvent.IdComponent = trigger.Id;
                                bubblingEvent.Name = trigger.Name;
                                bubblingEvent.AssemblyClassType = assemblyClass;
                                bubblingEvent.PollingRequired = trigger.PollingRequired;
                                bubblingEvent.Nop = trigger.Nop;
                                bubblingEvent.Shared = trigger.Shared;
                                bubblingEvent.Version = assembly.GetName().Version;
                                bubblingEvent.AssemblyContent = File.ReadAllBytes(assemblyFile);

                                LogEngine.ConsoleWriteLine(
                                    $"[SYNC ROOT-{Path.GetFileName(assemblyFile)}]-[NAME-{trigger.Name}]", 
                                    ConsoleColor.Gray);
                            }

                            bubblingEvent.Properties = new List<Property>();
                            foreach (var propertyInfo in assemblyClass.GetProperties())
                            {
                                var propertyAttributes =
                                    propertyInfo.GetCustomAttributes(typeof(TriggerPropertyContract), true);
                                if (propertyAttributes.Length > 0)
                                {
                                    var triggerProperty = (TriggerPropertyContract)propertyAttributes[0];

                                    // TODO 1004
                                    if (propertyInfo.Name != triggerProperty.Name)
                                    {
                                        throw new Exception(
                                            $"Critical error! the properies {propertyAttributes[0]} and {propertyInfo.Name} are different! Class name {assemblyClass.Name}");
                                    }

                                    bubblingEvent.Properties.Add(
                                        new Property(
                                            triggerProperty.Name, 
                                            triggerProperty.Description, 
                                            propertyInfo, 
                                            propertyInfo.GetType(), 
                                            null));
                                }
                            }

                            bubblingEvent.BaseActions = new List<BaseAction>();
                            foreach (var methodInfo in assemblyClass.GetMethods())
                            {
                                var methodInfoAttributes = methodInfo.GetCustomAttributes(
                                    typeof(TriggerActionContract), 
                                    true);
                                if (methodInfoAttributes.Length > 0)
                                {
                                    var triggerAction = (TriggerActionContract)methodInfoAttributes[0];

                                    // Add the method
                                    var baseAction = new BaseAction(
                                        triggerAction.Id, 
                                        triggerAction.Name, 
                                        triggerAction.Description, 
                                        methodInfo, 
                                        null) {
                                                 Parameters = new List<Parameter>() 
                                              };

                                    bubblingEvent.BaseActions.Add(baseAction);
                                }
                            }

                            // Add the bubbling event
                            GlobalEventListBaseDll.Add(bubblingEvent);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName, 
                            $"Assembly file {lastAssemblyFileLoaded} - Error in {MethodBase.GetCurrentMethod().Name}", 
                            Constant.ErrorEventIdHighCritical, 
                            Constant.TaskCategoriesError, 
                            ex, 
                            EventLogEntryType.Error);
                        return false;
                    }
                }

                // ****************************************************
                // Load Events
                // ****************************************************
                foreach (var assemblyFile in assemblyFilesEvents)
                {
                    try
                    {
                        // TODO 1003
                        lastAssemblyFileLoaded = assemblyFile;

                        // TEST SYNC 
                        if (Path.GetFileName(assemblyFile) == "Framework.Contracts.dll")
                        {
                            continue;
                        }

                        if (Path.GetFileName(assemblyFile).Substring(0, 10) == "Microsoft.")
                        {
                            continue;
                        }

                        if (Path.GetFileName(assemblyFile).Substring(0, 7) == "System.")
                        {
                            continue;
                        }

                        // Get all classes with Attribute = Event
                        var assembly = Assembly.LoadFrom(assemblyFile);
                        var assemblyClasses = from t in assembly.GetTypes()
                                              let attributes = t.GetCustomAttributes(typeof(EventContract), false)
                                              where t.IsClass && attributes != null && attributes.Length > 0
                                              select t;

                        foreach (var assemblyClass in assemblyClasses)
                        {
                            var bubblingEvent = new BubblingEvent();
                            var classAttributes = assemblyClass.GetCustomAttributes(typeof(EventContract), true);
                            if (classAttributes.Length > 0)
                            {
                                ++numOfEvents;
                                var classAttribute = (EventContract)classAttributes[0];

                                // Create event bubbling
                                bubblingEvent.AssemblyFile = assemblyFile;
                                bubblingEvent.AssemblyObject = assembly;
                                bubblingEvent.BubblingEventType = BubblingEventType.Event;
                                bubblingEvent.Description = classAttribute.Description;
                                bubblingEvent.IdComponent = classAttribute.Id;
                                bubblingEvent.Name = classAttribute.Name;
                                bubblingEvent.AssemblyClassType = assemblyClass;
                                bubblingEvent.PollingRequired = false;
                                bubblingEvent.Nop = false;
                                bubblingEvent.Shared = classAttribute.Shared;
                                bubblingEvent.Version = assembly.GetName().Version;
                                bubblingEvent.AssemblyContent = File.ReadAllBytes(assemblyFile);

                                LogEngine.ConsoleWriteLine(
                                    $"[SYNC ROOT-{Path.GetFileName(assemblyFile)}]-[NAME-{classAttribute.Name}]", 
                                    ConsoleColor.Gray);
                            }

                            bubblingEvent.Properties = new List<Property>();
                            foreach (var propertyInfo in assemblyClass.GetProperties())
                            {
                                var propertyAttributes = propertyInfo.GetCustomAttributes(
                                    typeof(EventPropertyContract), 
                                    true);
                                if (propertyAttributes.Length > 0)
                                {
                                    var propertyAttribute = (EventPropertyContract)propertyAttributes[0];
                                    if (propertyInfo.Name != propertyAttribute.Name)
                                    {
                                        throw new Exception(
                                            $"Critical error! the properies {propertyAttributes[0]} and {propertyInfo.Name} are different! Class name {assemblyClass.Name}");
                                    }

                                    bubblingEvent.Properties.Add(
                                        new Property(
                                            propertyAttribute.Name, 
                                            propertyAttribute.Description, 
                                            propertyInfo, 
                                            propertyInfo.GetType(), 
                                            null));
                                }
                            }

                            bubblingEvent.BaseActions = new List<BaseAction>();
                            foreach (var methodInfo in assemblyClass.GetMethods())
                            {
                                var methodInfoAttributes = methodInfo.GetCustomAttributes(
                                    typeof(EventActionContract), 
                                    true);
                                if (methodInfoAttributes.Length > 0)
                                {
                                    var methodInfoAttribute = (EventActionContract)methodInfoAttributes[0];

                                    // Add the method
                                    var baseAction = new BaseAction(
                                        methodInfoAttribute.Id, 
                                        methodInfoAttribute.Name, 
                                        methodInfoAttribute.Description, 
                                        methodInfo, 
                                        null) {
                                                 Parameters = new List<Parameter>() 
                                              };

                                    bubblingEvent.BaseActions.Add(baseAction);
                                }
                            }

                            // Add the bubbling event
                            GlobalEventListBaseDll.Add(bubblingEvent);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName, 
                            $"Assembly file {lastAssemblyFileLoaded} - Error in {MethodBase.GetCurrentMethod().Name} - Possible workaround: Check if the propeties name and the corresponding contract properties name are the same in the assembly.", 
                            Constant.ErrorEventIdHighCritical, 
                            Constant.TaskCategoriesError, 
                            ex, 
                            EventLogEntryType.Error);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Assembly file {lastAssemblyFileLoaded} - Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
                return false;
            }
        }

        /// <summary>
        ///     Load all active trigger from Bubbling trigger directory
        /// </summary>
        public static void RefreshBubblingSetting()
        {
            // Load active triggers and event property bag
            // Create the instance lists
            SyncConfigurationFileList = new List<SyncConfigurationFile>();
            TriggerConfigurationList = new List<TriggerConfiguration>();
            EventConfigurationList = new List<EventConfiguration>();

            BubblingTriggersEventsActive = new List<BubblingEvent>();
            BubblingTriggerConfigurationsPolling = new List<BubblingEvent>();
            BubblingTriggerConfigurationsSingleInstance = new List<BubblingEvent>();

            LogEngine.ConsoleWriteLine("Load configuration triggers and events.", ConsoleColor.Green);
            CollectSyncBubblingSetting();

            // TRIGGERS***************************************************************************
            // Loop in the directory
            var triggerBubblingDirectory = Configuration.DirectoryBubblingTriggers();
            var regTriggers = new Regex(Configuration.BubblingTriggersExtension);
            var triggerConfigurationsFiles =
                Directory.GetFiles(triggerBubblingDirectory, "*", SearchOption.AllDirectories)
                    .Where(path => regTriggers.IsMatch(path))
                    .ToList();

            // For each trigger search for the trigger in event bubbling and set the properties
            foreach (var triggerConfigurationsFile in triggerConfigurationsFiles)
            {
                TriggerConfiguration triggerConfiguration = null;

                var fileLocked = true;
                while (fileLocked)
                {
                    try
                    {
                        var triggerConfigurationsByteContent = File.ReadAllBytes(triggerConfigurationsFile);
                        fileLocked = false;

                        // TODO 10001
                        triggerConfiguration =
                            JsonConvert.DeserializeObject<TriggerConfiguration>(
                                Encoding.UTF8.GetString(triggerConfigurationsByteContent));

                        // Add to the global list
                        TriggerConfigurationList.Add(triggerConfiguration);
                    }
                    catch (IOException ioex)
                    {
                        Debug.WriteLine("<Lock>" + ioex.Message);
                        LogEngine.ConsoleWriteLine("<Lock>", ConsoleColor.Yellow);
                        fileLocked = true;
                    }
                    catch (Exception ex)
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName, 
                            $"Error in {MethodBase.GetCurrentMethod().Name} File {triggerConfigurationsFile}", 
                            Constant.ErrorEventIdHighCritical, 
                            Constant.TaskCategoriesError, 
                            ex, 
                            EventLogEntryType.Error);
                        Environment.Exit(0);
                    }
                }

                // Look for the trigger in the event bubbling list
                var bubblingEvent =
                    GlobalEventListBaseDll.Find(
                        property => property.IdComponent == triggerConfiguration.Trigger.IdComponent);

                // Dll founded
                if (bubblingEvent != null)
                {
                    var bubblingEventClone = (BubblingEvent)ObjectHelper.CloneObject(bubblingEvent);
                    bubblingEventClone.IsActive = true;
                    bubblingEventClone.Events = triggerConfiguration.Events;

                    // Set all the properties
                    if (triggerConfiguration.Trigger.TriggerProperties != null)
                    {
                        foreach (var triggerProperty in triggerConfiguration.Trigger.TriggerProperties)
                        {
                            var properties =
                                bubblingEventClone.Properties.Find(property => property.Name == triggerProperty.Name);
                            if (properties != null)
                            {
                                properties.Value = triggerProperty.Value;
                            }
                        }
                    }

                    // Add in the list
                    if (bubblingEventClone.PollingRequired)
                    {
                        BubblingTriggerConfigurationsPolling.Add(bubblingEventClone);
                        LogEngine.ConsoleWriteLine(
                            $"Load configuration trigger file - {bubblingEventClone.Name}", 
                            ConsoleColor.Green);
                    }
                    else
                    {
                        BubblingTriggerConfigurationsSingleInstance.Add(bubblingEventClone);
                        LogEngine.ConsoleWriteLine(
                            $"Load configuration trigger file- {bubblingEventClone.Name}", 
                            ConsoleColor.Green);
                    }
                }
                else
                {
                    LogEngine.WriteLog(
                        Configuration.EngineName, 
                        $"Warning in {MethodBase.GetCurrentMethod().Name},Trigger [{triggerConfiguration.Trigger.Name}] with  IDComponent {triggerConfiguration.Trigger.IdComponent} present in the configuration event directory and not found in the event dll directory.", 
                        Constant.ErrorEventIdHighCritical, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Warning);
                }
            }

            // EVENTS******************************************************************************
            // Loop in the directory
            var eventsBubblingDirectory = Configuration.DirectoryBubblingEvents();
            var regEvents = new Regex(Configuration.BubblingEventsExtension);
            var propertyEventsFiles =
                Directory.GetFiles(eventsBubblingDirectory, "*", SearchOption.AllDirectories)
                    .Where(path => regEvents.IsMatch(path))
                    .ToList();

            // For each trigger search for the trigger in event bubbling and set the properties
            foreach (var propertyEventsFile in propertyEventsFiles)
            {
                EventConfiguration eventPropertyBag = null;
                var fileLocked = true;
                while (fileLocked)
                {
                    try
                    {
                        var propertyEventsByteContent = File.ReadAllBytes(propertyEventsFile);
                        fileLocked = false;
                        eventPropertyBag =
                            JsonConvert.DeserializeObject<EventConfiguration>(
                                Encoding.UTF8.GetString(propertyEventsByteContent));

                        // Add to the global list
                        EventConfigurationList.Add(eventPropertyBag);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Source == "Newtonsoft.Json")
                        {
                            LogEngine.WriteLog(
                                Configuration.EngineName, 
                                $"Error in {MethodBase.GetCurrentMethod().Name} File {propertyEventsFile}", 
                                Constant.ErrorEventIdHighCritical, 
                                Constant.TaskCategoriesError, 
                                ex, 
                                EventLogEntryType.Error);
                        }
                        else
                        {
                            LogEngine.ConsoleWriteLine("Lock file warning", ConsoleColor.Yellow);
                        }

                        fileLocked = true;
                    }
                }

                var bubblingEvent =
                    GlobalEventListBaseDll.Find(property => property.IdComponent == eventPropertyBag.Event.IdComponent);

                // Dll founded
                if (bubblingEvent != null)
                {
                    var bubblingEventClone = (BubblingEvent)ObjectHelper.CloneObject(bubblingEvent);

                    bubblingEventClone.IdConfiguration = eventPropertyBag.Event.IdConfiguration;
                    bubblingEventClone.IsActive = true;
                    bubblingEventClone.Events = bubblingEvent.Events;

                    // Yes, so set all the properties
                    if (eventPropertyBag.Event.EventProperties != null)
                    {
                        foreach (var eventProperty in eventPropertyBag.Event.EventProperties)
                        {
                            var properties =
                                bubblingEventClone.Properties.Find(property => property.Name == eventProperty.Name);
                            if (properties != null)
                            {
                                properties.Value = eventProperty.Value;
                            }
                        }
                    }

                    // Add in the list
                    BubblingTriggersEventsActive.Add(bubblingEventClone);
                    LogEngine.ConsoleWriteLine(
                        $"Load configuration Event file- {bubblingEventClone.Name}", 
                        ConsoleColor.Green);
                }
                else
                {
                    LogEngine.WriteLog(
                        Configuration.EngineName, 
                        string.Format(
                            "Warning in {0}, Event [{2}] with IDComponent {1} present in the configuration event directory and not found in the event dll directory.", 
                            MethodBase.GetCurrentMethod().Name, 
                            eventPropertyBag.Event.IdComponent, 
                            eventPropertyBag.Event.Name), 
                        Constant.ErrorEventIdHighCritical, 
                        Constant.TaskCategoriesError, 
                        null, 
                        EventLogEntryType.Warning);
                }
            }
        }

        /// <summary>
        ///     Load all trigger and events from Bubbling trigger directory
        /// </summary>
        public static void CollectSyncBubblingSetting()
        {
            // Create the instance lists
            SyncConfigurationFileList = new List<SyncConfigurationFile>();
            LogEngine.ConsoleWriteLine("Load sync configuration triggers and events.", ConsoleColor.Green);

            // TRIGGERS***************************************************************************
            // Loop in the directory
            var triggerBubblingDirectory = Configuration.DirectoryBubblingTriggers();
            var regTriggers = new Regex(Configuration.GcEventsConfigurationFilesExtensions);
            var triggerConfigurationsFiles =
                Directory.GetFiles(triggerBubblingDirectory, "*", SearchOption.AllDirectories)
                    .Where(path => regTriggers.IsMatch(path))
                    .ToList();

            // For each trigger search for the trigger in event bubbling and set the properties
            foreach (var triggerConfigurationsFile in triggerConfigurationsFiles)
            {
                var fileLocked = true;
                while (fileLocked)
                {
                    try
                    {
                        var triggerConfigurationsByteContent = File.ReadAllBytes(triggerConfigurationsFile);
                        fileLocked = false;
                        JsonConvert.DeserializeObject<TriggerConfiguration>(
                            Encoding.UTF8.GetString(triggerConfigurationsByteContent));

                        // Add to the list
                        SyncConfigurationFileList.Add(
                            new SyncConfigurationFile(
                                Configuration.MessageDataProperty.Trigger, 
                                triggerConfigurationsFile, 
                                triggerConfigurationsByteContent, 
                                Configuration.ChannelId()));
                    }
                    catch (IOException ioex)
                    {
                        Debug.WriteLine("<Lock>" + ioex.Message);
                        LogEngine.ConsoleWriteLine("<Lock>", ConsoleColor.Yellow);
                        fileLocked = true;
                    }
                    catch (Exception ex)
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName, 
                            $"Error in {MethodBase.GetCurrentMethod().Name} File {triggerConfigurationsFile}", 
                            Constant.ErrorEventIdHighCritical, 
                            Constant.TaskCategoriesError, 
                            ex, 
                            EventLogEntryType.Error);
                        Environment.Exit(0);
                    }
                }
            }

            // EVENTS******************************************************************************
            // Loop in the directory
            var eventsBubblingDirectory = Configuration.DirectoryBubblingEvents();
            var regEvents = new Regex(Configuration.GcEventsConfigurationFilesExtensions);
            var propertyEventsFiles =
                Directory.GetFiles(eventsBubblingDirectory, "*", SearchOption.AllDirectories)
                    .Where(path => regEvents.IsMatch(path))
                    .ToList();

            // For each trigger search for the trigger in event bubbling and set the properties
            foreach (var propertyEventsFile in propertyEventsFiles)
            {
                var fileLocked = true;
                while (fileLocked)
                {
                    try
                    {
                        var propertyEventsByteContent = File.ReadAllBytes(propertyEventsFile);
                        fileLocked = false;
                        JsonConvert.DeserializeObject<EventConfiguration>(
                            Encoding.UTF8.GetString(propertyEventsByteContent));

                        // Add to the global list
                        SyncConfigurationFileList.Add(
                            new SyncConfigurationFile(
                                Configuration.MessageDataProperty.Event, 
                                propertyEventsFile, 
                                propertyEventsByteContent, 
                                Configuration.ChannelId()));
                    }
                    catch (Exception ex)
                    {
                        if (ex.Source == "Newtonsoft.Json")
                        {
                            LogEngine.WriteLog(
                                Configuration.EngineName, 
                                $"Error in {MethodBase.GetCurrentMethod().Name} File {propertyEventsFile}", 
                                Constant.ErrorEventIdHighCritical, 
                                Constant.TaskCategoriesError, 
                                ex, 
                                EventLogEntryType.Error);
                        }
                        else
                        {
                            LogEngine.ConsoleWriteLine("Lock file warning", ConsoleColor.Yellow);
                        }

                        fileLocked = true;
                    }
                }
            }
        }

        /// <summary>
        ///     Execute the triggres in polling required
        /// </summary>
        public static void ExecuteBubblingTriggerConfigurationPolling()
        {
            foreach (var bubblingTriggerConfiguration in BubblingTriggerConfigurationsPolling)
            {
                ExecuteTriggerConfiguration(bubblingTriggerConfiguration);
            }
        }

        /// <summary>
        ///     Execute the triggres in single instance required
        /// </summary>
        public static void ExecuteBubblingTriggerConfigurationsSingleInstance()
        {
            foreach (var bubblingTriggerConfiguration in BubblingTriggerConfigurationsSingleInstance)
            {
                // If NOP the execute
                // NOP is for the Not Operation Execute
                if (!bubblingTriggerConfiguration.Nop)
                {
                    LogEngine.ConsoleWriteLine(
                        $"Run single instances {bubblingTriggerConfiguration.Name}", 
                        ConsoleColor.Green);
                    var treadRun = new Thread(ExecuteTriggerConfiguration);
                    treadRun.Start(bubblingTriggerConfiguration);
                }
            }
        }

        /// <summary>
        /// Execute a trigger and if the Execute method return != null then it set all return value in a action and excute the
        ///     action
        /// </summary>
        /// <param name="bubblingTriggerConfiguration">
        /// The bubbling Trigger Configuration.
        /// </param>
        public static void ExecuteTriggerConfiguration(object bubblingTriggerConfiguration)
        {
            var triggerConfiguration = (BubblingEvent)bubblingTriggerConfiguration;
            try
            {
                if (triggerConfiguration == null)
                {
                    throw new ArgumentNullException(nameof(triggerConfiguration));
                }

                lock (bubblingTriggerConfiguration)
                {
                    // Set master EventActionContext eccoloqua
                    var eventActionContext = new EventActionContext(triggerConfiguration);

                    // In the first execute the main Execute method
                    var baseAction =
                        triggerConfiguration.BaseActions.Find(prop => prop.AssemblyMethodInfo.Name == "Execute");

                    if (baseAction == null)
                    {
                        return;
                    }

                    // Create the object instance
                    var classInstance = Activator.CreateInstance(triggerConfiguration.AssemblyClassType, null);

                    // Assign all propertyies value trigger to class instance and execute
                    foreach (var propertyEvent in triggerConfiguration.Properties)
                    {
                        // Look if exist the property in the trigger, if exist then copy value
                        var propertyTriger =
                            triggerConfiguration.Properties.Find(prop => prop.Name == propertyEvent.Name);
                        if (propertyTriger == null)
                        {
                            LogEngine.WriteLog(
                                Configuration.EngineName, 
                                $"Error in {MethodBase.GetCurrentMethod().Name}-The property {propertyEvent.Name} is missing.Possible issue: you are missing this property in the dll compnent or in the trigger configuration file for trigger:{triggerConfiguration.Name}", 
                                Constant.ErrorEventIdHighCritical, 
                                Constant.TaskCategoriesError, 
                                null, 
                                EventLogEntryType.Error);
                        }

                        // Inizialize the DataContext
                        if (propertyEvent.Name == "DataContext")
                        {
                            propertyEvent.Value = null;
                        }

                        if (propertyTriger != null)
                        {
                            var propertyInfo = classInstance.GetType().GetProperty(propertyTriger.Name);
                            if (propertyInfo == null)
                            {
                                LogEngine.ConsoleWriteLine(
                                    $"Critical error, missing property: {propertyTriger.Name}", 
                                    ConsoleColor.Cyan);
                            }
                            else
                            {
                                propertyInfo.SetValue(
                                    classInstance, 
                                    Convert.ChangeType(propertyTriger.Value, propertyInfo.PropertyType), 
                                    null);
                            }
                        }
                    }

                    eventActionContext.BubblingConfiguration.MessageId = Guid.NewGuid().ToString();
                    object[] parameters = { delegateActionTrigger, eventActionContext };
                    try
                    {
                        baseAction.AssemblyMethodInfo.Invoke(classInstance, parameters);
                    }
                    catch (TargetInvocationException ex)
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName, 
                            $"Critical Error in {MethodBase.GetCurrentMethod().Name} invoking the component {triggerConfiguration.AssemblyFile}", 
                            Constant.ErrorEventIdHighCritical, 
                            Constant.TaskCategoriesError, 
                            ex, 
                            EventLogEntryType.Error);
                    }

                    var baseActionCustoms = from action in triggerConfiguration.BaseActions
                                            where action.AssemblyMethodInfo.Name != "Execute"
                                            select action;

                    // Execute the other method
                    foreach (var baseCustomAction in baseActionCustoms)
                    {
                        // Result != null so set the result value
                        // Invoke the method and set the result
                        object[] parametersRet = { delegateActionTrigger, eventActionContext };
                        baseCustomAction.ReturnValue = baseAction.AssemblyMethodInfo.Invoke(
                            classInstance, 
                            parametersRet);
                    }
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
        /// Execute all the action correlate to the trigger
        ///     it send the trigger event
        /// </summary>
        /// <param name="bubblingEventConfiguration">
        /// </param>
        /// <param name="internalCall">
        /// if is an internal call or arrived from EH (Performances)
        /// </param>
        /// <param name="senderEndpointId">
        /// </param>
        public static void ExecuteBubblingActionEvent(
            BubblingEvent bubblingEventConfiguration, 
            bool internalCall, 
            string senderEndpointId)
        {
            if (bubblingEventConfiguration == null)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name} - BubblingTriggerConfiguration null.", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesError, 
                    null, 
                    EventLogEntryType.Error);
                return;
            }

            // Use a var because on lynq I'm able to instance a var only in the loop
            var eventExecuted = false;

            try
            {
                LogEngine.ConsoleWriteLine(
                    $"Trigger Event received from {(internalCall ? "Internal call" : "External call.")}.", ConsoleColor.DarkYellow);
                LogEngine.ConsoleWriteLine(
                    $"internalCall {internalCall}-SenderEndpointID {senderEndpointId}.", 
                    ConsoleColor.Green);
                LogEngine.ConsoleWriteLine($"Trigger name {bubblingEventConfiguration.Name}-", ConsoleColor.Green);

                // Set master EventActionContext eccoloqua
                var eventActionContext = new EventActionContext(bubblingEventConfiguration);

                LogEngine.ConsoleWriteLine("-!ACTIONS HAS TO BE EXECUTED!-", ConsoleColor.Green);

                // Lock (BubblingTriggerConfiguration)
                foreach (var _event in bubblingEventConfiguration.Events)
                {
                    // Trigger
                    // Execute the event
                    // Override the Correlation
                    if (_event.Correlation != null)
                    {
                        bubblingEventConfiguration.CorrelationOverride = _event.Correlation;
                    }

                    // Look for the Event
                    LogEngine.DebugWriteLine("Event check > " + _event.Name);
                    LogEngine.DebugWriteLine("Event.IDConfiguration > " + _event.IdConfiguration);
                    LogEngine.DebugWriteLine(
                        "BubblingTriggersEventsActive.Count > " + BubblingTriggersEventsActive.Count);

                    var bubblingEventsToExecute = (from bubblingTriggerConfiguration in BubblingTriggersEventsActive
                                                   where
                                                       bubblingTriggerConfiguration.IdConfiguration
                                                       == _event.IdConfiguration
                                                       && bubblingTriggerConfiguration.IdComponent == _event.IdComponent
                                                       && bubblingTriggerConfiguration.BubblingEventType
                                                       == BubblingEventType.Event
                                                   select bubblingTriggerConfiguration).ToList();
                    if (bubblingEventsToExecute.Count == 0)
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName, 
                            $"Error in {MethodBase.GetCurrentMethod().Name} - No found event, check if the event is configured and active in the Bubbling folder events."
                            + $"\rEvent ID Configuration requested {_event.IdConfiguration}"
                            + $"\rEvent ID Component requested {_event.IdComponent}", 
                            Constant.ErrorEventIdHighCritical, 
                            Constant.TaskCategoriesError, 
                            null, 
                            EventLogEntryType.Error);
                        return;
                    }

                    LogEngine.DebugWriteLine("BubblingEvent loop on > " + BubblingTriggersEventsActive.Count);
                    foreach (var bubblingEvent in bubblingEventsToExecute)
                    {
                        // Event
                        LogEngine.DebugWriteLine("BubblingEvent check > " + bubblingEvent.Name);

                        eventExecuted = true;

                        // All property mapped between trigger and event , now execute the event
                        // For each action with the same name the pass the parameter and execute the event
                        foreach (var baseActionEvent in bubblingEvent.BaseActions)
                        {
                            LogEngine.DebugWriteLine("BaseActionEvent check > " + baseActionEvent.Name);

                            // Look if exist the property in the trigger, if exist then copy value
                            var baseActionTriger =
                                bubblingEventConfiguration.BaseActions.Find(prop => prop.Name == baseActionEvent.Name);
                            if (baseActionTriger != null)
                            {
                                LogEngine.DebugWriteLine("BaseActionTriger found.");

                                // Assign the return value the pass it to the event method
                                baseActionEvent.ReturnValue = baseActionTriger.ReturnValue;

                                // Invoke the method and set the result
                                LogEngine.DebugWriteLine(
                                    "BubblingEvent.AssemblyClassType > " + bubblingEvent.AssemblyClassType);
                                var classInstance = Activator.CreateInstance(bubblingEvent.AssemblyClassType, null);

                                // Overriding properties
                                if (_event.EventProperties != null)
                                {
                                    foreach (var eventProperty in _event.EventProperties)
                                    {
                                        var propertyToOverride =
                                            (from prop in bubblingEvent.Properties
                                             where prop.Name == eventProperty.Name
                                             select prop).First();
                                        if (propertyToOverride != null)
                                        {
                                            propertyToOverride.Value = eventProperty.Value;
                                        }
                                        else
                                        {
                                            var sourceSerializedEvents = JsonConvert.SerializeObject(
                                                _event, 
                                                Formatting.Indented, 
                                                new JsonSerializerSettings
                                                    {
                                                        ReferenceLoopHandling =
                                                            ReferenceLoopHandling.Ignore
                                                    });
                                            var currentSerializedEvents = JsonConvert.SerializeObject(
                                                baseActionEvent, 
                                                Formatting.Indented, 
                                                new JsonSerializerSettings
                                                    {
                                                        ReferenceLoopHandling =
                                                            ReferenceLoopHandling.Ignore
                                                    });

                                            LogEngine.WriteLog(
                                                Configuration.EngineName, 
                                                $"Attemp to override an unknown event property{eventProperty.Name} Error in {MethodBase.GetCurrentMethod().Name} - Source Event: {sourceSerializedEvents} - Local Event: {currentSerializedEvents}", 
                                                Constant.ErrorEventIdHighCritical, 
                                                Constant.TaskCategoriesError, 
                                                null, 
                                                EventLogEntryType.Error);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    var eventConfigurationBase = (from evnt in EventConfigurationList
                                                                  where
                                                                      evnt.Event.IdConfiguration
                                                                      == bubblingEvent.IdConfiguration
                                                                      && evnt.Event.IdComponent
                                                                      == bubblingEvent.IdComponent
                                                                  select evnt).First();
                                    var eventConfiguration = eventConfigurationBase;
                                    if (eventConfiguration.Event.EventProperties != null)
                                    {
                                        foreach (var eventProperty in eventConfiguration.Event.EventProperties)
                                        {
                                            var propertyToOverride =
                                                (from prop in bubblingEvent.Properties
                                                 where prop.Name == eventProperty.Name
                                                 select prop).First();
                                            propertyToOverride.Value = eventProperty.Value;
                                        }
                                    }
                                }

                                foreach (var property in bubblingEvent.Properties)
                                {
                                    LogEngine.DebugWriteLine("Property > " + property.Name);

                                    var propertyFound =
                                        bubblingEventConfiguration.Properties.FirstOrDefault(
                                            obj => obj.Name == property.Name);
                                    if (propertyFound != null)
                                    {
                                        LogEngine.DebugWriteLine("propertyFound > " + propertyFound.Name);

                                        var propertyInfoT = classInstance.GetType().GetProperty(propertyFound.Name);
                                        propertyInfoT.SetValue(
                                            classInstance, 
                                            Convert.ChangeType(propertyFound.Value, propertyInfoT.PropertyType), 
                                            null);
                                    }
                                    else
                                    {
                                        var propertyInfoT = classInstance.GetType().GetProperty(property.Name);
                                        LogEngine.DebugWriteLine("propertyFound > " + propertyInfoT.Name);

                                        // TODO 1002
                                        propertyInfoT.SetValue(
                                            classInstance, 
                                            Convert.ChangeType(property.Value, propertyInfoT.PropertyType), 
                                            null);
                                    }
                                }

                                // Pass Data property to Execute
                                LogEngine.DebugWriteLine("Prepare Invoking delegation.");
                                object[] parameters = { delegateActionEvent, eventActionContext };
                                LogEngine.DebugWriteLine("Invoking > " + classInstance);
                                baseActionEvent.ReturnValue = baseActionEvent.AssemblyMethodInfo.Invoke(
                                    classInstance, 
                                    parameters);
                            }
                        }
                    }

                    if (eventExecuted)
                    {
                        LogEngine.ConsoleWriteLine($"-!EXECUTING EVENT {_event.Name}!-", ConsoleColor.Green);
                    }
                    else
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName, 
                            $"Warning! in {MethodBase.GetCurrentMethod().Name} - Try to execute a not available event for trigger {bubblingEventConfiguration.Name} and IdComponent {bubblingEventConfiguration.IdComponent}", 
                            Constant.ErrorEventIdHighCritical, 
                            Constant.TaskCategoriesError, 
                            null, 
                            EventLogEntryType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                LogEngine.DebugWriteLine("Error catched > " + ex.Message);
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Warning! in {MethodBase.GetCurrentMethod().Name} - Try to execute a not available event for trigger {bubblingEventConfiguration.Name} and IdComponent {bubblingEventConfiguration.IdComponent}", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// The create event up stream.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool CreateEventUpStream()
        {
            try
            {
                // Event Hub Configuration
                connectionString = Configuration.AzureNameSpaceConnectionString();
                eventHubName = Configuration.GroupEventHubsName();

                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Event Hubs transfort Type: {Configuration.ServiceBusConnectivityMode()}", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesError, 
                    null, 
                    EventLogEntryType.Information);

                var builder = new ServiceBusConnectionStringBuilder(connectionString)
                                  {
                                      TransportType =
                                          TransportType.Amqp
                                  };

                HubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);
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

        // *************EVENTS WATCHER MANAGEMENT*************************************
        // Main area managing the File system watcher (Change DLL, ChangeTrigger configuration ectcetera.)
        /// <summary>
        ///     By these area the engine is going to manage what is going to change and look and start the provisioning
        /// </summary>
        // *************EVENTS WATCHER MANAGEMENT*************************************
        public static void StartConfigurationSyncEngine()
        {
            try
            {
                FswEventFolder.Path = Configuration.DirectoryBubbling();
                FswEventFolder.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.LastWrite
                                              | NotifyFilters.Attributes;
                FswEventFolder.Filter = "*.*";

                FswEventFolder.EnableRaisingEvents = true;
                FswEventFolder.IncludeSubdirectories = true;

                FswEventFolder.Created += EventFolderChanged;
                FswEventFolder.Changed += EventFolderChanged;
                FswEventFolder.Deleted += EventFolderChanged;
                FswEventFolder.Renamed += EventFolderChanged;
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
        /// The event folder changed.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void EventFolderChanged(object source, FileSystemEventArgs e)
        {
            if (Regex.IsMatch(

                // ReSharper disable once AssignNullToNotNullAttribute
                Path.GetExtension(e.FullPath), 
                Configuration.GcEventsFilesExtensions, 
                RegexOptions.IgnoreCase))
            {
                try
                {
                    FswEventFolder.EnableRaisingEvents = false;
                    SyncProvider.RefreshBubblingSetting();
                }
                finally
                {
                    FswEventFolder.EnableRaisingEvents = true;
                }
            }
        }
    }

    /// <summary>
    /// The host context.
    /// </summary>
    public class HostContext
    {
        /// <summary>
        /// The data context.
        /// </summary>
        public object DataContext;
    }
}