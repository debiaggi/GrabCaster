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
            string[] data = this.listBoxChannels.SelectedItem.ToString().Split('\t');
            string channelId = data[0];
            var channelSelected =
                (from channel in eventInTrigger.Channels where channel.ChannelId == channelId select channel).First();

            FillListBoxPoints(channelSelected);

        }

        private void buttonAddChannel_Click(object sender, EventArgs e)
        {
            FormList formList = new FormList();
            formList.FillListBoxChannels(ChannelsIn);
            formList.ShowDialog();
            ChannelsOut = formList.ChannelsOut;

        }
    }
}
