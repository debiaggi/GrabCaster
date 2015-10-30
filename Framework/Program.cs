// --------------------------------------------------------------------------------------------------
// <copyright file = "LogMessage.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog: http://ninocrudele.me
//    
//    By accessing GrabCaster code here, you are agreeing to the following licensing terms.
//    If you do not agree to these terms, do not access the GrabCaster code.
//    Your license to the GrabCaster source and/or binaries is governed by the 
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.ServiceProcess;
    using System.Windows.Forms;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Log;
    using GrabCaster.Framework.NTService;

    /// <summary>
    /// Class containing the main entry to the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Handles the ProcessExit event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void CurrentDomainProcessExit(object sender, EventArgs e)
        {
            CoreEngine.StopEventEngine();
        } // CurrentDomain_ProcessExit

        /// <summary>
        /// Mains the main entry to the program.
        /// </summary>
        /// <param name="args">The arguments to the program.</param>
        /// <exception cref="System.NotImplementedException">
        /// Exception thrown if incorrect parameters are passed to the command-line.
        /// </exception>
        public static void Main(string[] args)
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

                if (!Environment.UserInteractive)
                {
                    Debug.WriteLine("GrabCaster-servicesToRun procedure initialization.");
                    ServiceBase[] servicesToRun = { new NTWindowsService() };
                    Debug.WriteLine("GrabCaster-servicesToRun procedure starting.");
                    ServiceBase.Run(servicesToRun);
                }
                else
                {
                    if (args.Length == 0)
                    {
                        // Set Console windows
                        Console.Title = Configuration.PointName();
                        Console.SetWindowPosition(0, 0);
                        Console.Clear();
                        Console.ForegroundColor= ConsoleColor.Green;
                        Console.WriteLine(@"[M] Run GrabCaster in MS-DOS Console mode.");
                        Console.WriteLine(@"[I] Install GrabCaster Windows NT Service.");
                        Console.WriteLine(@"[U] Uninstall GrabCaster Windows NT Service.");
                        Console.WriteLine(@"[C] Clone a new GrabCaster Point.");
                        Console.WriteLine(@"[Ctrl + C] Exit.");
                        Console.ForegroundColor = ConsoleColor.White;
                        var consoleKeyInfo = Console.ReadKey();

                        switch (consoleKeyInfo.Key)
                        {
                            case ConsoleKey.M:
                                AppDomain.CurrentDomain.ProcessExit += CurrentDomainProcessExit;
                                LogEngine.ConsoleWriteLine(
                                    "--GrabCaster Sevice Initialization--Start Engine.",
                                    ConsoleColor.Green);
                                CoreEngine.StartEventEngine();
                                Console.ReadLine();
                                break;
                            case ConsoleKey.I:
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Clear();
                                CoreNtService.ServiceName = AskInputLine("Specify the Windows NT Service Name:");
                                CoreNtService.InstallService();
                                break;
                            case ConsoleKey.U:
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Clear();
                                CoreNtService.ServiceName = AskInputLine("Specify the Windows NT Service Name:");
                                CoreNtService.StopService();
                                CoreNtService.UninstallService();
                                break;
                            case ConsoleKey.C:
                                //string CloneName = AskInputLine("Enter the Clone name.");
                                Process.Start(Path.Combine(Application.StartupPath, "Create new Clone.cmd"));
                                break;
                        }
                    } //if
                    else
                    {
                        //Run in batch and console mode
                        if (args.Length == 1 && args[0] == "-console".ToLower())
                        {
                            LogEngine.ConsoleWriteLine(
                                "--GrabCaster Sevice Initialization--Start Engine.",
                                ConsoleColor.Green);
                            CoreEngine.StartEventEngine();
                            Console.ReadLine();
                        }
                        else if (args.Length == 2 && args[0] == "-ntinstall".ToLower())
                        {
                            CoreNtService.ServiceName = string.Concat("GrabCaster", args[1]);
                            CoreNtService.InstallService();
                            Environment.Exit(0);
                        }
                        else if (args.Length == 2 && args[0] == "-ntuninstall".ToLower())
                        {
                            CoreNtService.ServiceName = string.Concat("GrabCaster", args[1]);
                            CoreNtService.StopService();
                            CoreNtService.UninstallService();
                            Environment.Exit(0);
                        }
                        else
                        {
                            Console.Clear();
                            LogEngine.ConsoleWriteLine(
                                @"GrabCaster [-console]  [-ntinstall servicename] [-ntuninstall servicename]",
                                ConsoleColor.Green);
                            LogEngine.ConsoleWriteLine("", ConsoleColor.DarkGreen);
                            LogEngine.ConsoleWriteLine(
                                @"[-console] Execute GrabCaster in MS Dos console mode.",
                                ConsoleColor.Green);
                            LogEngine.ConsoleWriteLine(
                                "[-ntinstall servicename] Install jiGate as Windows NT Service servicename.",
                                ConsoleColor.Green);
                            LogEngine.ConsoleWriteLine(
                                "[-ntuninstall servicename] Uninstall the GrabCaster Windows NT Service servicename.",
                                ConsoleColor.Green);

                            Console.ReadLine();
                            Environment.Exit(0);
                        }
                    }
                } // if
            }
            catch (NotImplementedException ex)
            {
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
                LogEngine.DirectEventViewerLog(ex);

                Environment.Exit(0);
            } // try/catch
        } // Main

        private static string AskInputLine(string message)
        {
            var ret = string.Empty;
            while (ret == string.Empty)
            {
                LogEngine.ConsoleWriteLine(message, ConsoleColor.Green);
                ret = Console.ReadLine();
            }
            return ret;
        }
    } // Program
} // namespace