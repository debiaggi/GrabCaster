// --------------------------------------------------------------------------------------------------
// <copyright file = "LogEngine.cs" company="Nino Crudele">
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