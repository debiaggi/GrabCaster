﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrabCasterUI
{
    using System.Collections.Concurrent;
    using System.Dynamic;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Text.RegularExpressions;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Channels;
    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Contracts.Syncronization;
    using GrabCaster.Framework.Contracts.Triggers;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Engine.OffRamp;
    using GrabCaster.Framework.Engine.OnRamp;
    using GrabCaster.Framework.Serialization.Object;
    using GrabCaster.Framework.Serialization.Xml;

    using Newtonsoft.Json;
    using GrabCaster.Framework.Syncronization;
    /// <summary>
    /// Main console form
    /// Everything using 1 like combobox1 treeeview1 and so on is on the left
    /// everything ending with 2 is on the right
    /// </summary>
    public partial class FormMain : Form
    {
        List<GcPointsFoldersData> gcPointsFoldersDataList = null;
        private TreeView treeViewActive = null;
        public FormMain()
        {
            InitializeComponent();
        }

        #region Variables

        //Constants 
        public const string CONST_CONSOLE = "CONSOLE";
        public const string CONST_CONSOLE_KEY = "CONSOLE";

        public const string CONST_POINT_KEY = "POINT";

        public const string CONST_CONFIGURATION = "CONFIGURATION";
        public const string CONST_CONFIGURATION_KEY = "FOLDER";

        public const string CONST_COMPONENTS = "COMPONENTS";
        public const string CONST_COMPONENTS_KEY = "FOLDER";
        public const string CONST_TRIGGERS = "TRIGGERS";
        public const string CONST_TRIGGERS_KEY = "FOLDER";
        public const string CONST_EVENTS = "EVENTS";
        public const string CONST_EVENTS_KEY = "FOLDER";

        public const string CONST_COMPONENT = "COMPONENT";
        public const string CONST_COMPONENT_KEY = "COMPONENT";
        public const string CONST_TRIGGERCOMPONENT_KEY = "TRIGGERCOMPONENT";
        public const string CONST_EVENTCOMPONENT_KEY = "EVENTCOMPONENT";
        public const string CONST_TRIGGER = "TRIGGER";
        public const string CONST_TRIGGER_KEY = "TRIGGER";
        public const string CONST_TRIGGEROFF_KEY = "TRIGGEROFF";
        public const string CONST_TRIGGERON_KEY = "TRIGGERON";
        public const string CONST_EVENT = "EVENT";
        public const string CONST_EVENT_KEY = "EVENT";
        public const string CONST_EVENTON_KEY = "EVENTON";
        public const string CONST_EVENTOFF_KEY = "EVENTOFF";

        public const string CONST_CORRELATION = "Correlation";
        public const string CONST_CORRELATION_KEY = "CORRELATION";

        private List<GcPointsFoldersData> GcPointsFoldersDataList = null;

        public const string Dictionary_File = "File";
        public const string Dictionary_Object = "Object";

        //Al channels
        private List<Channel> Channels { get; set; }

    

        /// <summary>
        /// The set event action event embedded.
        /// </summary>
        private static MessageIngestor.SetConsoleActionEventEmbedded setConsoleActionEventEmbedded;
        #endregion

        private void InizializeEnvironment()
        {
            //Load Configuration

            

            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView2.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView_DragEnter);
            this.treeView2.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView_DragEnter);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeView2.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
        
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
        private ConfigurationStorage GetConfigurationStorage(string workFolder,ref string configurationFileOverrided)
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
                configurationFileOverrided = configurationFile;
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
            ConcoleMessage("Syncronize all Points.");
            //Ask folder to all the points
            AskPointBag("*", "*", Configuration.PointId(), Configuration.MessageDataProperty.ConsoleRequestSendBubblingBag);
        }


        /// <summary>
        /// Refresh the configuration file list
        /// </summary>
        /// <returns></returns>
        private List<GcPointsFoldersData> RefreshConfigurationStorageList()
        {

            gcPointsFoldersDataList= new List<GcPointsFoldersData>();

            ConcoleMessage("Refresh operation requested...");
            //Get the locals configurations folders
            //Copy all the local point folder configurations in the GCPoints folder
            string directoryOperativeRootExeName = Configuration.ConfigurationStorage.BaseDirectory;
            string[] gcCongigurationFolders = Directory.GetDirectories(directoryOperativeRootExeName);

            ConcoleMessage("Adding local folders...");

            Channels = new List<Channel>();
            List<GrabCaster.Framework.Contracts.Points.Point> PointsAll = new List<GrabCaster.Framework.Contracts.Points.Point>();
            PointsAll.Add(new GrabCaster.Framework.Contracts.Points.Point("*", "All Points", "All Active Points"));
            Channel channelAll = new Channel("*", "All Channels", "All Active Channels", PointsAll);
            Channels.Add(channelAll);


            //Look in all the folder because I can have multiple GC point cloned in the same local folder
            foreach (var item in gcCongigurationFolders)
            {
                //If folder contains bubbling folder then is a config folder
                if (item.Contains("Root_"))
                {
                    ConfigurationStorage _configurationStorage = new ConfigurationStorage();
                    string configFileName = item.Split(Path.DirectorySeparatorChar).Last().Split('_').Last();
                    string configFileNameWithFolder = Path.Combine(
                        Configuration.ConfigurationStorage.BaseDirectory,
                        $"{configFileName}.cfg");
                    _configurationStorage = this.GetConfigurationStorage(configFileNameWithFolder, ref configFileNameWithFolder);
                    gcPointsFoldersDataList.Add(new GcPointsFoldersData(item, configFileNameWithFolder,_configurationStorage));

                    List<GrabCaster.Framework.Contracts.Points.Point> Points = new List<GrabCaster.Framework.Contracts.Points.Point>();
                    Points.Add(new GrabCaster.Framework.Contracts.Points.Point(_configurationStorage.PointId, _configurationStorage.PointName, _configurationStorage.PointDescription));
                    Channel channel = new Channel(_configurationStorage.ChannelId, _configurationStorage.ChannelName, _configurationStorage.ChannelDescription,Points);
                    Channels.Add(channel);
                }

            }


            ConcoleMessage("Adding remote folders...");

            //Get folders in GCPoints
            string gcPointsFolder = Configuration.SyncDirectoryGcPoints();
            string[] gcPointsFolders = Directory.GetDirectories(gcPointsFolder);

            foreach (var item in gcPointsFolders)
            {
                ConfigurationStorage _configurationStorage = new ConfigurationStorage();
                string fileName = null;
                _configurationStorage = this.GetConfigurationStorage(item, ref fileName);
                gcPointsFoldersDataList.Add(new GcPointsFoldersData(item, fileName, _configurationStorage));

                List<GrabCaster.Framework.Contracts.Points.Point> Points = new List<GrabCaster.Framework.Contracts.Points.Point>();
                Points.Add(new GrabCaster.Framework.Contracts.Points.Point(_configurationStorage.PointId, _configurationStorage.PointName, _configurationStorage.PointDescription));
                Channel channel = new Channel(_configurationStorage.ChannelId, _configurationStorage.ChannelName, _configurationStorage.ChannelDescription, Points);
                Channels.Add(channel);

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
            string consoleFolder = gcPointsFoldersData.FolderName.Contains("Root_GrabCasterUI") ? "[CONSOLE] " : string.Empty;
            return $"{consoleFolder} {gcPointsFoldersData.ConfigurationStorage.PointName} - {gcPointsFoldersData.ConfigurationStorage.PointId}";
        }

        #region Events

        //Drag and drop area
        private void treeView_ItemDrag(object sender,System.Windows.Forms.ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Copy);
        }
        private void treeView_DragEnter(object sender,System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        //When treeview receive the ndoe
        private void treeView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNode SourceNode;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                SourceNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

                //Get the tag data
                TreeviewBag SourcetreeviewBag = null;
                if (SourceNode.Tag != null)
                    SourcetreeviewBag = (TreeviewBag)SourceNode.Tag;
                TreeviewBag DestinationTreeviewBag = null;
                if (DestinationNode.Tag != null)
                    DestinationTreeviewBag = (TreeviewBag)DestinationNode.Tag;


                //trig conf> trigger nod
                if (SourcetreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.TriggerConfiguration &&
                    DestinationTreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.TriggerConfigurationRoot)
                {
                    CreateTriggerConfigurationFromNode(DestinationNode, SourceNode);

                }

                //copy trig event into another trig event
                if (SourcetreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.Event &&
                    DestinationTreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.TriggerEventConfigurationRoot)
                {
                    CreateTriggerEventConfigurationFromNode(DestinationNode, SourceNode);

                }


                //evento su evento trigger
                if (SourcetreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.EventConfiguration &&
                    DestinationTreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.TriggerEventConfigurationRoot)
                {
                    CreateTriggerEventConfigurationFromNode(DestinationNode, SourceNode);

                }

                //evento su evento su eventi conf di la
                if (SourcetreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.EventConfiguration &&
                    DestinationTreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.EventConfigurationRoot)
                {
                    CreateEventConfigurationFromNode(DestinationNode, SourceNode);

                }



                //trigger component su nodo triggres component
                if (SourcetreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.TriggerComponent &&
                    DestinationTreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.TriggerComponentRoot)
                {
                    CreateTriggerComponent(DestinationNode, SourceNode);

                }

                ///evento compo to evt roo
                if (SourcetreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.EventComponent &&
                    DestinationTreeviewBag.GrabCasterComponentType ==
                    GrabCasterComponentType.EventComponentRoot)
                {
                    CreateEventComponent(DestinationNode, SourceNode);

                }


            }
}
        //Riceve la configurazione
        private static void EventReceivedFromEmbedded(string DestinationConsolePointId, ISkeletonMessage skeletonMessage)
        {

            //devicercare la folder con linq e metterlo nei gcpoints
            byte[] bubblingContent = SerializationEngine.ObjectToByteArray(skeletonMessage.Body);

            string currentSyncFolder = Configuration.SyncBuildSpecificDirectoryGcPointsIn("");
            GrabCaster.Framework.CompressionLibrary.Helpers.CreateFromBytearray(skeletonMessage.Body, currentSyncFolder);
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
            ConcoleMessage("Syncronize all Points - OffRampEngineSending.SendNullMessageOnRamp.");
            OffRampEngineSending.SendNullMessageOnRamp(messageDataProperty,
                                                        DestinationChannelId,
                                                        DestinationPointId,
                                                        string.Empty,
                                                        string.Empty,
                                                        CurrentPointId);

        }

        private void SendPointBagToSyncronize(byte[] content, string DestinationChannelId, string DestinationPointId, string CurrentPointId, Configuration.MessageDataProperty messageDataProperty)
        {
            OffRampEngineSending.SendMessageOnRamp(content,
                                                        messageDataProperty,
                                                        DestinationChannelId,
                                                        DestinationPointId,
                                                        null,
                                                        string.Empty);

        }



        #endregion



        private void toolStripButtonSyncronize_Click(object sender, EventArgs e)
        {
            if (Global.MessageBoxForm("Send the syncronization request?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
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
            TreeNode treeNodePOINT = null;
            if (gcPointsFoldersData.FolderName.Contains("Root_GrabCasterUI"))
            {
                treeNodePOINT = treeView.Nodes.Add(
                    CONST_CONSOLE_KEY,
                    GetPointName(gcPointsFoldersData),
                    CONST_CONSOLE_KEY,
                    CONST_CONSOLE_KEY);
            }
            else
            {
                treeNodePOINT = treeView.Nodes.Add(
                    CONST_POINT_KEY,
                    GetPointName(gcPointsFoldersData),
                    CONST_POINT_KEY,
                    CONST_POINT_KEY);
            }

            LoadRooTreeViewNode(treeView, treeNodePOINT, gcPointsFoldersData);
        }

        private void LoadRooTreeViewNode(TreeView treeView,TreeNode treeNodePOINT, GcPointsFoldersData gcPointsFoldersData)
        {

            //treeNodePOINT****************************************************************************************************************

            var objRoot = new CustomObjectType();

            objRoot.Properties.Add(new CustomProperty { Category = "Point Information", Name = "Point Name", DefaultValue = gcPointsFoldersData.ConfigurationStorage.PointName, Type = typeof(string), Desc = "GrabCaster Point Name." });
            objRoot.Properties.Add(new CustomProperty { Category = "Point Information", Name = "Point Id", DefaultValue = gcPointsFoldersData.ConfigurationStorage.PointId, Type = typeof(string), Desc = "GrabCaster Point Name." });
            objRoot.Properties.Add(new CustomProperty { Category = "Point Information", Name = "Point Description", DefaultValue = gcPointsFoldersData.ConfigurationStorage.PointDescription, Type = typeof(string), Desc = "GrabCaster Point Name." });
            objRoot.Properties.Add(new CustomProperty { Category = "Point Information", Name = "Channel Name", DefaultValue = gcPointsFoldersData.ConfigurationStorage.ChannelName, Type = typeof(string), Desc = "GrabCaster Point Name." });
            objRoot.Properties.Add(new CustomProperty { Category = "Point Information", Name = "Channel Id", DefaultValue = gcPointsFoldersData.ConfigurationStorage.ChannelId, Type = typeof(string), Desc = "GrabCaster Point Name." });
            objRoot.Properties.Add(new CustomProperty { Category = "Point Information", Name = "Channel Description", DefaultValue = gcPointsFoldersData.ConfigurationStorage.ChannelDescription, Type = typeof(string), Desc = "GrabCaster Point Name." });


            TreeviewBag treeviewBagRoot = new TreeviewBag(gcPointsFoldersData.FolderName,
                                                         GrabCasterComponentType.Root,
                                                         gcPointsFoldersData,
                                                         "",
                                                        objRoot, null, null);
            treeNodePOINT.Tag = treeviewBagRoot;
            //****************************************************************************************************************




            //treeNodeBubbling****************************************************************************************************************
            TreeNode treeNodeBubbling = treeNodePOINT.Nodes.Add(
                CONST_CONFIGURATION,
                CONST_CONFIGURATION,
                CONST_CONFIGURATION_KEY,
                CONST_CONFIGURATION_KEY);
            //****************************************************************************************************************


            //treeNodeTriggers****************************************************************************************************************
            TreeNode treeNodeTriggers = treeNodeBubbling.Nodes.Add(
                CONST_TRIGGERS,
                CONST_TRIGGERS,
                CONST_TRIGGERS_KEY,
                CONST_TRIGGERS_KEY);

            TreeviewBag treeviewBagtreeNodeTriggers = new TreeviewBag("Triggers Configuration Group.",
                                                    GrabCasterComponentType.TriggerConfigurationRoot,
                                                    "",
                                                    "",
                                                    null, null, null);
            treeNodeTriggers.Tag = treeviewBagtreeNodeTriggers;
            //****************************************************************************************************************



            //treeNodeEvents****************************************************************************************************************
            TreeNode treeNodeEvents = treeNodeBubbling.Nodes.Add(
                CONST_EVENTS,
                CONST_EVENTS,
                CONST_EVENTS_KEY,
                CONST_EVENTS_KEY);

            TreeviewBag treeviewBagtreeNodeEvents = new TreeviewBag("Events Configuration Group.",
                                        GrabCasterComponentType.EventConfigurationRoot,
                                        "",
                                        "",
                                        null, null, null);
            treeNodeEvents.Tag = treeviewBagtreeNodeEvents;
            //****************************************************************************************************************





            //****************************************************************************************************************
            TreeNode treeNodeComponents = treeNodePOINT.Nodes.Add(
                CONST_COMPONENTS,
                CONST_COMPONENTS,
                CONST_COMPONENTS_KEY,
                CONST_COMPONENTS_KEY);
            //****************************************************************************************************************




            //****************************************************************************************************************
            TreeNode treeNodeTriggersComponents = treeNodeComponents.Nodes.Add(
                CONST_TRIGGERS,
                CONST_TRIGGERS,
                CONST_TRIGGERS_KEY,
                CONST_TRIGGERS_KEY);

            TreeviewBag treeviewBagtreeNodeTriggersComponents = new TreeviewBag("Component Triggers Group.",
                GrabCasterComponentType.TriggerComponentRoot,
                "",
                "",
                null, null, null);
            treeNodeTriggersComponents.Tag = treeviewBagtreeNodeTriggersComponents;
            //****************************************************************************************************************




            //****************************************************************************************************************
            TreeNode treeNodeEventsComponents = treeNodeComponents.Nodes.Add(
                CONST_EVENTS,
                CONST_EVENTS,
                CONST_EVENTS_KEY,
                CONST_EVENTS_KEY);

            TreeviewBag treeviewBagtreeNodeEventsComponents = new TreeviewBag("Component Events Group.",
                            GrabCasterComponentType.EventComponentRoot,
                            "",
                            "",
                            null, null, null);
            treeNodeEventsComponents.Tag = treeviewBagtreeNodeEventsComponents;
            //****************************************************************************************************************



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
                CreateTriggerConfigurationNode(treeviewBagRoot, treeNodeTriggers, triggerConfigurationsFile);
            }


            // EVENTS******************************************************************************
            // Loop in the directory

            string eventsBubblingDirectory = Path.Combine(gcPointsFoldersData.FolderName,
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
                CreateEventConfigurationNode(treeviewBagRoot, treeNodeEvents, propertyEventsFile);
            }

            //***********************************************************************************
            //Load components


            // Load triggers bubbling path

            var triggersDirectory = Path.Combine(gcPointsFoldersData.FolderName, Configuration.DirectoryNameTriggers);
            var assemblyFilesTriggers =
                Directory.GetFiles(triggersDirectory, Configuration.TriggersDllExtensionLookFor)
                    .Where(path => regTriggers.IsMatch(path))
                    .ToList();

            // Load event bubbling path
            var eventsDirectory = Path.Combine(gcPointsFoldersData.FolderName, Configuration.DirectoryNameEvents);
            var assemblyFilesEvents =
                Directory.GetFiles(eventsDirectory, Configuration.EventsDllExtensionLookFor)
                    .Where(path => regEvents.IsMatch(path))
                    .ToList();


            // ****************************************************
            // Load Triggers
            // ****************************************************
            foreach (var assemblyFile in assemblyFilesTriggers)
            {
                var assembly = Assembly.LoadFrom(assemblyFile);
                var assemblyClasses = from t in assembly.GetTypes()
                                      let attributes = t.GetCustomAttributes(typeof(TriggerContract), false)
                                      where t.IsClass && attributes != null && attributes.Length > 0
                                      select t;
                foreach (var assemblyClass in assemblyClasses)
                {
                    var classAttributes = assemblyClass.GetCustomAttributes(typeof(TriggerContract), true);
                    if (classAttributes.Length > 0)
                    {

                        var triggerContract = (TriggerContract)classAttributes[0];
                        TreeNode treeNodeTriggersComponent = treeNodeTriggersComponents.Nodes.Add(
                                        CONST_TRIGGERCOMPONENT_KEY,
                                        triggerContract.Name,
                                        CONST_TRIGGERCOMPONENT_KEY,
                                        CONST_TRIGGERCOMPONENT_KEY);

                        componentTrigger componentTrigger = new componentTrigger(
                            triggerContract,
                            assemblyClass,
                            assemblyFile,
                            assembly);
                        TreeviewBag treeviewBag = new TreeviewBag(assemblyFile,
                                                                    GrabCasterComponentType.TriggerComponent,
                                                                    triggerContract,
                                                                    assemblyClass.GetProperties(),
                                                                    assemblyClasses, componentTrigger, null);
                        treeviewBagRoot.componentTriggerList.Add(componentTrigger);
                        treeNodeTriggersComponent.Tag = treeviewBag;

                    }
                }


            }
            // ****************************************************
            // Load Events
            // ****************************************************
            foreach (var assemblyFile in assemblyFilesEvents)
            {
                // Get all classes with Attribute = Event
                var assembly = Assembly.LoadFrom(assemblyFile);
                var assemblyClasses = from t in assembly.GetTypes()
                                      let attributes = t.GetCustomAttributes(typeof(EventContract), false)
                                      where t.IsClass && attributes != null && attributes.Length > 0
                                      select t;

                foreach (var assemblyClass in assemblyClasses)
                {
                    var bubblingEvent = new BubblingEvent();
                    var classAttributes = assemblyClass.GetCustomAttributes(typeof(EventContract), true);
                    if (classAttributes.Length > 0)
                    {

                        var eventContract = (EventContract)classAttributes[0];
                        TreeNode treeNodeEventsComponent = treeNodeEventsComponents.Nodes.Add(CONST_EVENTCOMPONENT_KEY,
                                                                                                eventContract.Name,
                                                                                                CONST_EVENTCOMPONENT_KEY,
                                                                                                CONST_EVENTCOMPONENT_KEY);
                        var objEvent = new CustomObjectType();
                        objEvent.Properties.Clear();
                        objEvent.Properties.Add(new CustomProperty { Category = "Component Information", Name = "Type", DefaultValue = "Event", Type = typeof(string), Desc = "Component object type [Trigger or Event]." });
                        objEvent.Properties.Add(new CustomProperty { Category = "Component Information", Name = "Assembly", DefaultValue = assemblyFile, Type = typeof(string), Desc = "Assembly file path." });
                        objEvent.Properties.Add(new CustomProperty { Category = "Component Information", Name = "Name", DefaultValue = eventContract.Name, Type = typeof(string), Desc = "Component object name." });
                        objEvent.Properties.Add(new CustomProperty { Category = "Component Information", Name = "Description", DefaultValue = eventContract.Description, Type = typeof(string), Desc = "Component object description." });
                        objEvent.Properties.Add(new CustomProperty { Category = "Component Information", Name = "ComponentId", DefaultValue = eventContract.Id, Type = typeof(string), Desc = "Component uniquque identifier." });

                        foreach (var propertyInfo in assemblyClass.GetProperties())
                        {
                            var propertyAttributes = propertyInfo.GetCustomAttributes(
                                typeof(EventPropertyContract),
                                true);
                            if (propertyAttributes.Length > 0)
                            {
                                var propertyAttribute = (EventPropertyContract)propertyAttributes[0];
                                if (propertyInfo.Name != propertyAttribute.Name)
                                {
                                    throw new Exception(
                                        $"Critical error! the properies {propertyAttributes[0]} and {propertyInfo.Name} are different! Class name {assemblyClass.Name}");
                                }

                                if (propertyAttribute.Name != "DataContext")
                                    objEvent.Properties.Add(new CustomProperty { Category = "Properties", Name = propertyAttribute.Name, DefaultValue = propertyInfo.PropertyType.Name, Type = typeof(string), Desc = propertyAttribute.Description });

                            }
                        }

                        componentEvent componentEvent = new componentEvent(eventContract,
                                                                            assemblyClass,
                                                                            assemblyFile,
                                                                            assembly);
                        TreeviewBag treeviewBag = new TreeviewBag(assemblyFile,
                                                                     GrabCasterComponentType.EventComponent,
                                                                     eventContract,
                                                                     assemblyClass.GetProperties(),
                                                                    objEvent,
                                                                    null,
                                                                    componentEvent);
                        treeviewBagRoot.componentEventList.Add(componentEvent);
                        treeNodeEventsComponent.Tag = treeviewBag;


                    }
                }
            }
        }
        public void CreateEventConfigurationNode(
            TreeviewBag treeviewBagRoot,
            TreeNode treeNodeEvents,
            string propertyEventsFile)
        {
            EventConfiguration eventPropertyBag = null;
            var propertyEventsByteContent = File.ReadAllBytes(propertyEventsFile);
            eventPropertyBag =
                JsonConvert.DeserializeObject<EventConfiguration>(
                    Encoding.UTF8.GetString(propertyEventsByteContent));

            TreeNode treeNodeEvent = treeNodeEvents.Nodes.Add(
                                        CONST_EVENT,
                                        eventPropertyBag.Event.Name,
                                        IsTriggerEventActive(propertyEventsFile, BubblingEventType.Event) ? CONST_EVENTON_KEY : CONST_EVENTOFF_KEY,
                                        IsTriggerEventActive(propertyEventsFile, BubblingEventType.Event) ? CONST_EVENTON_KEY : CONST_EVENTOFF_KEY);

            //Add event

            var objEventConfiguration = new CustomObjectType();
            objEventConfiguration.Properties.Clear();

            objEventConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "Name", DefaultValue = eventPropertyBag.Event.Name, Type = typeof(string), Desc = "Specify the trigger name." });
            objEventConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "Description", DefaultValue = eventPropertyBag.Event.Description, Type = typeof(string), Desc = "Specify the trigger description." });
            objEventConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "IdComponent", DefaultValue = eventPropertyBag.Event.IdComponent, Type = typeof(string), Desc = "Specify the Id Componet to use." });
            objEventConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "IdConfiguration", DefaultValue = eventPropertyBag.Event.IdConfiguration, Type = typeof(string), Desc = "Specify the Id group Configuration to use." });
            objEventConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "Channels", DefaultValue = eventPropertyBag.Event.Channels, Type = typeof(List<>), Desc = "Specify the Channels to use." });

            if (eventPropertyBag.Event.EventProperties != null)
            {
                foreach (var _item in eventPropertyBag.Event.EventProperties)
                {
                    objEventConfiguration.Properties.Add(new CustomProperty { Category = "Properties", Name = _item.Name, DefaultValue = _item.Value, Type = typeof(string), Desc = "Event property." });
                }

            }


            TreeviewBag treeviewBag = new TreeviewBag(propertyEventsFile,
                                         GrabCasterComponentType.EventConfiguration,
                                         eventPropertyBag,
                                         eventPropertyBag.Event.EventProperties,
                                        objEventConfiguration,null,null);
            treeviewBagRoot.eventConfigurationList.Add(eventPropertyBag);
            treeNodeEvent.Tag = treeviewBag;

            if (eventPropertyBag.Event.Correlation != null)
                CheckCorrelation(propertyEventsFile, treeNodeEvent, eventPropertyBag.Event);

        }
        public void CreateTriggerConfigurationNode(TreeviewBag treeviewBagRoot,TreeNode treeNodeTriggers, string triggerConfigurationsFile)
        {
            TriggerConfiguration triggerConfiguration = null;
            var triggerConfigurationsByteContent = File.ReadAllBytes(triggerConfigurationsFile);

            triggerConfiguration =
                JsonConvert.DeserializeObject<TriggerConfiguration>(
                    Encoding.UTF8.GetString(triggerConfigurationsByteContent));

            TreeNode treeNodeTrigger = treeNodeTriggers.Nodes.Add(
                CONST_TRIGGER,
                triggerConfiguration.Trigger.Name,
                IsTriggerEventActive(triggerConfigurationsFile, BubblingEventType.Trigger) ? CONST_TRIGGERON_KEY : CONST_TRIGGEROFF_KEY,
                IsTriggerEventActive(triggerConfigurationsFile, BubblingEventType.Trigger) ? CONST_TRIGGERON_KEY : CONST_TRIGGEROFF_KEY);

            //Add trigger node
            var objTriigerConfiguration = new CustomObjectType();
            objTriigerConfiguration.Properties.Clear();

            objTriigerConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "Name", DefaultValue = triggerConfiguration.Trigger.Name, Type = typeof(string), Desc = "Specify the trigger name." });
            objTriigerConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "Description", DefaultValue = triggerConfiguration.Trigger.Description, Type = typeof(string), Desc = "Specify the trigger description." });
            objTriigerConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "IdComponent", DefaultValue = triggerConfiguration.Trigger.IdComponent, Type = typeof(string), Desc = "Specify the Id Componet to use." });
            objTriigerConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "IdConfiguration", DefaultValue = triggerConfiguration.Trigger.IdConfiguration, Type = typeof(string), Desc = "Specify the Id group Configuration to use." });

            if (triggerConfiguration.Trigger.TriggerProperties != null)
            {
                foreach (var item in triggerConfiguration.Trigger.TriggerProperties)
                {
                    objTriigerConfiguration.Properties.Add(new CustomProperty { Category = "Properties", Name = item.Name, DefaultValue = item.Value, Type = typeof(string), Desc = "Trigger property." });
                }
            }


            TreeviewBag treeviewBagTrigger = new TreeviewBag(triggerConfigurationsFile,
                                                                GrabCasterComponentType.TriggerConfiguration,
                                                                triggerConfiguration,
                                                                triggerConfiguration.Trigger.TriggerProperties,
                                                                objTriigerConfiguration,null,null);

            treeviewBagRoot.triggerConfigurationList.Add(triggerConfiguration);

            treeNodeTrigger.Tag = treeviewBagTrigger;

            //Check if event is created
            if (triggerConfiguration.Events.Count != 0)
            {

                TreeviewBag treeviewBagtreeNodeEvents = new TreeviewBag("Events Configuration Group.",
                                            GrabCasterComponentType.TriggerEventConfigurationRoot,
                                            "",
                                            "",
                                            null, null, null);

                TreeNode treeNodeEventsInTrigger = treeNodeTrigger.Nodes.Add(CONST_EVENTS,
                                                                    CONST_EVENTS,
                                                                    CONST_EVENTS_KEY,
                                                                    CONST_EVENTS_KEY);
                treeNodeEventsInTrigger.Tag = treeviewBagtreeNodeEvents;
                foreach (var item in triggerConfiguration.Events)
                {

                    TreeNode treeNodeEvent = treeNodeEventsInTrigger.Nodes.Add(
                                                CONST_EVENT,
                                                item.Name,
                                                CONST_EVENT_KEY,
                                                CONST_EVENT_KEY);
                    //Add event node

                    var objTriigerEventConfiguration = new CustomObjectType();
                    objTriigerEventConfiguration.Properties.Clear();

                    objTriigerEventConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "Name", DefaultValue = item.Name, Type = typeof(string), Desc = "Specify the trigger name." });
                    objTriigerEventConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "Description", DefaultValue = item.Description, Type = typeof(string), Desc = "Specify the trigger description." });
                    objTriigerEventConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "IdComponent", DefaultValue = item.IdComponent, Type = typeof(string), Desc = "Specify the Id Componet to use." });
                    objTriigerEventConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "IdConfiguration", DefaultValue = item.IdConfiguration, Type = typeof(string), Desc = "Specify the Id group Configuration to use." });
                    objTriigerEventConfiguration.Properties.Add(new CustomProperty { Category = "Main Properties", Name = "Channels", DefaultValue = item.Channels, Type = typeof(List<>), Desc = "Specify the Channels to use." });

                    if (item.EventProperties != null)
                    {
                        foreach (var _item in item.EventProperties)
                        {
                            objTriigerConfiguration.Properties.Add(new CustomProperty { Category = "Properties", Name = _item.Name, DefaultValue = _item.Value, Type = typeof(string), Desc = "Event property." });
                        }
                    }
                    TreeviewBag treeviewBag = new TreeviewBag(triggerConfigurationsFile,
                                                 GrabCasterComponentType.Event,
                                                 item,
                                                 item.EventProperties,
                                                objTriigerEventConfiguration, null, null);
                    treeNodeEvent.Tag = treeviewBag;
                    if (item.Correlation != null)
                        CheckCorrelation(triggerConfigurationsFile, treeNodeEvent, item);

                }
            }

        }
        private void CheckCorrelation(string fileName, TreeNode treeNodeEvent, Event eventCorrelation)
        {
            if (eventCorrelation.Correlation == null) return;
            TreeNode treeNodeCorrelation = treeNodeEvent.Nodes.Add(
                CONST_CORRELATION,
                CONST_CORRELATION,
                CONST_CORRELATION_KEY,
                CONST_CORRELATION_KEY);

            var jsonSerialization = JsonConvert.SerializeObject(eventCorrelation.Correlation, Formatting.Indented);

            TreeviewBag treeviewBagC = new TreeviewBag(fileName,
                                              GrabCasterComponentType.Correlation,
                                              eventCorrelation,
                                              eventCorrelation.EventProperties, jsonSerialization, null,null);

            treeNodeCorrelation.Tag = treeviewBagC;

            
        }

        private bool IsTriggerEventActive(string ConfigurationFile,BubblingEventType bubblingEventType)
        {
            bool ret = false;
            try
            {
                if (bubblingEventType == BubblingEventType.Trigger)
                {
                    ret = Path.GetExtension(ConfigurationFile).ToLower() == Configuration.BubblingTriggersExtension.ToLower();
                }
                if (bubblingEventType == BubblingEventType.Event)
                {
                    ret = Path.GetExtension(ConfigurationFile).ToLower() == Configuration.BubblingEventsExtension.ToLower();
                }
            }
            catch (Exception ex)
            {

                ConcoleMessage($"Error! {ex.Message}");


            }

            return ret;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_SelectedIndexChanged((ComboBox)sender);
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        private void treeView_AfterSelect(TreeView treeView, Panel panelUCContainer)
        {

            if (treeView.SelectedNode?.Tag != null)
            {
                TreeviewBag treeviewBag = (TreeviewBag)treeView.SelectedNode.Tag;
                this.toolStripStatusLabelMessage.Text = treeviewBag.File;

                switch (treeviewBag.GrabCasterComponentType)
                {
                    case GrabCasterComponentType.TriggerConfiguration:
                        UserControlComponentTriggerConfiguration userControlComponentConfigurationTrg = new UserControlComponentTriggerConfiguration();
                        userControlComponentConfigurationTrg.Visible = false;
                        userControlComponentConfigurationTrg.LoadComponentData(treeviewBag);
                        panelUCContainer.Controls.Clear();
                        panelUCContainer.BackColor = SystemColors.Control;
                        userControlComponentConfigurationTrg.Dock = DockStyle.Fill;
                        userControlComponentConfigurationTrg.TreeViewSide = treeView;
                        userControlComponentConfigurationTrg.TreeNodeSide = treeView.SelectedNode;
                        panelUCContainer.Controls.Add(userControlComponentConfigurationTrg);
                        userControlComponentConfigurationTrg.Visible = true;
                        break;
                    case GrabCasterComponentType.Event:
                        UserControlComponentEventTrigger userControlComponentConfigurationEvtTrg = new UserControlComponentEventTrigger();
                        userControlComponentConfigurationEvtTrg.LoadComponentData(treeviewBag);
                        panelUCContainer.Controls.Clear();
                        panelUCContainer.BackColor = SystemColors.Control;
                        userControlComponentConfigurationEvtTrg.Dock = DockStyle.Fill;
                        userControlComponentConfigurationEvtTrg.TreeViewSide = treeView;
                        userControlComponentConfigurationEvtTrg.TreeNodeSide = treeView.SelectedNode;
                        userControlComponentConfigurationEvtTrg.ChannelsIn = Channels;
                        panelUCContainer.Controls.Add(userControlComponentConfigurationEvtTrg);
                        userControlComponentConfigurationEvtTrg.Visible = true;
                        break;
                    case GrabCasterComponentType.EventConfiguration:
                        UserControlComponentEventConfiguration userControlComponentConfigurationEvt = new UserControlComponentEventConfiguration();
                        userControlComponentConfigurationEvt.LoadComponentData(treeviewBag);
                        panelUCContainer.Controls.Clear();
                        panelUCContainer.BackColor = SystemColors.Control;
                        userControlComponentConfigurationEvt.Dock = DockStyle.Fill;
                        userControlComponentConfigurationEvt.TreeViewSide = treeView;
                        userControlComponentConfigurationEvt.TreeNodeSide = treeView.SelectedNode;
                        panelUCContainer.Controls.Add(userControlComponentConfigurationEvt);
                        userControlComponentConfigurationEvt.Visible = true;
                        break;
                    case GrabCasterComponentType.TriggerComponent:
                        UserControlComponent userControlComponentTrg = new UserControlComponent();
                        userControlComponentTrg.LoadComponentData(treeviewBag);
                        panelUCContainer.Controls.Clear();
                        panelUCContainer.BackColor = SystemColors.Control;
                        userControlComponentTrg.Dock = DockStyle.Fill;
                        userControlComponentTrg.TreeViewSide = treeView;
                        userControlComponentTrg.TreeNodeSide = treeView.SelectedNode;
                        panelUCContainer.Controls.Add(userControlComponentTrg);
                        userControlComponentTrg.Visible = true;
                        break;
                    case GrabCasterComponentType.EventComponent:
                        UserControlComponent userControlComponentEvt = new UserControlComponent();
                        userControlComponentEvt.LoadComponentData(treeviewBag);
                        panelUCContainer.Controls.Clear();
                        panelUCContainer.BackColor = SystemColors.Control;
                        userControlComponentEvt.Dock = DockStyle.Fill;
                        userControlComponentEvt.TreeViewSide = treeView;
                        userControlComponentEvt.TreeNodeSide = treeView.SelectedNode;
                        panelUCContainer.Controls.Add(userControlComponentEvt);
                        userControlComponentEvt.Visible = true;
                        break;
                    case GrabCasterComponentType.Correlation:
                        UserControlComponent userControlComponentCorrelation = new UserControlComponent();
                        userControlComponentCorrelation.LoadComponentData(treeviewBag);
                        panelUCContainer.Controls.Clear();
                        panelUCContainer.BackColor = SystemColors.Control;
                        userControlComponentCorrelation.Dock = DockStyle.Fill;
                        userControlComponentCorrelation.TreeViewSide = treeView;
                        userControlComponentCorrelation.TreeNodeSide = treeView.SelectedNode;
                        panelUCContainer.Controls.Add(userControlComponentCorrelation);
                        userControlComponentCorrelation.Visible = true;
                        break;
                    case GrabCasterComponentType.Root:
                        UserControlConfiguration userControlConfiguration = new UserControlConfiguration();
                        userControlConfiguration.LoadComponentData(treeviewBag);
                        panelUCContainer.Controls.Clear();
                        panelUCContainer.BackColor = SystemColors.Control;
                        userControlConfiguration.Dock = DockStyle.Fill;
                        userControlConfiguration.TreeViewSide = treeView;
                        userControlConfiguration.TreeNodeSide = treeView.SelectedNode;
                        panelUCContainer.Controls.Add(userControlConfiguration);
                        userControlConfiguration.Visible = true;
                        break;
                    default:
                        break;
                }


            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeView_AfterSelect(treeView1, this.panelUCContainer1);
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeView_AfterSelect(treeView2, this.panelUCContainer2);
        }


        private void toolStripStatusLabelMessage_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(toolStripStatusLabelMessage.Text);
            Global.MessageBoxForm("Content copied in the clipboard.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveJob()
        {
            var TriggersToUpdate = from trg in treeViewActive.Nodes.OfType<TreeNode>() where ((TreeviewBag)trg.Tag).GrabCasterComponentType == GrabCasterComponentType.TriggerComponent select trg.Tag;

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private TreeNode treeNodeCurrent = null;

        private void treeView_NodeMouseClick(TreeView treeView, TreeNodeMouseClickEventArgs e)
        {
            if (treeView.SelectedNode?.Tag != null && e.Button == MouseButtons.Right)
            {
                treeView.SelectedNode = e.Node;
                treeNodeCurrent = treeView.SelectedNode;
                TreeviewBag treeviewBag = (TreeviewBag)treeNodeCurrent.Tag;
                this.toolStripStatusLabelMessage.Text = treeviewBag.File;

                switch (treeviewBag.GrabCasterComponentType)
                {
                    case GrabCasterComponentType.TriggerConfiguration:
                        contextMenuStripTriggerConfiguration.Items[3].Text = TreeNodeTextEnableDisable(treeNodeCurrent);
                        treeView.SelectedNode.ContextMenuStrip = this.contextMenuStripTriggerConfiguration;
                        break;
                    case GrabCasterComponentType.EventConfiguration:
                        contextMenuStripEventConfiguration.Items[2].Text = TreeNodeTextEnableDisable(treeNodeCurrent);
                        contextMenuStripEventConfiguration.Items[1].Visible = true;
                        contextMenuStripEventConfiguration.Items[2].Visible = true;
                        treeView.SelectedNode.ContextMenuStrip = this.contextMenuStripEventConfiguration;
                        break;
                    case GrabCasterComponentType.Event:
                        contextMenuStripEventConfiguration.Items[1].Visible = false;
                        contextMenuStripEventConfiguration.Items[2].Visible = false;
                        treeView.SelectedNode.ContextMenuStrip = this.contextMenuStripEventConfiguration;
                        break;
                    case GrabCasterComponentType.TriggerComponent:
                        treeView.SelectedNode.ContextMenuStrip = this.contextMenuStripTriggerComponent;
                        break;
                    case GrabCasterComponentType.EventComponent:
                        treeView.SelectedNode.ContextMenuStrip = this.contextMenuStripEventComponent;
                        break;
                    case GrabCasterComponentType.Correlation:
                        treeView.SelectedNode.ContextMenuStrip = this.contextMenuStripCorrelation;
                        break;
                    case GrabCasterComponentType.Root:
                        treeView.SelectedNode.ContextMenuStrip = this.contextMenuStripRoot;

                        break;
                    default:
                        break;
                }


            }
        }


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeViewActive = treeView1;
            treeView_NodeMouseClick(treeView1,e);
        }

        private void contextMenuStripTriggerComponent_Opening(object sender, CancelEventArgs e)
        {

        }
        public void CreateSyncronizationFile(TreeNode treeNodeCurrent,string pathOverride)
        {
            TreeNode treeNodeRoot = GetRootNode(treeViewActive.SelectedNode);
            TreeviewBag treeviewBagRoot = (TreeviewBag)treeNodeRoot.Tag;
            TreeviewBag treeviewBagCurrent = (TreeviewBag)treeNodeCurrent.Tag;

            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBagRoot.Component;
            
            string fileName = Path.Combine(pathOverride.Length==0? gcPointsFoldersData.FolderName : pathOverride, GrabCaster.Framework.Syncronization.Helpers.syncFile);
            File.WriteAllText(fileName, DateTime.UtcNow.ToString());
        }

        private void toolStripMenuItemNew_Click(object sender, EventArgs e)
        {
            FormTextInput formTextInput = new FormTextInput();
            DialogResult dialogResult = formTextInput.ShowDialog();
            if (dialogResult != DialogResult.Yes)
                return;

            //Filename
            string fileName = formTextInput.textBox1.Text;
            
            //Prepare the bags
            TreeNode treeNodeCurrent = treeViewActive.SelectedNode;
            TreeNode treeNodeRoot = GetRootNode(treeViewActive.SelectedNode);
            TreeviewBag treeviewBagRoot = (TreeviewBag)treeNodeRoot.Tag;
            TreeviewBag treeviewBagCurrent = (TreeviewBag)treeNodeCurrent.Tag;


            //Get the bubbling event
            BubblingEvent bubblingEvent = EventsEngine.CreateBubblingTrigger(treeviewBagCurrent.componentTrigger.triggerClass,
                                                                                treeviewBagCurrent.componentTrigger.assembly,
                                                                                treeviewBagCurrent.File);

            var jsonSerialization = SerializationHelper.CreteJsonTriggerConfigurationTemplate(bubblingEvent);
            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBagRoot.Component;

            string DirectoryBubblingTriggers = Path.Combine(gcPointsFoldersData.FolderName,
                                                            Configuration.DirectoryNameBubbling,
                                                            Configuration.DirectoryNameTriggers);

            string triggerConfigurationsFile = Path.Combine(DirectoryBubblingTriggers, fileName + ".off");

            File.WriteAllText(triggerConfigurationsFile, jsonSerialization);
 
            TreeNode treeNodeTriggers = treeNodeRoot.Nodes[0].Nodes[0];

            CreateTriggerConfigurationNode(treeviewBagRoot, treeNodeTriggers, triggerConfigurationsFile);
            CreateSyncronizationFile(treeNodeCurrent, gcPointsFoldersData.FolderName);



        }

        
        private void contextMenuStripTriggerConfigurationNew_Click(object sender, EventArgs e)
        {
 
        }
        private TreeNode GetRootNode(TreeNode node)
        {

            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }

        private void contextMenuStripEventComponentNew_Click(object sender, EventArgs e)
        {
            FormTextInput formTextInput = new FormTextInput();
            DialogResult dialogResult = formTextInput.ShowDialog();
            if (dialogResult != DialogResult.Yes)
                return;

            //Filename
            string fileName = formTextInput.textBox1.Text;

            //Prepare the bags
            TreeNode treeNodeCurrent = treeViewActive.SelectedNode;
            TreeNode treeNodeRoot = GetRootNode(treeViewActive.SelectedNode);
            TreeviewBag treeviewBagRoot = (TreeviewBag)treeNodeRoot.Tag;
            TreeviewBag treeviewBagCurrent = (TreeviewBag)treeNodeCurrent.Tag;


            //Get the bubbling event
            BubblingEvent bubblingEvent = EventsEngine.CreateBubblingEvent(treeviewBagCurrent.componentEvent.eventClass,
                                                                                treeviewBagCurrent.componentEvent.assembly,
                                                                                treeviewBagCurrent.File);

            var jsonSerialization = SerializationHelper.CreteJsonEventConfigurationTemplate(bubblingEvent);
            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBagRoot.Component;

            string DirectoryBubblingEvents = Path.Combine(gcPointsFoldersData.FolderName,
                                                            Configuration.DirectoryNameBubbling,
                                                            Configuration.DirectoryNameEvents);

            string eventConfigurationsFile = Path.Combine(DirectoryBubblingEvents, fileName + ".off");

            File.WriteAllText(eventConfigurationsFile, jsonSerialization);

            TreeNode treeNodeEvents = treeNodeRoot.Nodes[0].Nodes[1];

            this.CreateEventConfigurationNode(treeviewBagRoot, treeNodeEvents, eventConfigurationsFile);
            CreateSyncronizationFile(treeNodeCurrent, gcPointsFoldersData.FolderName);
        }

        private void contextMenuStripTriggerComponentDelete_Click(object sender, EventArgs e)
        {
            if (Global.MessageBoxForm("Delete the Trigger?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.No) return;
            DeleteTreeNodeComponent(treeViewActive.SelectedNode);
        }

        private void DeleteTreeNodeComponent(TreeNode treeNodeCurrent)
        {
            try
            {
                TreeviewBag treeviewBagCurrent = (TreeviewBag)treeNodeCurrent.Tag;

                if (treeviewBagCurrent.GrabCasterComponentType == GrabCasterComponentType.Event)
                {
                    Event eventTrg = (Event)treeviewBagCurrent.Component;
                    TriggerConfiguration triggerConfiguration = JsonConvert.DeserializeObject<TriggerConfiguration>(File.ReadAllText(treeviewBagCurrent.File));
                    triggerConfiguration.Events.RemoveAll(
                        x => x.IdComponent == eventTrg.IdComponent && x.IdConfiguration == eventTrg.IdConfiguration);
                    string trgSerialized = JsonConvert.SerializeObject(triggerConfiguration);
                    File.WriteAllText(treeviewBagCurrent.File, trgSerialized);
                    treeViewActive.SelectedNode.Remove();
                }
                else
                {
                    File.Delete(treeviewBagCurrent.File);
                    treeViewActive.SelectedNode.Remove();
                }

            CreateSyncronizationFile(treeNodeCurrent, "");
            }

            catch (Exception ex)
            {
                Global.MessageBoxForm($"Error {ex.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void contextMenuStripEventComponentDelete_Click(object sender, EventArgs e)
        {
            if (Global.MessageBoxForm("Delete the Event?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                            DialogResult.No) return;
            DeleteTreeNodeComponent(treeViewActive.SelectedNode);
        }

        private void contextMenuStripTriggerConfigurationDelete_Click(object sender, EventArgs e)
        {
            if (Global.MessageBoxForm("Delete the Trigger?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                                            DialogResult.No) return;
            DeleteTreeNodeComponent(treeViewActive.SelectedNode);
        }

        private void contextMenuStripEventConfigurationDelete_Click(object sender, EventArgs e)
        {
            DeleteTreeNodeComponent(treeViewActive.SelectedNode);
        }

        private void toolStripMenuItemCorrelationDelete_Click(object sender, EventArgs e)
        {
            if (Global.MessageBoxForm("Delete the Correlation?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                                        DialogResult.No) return;
            DeleteTreeNodeComponent(treeViewActive.SelectedNode);
        }

        private void toolStripMenuItemRootRemove_Click(object sender, EventArgs e)
        {
            treeViewActive.SelectedNode.Remove();
        }

        private void toolStripMenuItemRootRefresh_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = treeViewActive.SelectedNode;
            while (treeNode.Nodes.Count != 0)
            {
                treeNode.Nodes[0].Remove();
            }
            TreeviewBag treeviewBag = (TreeviewBag)treeNode.Tag;
            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBag.Component;
            LoadRooTreeViewNode(treeViewActive, treeNode, gcPointsFoldersData);
            treeNode.Expand();
        }

        private void expandAllToolStripMenuItemRootExpandall_Click(object sender, EventArgs e)
        {
            treeViewActive.SelectedNode.ExpandAll();
        }

        private void contextMenuStripEventComponent_Opening(object sender, CancelEventArgs e)
        {

        }

        private void contextMenuStripEventConfigurationDelete_Click_1(object sender, EventArgs e)
        {
            if (Global.MessageBoxForm("Delete the Event from trigger?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                                    DialogResult.No) return;
            DeleteTreeNodeComponent(treeViewActive.SelectedNode);
        }

        private void contextMenuStripTriggerConfigurationSend_Click(object sender, EventArgs e)
        {
            TreeviewBag treeviewBag = (TreeviewBag)treeViewActive.SelectedNode.Tag;
            CopyAndSendFileComponent(treeviewBag.GrabCasterComponentType);
        }

        private void contextMenuStripTriggerComponentSend_Click(object sender, EventArgs e)
        {
            TreeviewBag treeviewBag = (TreeviewBag)treeViewActive.SelectedNode.Tag;
            CopyAndSendFileComponent(treeviewBag.GrabCasterComponentType);
        }
        private void CopyAndSendFileComponent(GrabCasterComponentType grabCasterComponentType)
        {
            TreeNode treeNodeCurrent = treeViewActive.SelectedNode;
            TreeviewBag treeviewBagCurrent = (TreeviewBag)treeNodeCurrent.Tag;

            FormGCPointsList FormGCPointsList = new FormGCPointsList();
            FormGCPointsList.LoadList(gcPointsFoldersDataList);

            string DirectoryBubblingFileSelected = string.Empty;
            string message = string.Empty;

            if (FormGCPointsList.ShowDialog() == DialogResult.OK)
            {
                GcPointsFoldersData gcPointsFoldersData = FormGCPointsList.gcPointsFoldersDataSelected;

                switch (grabCasterComponentType)
                {
                    case GrabCasterComponentType.TriggerConfiguration:
                        DirectoryBubblingFileSelected = Path.Combine(gcPointsFoldersData.FolderName,
                                                        Configuration.DirectoryNameBubbling,
                                                        Configuration.DirectoryNameTriggers);
                        message ="Trigger configuration file assigned, syncronize the points to commit the changes.";
                        break;
                    case GrabCasterComponentType.EventConfiguration:
                        DirectoryBubblingFileSelected = Path.Combine(gcPointsFoldersData.FolderName,
                                                        Configuration.DirectoryNameBubbling,
                                                        Configuration.DirectoryNameEvents);
                        message = "Event configuration file assigned, syncronize the points to commit the changes.";
                        break;
                    case GrabCasterComponentType.TriggerComponent:
                        DirectoryBubblingFileSelected = Path.Combine(gcPointsFoldersData.FolderName,
                                                        Configuration.DirectoryNameTriggers);
                        message = "Trigger component file assigned, syncronize the points to commit the changes.";
                        break;
                    case GrabCasterComponentType.EventComponent:
                        DirectoryBubblingFileSelected = Path.Combine(gcPointsFoldersData.FolderName,
                                                        Configuration.DirectoryNameEvents);
                        message = "Event component file assigned, syncronize the points to commit the changes.";
                        break;
                    default:
                        break;
                }
                File.Copy(treeviewBagCurrent.File, Path.Combine(DirectoryBubblingFileSelected, Path.GetFileName(treeviewBagCurrent.File)),true);
                CreateSyncronizationFile(treeNodeCurrent, gcPointsFoldersData.FolderName);
                Global.MessageBoxForm(message,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void contextMenuStripEventComponentSend_Click(object sender, EventArgs e)
        {
            TreeviewBag treeviewBag = (TreeviewBag)treeViewActive.SelectedNode.Tag;
            CopyAndSendFileComponent(treeviewBag.GrabCasterComponentType);
        }

        private void contextMenuStripEventConfiguration_Opening(object sender, CancelEventArgs e)
        {

        }

        private void executeToolStripMenuItemTriggerConfigurationExecute_Click(object sender, EventArgs e)
        {


            
            TreeviewBag treeviewBag = (TreeviewBag)treeViewActive.SelectedNode.Tag;
            TreeviewBag treeviewBagRoot = (TreeviewBag)GetRootNode(treeViewActive.SelectedNode).Tag;
            string folder = treeviewBagRoot.File;
            if (!folder.Contains("Root_GrabCasterUI"))
            {
                Global.MessageBoxForm(
                    "This functionality is avalable from the GrabCaster UI node.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }
            TriggerConfiguration triggerConfiguration = (TriggerConfiguration)treeviewBag.Component;

            RestEventsEngine restEventsEngine = new RestEventsEngine();
            restEventsEngine.ExecuteTrigger(triggerConfiguration.Trigger.IdConfiguration, triggerConfiguration.Trigger.IdComponent, null);
        }

        private void contextMenuStripEventConfigurationSend_Click(object sender, EventArgs e)
        {
            TreeviewBag treeviewBag = (TreeviewBag)treeViewActive.SelectedNode.Tag;
            CopyAndSendFileComponent(treeviewBag.GrabCasterComponentType);
        }

        private void contextMenuStripTriggerConfiguration_Opening(object sender, CancelEventArgs e)
        {

        }

        private void toolStripMenuItemRootSyncronize_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = treeViewActive.SelectedNode;
            TreeviewBag treeviewBag = (TreeviewBag)treeNode.Tag;
            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBag.Component;


            byte[] content = GrabCaster.Framework.CompressionLibrary.Helpers.CreateFromDirectory(gcPointsFoldersData.FolderName);
            SendPointBagToSyncronize(content,
                                        gcPointsFoldersData.ConfigurationStorage.ChannelId, 
                                        gcPointsFoldersData.ConfigurationStorage.PointId, 
                                        Configuration.PointId(), Configuration.MessageDataProperty.ConsoleBubblingBagToSyncronize);
        }

        private void toolStripButtonSyncronizeOut_Click(object sender, EventArgs e)
        {
            if (Global.MessageBoxForm("Send the syncronization to all the points?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                                    DialogResult.No) return;
            foreach (TreeNode treeNode in treeViewActive.Nodes)
            {
                TreeviewBag treeviewBag = (TreeviewBag)treeNode.Tag;
                GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBag.Component;


                byte[] content = GrabCaster.Framework.CompressionLibrary.Helpers.CreateFromDirectory(gcPointsFoldersData.FolderName);
                SendPointBagToSyncronize(content,
                                            gcPointsFoldersData.ConfigurationStorage.ChannelId,
                                            gcPointsFoldersData.ConfigurationStorage.PointId,
                                            Configuration.PointId(), Configuration.MessageDataProperty.ConsoleBubblingBagToSyncronize);
            }
        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //This event is used for treeview menus
            treeViewActive = treeView2;
            treeView_NodeMouseClick(treeView2,e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //Nodes operations

            /// <summary>
            /// Create a trigger from a node, drag and drop
            /// </summary>
        private void CreateTriggerConfigurationFromNode(TreeNode treeNodeDestination, TreeNode treeNodeSource)
        {
            if (Global.MessageBoxForm("Copy the trigger in this point?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
                return;

            //Filename

            //Prepare the bags
            TreeNode treeNodeRoot = GetRootNode(treeViewActive.SelectedNode);
            TreeviewBag treeviewBagRoot = (TreeviewBag)treeNodeRoot.Tag;
            TreeviewBag TreeviewBagNodeDestination = (TreeviewBag)treeNodeDestination.Tag;
            TreeviewBag TreeviewBagNodeSource = (TreeviewBag)treeNodeSource.Tag;


            //Get the bubbling event
            BubblingEvent bubblingEvent = EventsEngine.CreateBubblingTrigger(TreeviewBagNodeDestination.componentTrigger.triggerClass,
                                                                                TreeviewBagNodeDestination.componentTrigger.assembly,
                                                                                TreeviewBagNodeDestination.File);

            var jsonSerialization = SerializationHelper.CreteJsonTriggerConfigurationTemplate(bubblingEvent);
            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBagRoot.Component;

            string DirectoryBubblingTriggers = Path.Combine(gcPointsFoldersData.FolderName,
                                                            Configuration.DirectoryNameBubbling,
                                                            Configuration.DirectoryNameTriggers);

            string triggerConfigurationsFile = Path.Combine(DirectoryBubblingTriggers, TreeviewBagNodeSource.File + ".off");

            File.WriteAllText(triggerConfigurationsFile, jsonSerialization);

            TreeNode treeNodeTriggers = treeNodeRoot.Nodes[0].Nodes[0];

            CreateTriggerConfigurationNode(treeviewBagRoot, treeNodeTriggers, triggerConfigurationsFile);
            CreateSyncronizationFile(treeNodeDestination, "");

        }


        private void CreateTriggerEventConfigurationFromNode(TreeNode treeNodeDestination, TreeNode treeNodeSource)
        {
            if (Global.MessageBoxForm("Assign this event to the current trigger in this point?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            //Filename

            //Prepare the bags

            TreeNode treeNodeRoot = GetRootNode(treeViewActive.SelectedNode);
            TreeNode treeNodeTriggerParent  = treeNodeDestination.Parent;


            TreeviewBag treeviewBagRoot = (TreeviewBag)treeNodeRoot.Tag;
            TreeviewBag treeviewBagTriggerParent = (TreeviewBag)treeNodeTriggerParent.Tag;
            TreeviewBag treeviewBagDestination = (TreeviewBag)treeNodeDestination.Tag;
            TreeviewBag treeviewBagSource = (TreeviewBag)treeNodeSource.Tag;



            //Get the bubbling event
            BubblingEvent bubblingEvent = EventsEngine.CreateBubblingEvent(treeviewBagDestination.componentEvent.eventClass,
                                                                                treeviewBagDestination.componentEvent.assembly,
                                                                                treeviewBagDestination.File);

            var jsonSerialization = SerializationHelper.CreteJsonEventConfigurationTemplate(bubblingEvent);
            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBagRoot.Component;

            string DirectoryBubblingEvents = Path.Combine(gcPointsFoldersData.FolderName,
                                                            Configuration.DirectoryNameBubbling,
                                                            Configuration.DirectoryNameEvents);

            string eventConfigurationsFile = Path.Combine(DirectoryBubblingEvents, treeviewBagSource.File + ".off");

            File.WriteAllText(eventConfigurationsFile, jsonSerialization);

            TreeNode treeNodeEvents = treeNodeRoot.Nodes[0].Nodes[1];

            this.CreateEventConfigurationNode(treeviewBagRoot, treeNodeEvents, eventConfigurationsFile);
            CreateSyncronizationFile(treeNodeDestination, "");
        }

        private void CreateEventConfigurationFromNode(TreeNode treeNodeDestination, TreeNode treeNodeSource)
        {
            if (Global.MessageBoxForm("Assign this event to the current trigger in this point?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
                return;

            //Filename

            //Prepare the bags

            TreeNode treeNodeRoot = GetRootNode(treeViewActive.SelectedNode);
            TreeviewBag treeviewBagRoot = (TreeviewBag)treeNodeRoot.Tag;
            TreeviewBag treeviewBagDestination = (TreeviewBag)treeNodeDestination.Tag;
            TreeviewBag treeviewBagSource = (TreeviewBag)treeNodeSource.Tag;


            //Get the bubbling event
            BubblingEvent bubblingEvent = EventsEngine.CreateBubblingEvent(treeviewBagDestination.componentEvent.eventClass,
                                                                                treeviewBagDestination.componentEvent.assembly,
                                                                                treeviewBagDestination.File);

            var jsonSerialization = SerializationHelper.CreteJsonEventConfigurationTemplate(bubblingEvent);
            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBagRoot.Component;

            string DirectoryBubblingEvents = Path.Combine(gcPointsFoldersData.FolderName,
                                                            Configuration.DirectoryNameBubbling,
                                                            Configuration.DirectoryNameEvents);

            string eventConfigurationsFile = Path.Combine(DirectoryBubblingEvents, treeviewBagSource.File + ".off");

            File.WriteAllText(eventConfigurationsFile, jsonSerialization);

            TreeNode treeNodeEvents = treeNodeRoot.Nodes[0].Nodes[1];

            this.CreateEventConfigurationNode(treeviewBagRoot, treeNodeEvents, eventConfigurationsFile);
            CreateSyncronizationFile(treeNodeDestination, "");
        }


        private void CreateTriggerComponent(TreeNode treeNodeDestination, TreeNode treeNodeSource)
        {
            if (Global.MessageBoxForm("Copy the trigger component in this point?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
                return;

            //Filename

            //Prepare the bags

            TreeNode treeNodeRoot = GetRootNode(treeViewActive.SelectedNode);
            TreeviewBag treeviewBagRoot = (TreeviewBag)treeNodeRoot.Tag;
            TreeviewBag treeviewBagDestination = (TreeviewBag)treeNodeDestination.Tag;
            TreeviewBag treeviewBagSource = (TreeviewBag)treeNodeSource.Tag;

            File.Copy(treeviewBagSource.File, treeviewBagDestination.File,true);
            treeNodeDestination.Parent.Nodes.Add(treeNodeSource);

            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBagRoot.Component;

            CreateSyncronizationFile(treeNodeDestination, "");
        }

        private void CreateEventComponent(TreeNode treeNodeDestination, TreeNode treeNodeSource)
        {
            if (Global.MessageBoxForm("Copy the trigger component in this point?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
                return;

            //Filename

            //Prepare the bags

            TreeNode treeNodeRoot = GetRootNode(treeViewActive.SelectedNode);
            TreeviewBag treeviewBagRoot = (TreeviewBag)treeNodeRoot.Tag;
            TreeviewBag treeviewBagDestination = (TreeviewBag)treeNodeDestination.Tag;
            TreeviewBag treeviewBagSource = (TreeviewBag)treeNodeSource.Tag;

            File.Copy(treeviewBagSource.File, treeviewBagDestination.File, true);
            treeNodeDestination.Parent.Nodes.Add(treeNodeSource);

            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBagRoot.Component;

            CreateSyncronizationFile(treeNodeDestination, "");
        }

        private void statusTriggerEnableDisable_Click(object sender, EventArgs e)
        {
            string disableEnable = TreeNodeTextEnableDisable(treeViewActive.SelectedNode);
            EnableDisableTriggerEvent(treeViewActive.SelectedNode);
        }
        private bool IsComponentEnabled(TreeviewBag treeviewBag)
        {
            string extension = treeviewBag.GrabCasterComponentType == GrabCasterComponentType.TriggerConfiguration? "trg" : "evt";

            return Path.GetExtension(treeviewBag.File).ToLower() == extension;

        }

        private string TreeNodeTextEnableDisable(TreeNode treeNode)
        {
            TreeviewBag treeviewBag = (TreeviewBag) treeNode.Tag;
            string text = IsComponentEnabled(treeviewBag) ? "Disable" : "Enable";
            return text;

        }

        private TreeNode EnableDisableTriggerEvent(TreeNode treeNode)
        {
            TreeviewBag treeviewBag = (TreeviewBag)treeNode.Tag;
            if (treeviewBag.GrabCasterComponentType == GrabCasterComponentType.TriggerConfiguration)
            {
                string OnOff = IsComponentEnabled(treeviewBag) ? CONST_TRIGGEROFF_KEY : CONST_TRIGGERON_KEY;
                string extension = IsComponentEnabled(treeviewBag) ? ".off" :".trg";
                string newName = Path.ChangeExtension(treeviewBag.File, extension);
                File.Move(treeviewBag.File, newName);
                CreateSyncronizationFile(treeNode,"");
                treeNode.ImageKey = OnOff;
                treeNode.SelectedImageKey = OnOff;
            }

            if (treeviewBag.GrabCasterComponentType == GrabCasterComponentType.EventConfiguration)
            {
                string OnOff = IsComponentEnabled(treeviewBag) ? CONST_EVENTOFF_KEY : CONST_EVENTON_KEY;
                string extension = IsComponentEnabled(treeviewBag) ? ".off" : ".evn";
                string newName = Path.ChangeExtension(treeviewBag.File, extension);
                File.Move(treeviewBag.File,newName);
                treeNode.ImageKey = OnOff;
                treeNode.SelectedImageKey = OnOff;
            }
            return treeNode;

        }

        private void statusEventEnableDisable_Click(object sender, EventArgs e)
        {
            EnableDisableTriggerEvent(treeViewActive.SelectedNode);
        }
    }



    public static class TypeExtension
    {
        //a thread-safe way to hold default instances created at run-time
        private static ConcurrentDictionary<Type, object> typeDefaults = new ConcurrentDictionary<Type, object>();

        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? typeDefaults.GetOrAdd(type, t => Activator.CreateInstance(t)) : null;
        }
    }

    



    }



