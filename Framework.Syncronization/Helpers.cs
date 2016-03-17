using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Log;

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
        private static string syncFile = "SyncronizationStatus.gc";
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
            string fileName = Path.Combine(folder, syncFile);
            File.WriteAllText(fileName,DateTime.UtcNow.ToString());
        }

        //Less than zero t1 is earlier than t2.
        //Zero t1 is the same as t2.
        //Greater than zero t1 is later than t2.
        public static int ToBeSyncronized(string sourceFolder, string restinationFolder, bool syncronize)
        {
            int synTo = 0;
            try
            {
                string sFile = Path.Combine(sourceFolder, syncFile);
                string dFile = Path.Combine(restinationFolder, syncFile);

                if (Configuration.IamConsole()) return 0;

                //The source directory from console does not exist
                if (!File.Exists(sFile))
                {
                    LogEngine.WriteLog(Configuration.EngineName,
                                          $"Syncronization: Nothing to syncronize.",
                                          Constant.DefconOne,
                                          Constant.TaskCategoriesError,
                                          null,
                                          EventLogEntryType.Information);
                    return 0;
                }
                else
                {
                    //The source directory exist but first time to syncronize, so force the sync
                    if (!File.Exists(dFile))
                    {
                        synTo = 100000;
                    }
                    else
                    {
                        if (File.Exists(dFile))
                        {
                            string dateSourceString = File.ReadAllText(sFile);
                            string dateDestinationString = File.ReadAllText(dFile);

                            DateTime srcDt = DateTime.Parse(dateSourceString);
                            DateTime destDt = DateTime.Parse(dateDestinationString);
                            synTo = DateTime.Compare(srcDt, destDt);
                        }
                    }
                }

                if (synTo > 0)
                {
                    if (syncronize)
                    {
                        SyncFolders(sourceFolder, restinationFolder);
                    }
                }
            }
            catch(Exception ex)
            {

                LogEngine.WriteLog(Configuration.EngineName,
                              $"Error in {MethodBase.GetCurrentMethod().Name}",
                              Constant.DefconOne,
                              Constant.TaskCategoriesError,
                              ex,
                              EventLogEntryType.Error);


            }



            return synTo;
        }
    }
}
