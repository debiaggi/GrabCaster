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
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Engine.OffRamp;
    using GrabCaster.Framework.Engine.OnRamp;
    using GrabCaster.Framework.Library;

    public partial class FormPoint : Form
    {
        //Main objects
        private TreeNode treeNodeMainPoint = null;

        //Constants 
        public const string CONST_ROOT = "ROOT";
        public const string CONST_ROOT_KEY = "ROOT";

        public static string PointId = "";
        public static string PointName = "";


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

            PointId = Guid.NewGuid().ToString();
            PointName = "Console";

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

        public TreeNode RefreshTreeviewPointRootNode(string PointName)
        {


            this.treeNodeMainPoint = this.treeNodeMainPoint.Nodes.Add(CONST_ROOT, PointName, CONST_ROOT_KEY, CONST_ROOT_KEY);

            return null;
        }

        private static void EventReceivedFromEmbedded(string DestinationConsolePointId, ISkeletonMessage skeletonMessage)
        {
            if (DestinationConsolePointId == PointId)
            {
                Console.WriteLine("---------------EVENT RECEIVED FROM EMBEDDED LIBRARY---------------");
            }


        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {

            OffRampEngineSending.SendNullMessageOnRamp(
                Configuration.MessageDataProperty.SyncSendRequestConfiguration,
                "*",
                "*",
                PointId,
                PointName);
        }

        private void FormPoint_Load(object sender, EventArgs e)
        {
            InizializeEnvironment();

        }
    }
}
