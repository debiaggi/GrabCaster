// --------------------------------------------------------------------------------------------------
// <copyright file = "CoreEngine.cs" company="Nino Crudele">
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
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Engine.OffRamp;
    using GrabCaster.Framework.Engine.OnRamp;
    using GrabCaster.Framework.Log;

    using Microsoft.ServiceBus;

    /// <summary>
    ///     Primary engine, it start all, this is the first point
    /// </summary>
    public static class CoreEngine
    {
        private static int pollingStep;

        private static Assembly HandleAssemblyResolve(object sender, ResolveEventArgs args)
        {
            /* Load the assembly specified in 'args' here and return it, 
               if the assembly is already loaded you can return it here */
            try
            {
                if (args.Name.Substring(0, 10) == "Microsoft.")
                {
                    return null;
                }
                if (args.Name.Substring(0, 7) == "System.")
                {
                    return null;
                }

                var bubblingEvent = (from assemblies in EventsEngine.GlobalEventListBaseDll
                                     where assemblies.AssemblyObject.FullName == args.Name
                                     select assemblies).First();

                var assembly = bubblingEvent.AssemblyObject;
                return assembly;
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine(
                    $"Critical error in {MethodBase.GetCurrentMethod().Name} - The Assembly [{args.Name}] not found.");
                sb.AppendLine(
                    "Workaround: this error because a trigger or event is looking for a particular external library in reference, check if all the libraries referenced by triggers and events are in the triggers and events directories dll or registered in GAC.");

                LogEngine.WriteLog(
                    Configuration.EngineName,
                    sb.ToString(),
                    Constant.ErrorEventIdHighCritical,
                    Constant.TaskCategoriesError,
                    ex,
                    EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        ///     Execute polling
        /// </summary>
        /// restart
        public static void StartEventEngine(SetEventActionEvent delegateActionEventEmbedded)
        {
            try
            {

                var current = AppDomain.CurrentDomain;
                current.AssemblyResolve += HandleAssemblyResolve;

                LogEngine.Enabled = Configuration.LoggingEngineEnabled();
                LogEngine.ConsoleWriteLine("Load Engine configuration.", ConsoleColor.White);

                //****************************Check for updates
                //Check if need to update files received from partners
                LogEngine.ConsoleWriteLine("Check Engine Syncronization.", ConsoleColor.White);
                EventsEngine.CheckForFileToUpdate();
                //****************************Check for updates

                //Set service states
                LogEngine.ConsoleWriteLine("Initialize Engine Service states.", ConsoleColor.White);
                ServiceStates.RunPolling = true;
                ServiceStates.RestartNeeded = false;

                LogEngine.ConsoleWriteLine("Initialize Engine.", ConsoleColor.Cyan);
                EventsEngine.InitializeEventEngine(delegateActionEventEmbedded);

                //Init Message ingestor
                MessageIngestor.Init();

                //Create the two sends layers
                // in EventsEngine
                if (!Configuration.DisableDeviceProviderInterface())
                {
                    LogEngine.ConsoleWriteLine("Start Internal Event Engine Channel.", ConsoleColor.Yellow);
                    var canStart = EventsEngine.CreateEventUpStream();

                    if (!canStart)
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName,
                            $"Error during engine service starting. Name: {Configuration.EngineName} - ID: {Configuration.ChannelId()}",
                            Constant.ErrorEventIdHighCritical,
                            Constant.TaskCategoriesError,
                            null,
                            EventLogEntryType.Error);
                        Thread.Sleep(Configuration.WaitTimeBeforeRestarting());
                        Environment.Exit(0);
                    }

                    //in EventUpStream
                    LogEngine.ConsoleWriteLine("Start External Event Engine Channel.", ConsoleColor.Yellow);
                    //OnRamp start the OnRamp Engine
                    canStart = OffRampEngineSending.Init("MSP Device Component.dll (vNext)");

                    if (!canStart)
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName,
                            $"Error during engine service starting. Name: {Configuration.ChannelName()} - ID: {Configuration.ChannelId()}",
                            Constant.ErrorEventIdHighCritical,
                            Constant.TaskCategoriesError,
                            null,
                            EventLogEntryType.Error);
                        Thread.Sleep(Configuration.WaitTimeBeforeRestarting());
                        Environment.Exit(0);
                    }
                }

                //*****************Event object stream area*********************
                //Load the global event and triggers dlls
                var numOfTriggers = 0;
                var numOfEvents = 0;

                var triggersAndEventsLoaded = EventsEngine.LoadBubblingEventList(ref numOfTriggers, ref numOfEvents);
                if (triggersAndEventsLoaded)
                {
                    LogEngine.ConsoleWriteLine(
                        $"Triggers loaded {numOfTriggers} - Events loaded {numOfEvents}",
                        ConsoleColor.DarkCyan);
                }

                EventsEngine.UpdateAssemblyEventListShared();
                //Load the Active triggers and the active events
                EventsEngine.RefreshBubblingSetting();
                //Start triggers single instances
                EventsEngine.ExecuteBubblingTriggerConfigurationsSingleInstance();
                var treadPollingRun = new Thread(StartTriggerPolling);
                treadPollingRun.Start();

                //Start Engine Service
                LogEngine.ConsoleWriteLine(
                    "Asyncronous Threading Service state active.",
                    ConsoleColor.DarkCyan);
                var treadEngineStates = new Thread(CheckServiceStates);
                treadEngineStates.Start();

                if (!Configuration.DisableDeviceProviderInterface())
                {
                    LogEngine.ConsoleWriteLine("Start On Ramp Engine.", ConsoleColor.Green);
                    var onRampEngineReceiving = new OnRampEngineReceiving();
                    onRampEngineReceiving.Init("component.dll name");
                }
                // Configuration files watcher
                EventsEngine.StartConfigurationSyncEngine();

                LogEngine.WriteLog(
                    Configuration.EngineName,
                    $"Engine service initialization procedure terminated. Name: {Configuration.ChannelName()} - ID: {Configuration.ChannelId()}",
                    Constant.ErrorEventIdHighCritical,
                    Constant.TaskCategoriesError,
                    null,
                    EventLogEntryType.Information);
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

        public static void StopEventEngine()
        {
            //EventsEngine.DisposeEngine();
        }

        /// <summary>
        ///     If restart required it perform the operations
        /// </summary>
        public static void CheckServiceStates()
        {
            while (true)
            {
                Thread.Sleep(10000);
                if (ServiceStates.RestartNeeded)
                {
                    LogEngine.ConsoleWriteLine(
                        "--------------------------------------------------------",
                        ConsoleColor.DarkYellow);
                    LogEngine.ConsoleWriteLine(
                        "Service needs restarting.",
                        ConsoleColor.Red);
                    LogEngine.ConsoleWriteLine(
                        "--------------------------------------------------------",
                        ConsoleColor.DarkYellow);
                    ServiceStates.RestartNeeded = false;
                    //Thread.Sleep(Configuration.WaitTimeBeforeRestarting());
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        /// <summary>
        ///     Execute polling
        /// </summary>
        private static void StartTriggerPolling()
        {
            //running thread polling
            var pollingTime = Configuration.EnginePollingTime();
            try
            {
                LogEngine.ConsoleWriteLine("Start Trigger Polling Cycle", ConsoleColor.Blue);

                while (ServiceStates.RunPolling)
                {
                    if (ServiceStates.RestartNeeded)
                    {
                        LogEngine.ConsoleWriteLine(
                            "--------------------------------------------------------",
                            ConsoleColor.DarkYellow);
                        LogEngine.ConsoleWriteLine("- UPDATE READY - SERVICE NEEDS RESTART -", ConsoleColor.Red);
                        LogEngine.ConsoleWriteLine(
                            "--------------------------------------------------------",
                            ConsoleColor.DarkYellow);
                        //ServiceStates.RunPolling = false;
                        return;
                    }
                    ++pollingStep;
                    if (pollingStep > 9)
                    {
                        LogEngine.ConsoleWriteLine(
                            $"Execute Trigger Polling {pollingStep} Cycle",
                            ConsoleColor.Blue);
                        pollingStep = 0;
                    }
                    Thread.Sleep(pollingTime);
                    var treadPollingRun = new Thread(EventsEngine.ExecuteBubblingTriggerConfigurationPolling);
                    lock (treadPollingRun)
                    {
                        treadPollingRun.Start();
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
    }
}