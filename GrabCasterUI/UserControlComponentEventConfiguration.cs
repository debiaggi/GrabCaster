using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

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
        private EventConfiguration eventConfiguration = null;
        private object objectToUpdate { get; set; }

        public TreeView TreeViewSide { get; set; }
        public TreeNode TreeNodeSide { get; set; }
        private TreeviewBag treeviewBagUsed { get; set; }
        public UserControlComponentEventConfiguration()
        {
            InitializeComponent();
        }

        public void LoadComponentData(TreeviewBag treeviewBag)
        {
            treeviewBagUsed = treeviewBag;
            grabCasterComponentType = treeviewBag.GrabCasterComponentType;
            objectToUpdate = treeviewBag.Component;
            eventConfiguration = (EventConfiguration)objectToUpdate;
            this.textBoxIdConfiguration.Text = eventConfiguration.Event.IdConfiguration;
            this.textBoxIdComponent.Text = eventConfiguration.Event.IdComponent;
            this.textBoxName.Text = eventConfiguration.Event.Name;
            this.textBoxDescription.Text = eventConfiguration.Event.Description;
            this.dataGridViewProperties.DataSource = eventConfiguration.Event.EventProperties;
            setDataGridColumnsWidth();

        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (Global.MessageBoxForm("Save the Event?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==DialogResult.No) return;

            eventConfiguration.Event.IdConfiguration = this.textBoxIdConfiguration.Text;
            eventConfiguration.Event.IdComponent = this.textBoxIdComponent.Text;
            eventConfiguration.Event.Name = this.textBoxName.Text;
            eventConfiguration.Event.Description = this.textBoxDescription.Text;
            foreach (DataGridViewRow row in dataGridViewProperties.Rows)
            {
                string propertyToSet = row.Cells[0].Value.ToString();
                try
                {
                    EventProperty eventProperty = (from proptoset in eventConfiguration.Event.EventProperties
                                                       where proptoset.Name == propertyToSet
                                                       select proptoset).First();
                    eventProperty.Value = row.Cells[1].Value.ToString();
                }
                catch (Exception ex)
                {

                    Global.MessageBoxForm($"Error finding property {propertyToSet} or wrong data type.\r{ex.Message}", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }


            }
            var serializedMessage = JsonConvert.SerializeObject(eventConfiguration, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText(treeviewBagUsed.File, serializedMessage);
            Global.MessageBoxForm("Event saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
