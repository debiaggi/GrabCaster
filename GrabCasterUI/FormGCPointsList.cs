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
    using GrabCaster.Framework.Base;

    public partial class FormGCPointsList : Form
    {

        private List<GcPointsFoldersData> _gcPointsFoldersDataList { get; set; }
        public GcPointsFoldersData gcPointsFoldersDataSelected = null;

        public FormGCPointsList()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void LoadList(List<GcPointsFoldersData> gcPointsFoldersDataList)
        {
          
            this.listView1.Columns.Add("Channel Name"); //column 1 heading
            listView1.Columns.Add("Channel Description"); //column 2 heading
            listView1.Columns.Add("Channel Id"); //column 2 heading
            listView1.Columns.Add("Point name"); //column 2 heading
            listView1.Columns.Add("Point Description"); //column 2 heading
            listView1.Columns.Add("Point Id"); //column 2 heading
            listView1.View = View.Details;

            _gcPointsFoldersDataList = gcPointsFoldersDataList;
            foreach (var item in gcPointsFoldersDataList)
            {

                ListViewItem lvi = new ListViewItem(item.ConfigurationStorage.ChannelName);
                lvi.SubItems.Add(item.ConfigurationStorage.ChannelDescription);
                lvi.SubItems.Add(item.ConfigurationStorage.ChannelId);
                lvi.SubItems.Add(item.ConfigurationStorage.PointName);
                lvi.SubItems.Add(item.ConfigurationStorage.PointDescription);
                lvi.SubItems.Add(item.ConfigurationStorage.PointId);
                lvi.Tag = item;
                // add the listviewitem to a new row of the ListView control
                listView1.Items.Add(lvi); //show Text1 in column1, Text2 in col2


            }
            ResizeColumns();
        }

        private void ResizeColumns()
        {
            foreach (ColumnHeader item in this.listView1.Columns)
            {
                item.Width = this.listView1.Width/this.listView1.Columns.Count;
            }
        }
        private void FormGCPointsList_Load(object sender, EventArgs e)
        {


        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count < 0) return;

            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)this.listView1.SelectedItems[0].Tag;
            this.gcPointsFoldersDataSelected = gcPointsFoldersData;
            this.Close();
            
        }

        private void FormGCPointsList_SizeChanged(object sender, EventArgs e)
        {
            ResizeColumns();
        }
    }


}
