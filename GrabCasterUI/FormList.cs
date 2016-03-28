// -----------------------------------------------------------------------------------
// 
// GRABCASTER LTD CONFIDENTIAL
// ___________________________
// 
// Copyright © 2013 - 2016 GrabCaster Ltd. All rights reserved.
// This work is registered with the UK Copyright Service: Registration No:284701085
// 
// 
// NOTICE:  All information contained herein is, and remains
// the property of GrabCaster Ltd and its suppliers,
// if any.  The intellectual and technical concepts contained
// herein are proprietary to GrabCaster Ltd
// and its suppliers and may be covered by UK and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from GrabCaster Ltd.
// 
// -----------------------------------------------------------------------------------
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
    using GrabCaster.Framework.Contracts.Channels;
    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Points;

    public partial class FormList : Form
    {
        private List<Channel> ChannelsIn { get; set; }
        public List<Channel> ChannelsOut { get; set; }

        public FormList()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            List<Channel> channelsSelected = new List<Channel>();

            //Multiple channels selected then add channels
            if (listBoxChannels.SelectedItems.Count > 1)
            {
                DialogResult dresult = MessageBox.Show(
                    "Multiple channels selected, add these channels to the event?",
                    "GrabCaster",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dresult == DialogResult.Yes)
                {
                    foreach (var item in this.listBoxChannels.SelectedIndices)
                    {
                        string[] data = this.listBoxChannels.Items[(int)item].ToString().Split('\t');
                        string channelId = data[0];
                        var channelRet =
                            (from channel in ChannelsIn where channel.ChannelId == channelId select channel).First();

                        channelsSelected.Add(channelRet);
                    }

                }

            }

            if (listBoxPoints.SelectedItems.Count > 0 && listBoxChannels.SelectedItems.Count == 1)
            {
                string[] dataNode = this.listBoxChannels.SelectedItem.ToString().Split('\t');
                string channelId = dataNode[0];
                string channelName = dataNode[1];
                string channelDescription = dataNode[2];

                DialogResult dresult = MessageBox.Show(
                    $"Add selected points of channel [{channelName}] to the event?",
                    "GrabCaster",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dresult == DialogResult.Yes)
                {


                    List<GrabCaster.Framework.Contracts.Points.Point> PointsAll = new List<GrabCaster.Framework.Contracts.Points.Point>();
                    foreach (var item in this.listBoxPoints.SelectedIndices)
                    {
                        string[] data = this.listBoxPoints.Items[(int)item].ToString().Split('\t');
                        string pointId = data[0];
                        string pointName = data[1];
                        string pointDescription = data[2];

                        PointsAll.Add(new GrabCaster.Framework.Contracts.Points.Point(pointId,pointName,pointDescription));
                    }

                    Channel channelRet = new Channel(channelId, channelName,channelDescription, PointsAll);
                    channelsSelected.Add(channelRet);
                }
            }

            ChannelsOut = channelsSelected;
            this.Close();
        }

        private void FormList_Load(object sender, EventArgs e)
        {
      
        }

        public void FillListBoxChannels(List<Channel> channels)
        {
            ChannelsIn = channels;

            this.listBoxChannels.Items.Clear();
            this.listBoxChannels.Items.Clear();
            this.listBoxPoints.Items.Clear();

            foreach (var item in ChannelsIn)
            {
                listBoxChannels.Items.Add(string.Concat(item.ChannelId, "\t", item.ChannelName, "\t", item.ChannelDescription));
            }
            listBoxChannels.SelectedIndex = 0;

        }

        private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] data = this.listBoxChannels.SelectedItem.ToString().Split('\t');
            string channelId = data[0];
            var channelSelected =
                (from channel in ChannelsIn where channel.ChannelId == channelId select channel).First();

            FillListBoxPoints(channelSelected,ChannelsIn);
        }

        private void FillListBoxPoints(Channel channel, List<Channel> channels)
        {
            this.listBoxPoints.Items.Clear();
            var selectedChildren = ChannelsIn.SelectMany(i => i.Points).Distinct();

            if (channel.ChannelId == "*")
            {
                foreach (var item in selectedChildren)
                {
                    listBoxPoints.Items.Add(string.Concat(item.PointId, "\t", item.Name, "\t", item.Description));
                }
            }
            else
            {
                foreach (var item in channel.Points)
                {
                    listBoxPoints.Items.Add(string.Concat(item.PointId, "\t", item.Name, "\t", item.Description));

                }
            }

            listBoxPoints.SelectedIndex = 0;
        }
    }
}
