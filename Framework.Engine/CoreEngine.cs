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
                    Constant.DefconOne,
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
                if (!Configuration.DisableExternalEventsStreamEngine())
                {
                    LogEngine.ConsoleWriteLine("Start Internal Event Engine Channel.", ConsoleColor.Yellow);
                    var canStart = EventsEngine.CreateEventUpStream();

                    if (!canStart)
                    {
                        LogEngine.WriteLog(
                            Configuration.EngineName,
                            $"Error during engine service starting. Name: {Configuration.EngineName} - ID: {Configuration.ChannelId()}",
                            Constant.DefconOne,
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
                            Constant.DefconOne,
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
                //Start triggers polling instances
                if (Configuration.EnginePollingTime() > 0)
                {
                    var treadPollingRun = new Thread(StartTriggerPolling);
                    treadPollingRun.Start();
                }
                else
                {
                    LogEngine.WriteLog(Configuration.EngineName,
                                            $"Configuration.EnginePollingTime = {Configuration.EnginePollingTime()}, internal polling system disabled.",
                                            Constant.DefconOne,
                                            Constant.TaskCategoriesError,
                                            null,
                                            EventLogEntryType.Warning);
                }

                //Start Engine Service
                LogEngine.ConsoleWriteLine(
                    "Asyncronous Threading Service state active.",
                    ConsoleColor.DarkCyan);
                var treadEngineStates = new Thread(CheckServiceStates);
                treadEngineStates.Start();

                if (!Configuration.DisableExternalEventsStreamEngine())
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
                    Constant.DefconOne,
                    Constant.TaskCategoriesError,
                    null,
                    EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.DefconOne,
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
                if (pollingTime == 0)
                {
                    LogEngine.WriteLog(
                        Configuration.EngineName,
                        $"EnginePollingTime = 0 - Internal logging system disabled.",
                        Constant.DefconOne,
                        Constant.TaskCategoriesError,
                        null,
                        EventLogEntryType.Warning);
                    return;

                }
                else
                {
                    LogEngine.ConsoleWriteLine("Start Trigger Polling Cycle", ConsoleColor.Blue);
                }
                

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
                    Constant.DefconOne,
                    Constant.TaskCategoriesError,
                    ex,
                    EventLogEntryType.Error);
            }
        }
    }
}