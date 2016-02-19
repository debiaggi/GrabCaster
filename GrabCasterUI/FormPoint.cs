using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrabCasterUI
{
    using System.IO;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Contracts.Syncronization;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Engine.OffRamp;
    using GrabCaster.Framework.Engine.OnRamp;
    using GrabCaster.Framework.Library;
    using GrabCaster.Framework.Serialization.Object;

    using Newtonsoft.Json;

    public partial class FormPoint : Form
    {
        //Main objects
        private TreeNode treeNodeMainPoint = null;

        //Constants 
        public const string CONST_ROOT = "ROOT";
        public const string CONST_ROOT_KEY = "ROOT";

        public static string PointId = "";
        public static string ChannelId = "";
        public static string PointName = "";

        private static bool localPoint = false;

        private ConfigurationStorage ConfigurationStorage = null;
        private List<ConfigurationStorage> ConfigurationStorageList = null;


        /// <summary>
        /// The set event action event embedded.
        /// </summary>
        private static MessageIngestor.SetConsoleActionEventEmbedded setConsoleActionEventEmbedded;

        public FormPoint()
        {
            InitializeComponent();
        }

        private void InizializeEnvironment()
        {
            //Load Configuration

            // solo per debug ma va calcolato dalla combo
            localPoint = true;
            string exenameTomonitor = "GrabCaster";

            Configuration.LoadConfiguration(exenameTomonitor);
            if (localPoint)
            {
                PointId = Configuration.PointId();
                PointName = Configuration.PointName();
            }
            else
            {
                //la combo propone le directies, 
                //si selezione, local oppure una delle dir a si splitta channelid e pointid


                //prender il valori dal config file
                string workFolder = Configuration.SyncBuildSpecificDirectoryGcPoints(PointId);

                ConfigurationStorage = GetConfigurationStorage(workFolder);
                PointId = ConfigurationStorage.PointId;
                PointName = ConfigurationStorage.PointName;

            }
            setConsoleActionEventEmbedded = EventReceivedFromEmbedded;
            MessageIngestor.setConsoleActionEventEmbedded = setConsoleActionEventEmbedded;
            Console.WriteLine("Start GrabCaster Embedded Library");
            Thread t = new Thread(start);
            t.Start();

        }

        static void start()
        {
            GrabCaster.Framework.Library.Embedded.StartEngine();
        }

        private ConfigurationStorage GetConfigurationStorage(string workFolder)
        {
            ConfigurationStorage configurationStorage = new ConfigurationStorage();

            //deve prelevare il file cfg nella dir, puo avere nome diverso percio va cercato
            string[] configurationFileFound = System.IO.Directory.GetFiles(workFolder, "*.cfg");
            string configurationFile = Path.Combine(workFolder, configurationFileFound[0]);
            configurationStorage = JsonConvert.DeserializeObject<ConfigurationStorage>(
                                                Encoding.UTF8.GetString(File.ReadAllBytes(configurationFile)));
            return configurationStorage;
        }
        public TreeNode RefreshTreeviewPointRootNode()
        {


            this.treeNodeMainPoint = this.treeNodeMainPoint.Nodes.Add(CONST_ROOT, PointName, CONST_ROOT_KEY, CONST_ROOT_KEY);

            return null;
        }

        //Riceve la configurazione
        private static void EventReceivedFromEmbedded(string DestinationConsolePointId, ISkeletonMessage skeletonMessage)
        {
            if (DestinationConsolePointId == PointId)
            {
                GCPointBag gcPointBag = (GCPointBag)SerializationEngine.ByteArrayToObject(skeletonMessage.Body);
                string notes = gcPointBag.Notes;

                string currentSyncFolder = DestinationConsolePointId == Configuration.PointId()
                                               ? Configuration.SyncDirectorySyncIn()
                                               : Configuration.SyncBuildSpecificDirectoryGcPointsIn(PointId);
                GrabCaster.Framework.Compression.Helpers.CreateFromBytearray(skeletonMessage.Body, currentSyncFolder);
                //GrabCaster.Framework.Syncronization.Helpers.SyncFolders();

                Console.WriteLine("---------------EVENT RECEIVED FROM EMBEDDED LIBRARY---------------");
            }


        }

        
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            AskPointBag("*","*",PointId, Configuration.MessageDataProperty.ConsoleSendBubblingBag);
        }

        /// <summary>
        /// Send a message request in the channels
        /// </summary>
        /// <param name="DestinationChannelId"></param>
        /// <param name="DestinationPointId"></param>
        /// <param name="CurrentPointId"></param>
        /// <param name="messageDataProperty"></param>
        private void AskPointBag(string DestinationChannelId, string DestinationPointId, string CurrentPointId, Configuration.MessageDataProperty messageDataProperty)
        {
            OffRampEngineSending.SendNullMessageOnRamp(messageDataProperty,
                                                        DestinationChannelId,
                                                        DestinationPointId,
                                                        string.Empty,
                                                        string.Empty,
                                                        CurrentPointId);

        }
        private void FormPoint_Load(object sender, EventArgs e)
        {
            InizializeEnvironment();

        }

        private void buttonAskSpecific_Click(object sender, EventArgs e)
        {

            ConfigurationStorageList = RefreshConfigurationStorageList();
        }

        private List<ConfigurationStorage> RefreshConfigurationStorageList()
        {
            listBoxGCPoints.Items.Clear();
            var configurationStorageList = new List<ConfigurationStorage>();

            string gcPointsFolder = Configuration.SyncDirectoryGcPoints();
            string[] gcPointsFolders = Directory.GetDirectories(gcPointsFolder);

            foreach (var item in gcPointsFolders)
            {
                ConfigurationStorage _configurationStorage = new ConfigurationStorage();
                _configurationStorage = this.GetConfigurationStorage(item);
                configurationStorageList.Add(_configurationStorage);
                this.listBoxGCPoints.Items.Add(_configurationStorage.PointId);
            }

            return configurationStorageList;
        }

        private void buttonSelecteditem_Click(object sender, EventArgs e)
        {
            if (listBoxGCPoints.SelectedIndex < 0)
            {
                MessageBox.Show("Select a point.", "GrabCaster", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ConfigurationStorage configurationStorage = ConfigurationStorageList[listBoxGCPoints.SelectedIndex];

        }

        private void buttonLoadBubbling_Click(object sender, EventArgs e)
        {

        }
    }
}
