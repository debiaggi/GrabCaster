﻿// --------------------------------------------------------------------------------------------------
// <copyright file = "Embedded.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Engine.OffRamp;
    using GrabCaster.Framework.Log;

    /// <summary>
    /// The embedded point.
    /// </summary>
    public static class Embedded
    {

        public delegate void SetEventActionEventEmbedded(IEventType _this, EventActionContext context);
        /// <summary>
        /// Used internally by the embedded
        /// </summary>
        public static SetEventActionEventEmbedded setEventActionEventEmbedded { get; set; }

        // Global Action Events
        /// <summary>
        /// The delegate action event.
        /// </summary>
        private static SetEventActionEvent delegateActionEvent;


        /// <summary>
        /// Handles the ProcessExit event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void CurrentDomainProcessExit(object sender, EventArgs e)
        {
            CoreEngine.StopEventEngine();
        } // CurrentDomain_ProcessExit

        public static void StartEngine()
        {
            try
            {
                Configuration.LoadConfiguration();
                LogEngine.Init();
                LogEngine.ConsoleWriteLine(
                    $"Version {Assembly.GetExecutingAssembly().GetName().Version}",
                    ConsoleColor.Green);

                if (Configuration.DisableDeviceProviderInterface())
                {
                    LogEngine.WriteLog(
                        Configuration.EngineName,
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.",
                        Constant.ErrorEventIdHighCritical,
                        Constant.TaskCategoriesError,
                        null,
                        EventLogEntryType.Warning);
                }
                // Could be useful?
                //if (!Environment.UserInteractive)
                //{
                //    Debug.WriteLine("GrabCaster-servicesToRun procedure initialization.");
                //    // ServiceBase[] servicesToRun = { new NTWindowsService() };
                //    Debug.WriteLine("GrabCaster-servicesToRun procedure starting.");
                //    // ServiceBase.Run(servicesToRun);
                //}

                LogEngine.ConsoleWriteLine("--GrabCaster Sevice Initialization--Start Engine.", ConsoleColor.Green);
                delegateActionEvent = delegateActionEventEmbedded;
                CoreEngine.StartEventEngine(delegateActionEvent);

                Thread.Sleep(Timeout.Infinite);
            }
            catch (NotImplementedException ex)
            {
                Methods.DirectEventViewerLog(ex);
                LogEngine.WriteLog(
                    Configuration.EngineName,
                    "Error in " + MethodBase.GetCurrentMethod().Name,
                    Constant.ErrorEventIdHighCritical,
                    Constant.TaskCategoriesError,
                    ex,
                    EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                Methods.DirectEventViewerLog(ex);
                Environment.Exit(0);
            } // try/catch
            finally
            {
                //Spool log queues
                if (LogEngine.QueueAbstractMessage != null)
                {
                    LogEngine.QueueAbstractMessageOnPublish(LogEngine.QueueAbstractMessage.ToArray().ToList());
                }
                if (LogEngine.QueueConsoleMessage != null)
                {
                    LogEngine.QueueConsoleMessageOnPublish(LogEngine.QueueConsoleMessage.ToArray().ToList());
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
        private static void delegateActionEventEmbedded(IEventType eventType, EventActionContext context)
        {
            lock (context)
            {
                try
                {
                    setEventActionEventEmbedded(eventType, context);

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

    }
}
