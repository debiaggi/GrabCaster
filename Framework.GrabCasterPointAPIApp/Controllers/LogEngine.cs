using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Library.Azure
{
    public static class LogEngine
    {
        public static void WriteLog(string message)
        {
            string logMessage = $"{ConfigurationLibrary.EngineName()} - {message}";
            System.Diagnostics.Trace.WriteLine(logMessage);
        }
    }
}
