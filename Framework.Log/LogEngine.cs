﻿// -----------------------------------------------------------------------------------
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
namespace GrabCaster.Framework.Log
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Log;

    /// <summary>
    /// Class to manage the console messages
    /// </summary>
    public class ConsoleMessage
    {
        public ConsoleMessage(string message, ConsoleColor consoleColor)
        {
            this.ConsoleColor = consoleColor;
            this.Message = message;
        }

        public ConsoleColor ConsoleColor { get; set; }

        public string Message { get; set; }
    }

    /// <summary>
    /// Log engine master class
    /// </summary>
    public static class LogEngine
    {
        public enum Level
        {
            Info,

            Warning,

            Error
        }

        public static LogQueueConsoleMessage QueueConsoleMessage;

        public static LogQueueAbstractMessage QueueAbstractMessage;

        public static bool Enabled;

        public static bool Verbose;

        public static bool ConsoleOut = true;

        private static readonly string EventViewerSource = Configuration.EngineName;

        private static readonly string EventViewerLog = Configuration.EngineName;

        private static MethodInfo methodLogInfoInit;

        private static MethodInfo methodLogInfoWrite;

        private static readonly object[] ParametersRet = { null };

        private static object classInstance;

        public static void Init()
        {
            try
            {
                Configuration.LoadConfiguration();
                Enabled = Configuration.LoggingEngineEnabled();
                Verbose = Configuration.LoggingVerbose();
                //Load logging external component
                var loggingComponent = Path.Combine(
                    Configuration.DirectoryOperativeRootExeName(),
                    Configuration.LoggingComponent());

                DirectConsoleWriteLine("Check Abstract Logging Engine.", ConsoleColor.Yellow);

                //Create the reflection method cached 
                var assembly = Assembly.LoadFrom(loggingComponent);
                //Main class logging
                var assemblyClass = (from t in assembly.GetTypes()
                                     let attributes = t.GetCustomAttributes(typeof(LogContract), true)
                                     where t.IsClass && attributes != null && attributes.Length > 0
                                     select t).First();

                var classAttributes = assemblyClass.GetCustomAttributes(typeof(LogContract), true);

                if (classAttributes.Length > 0)
                {
                    Debug.WriteLine("LogEventUpStream - InitLog caller");
                    methodLogInfoInit = assemblyClass.GetMethod("InitLog");
                    Debug.WriteLine("LogEventUpStream - WriteLog caller");
                    methodLogInfoWrite = assemblyClass.GetMethod("WriteLog");
                }

                classInstance = Activator.CreateInstance(assemblyClass, null);

                Debug.WriteLine("LogEventUpStream - Inizialize the external log");
                methodLogInfoInit.Invoke(classInstance, null);
                DirectConsoleWriteLine("Initialize Abstract Logging Engine.", ConsoleColor.Yellow);

                Debug.WriteLine("LogEventUpStream - CreateEventSource if not exist");
                if (!EventLog.SourceExists(EventViewerSource))
                {
                    EventLog.CreateEventSource(EventViewerSource, EventViewerLog);
                }

                //Create the QueueConsoleMessage internal queue
                Debug.WriteLine(
                    "LogEventUpStream - logQueueConsoleMessage.OnPublish += LogQueueConsoleMessageOnPublish");
                QueueConsoleMessage =
                    new LogQueueConsoleMessage(
                        Configuration.ThrottlingConsoleLogIncomingRateNumber(),
                        Configuration.ThrottlingConsoleLogIncomingRateSeconds());
                QueueConsoleMessage.OnPublish += QueueConsoleMessageOnPublish;

                //Create the QueueAbstractMessage internal queue
                Debug.WriteLine(
                    "LogEventUpStream - logQueueAbstractMessage.OnPublish += LogQueueAbstractMessageOnPublish");
                QueueAbstractMessage = new LogQueueAbstractMessage(
                    Configuration.ThrottlingLsiLogIncomingRateNumber(),
                    Configuration.ThrottlingLsiLogIncomingRateSeconds());
                QueueAbstractMessage.OnPublish += QueueAbstractMessageOnPublish;
                Debug.WriteLine("LogEventUpStream - Log Queues initialized.");
            }
            catch (Exception ex)
            {
                LogEngine.EventViewerWriteLog(
                    Configuration.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.DefconOne,
                    Constant.TaskCategoriesError,
                    ex,
                    EventLogEntryType.Error);
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

        public static void WriteConsoleLog(LogMessage logMessage)
        {
            Console.ForegroundColor = logMessage.Level == EventLogEntryType.Error
                                          ? ConsoleColor.Red
                                          : logMessage.Level == EventLogEntryType.Warning
                                                ? ConsoleColor.Yellow
                                                : ConsoleColor.White;
            ConsoleWriteLine(logMessage.Message, Console.ForegroundColor);

            EventLog.WriteEntry(EventViewerSource, logMessage.Message, logMessage.Level, logMessage.EventId);
        }

        public static void WriteLog(
        string source,
        string message,
        int eventId,
        string taskCategory,
        Exception exception,
        EventLogEntryType level)
            {
                Debug.WriteLine($"GrabCaster-{message}");
                try
                {
                    var logMessage = new LogMessage();

                    if (exception != null)
                    {
                        logMessage.ExceptionObject =
                            $"-HResult: {exception.HResult}\r -Error Message: {exception.Message + ""}\r -InnerExcetion: {exception.InnerException}\r -Source: {exception.Source}\r -StackTrace: {exception.StackTrace}";
                    }
                    else
                    {
                        logMessage.ExceptionObject = "";
                    }

                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    logMessage.DateTime = DateTime.Now.ToString();
                    logMessage.EventId = eventId;
                    logMessage.MessageId = Guid.NewGuid().ToString();
                    logMessage.Level = level;
                    logMessage.Source = Configuration.EngineName;
                    logMessage.ChannelId = Configuration.ChannelId();
                    logMessage.ChannelName = Configuration.ChannelName();
                    logMessage.TaskCategory = taskCategory;
                    var exceptionText = logMessage.ExceptionObject != "" ? "\r-->Exception:" + logMessage.ExceptionObject : "";
                    logMessage.Message =
                        $"-Level:{level}\r-Source:{source}\r-Message:{message}\r-Severity:{eventId}\r-TaskCategory:{taskCategory}{exceptionText}";

                    if (QueueAbstractMessage != null)
                    {
                        lock (QueueAbstractMessage)
                        {
                            QueueAbstractMessage.Enqueue(logMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Last error point
                    Methods.DirectEventViewerLog(ex);
                }
            }



        public static void EventViewerWriteLog(
            string source,
            string message,
            int eventId,
            string taskCategory,
            Exception exception,
            EventLogEntryType level)
        {
            Debug.WriteLine($"GrabCaster-{message}");
            try
            {
                var logMessage = new LogMessage();

                if (exception != null)
                {
                    logMessage.ExceptionObject =
                        $"-HResult: {exception.HResult}\r -Error Message: {exception.Message +""}\r -InnerExcetion: {exception.InnerException}\r -Source: {exception.Source}\r -StackTrace: {exception.StackTrace}";
                }
                else
                {
                    logMessage.ExceptionObject = "";
                }

                // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                logMessage.DateTime = DateTime.Now.ToString();
                logMessage.EventId = eventId;
                logMessage.MessageId = Guid.NewGuid().ToString();
                logMessage.Level = level;
                logMessage.Source = Configuration.EngineName;
                logMessage.ChannelId = Configuration.ChannelId();
                logMessage.ChannelName = Configuration.ChannelName();
                logMessage.TaskCategory = taskCategory;
                var exceptionText = logMessage.ExceptionObject != "" ? "\r-->Exception:" + logMessage.ExceptionObject : "";
                logMessage.Message =
                    $"-Level:{level}\r-Source:{source}\r-Message:{message}\r-EventID:{eventId}\r-TaskCategory:{taskCategory}{exceptionText}";

                Methods.DirectEventViewerLog(logMessage.Message, level);
            }
            catch (Exception ex)
            {
                //Last error point
                Methods.DirectEventViewerLog(ex);
            }
        }



        public static void DebugWriteLine(string message)
        {
            Debug.WriteLine($"Engine debug log > {message}");
        }

        #region MAIN LOG ABSTRACTED ENGINE

        /// <summary>
        /// Lock slim class for console messages
        /// </summary>
        public sealed class LogQueueAbstractMessage : LockSlimQueueLog<LogMessage>
        {
            public LogQueueAbstractMessage(int capLimit, int timeLimit)
            {
                this.CapLimit = capLimit;
                this.TimeLimit = timeLimit;
                this.InitTimer();
            }
        }

        public static void QueueAbstractMessageOnPublish(List<LogMessage> logMessages)
        {
            foreach (var logMessage in logMessages)
            {
                ParametersRet[0] = logMessage;
                methodLogInfoWrite.Invoke(classInstance, ParametersRet);
                //Write the message also in Console
                WriteConsoleLog(logMessage);
            }
        }

        #endregion

        #region INTERNAL CONSOLE LOG

        /// <summary>
        /// Lock slim class for console messages
        /// </summary>
        public sealed class LogQueueConsoleMessage : LockSlimQueueLog<ConsoleMessage>
        {
            public LogQueueConsoleMessage(int capLimit, int timeLimit)
            {
                this.CapLimit = capLimit;
                this.TimeLimit = timeLimit;
                this.InitTimer();
            }
        }

        public static void ConsoleWriteLine(string message, ConsoleColor color)
        {
            var consoleMessage = message.Replace("\r", "");
            Debug.WriteLine($"GrabCaster-{consoleMessage}");
            if (Enabled)
            {
                lock (QueueConsoleMessage)
                {
                    QueueConsoleMessage.Enqueue(new ConsoleMessage(consoleMessage, color));
                }

            }
        }

        public static void ConsoleWriteLineNoLog(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void QueueConsoleMessageOnPublish(List<ConsoleMessage> consoleMessages)
        {
            foreach (var consoleMessage in consoleMessages)
            {
                DirectConsoleWriteLine(consoleMessage.Message, consoleMessage.ConsoleColor);
            }
        }

    /// <summary>
    /// Write console message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="color"></param>
    public static void DirectConsoleWriteLine(string message, ConsoleColor color)
        {
            if (Enabled)
            {
                if (Verbose)
                {
                    Methods.DirectEventViewerLog(message, EventLogEntryType.Information);
                }

                var logMessage = new LogMessage();
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;

                if (Configuration.LoggingVerbose())
                {
                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    logMessage.DateTime = DateTime.Now.ToString();
                    logMessage.EventId = 0;
                    logMessage.MessageId = Guid.NewGuid().ToString();
                    logMessage.ExceptionObject = "";
                    logMessage.Level = EventLogEntryType.Information;
                    logMessage.Message = message;
                    logMessage.Source = Configuration.EngineName;
                    logMessage.TaskCategory = "Verbose";

                    //Send the verbose also to the SLI component
                    ParametersRet[0] = logMessage;
                    // ReSharper disable once UseNullPropagation
                    if (methodLogInfoWrite != null)
                    {
                        methodLogInfoWrite.Invoke(classInstance, ParametersRet);
                    }
                }
            }
        }

        #endregion
    }
}