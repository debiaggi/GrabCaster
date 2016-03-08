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
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Globals;

    public partial class UserControlComponent : UserControl
    {
        private const string DefaultMessage = "Component configuration area.";
        private GrabCasterComponentType grabCasterComponentType { get; set; }

        private object objectToUpdate { get; set; }

        public TreeView TreeViewSide { get; set; }
        public TreeNode TreeNodeSide { get; set; }

        public UserControlComponent()
        {
            InitializeComponent();

        }

        public void LoadComponentData(TreeviewBag treeviewBag)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            switch (treeviewBag.GrabCasterComponentType)
            {

                case GrabCasterComponentType.TriggerComponent:
                    TriggerContract triggerContract = (TriggerContract)treeviewBag.Component;
                    stringBuilder.AppendLine($"Component type Trigger.");
                    stringBuilder.AppendLine($"Assembly file {treeviewBag.File}");
                    stringBuilder.AppendLine($"Name: {triggerContract.Name}");
                    stringBuilder.AppendLine($"Description: {triggerContract.Description}");
                    stringBuilder.AppendLine($"Component Id: {triggerContract.Id}");
                    stringBuilder.AppendLine($"No operations required: {triggerContract.Nop}");
                    stringBuilder.AppendLine($"Polling Required: {triggerContract.PollingRequired}");
                    stringBuilder.AppendLine($"Shared: {triggerContract.Shared}");

                    break;
                case GrabCasterComponentType.EventComponent:
                    EventContract eventContract = (EventContract)treeviewBag.Component;
                    stringBuilder.AppendLine($"Component type Event.");
                    stringBuilder.AppendLine($"Assembly file {treeviewBag.File}");
                    stringBuilder.AppendLine($"Name: {eventContract.Name}");
                    stringBuilder.AppendLine($"Description: {eventContract.Description}");
                    stringBuilder.AppendLine($"Component Id: {eventContract.Id}");
                    stringBuilder.AppendLine($"Shared: {eventContract.Shared}");
                    break;
                default:
                    break;


            }
            PropertyInfo[] propertyInfos = (PropertyInfo[])treeviewBag.ComponentDetails;
            foreach (var propertyInfo in propertyInfos)
            {
                var propertyAttributes =
                    propertyInfo.GetCustomAttributes(typeof(TriggerPropertyContract), true);
                if (propertyAttributes.Length > 0)
                {
                    var triggerProperty = (TriggerPropertyContract)propertyAttributes[0];

                    // TODO 1004
                    if (propertyInfo.Name != triggerProperty.Name)
                    {
                        //throw new Exception(
                        //    $"Critical error! the properies {propertyAttributes[0]} and {propertyInfo.Name} are different! Class name {assemblyClass.Name}");
                    }
                    if (triggerProperty.Name != "DataContext")
                        stringBuilder.AppendLine($"Name: {triggerProperty.Name} Description: {triggerProperty.Description}");

                }
            }

            this.richTextBoxSummary.Text = stringBuilder.ToString();



        }




    }
}
