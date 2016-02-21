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
    using System.Text.RegularExpressions;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Contracts.Syncronization;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Engine.OffRamp;
    using GrabCaster.Framework.Engine.OnRamp;
    using GrabCaster.Framework.Serialization.Object;

    using Newtonsoft.Json;

    /// <summary>
    /// Main console form
    /// Everything using 1 like combobox1 treeeview1 and so on is on the left
    /// everything ending with 2 is on the right
    /// </summary>
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        #region Variables

        //Constants 
        public const string CONST_POINT_KEY = "POINT";

        public const string CONST_BUBBLING = "BUBBLING";
        public const string CONST_BUBBLING_KEY = "FOLDER";

        public const string CONST_COMPONENTS = "COMPONENTS";
        public const string CONST_COMPONENTS_KEY = "FOLDER";
        public const string CONST_TRIGGERS = "TRIGGERS";
        public const string CONST_TRIGGERS_KEY = "FOLDER";
        public const string CONST_EVENTS = "EVENTS";
        public const string CONST_EVENTS_KEY = "FOLDER";

        public const string CONST_COMPONENT = "COMPONENT";
        public const string CONST_COMPONENT_KEY = "COMPONENT";
        public const string CONST_TRIGGER = "TRIGGER";
        public const string CONST_TRIGGER_KEY = "TRIGGER";
        public const string CONST_EVENT = "EVENT";
        public const string CONST_EVENT_KEY = "EVENT";


      private List<GcPointsFoldersData> GcPointsFoldersDataList = null;

        /// <summary>
        /// The set event action event embedded.
        /// </summary>
        private static MessageIngestor.SetConsoleActionEventEmbedded setConsoleActionEventEmbedded;
        #endregion

        private void InizializeEnvironment()
        {
            //Load Configuration

            Configuration.LoadConfiguration();

            this.comboBox1.Tag = this.treeView1;
            this.comboBox2.Tag = this.treeView2;


            setConsoleActionEventEmbedded = EventReceivedFromEmbedded;
            MessageIngestor.setConsoleActionEventEmbedded = setConsoleActionEventEmbedded;
            Console.WriteLine("Start GrabCaster Embedded Library");
            Thread t = new Thread(start);
            t.Start();

        }

        /// <summary>
        /// Start engine
        /// </summary>
        static void start()
        {
            GrabCaster.Framework.Library.Embedded.StartEngine();
        }

        /// <summary>
        /// Get configuration storage from folder
        /// </summary>
        /// <param name="workFolder"></param>
        /// <returns></returns>
        private ConfigurationStorage GetConfigurationStorage(string workFolder,string configurationFileOverrided)
        {
            ConfigurationStorage configurationStorage = new ConfigurationStorage();
            string configurationFile = "";
            string[] configurationFileFound = null;
            //deve prelevare il file cfg nella dir, puo avere nome diverso percio va cercato


            if (configurationFileOverrided != null) configurationFile = configurationFileOverrided;
            else
            {
                configurationFileFound = System.IO.Directory.GetFiles(workFolder, "*.cfg");
                configurationFile = Path.Combine(workFolder, configurationFileFound[0]);
            }


            configurationStorage = JsonConvert.DeserializeObject<ConfigurationStorage>(
                                                Encoding.UTF8.GetString(File.ReadAllBytes(configurationFile)));
            return configurationStorage;
        }


        private void ConcoleMessage(string message)
        {
            this.textBoxConsole.AppendText($"[{DateTime.Now}]-{message}\r\n");
        }

        /// <summary>
        /// Syncronize all 
        /// </summary>
        private void SyncGCPointsFolder()
        {

            //Ask folder to all the points
            AskPointBag("*", "*", Configuration.PointId(), Configuration.MessageDataProperty.ConsoleSendBubblingBag);
        }


        /// <summary>
        /// Refresh the configuration file list
        /// </summary>
        /// <returns></returns>
        private List<GcPointsFoldersData> RefreshConfigurationStorageList()
        {

            List<GcPointsFoldersData> gcPointsFoldersDataList= new List<GcPointsFoldersData>();

            ConcoleMessage("Refresh operation requested...");
            //Get the locals configurations folders
            //Copy all the local point folder configurations in the GCPoints folder
            string directoryOperativeRootExeName = Configuration.ConfigurationStorage.BaseDirectory;
            string[] gcCongigurationFolders = Directory.GetDirectories(directoryOperativeRootExeName);

            ConcoleMessage("Adding local folders...");

            //Look in all the folder because I can have multiple GC point cloned in the same local folder
            foreach (var item in gcCongigurationFolders)
            {
                //If folder contains bubbling folder then is a config folder
                if (item.Contains("Root_") && !item.Contains("Root_GrabCasterUI"))
                {
                    ConfigurationStorage _configurationStorage = new ConfigurationStorage();
                    string configFileName = item.Split(Path.DirectorySeparatorChar).Last().Split('_').Last();
                    string configFileNameWithFolder = Path.Combine(
                        Configuration.ConfigurationStorage.BaseDirectory,
                        $"{configFileName}.cfg");
                    _configurationStorage = this.GetConfigurationStorage(configFileNameWithFolder, configFileNameWithFolder);
                    gcPointsFoldersDataList.Add(new GcPointsFoldersData(item,_configurationStorage));
                }

            }


            ConcoleMessage("Adding remote folders...");

            //Get folders in GCPoints
            string gcPointsFolder = Configuration.SyncDirectoryGcPoints();
            string[] gcPointsFolders = Directory.GetDirectories(gcPointsFolder);

            foreach (var item in gcPointsFolders)
            {
                ConfigurationStorage _configurationStorage = new ConfigurationStorage();
                _configurationStorage = this.GetConfigurationStorage(item,null);
                gcPointsFoldersDataList.Add(new GcPointsFoldersData(item, _configurationStorage));
            }

            ComboBoxRefreshConfigurationStorageList(gcPointsFoldersDataList);

            return gcPointsFoldersDataList;
        }

        /// <summary>
        /// Refresh the comboboxes list
        /// </summary>
        /// <returns></returns>
        private void ComboBoxRefreshConfigurationStorageList(List<GcPointsFoldersData> gcPointsFoldersDataList)
        {
            this.comboBox1.Items.Clear();
            this.comboBox2.Items.Clear();

            foreach (var item in gcPointsFoldersDataList)
            {
                ConcoleMessage($"Adding {GetPointName(item)}");
                this.comboBox1.Items.Add(GetPointName(item));
                this.comboBox2.Items.Add(GetPointName(item));
            }

        }

        private string GetPointName(GcPointsFoldersData gcPointsFoldersData)
        {
            return $"{gcPointsFoldersData.ConfigurationStorage.PointName} - {gcPointsFoldersData.ConfigurationStorage.PointId}";
        }

        #region Events
        //Riceve la configurazione
        private static void EventReceivedFromEmbedded(string DestinationConsolePointId, ISkeletonMessage skeletonMessage)
        {
            GCPointBag gcPointBag = (GCPointBag)SerializationEngine.ByteArrayToObject(skeletonMessage.Body);
            string notes = gcPointBag.Notes;

            string currentSyncFolder = DestinationConsolePointId == Configuration.PointId()
                                            ? Configuration.SyncDirectorySyncIn()
                                            : Configuration.SyncBuildSpecificDirectoryGcPointsIn("");
            GrabCaster.Framework.Compression.Helpers.CreateFromBytearray(skeletonMessage.Body, currentSyncFolder);
            //GrabCaster.Framework.Syncronization.Helpers.SyncFolders();

            Console.WriteLine("---------------EVENT RECEIVED FROM EMBEDDED LIBRARY---------------");

        }
        /// <summary>
        /// Selct combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_SelectedIndexChanged((ComboBox)sender);

        }


        private void FormMain_Load(object sender, EventArgs e)
        {
            InizializeEnvironment();
        }
        /// <summary>
        /// Add the last configuration to the GCPoints folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            var gcPointsFoldersDataList = this.GcPointsFoldersDataList;
            if (gcPointsFoldersDataList != null)
            {
                gcPointsFoldersDataList.Clear();
            }
            GcPointsFoldersDataList = RefreshConfigurationStorageList();
        }

        #endregion

        #region Communication
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



        #endregion

        private DialogResult MessageBoxForm(string message, MessageBoxButtons messageBoxButtons, MessageBoxIcon messageBoxIcon)
        {
            return MessageBox.Show(message, "GrabCaster", messageBoxButtons, messageBoxIcon);
        }

        private void toolStripButtonSyncronize_Click(object sender, EventArgs e)
        {
            if (MessageBoxForm("Send the syncronization request?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                SyncGCPointsFolder();
            }
        }

        /// <summary>
        /// Selct combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_SelectedIndexChanged(ComboBox comboBox)
        {
            if (comboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Select a point.", "GrabCaster", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            GcPointsFoldersData gcPointsFoldersData = GcPointsFoldersDataList[comboBox.SelectedIndex];
            LoadTreeview((TreeView)comboBox.Tag, gcPointsFoldersData);
            

        }

        private void LoadTreeview(TreeView treeView, GcPointsFoldersData gcPointsFoldersData)
        {
            treeView.Tag = gcPointsFoldersData;

            TreeNode treeNodePOINT = treeView.Nodes.Add(
                CONST_POINT_KEY,
                GetPointName(gcPointsFoldersData),
                CONST_POINT_KEY,
                CONST_POINT_KEY);
            TreeNode treeNodeBubbling = treeNodePOINT.Nodes.Add(
                CONST_BUBBLING,
                CONST_BUBBLING,
                CONST_BUBBLING_KEY,
                CONST_BUBBLING_KEY);
            TreeNode treeNodeTriggers = treeNodeBubbling.Nodes.Add(
                CONST_TRIGGERS,
                CONST_TRIGGERS,
                CONST_TRIGGERS_KEY,
                CONST_TRIGGERS_KEY);
            TreeNode treeNodeEvents = treeNodeBubbling.Nodes.Add(
                CONST_EVENTS,
                CONST_EVENTS,
                CONST_EVENTS_KEY,
                CONST_EVENTS_KEY);
            TreeNode treeNodeComponents = treeNodePOINT.Nodes.Add(
                CONST_COMPONENTS,
                CONST_COMPONENTS,
                CONST_COMPONENTS_KEY,
                CONST_COMPONENTS_KEY);
            TreeNode treeNodeTriggersComponents = treeNodeComponents.Nodes.Add(
                CONST_TRIGGERS,
                CONST_TRIGGERS,
                CONST_TRIGGERS_KEY,
                CONST_TRIGGERS_KEY);
            TreeNode treeNodeEventsComponents = treeNodeComponents.Nodes.Add(
                CONST_EVENTS,
                CONST_EVENTS,
                CONST_EVENTS_KEY,
                CONST_EVENTS_KEY);
            // TRIGGERS***************************************************************************
            // Loop in the directory

            string DirectoryBubblingTriggers = Path.Combine(
                gcPointsFoldersData.FolderName,
                Configuration.DirectoryNameBubbling,
                Configuration.DirectoryNameTriggers);



            var triggerBubblingDirectory = DirectoryBubblingTriggers;

            var regTriggers = new Regex(".*");
            var triggerConfigurationsFiles =
                Directory.GetFiles(triggerBubblingDirectory, "*", SearchOption.AllDirectories)
                    .Where(path => regTriggers.IsMatch(path))
                    .ToList();

            foreach (var triggerConfigurationsFile in triggerConfigurationsFiles)
            {
                TriggerConfiguration triggerConfiguration = null;
                var triggerConfigurationsByteContent = File.ReadAllBytes(triggerConfigurationsFile);

                triggerConfiguration =
                    JsonConvert.DeserializeObject<TriggerConfiguration>(
                        Encoding.UTF8.GetString(triggerConfigurationsByteContent));

                TreeNode treeNodeTrigger = treeNodeTriggers.Nodes.Add(
                    CONST_TRIGGER,
                    triggerConfiguration.Trigger.Name,
                    CONST_TRIGGER_KEY,
                    CONST_TRIGGER_KEY);
                treeNodeTrigger.Tag = triggerConfiguration;

            }


            // EVENTS******************************************************************************
            // Loop in the directory

            string eventsBubblingDirectory = Path.Combine(
                gcPointsFoldersData.FolderName,
                Configuration.DirectoryNameBubbling,
                Configuration.DirectoryNameEvents);

            var regEvents = new Regex(".*");
            var propertyEventsFiles =
                Directory.GetFiles(eventsBubblingDirectory, "*", SearchOption.AllDirectories)
                    .Where(path => regEvents.IsMatch(path))
                    .ToList();

            // For each trigger search for the trigger in event bubbling and set the properties
            foreach (var propertyEventsFile in propertyEventsFiles)
            {
                EventConfiguration eventPropertyBag = null;
                var propertyEventsByteContent = File.ReadAllBytes(propertyEventsFile);
                eventPropertyBag =
                    JsonConvert.DeserializeObject<EventConfiguration>(
                        Encoding.UTF8.GetString(propertyEventsByteContent));

                TreeNode treeNodeEvent = treeNodeEvents.Nodes.Add(
                                            CONST_EVENT,
                                            eventPropertyBag.Event.Name,
                                            CONST_EVENT_KEY,
                                            CONST_EVENT_KEY);
                treeNodeEvent.Tag = eventPropertyBag;

            }
        }
    }
}
