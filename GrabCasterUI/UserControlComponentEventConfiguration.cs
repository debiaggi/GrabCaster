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

    public partial class UserControlComponentEventConfiguration : UserControl
    {
        private const string DefaultMessage = "Component configuration area.";
        private GrabCasterComponentType grabCasterComponentType { get; set; }

        private object objectToUpdate { get; set; }

        public TreeView TreeViewSide { get; set; }
        public TreeNode TreeNodeSide { get; set; }

        public UserControlComponentEventConfiguration()
        {
            InitializeComponent();
        }

        public void LoadComponentData(TreeviewBag treeviewBag)
        {
            grabCasterComponentType = treeviewBag.GrabCasterComponentType;
            objectToUpdate = treeviewBag.Component;
            EventConfiguration eventConfiguration = (EventConfiguration)objectToUpdate;
            this.textBoxIdConfiguration.Text = eventConfiguration.Event.IdConfiguration;
            this.textBoxIdComponent.Text = eventConfiguration.Event.IdComponent;
            this.textBoxName.Text = eventConfiguration.Event.Name;
            this.textBoxDescription.Text = eventConfiguration.Event.Description;
            this.dataGridViewProperties.DataSource = eventConfiguration.Event.EventProperties;
            setDataGridColumnsWidth();

        }


        private void buttonSave_Click(object sender, EventArgs e)
        {

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

        }
    }
}
