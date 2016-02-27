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
    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Globals;

    public partial class UserControlComponentConfiguration : UserControl
    {
        private const string DefaultMessage = "Component configuration area.";
        private GrabCasterComponentType grabCasterComponentType { get; set; }

        private object objectToUpdate { get; set; }

        public UserControlComponentConfiguration()
        {
            InitializeComponent();
        }

        public void LoadComponentData(TreeviewBag treeviewBag)
        {
            grabCasterComponentType = treeviewBag.GrabCasterComponentType;
            

            switch (grabCasterComponentType)
            {
                case GrabCasterComponentType.TriggerConfiguration:
                    objectToUpdate = treeviewBag.Component;
                    TriggerConfiguration triggerConfiguration = (TriggerConfiguration)objectToUpdate;
                    this.textBoxIdConfiguration.Text = triggerConfiguration.Trigger.IdConfiguration;
                    this.textBoxIdComponent.Text = triggerConfiguration.Trigger.IdComponent;
                    this.textBoxName.Text = triggerConfiguration.Trigger.Name;
                    this.textBoxDescription.Text = triggerConfiguration.Trigger.Description;
                    this.dataGridViewProperties.DataSource = triggerConfiguration.Trigger.TriggerProperties;
                    setDataGridColumnsWidth();
                    break;
                case GrabCasterComponentType.Event:
                    objectToUpdate = treeviewBag.Component;
                    Event eventInTrigger = (Event)objectToUpdate;
                    this.textBoxIdConfiguration.Text = eventInTrigger.IdConfiguration;
                    this.textBoxIdComponent.Text = eventInTrigger.IdComponent;
                    this.textBoxName.Text = eventInTrigger.Name;
                    this.textBoxDescription.Text = eventInTrigger.Description;
                    this.dataGridViewProperties.DataSource = eventInTrigger.EventProperties;
                    setDataGridColumnsWidth();
                    break;
                case GrabCasterComponentType.EventConfiguration:
                    objectToUpdate = treeviewBag.Component;
                    EventConfiguration eventConfiguration = (EventConfiguration)objectToUpdate;
                    this.textBoxIdConfiguration.Text = eventConfiguration.Event.IdConfiguration;
                    this.textBoxIdComponent.Text = eventConfiguration.Event.IdComponent;
                    this.textBoxName.Text = eventConfiguration.Event.Name;
                    this.textBoxDescription.Text = eventConfiguration.Event.Description;
                    this.dataGridViewProperties.DataSource = eventConfiguration.Event.EventProperties;
                    setDataGridColumnsWidth();
                    break;

                default:
                    break;
            }
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
                dataGridViewProperties.Columns[i].Width = averageWitdh;
            }


        }


        private void dataGridViewProperties_Resize(object sender, EventArgs e)
        {
            setDataGridColumnsWidth();
        }
    }
}
