using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCasterUI
{
    using GrabCaster.Framework.Base;

    internal class GcPointsFoldersData
    {
        public string FolderName { get; set; }

        public ConfigurationStorage ConfigurationStorage { get; set; }

        public GcPointsFoldersData(string FolderName, ConfigurationStorage ConfigurationStorage)
        {
            this.FolderName = FolderName;
            this.ConfigurationStorage = ConfigurationStorage;
        }

    }
}
