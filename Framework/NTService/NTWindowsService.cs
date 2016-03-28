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
namespace GrabCaster.Framework.NTService
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.ServiceProcess;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Log;

    /// <summary>
    /// Component that represents the Windows Service.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public partial class NTWindowsService : ServiceBase
    {
        /// <summary>
        /// Starts the core Engine.
        /// </summary>
        public static void StartEngine()
        {
            try
            {
                // Start NT service
                Debug.WriteLine("LogEventUpStream - Initialization--Start Engine.");
                LogEngine.ConsoleWriteLine("Initialization--Start Engine.", ConsoleColor.Green);
                LogEngine.Init();
                Debug.WriteLine("LogEventUpStream - StartEventEngine.");
                CoreEngine.StartEventEngine(null);
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
                Thread.Sleep(Configuration.WaitTimeBeforeRestarting());
                Environment.Exit(0);
            }
        }
 // StartEngine

        /// <summary>
        /// Called when the Windows Service starts.
        /// </summary>
        /// <param name="args">
        /// The arguments.
        /// </param>
        protected override void OnStart(string[] args)
        {
            try
            {
                // ******************************************
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Instance {this.ServiceName} engine starting.", 
                    Constant.DefconOne, 
                    Constant.TaskCategoriesError, 
                    null, 
                    EventLogEntryType.Information);

                var engineThreadProcess = new Thread(StartEngine);

                engineThreadProcess.Start();

                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Instance {this.ServiceName} engine started.", 
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
                Thread.Sleep(Configuration.WaitTimeBeforeRestarting());
                Environment.Exit(0);
            }
        }
 // OnStart

        /// <summary>
        /// Called when Windows Service stops.
        /// </summary>
        protected override void OnStop()
        {
            CoreEngine.StopEventEngine();
        }
 // OnStop
    } // NTWindowsService
} // namespace