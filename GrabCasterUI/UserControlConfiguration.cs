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

    public partial class UserControlConfiguration : UserControl
    {
        private const string DefaultMessage = "Configuration area.";
        private GrabCasterComponentType grabCasterComponentType { get; set; }

        private object objectToUpdate { get; set; }

        public UserControlConfiguration()
        {
            InitializeComponent();
        
        }

        public void LoadComponentData(TreeviewBag treeviewBag)
        {
            this.textBoxFile.Text = treeviewBag.File;

            GcPointsFoldersData gcPointsFoldersData = (GcPointsFoldersData)treeviewBag.Component;
            PropertyInfo[] propertyInfos = gcPointsFoldersData.ConfigurationStorage.GetType().GetProperties();
            List<propertyConfiguration> propertyConfigurations = new List<propertyConfiguration>();

            foreach (var item in propertyInfos)
            {
                var value = item.GetValue(gcPointsFoldersData.ConfigurationStorage) ?? "";

                propertyConfigurations.Add(new propertyConfiguration(item.Name, value.ToString()));
            }
            this.dataGridViewProperties.DataSource = propertyConfigurations;

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
                dataGridViewProperties.Columns[i].Width = averageWitdh - 30;
            }


        }


        private void dataGridViewProperties_Resize(object sender, EventArgs e)
        {
            setDataGridColumnsWidth();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

        }
    }
}
