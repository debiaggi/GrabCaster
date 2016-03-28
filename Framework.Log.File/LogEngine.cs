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
namespace GrabCaster.Framework.Log.File
{
    using Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Log;
    using System;
    using System.IO;
    /// <summary>
    /// The log engine, simple version.
    /// </summary>
    [LogContract("{4DACE829-1462-4A3D-ACC9-1EE41B3C2D53}", "LogEngine", "File Log System")]
    public class LogEngine : ILogEngine
    {
        private string PathFile = "";
        StreamWriter logFile = null; 
        /// <summary>
        /// Initialize log.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool InitLog()
        {
            Directory.CreateDirectory(Configuration.DirectoryLog());
            PathFile = Path.Combine(Configuration.DirectoryLog(),$"{DateTime.Now.Month.ToString()}{DateTime.Now.Day.ToString()}{DateTime.Now.Year.ToString()}-{Guid.NewGuid().ToString()}.txt");
            logFile = File.AppendText(PathFile);
            return true;
        }

        /// <summary>
        /// The write log.
        /// </summary>
        /// <param name="logMessage">
        /// The log message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool WriteLog(LogMessage logMessage)
        {
            logFile.WriteLine($"{DateTime.Now.ToString()} - {logMessage.Message}");
            //logFile.FlushAsync();
            return true;
        }

    }
}