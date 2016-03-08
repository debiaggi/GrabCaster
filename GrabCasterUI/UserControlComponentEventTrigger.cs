using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrabCasterUI
{
    using System.Reflection;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Channels;
    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Globals;

    public partial class UserControlComponentEventTrigger : UserControl
    {
        private const string DefaultMessage = "Component configuration area.";
        private GrabCasterComponentType grabCasterComponentType { get; set; }

        private object objectToUpdate { get; set; }

        public List<Channel> ChannelsIn { get; set; }
        public List<Channel> ChannelsOut { get; set; }

        private Event eventInTrigger = null;

        public TreeView TreeViewSide { get; set; }
        public TreeNode TreeNodeSide { get; set; }

        public UserControlComponentEventTrigger()
        {
            InitializeComponent();
        }

        public void LoadComponentData(TreeviewBag treeviewBag)
        {
            grabCasterComponentType = treeviewBag.GrabCasterComponentType;

            objectToUpdate = treeviewBag.Component;
            eventInTrigger = (Event)objectToUpdate;
            this.textBoxIdConfiguration.Text = eventInTrigger.IdConfiguration;
            this.textBoxIdComponent.Text = eventInTrigger.IdComponent;
            this.textBoxName.Text = eventInTrigger.Name;
            this.textBoxDescription.Text = eventInTrigger.Description;
            this.dataGridViewProperties.DataSource = eventInTrigger.EventProperties;
            FillListBoxChannels(eventInTrigger);
            setDataGridColumnsWidth();

        }


        private void buttonSave_Click(object sender, EventArgs e)
        {

        }

        private void FillListBoxChannels(Event eventInTrigger)
        {
            this.listBoxChannels.Items.Clear();
            this.listBoxPoints.Items.Clear();

            if (eventInTrigger.Channels == null)
            {
                this.listBoxChannels.Items.Add("No channels present, the event will be executed locally.");
                this.listBoxPoints.Items.Add("No channels present, the event will be executed locally.");
                return;
            }


            foreach (var item in eventInTrigger.Channels)
            {
                listBoxChannels.Items.Add(string.Concat(item.ChannelId,"\t",item.ChannelName,"\t",item.ChannelDescription));
            }
            listBoxChannels.SelectedIndex = 0;

        }

        private void FillListBoxPoints(Channel channel)
        {
            this.listBoxPoints.Items.Clear(); ;
            foreach (var item in channel.Points)
            {
                listBoxPoints.Items.Add(string.Concat(item.PointId, "\t", item.Name, "\t", item.Description));

            }
            listBoxPoints.SelectedIndex = 0;
        }

        private void setDataGridColumnsWidth()
        {
            int numOfColumns = this.dataGridViewProperties.ColumnCount;
            if (numOfColumns == 0)
            {
                return;
            }
            int averageWitdh = dataGridViewProperties.Width / numOfColumns;

            for (int i = 0; i < numOfColumns; i++)
            {
                dataGridViewProperties.Columns[i].Width = averageWitdh-30;
            }


        }


        private void dataGridViewProperties_Resize(object sender, EventArgs e)
        {
            setDataGridColumnsWidth();
        }

        private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.listBoxChannels.SelectedItem==null) return;
            if(eventInTrigger.Channels == null) return;

            string[] data = this.listBoxChannels.SelectedItem.ToString().Split('\t');
            string channelId = data[0];
            var channelSelected =
                (from channel in eventInTrigger.Channels where channel.ChannelId == channelId select channel).First();

            FillListBoxPoints(channelSelected);

        }

        private void buttonAddChannel_Click(object sender, EventArgs e)
        {
            OpenChannelsForm();

        }

        private void OpenChannelsForm()
        {
            FormList formList = new FormList();
            formList.FillListBoxChannels(ChannelsIn);
            DialogResult dialogResult = formList.ShowDialog();
            ChannelsOut = null;
            ChannelsOut = formList.ChannelsOut;

            if (dialogResult == DialogResult.Cancel) return;
            if (ChannelsOut != null)
            {
                if (eventInTrigger.Channels == null)
                {
                    eventInTrigger.Channels = new List<Channel>();
                    foreach (var item in ChannelsOut)
                    {
                        eventInTrigger.Channels.Add(item);
                    }

                }
                else
                {
                    foreach (var item in ChannelsOut)
                    {
                        eventInTrigger.Channels.RemoveAll(x => x.ChannelId == item.ChannelId);
                        eventInTrigger.Channels.Add(item);
                    }

                }
                FillListBoxChannels(this.eventInTrigger);
            }

        }

        private void buttonDelChannel_Click(object sender, EventArgs e)
        {
            if (this.listBoxChannels.SelectedIndices.Count <= 0) return;
            if (eventInTrigger.Channels == null) return;

            string[] data = this.listBoxChannels.SelectedItem.ToString().Split('\t');
            string channelId = data[0];
            eventInTrigger.Channels.RemoveAll(x => x.ChannelId == channelId);
            FillListBoxChannels(this.eventInTrigger);
        }

        private void buttonAddPoints_Click(object sender, EventArgs e)
        {
            OpenChannelsForm();
        }

        private void buttonDelPoint_Click(object sender, EventArgs e)
        {
            if (this.listBoxChannels.SelectedIndices.Count <= 0 && this.listBoxPoints.SelectedIndices.Count <= 0) return;
            if (eventInTrigger.Channels == null) return;

            string[] dataC = this.listBoxChannels.SelectedItem.ToString().Split('\t');
            string channelId = dataC[0];

            string[] dataP = this.listBoxPoints.SelectedItem.ToString().Split('\t');
            string pointId = dataP[0];
            this.eventInTrigger.Channels.RemoveAll(x => x.ChannelId == channelId && (x.Points.Exists(p => p.PointId == pointId)));

            var channels = this.eventInTrigger.Channels.FindAll(x => x.ChannelId == channelId);
            foreach (var item in channels)
            {
                item.Points.RemoveAll(p => p.PointId == pointId);
            }

            this.FillListBoxChannels(this.eventInTrigger);
        }
        List<propertyConfiguration> propertyConfigurations = new List<propertyConfiguration>();
        private void buttonAddProperty_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Insert the properties template from component?",
                "GrabCaster",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //Get root node
                TreeNode treeNode = GetRootNode(TreeNodeSide);
                TreeviewBag treeviewBag = (TreeviewBag)treeNode.Tag;

                componentEvent componentEvent = (from typeClass in treeviewBag.componentEventList
                                                 where typeClass.eventContract.Id == this.textBoxIdComponent.Text
                                                 select typeClass).First();
                
                foreach (var item in componentEvent.eventClass.GetProperties())
                {
                    if(item.Name != "SetEventActionEvent" && item.Name != "DataContext" && item.Name != "Context")
                    propertyConfigurations.Add(new propertyConfiguration(item.Name,""));
                }
                if (propertyConfigurations.Count == 0)
                {
                    Global.MessageBoxForm(
                        $"The component {componentEvent.eventContract.Name} does not contain properties to use.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    this.dataGridViewProperties.DataSource = null;
                    return;
                }
                this.dataGridViewProperties.DataSource = propertyConfigurations.ToArray();
                this.setDataGridColumnsWidth();
            }
        }

        private TreeNode GetRootNode(TreeNode node)
        {

            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }

        private void buttonDelProperty_Click(object sender, EventArgs e)
        {
            if(dataGridViewProperties.SelectedRows.Count<=0)
            {
                MessageBox.Show("Select a row to delete.", "GrabCaster", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show(
                "Remove the selected properties?",
                "GrabCaster",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in this.dataGridViewProperties.SelectedRows)
                {
                    propertyConfigurations.RemoveAt(item.Index);
                    dataGridViewProperties.DataSource = this.propertyConfigurations.ToArray();
                }
            }
        }
    }
}
