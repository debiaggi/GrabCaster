// --------------------------------------------------------------------------------------------------
// <copyright file = "NTService.cs" company="Nino Crudele">
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
    using System.Collections;
    using System.Configuration.Install;
    using System.Diagnostics;
    using System.Reflection;
    using System.ServiceProcess;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Log;

    /// <summary>
    /// Contains helper methods to start, stop and (un)install service.
    /// </summary>
    internal static class CoreNtService
    {
        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public static string ServiceName { get; set; }

        /// <summary>
        /// Determines whether this Windows Service is installed.
        /// </summary>
        /// <returns><c>true</c> is installed, <c>false</c> otherwise.</returns>
        public static bool IsInstalled()
        {
            using (var controller = new ServiceController(ServiceName))
            {
                try
                {
                    // ReSharper disable once UnusedVariable
                    var status = controller.Status;
                }
                catch
                {
                    return false;
                }
 // try/catch

                return true;
            }
 // using
        }
 // IsInstalled

        /// <summary>
        /// Determines whether the Windows Service is running.
        /// </summary>
        /// <returns><c>true</c> is running, <c>false</c> otherwise.</returns>
        public static bool IsRunning()
        {
            using (var controller = new ServiceController(ServiceName))
            {
                if (!IsInstalled())
                {
                    return false;
                }
 // if

                return controller.Status == ServiceControllerStatus.Running;
            }
 // using
        }
 // IsRunning

        /// <summary>
        /// Creates an <see cref="AssemblyInstaller"/> object to perform the service installation.
        /// </summary>
        /// <returns>Returns an <see cref="AssemblyInstaller"/> object to perform the service installation.</returns>
        public static AssemblyInstaller GetInstaller()
        {
            var installer = new AssemblyInstaller(typeof(NTWindowsService).Assembly, null)
                                              {
                                                  UseNewContext
                                                      = true
                                              };

            return installer;
        }
 // GetInstaller

        /// <summary>
        /// Installs the Windows Service.
        /// </summary>
        public static void InstallService()
        {
            if (IsInstalled())
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"NT Service instance {ServiceName} is already installed.", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesConsole, 
                    null, 
                    EventLogEntryType.Information);
                Console.ReadLine();

                return;
            }
 // if

            using (var installer = GetInstaller())
            {
                IDictionary state = new Hashtable();
                try
                {
                    installer.Install(state);
                    installer.Commit(state);

                    LogEngine.WriteLog(
                        Configuration.EngineName, 
                        $"NT Service instance {ServiceName} installation completed.", 
                        Constant.ErrorEventIdHighCritical, 
                        Constant.TaskCategoriesConsole, 
                        null, 
                        EventLogEntryType.Information);
                    Console.ReadLine();
                }
                catch
                {
                    try
                    {
                        installer.Rollback(state);
                    }
                    catch
                    {
                        // ignored
                    }
                    // try/catch

                    throw;
                }
            }
        }
 // InstallService

        /// <summary>
        /// Uninstalls the Windows Service.
        /// </summary>
        public static void UninstallService()
        {
            if (!IsInstalled())
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"NT Service instance {ServiceName} is not installed.", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesConsole, 
                    null, 
                    EventLogEntryType.Warning);
                Console.ReadLine();

                return;
            }
 // if

            using (var installer = GetInstaller())
            {
                IDictionary state = new Hashtable();
                installer.Uninstall(state);
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"Service {ServiceName} Uninstallation completed.", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesConsole, 
                    null, 
                    EventLogEntryType.Information);
                Console.ReadLine();
            }
 // using
        }
 // UninstallService

        /// <summary>
        /// Starts the Windows Service.
        /// </summary>
        public static void StartService()
        {
            if (!IsInstalled())
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    $"NT Service instance {ServiceName} is not installed.", 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesConsole, 
                    null, 
                    EventLogEntryType.Warning);
                Console.ReadLine();

                return;
            }
 // if

            try
            {
                ServiceBase[] servicesToRun = { new NTWindowsService() };
                ServiceBase.Run(servicesToRun);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName, 
                    "Error in " + MethodBase.GetCurrentMethod().Name, 
                    Constant.ErrorEventIdHighCritical, 
                    Constant.TaskCategoriesConsole, 
                    ex, 
                    EventLogEntryType.Error);
                Console.ReadLine();

                throw;
            }
 // try/catch
        }
 // StartService

        /// <summary>
        /// Stops the Windows Service.
        /// </summary>
        public static void StopService()
        {
            if (!IsInstalled())
            {
                return;
            }

            using (var controller = new ServiceController(ServiceName))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Stopped)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                    }
 // if
                }
                catch (Exception ex)
                {
                    LogEngine.WriteLog(
                        Configuration.EngineName, 
                        "Error in " + MethodBase.GetCurrentMethod().Name, 
                        Constant.ErrorEventIdHighCritical, 
                        Constant.TaskCategoriesConsole, 
                        ex, 
                        EventLogEntryType.Error);
                    Console.ReadLine();

                    throw;
                }
 // try/catch
            }
 // usnig
        }
 // StopService
    } // NTService
} // namespace