// --------------------------------------------------------------------------------------------------
// <copyright file = "NTWindowsService.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Copyright (c) 2013 - 2015 Nino Crudele
//    Blog: http://ninocrudele.me
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License. 
// </summary>
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