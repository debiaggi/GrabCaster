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

        private ConfigurationStorage configurationStorage = null;


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

            if (localPoint)
            {
                PointId = Configuration.PointId();
                PointName = Configuration.PointName();
            }
            else
            {
                //la combo propone le directies, 
                //si selezione, local oppure una delle dir a si splitta channelid e pointid


                string direname="";
                string[] data = direname.Split('_');
                ChannelId = data[0];
                PointId = data[1];

                //prender il valori dal config file
                configurationStorage = new ConfigurationStorage();
                string workFolder = Configuration.SyncBuildSpecificDirectoryGcPoints(
                    ChannelId,
                    PointId);

                //deve prelevare il file cfg nella dir, puo avere nome diverso percio va cercato
                string[] configurationFileFound = System.IO.Directory.GetFiles(workFolder, "*.cfg");
                string configurationFile = Path.Combine(workFolder, configurationFileFound[0]);
                configurationStorage = JsonConvert.DeserializeObject<ConfigurationStorage>(
                                                    Encoding.UTF8.GetString(File.ReadAllBytes(configurationFile)));

                PointId = configurationStorage.PointId;
                PointName = configurationStorage.PointName;

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
                                               : Configuration.SyncBuildSpecificDirectoryGcPointsIn(ChannelId, PointId);
                GrabCaster.Framework.Compression.Helpers.CreateFromBytearray(skeletonMessage.Body, currentSyncFolder);
                //GrabCaster.Framework.Syncronization.Helpers.SyncFolders();

                Console.WriteLine("---------------EVENT RECEIVED FROM EMBEDDED LIBRARY---------------");
            }


        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {

            OffRampEngineSending.SendNullMessageOnRamp(
                Configuration.MessageDataProperty.ConsoleSendBubblingBag,
                "*",
                "*",
                string.Empty,
                string.Empty,
                PointId);
        }

        private void FormPoint_Load(object sender, EventArgs e)
        {
            InizializeEnvironment();

        }
    }
}
