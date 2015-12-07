// --------------------------------------------------------------------------------------------------
// <copyright file = "NTWindowsService.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    Info:   http://GrabCaster.io
// 
//    By accessing GrabCaster code here, you are agreeing to the following licensing terms.
//    If you do not agree to these terms, do not access the GrabCaster code.
//    Your license to the GrabCaster source and/or binaries is governed by the 
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//    
//    This work is registered with the UK Copyright Service.
//    Registration No:284695248  
//  </summary>
// --------------------------------------------------------------------------------------------------
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
                CoreEngine.StartEventEngine();
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
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesError, 
                    null, 
                    EventLogEntryType.Information);

                var engineThreadProcess = new Thread(StartEngine);

                engineThreadProcess.Start();

                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Instance {this.ServiceName} engine started.", 
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