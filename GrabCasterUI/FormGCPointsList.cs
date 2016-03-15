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
    
    }


}
