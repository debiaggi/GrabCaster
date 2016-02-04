using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Library.Azure
{
    using Microsoft.Azure;
    using Microsoft.WindowsAzure;

    public enum EventHubsCheckPointPattern
    {
        CheckPoint,

        Dt,

        Dtepoch,

        Dtutcnow,

        Dtnow,

        Dtutcnowepoch,

        Dtnowepoch
    }

    /// <summary>
    /// The configuration.
    /// </summary>
    public static class ConfigurationLibrary
    {


        /// <summary>
        /// The azure name space connection string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AzureNameSpaceConnectionString()
        {
            return CloudConfigurationManager.GetSetting("AzureNameSpaceConnectionString");
        }

        /// <summary>
        /// The group event hubs storage account name.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GroupEventHubsStorageAccountName()
        {
            return CloudConfigurationManager.GetSetting("GroupEventHubsStorageAccountName");
        }

        /// <summary>
        /// The group event hubs storage account key.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GroupEventHubsStorageAccountKey()
        {
            return CloudConfigurationManager.GetSetting("GroupEventHubsStorageAccountKey");
        }
        /// <summary>
        /// The group event hubs name.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GroupEventHubsName()
        {
            return CloudConfigurationManager.GetSetting("GroupEventHubsName");

        }

        /// <summary>
        /// The engine name.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string EngineName()
        {
            return "GrabCaster";

        }

        /// <summary>
        /// The point id.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string PointId()
        {
            return CloudConfigurationManager.GetSetting("PointId");
        }

        /// <summary>
        /// The point name.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string PointName ()
        {
            return CloudConfigurationManager.GetSetting("PointName");
        }

        /// <summary>
        /// The channel id.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ChannelId()
        {
            return CloudConfigurationManager.GetSetting("ChannelId");
        }

        /// <summary>
        /// The channel name.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ChannelName()
        {
            return CloudConfigurationManager.GetSetting("ChannelName");
        }

    }
}
