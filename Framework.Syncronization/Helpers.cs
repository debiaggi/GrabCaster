﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Syncronization
{
    using Microsoft.Synchronization;
    using Microsoft.Synchronization.Files;

    /// <summary>
    /// Syncronozation Class
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Syncronize 2 folders
        /// </summary>
        /// <param name="SourceFolder"></param>
        /// <param name="DestinationFolder"></param>
        public static void SyncFolders(string SourceFolder, string DestinationFolder)
        {
            SyncOrchestrator syncOrchestrator = new SyncOrchestrator();


            syncOrchestrator.LocalProvider = new FileSyncProvider(SourceFolder);
            syncOrchestrator.RemoteProvider = new FileSyncProvider(DestinationFolder);
            syncOrchestrator.Synchronize();
        }

    }
}