using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.CompressionLibrary
{
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Log;

    public static class Helpers
    {
        /// <summary>
        /// Compress a folder
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns>a byte array</returns>
        public static byte[] CreateFromDirectory(string directoryPath)
        {
            string zipFolderFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.zip");

            try
            {
                ZipFile.CreateFromDirectory(directoryPath, zipFolderFile, CompressionLevel.Fastest, true);
                byte[] fileStream = File.ReadAllBytes(zipFolderFile);
                File.Delete(zipFolderFile);
                return fileStream;
            }
            catch (Exception ex)
            {


                LogEngine.WriteLog(Configuration.EngineName,
                              $"Error in {MethodBase.GetCurrentMethod().Name}",
                              Constant.DefconOne,
                              Constant.TaskCategoriesError,
                              ex,
                              EventLogEntryType.Error);
                return null;

            }
        }

        /// <summary>
        /// Decompress byte stream
        /// </summary>
        /// <param name="fileContent"></param>
        public static void CreateFromBytearray(byte[] fileContent,string unzipFolder)
        {
            string unzipFolderFile = Path.Combine(Path.GetTempPath(),$"{Guid.NewGuid().ToString()}.zip");
            

            try
            {
                File.WriteAllBytes(unzipFolderFile, fileContent);
                ZipFile.ExtractToDirectory(unzipFolderFile, unzipFolder);
                File.Delete(unzipFolderFile);
            }
            catch (Exception ex)
            {


                LogEngine.WriteLog(Configuration.EngineName,
                              $"Error in {MethodBase.GetCurrentMethod().Name}",
                              Constant.DefconOne,
                              Constant.TaskCategoriesError,
                              ex,
                              EventLogEntryType.Error);

            }
        }
    }
}
