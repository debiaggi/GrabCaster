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
        public List<ConfigurationStorage> configurationStorages { get; set; }

        public ConfigurationStorage configurationStorageSelected = null;

        public FormGCPointsList()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormGCPointsList_Load(object sender, EventArgs e)
        {

            foreach (var item in configurationStorages)
            {
                string[] record = { item.PointId, item.PointName, item.ChannelId, item.ChannelName };
                this.listBoxPoints.Items.AddRange(record);

            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.configurationStorageSelected = this.listBoxPoints.SelectedIndex == 0 ? Configuration.ConfigurationStorage : this.configurationStorages[this.listBoxPoints.SelectedIndex];
        }
    
    }


}
