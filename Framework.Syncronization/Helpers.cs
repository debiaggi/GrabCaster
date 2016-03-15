using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Syncronization
{
    using System.IO;

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

        public static void CreateSyncronizationFile(string folder)
        {
            string fileName = Path.Combine(folder,"SyncronizationStatus.gc");
            File.WriteAllText(fileName,DateTime.UtcNow.ToString());
        }

        //Less than zero t1 is earlier than t2.
        //Zero t1 is the same as t2.
        //Greater than zero t1 is later than t2.
        public static int ToBeSyncronized(string sourceFolder, string destFolder)
        {
            string dateSourceString = File.ReadAllText(sourceFolder);
            string dateDestinationString = File.ReadAllText(destFolder);

            DateTime srcDt = DateTime.Parse(dateSourceString);
            DateTime destDt = DateTime.Parse(dateDestinationString);

            return DateTime.Compare(srcDt, destDt);
        }
    }
}
