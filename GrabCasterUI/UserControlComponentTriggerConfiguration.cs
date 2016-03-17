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
    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Globals;

    public partial class UserControlComponentTriggerConfiguration : UserControl
    {
        private const string DefaultMessage = "Component configuration area.";
        private GrabCasterComponentType grabCasterComponentType { get; set; }
        private TriggerConfiguration triggerConfiguration = null;
        private object objectToUpdate { get; set; }

        public TreeView TreeViewSide { get; set; }
        public TreeNode TreeNodeSide { get; set; }
        private TreeviewBag treeviewBagUsed { get; set; }

        public UserControlComponentTriggerConfiguration()
        {
            InitializeComponent();
        }

        public void LoadComponentData(TreeviewBag treeviewBag)
        {
            treeviewBagUsed = treeviewBag;
            grabCasterComponentType = treeviewBag.GrabCasterComponentType;
            

            switch (grabCasterComponentType)
            {
                case GrabCasterComponentType.TriggerConfiguration:
                    objectToUpdate = treeviewBag.Component;
                    triggerConfiguration = (TriggerConfiguration)objectToUpdate;
                    this.textBoxIdConfiguration.Text = triggerConfiguration.Trigger.IdConfiguration;
                    this.textBoxIdComponent.Text = triggerConfiguration.Trigger.IdComponent;
                    this.textBoxName.Text = triggerConfiguration.Trigger.Name;
                    this.textBoxDescription.Text = triggerConfiguration.Trigger.Description;
                    this.dataGridViewProperties.DataSource = triggerConfiguration.Trigger.TriggerProperties;
                    setDataGridColumnsWidth();
                    break;
                default:
                    break;
            }
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (Global.MessageBoxForm("Save the Trigger?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                DialogResult.No) return;

            triggerConfiguration.Trigger.IdConfiguration = this.textBoxIdConfiguration.Text;
            triggerConfiguration.Trigger.IdComponent = this.textBoxIdComponent.Text;
            triggerConfiguration.Trigger.Name = this.textBoxName.Text;
            triggerConfiguration.Trigger.Description = this.textBoxDescription.Text;
            foreach (DataGridViewRow row in dataGridViewProperties.Rows)
            {
                string propertyToSet = row.Cells[0].Value.ToString();
                try
                {
                    TriggerProperty triggerProperty = (from proptoset in triggerConfiguration.Trigger.TriggerProperties
                               where proptoset.Name == propertyToSet
                               select proptoset).First();
                    triggerProperty.Value = row.Cells[1].Value.ToString();
                }
                catch (Exception ex)
                {

                    Global.MessageBoxForm($"Error finding property {propertyToSet} or wrong data type.\r{ex.Message}", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                

            }
            var serializedMessage = JsonConvert.SerializeObject(triggerConfiguration,Formatting.Indented,new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText(treeviewBagUsed.File, serializedMessage);
            Global.MessageBoxForm("Trigger saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
