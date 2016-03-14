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

        private string boldText(string text)
        {
            return @"\b " + text + @"\b0";
        }
        private string ansiText(string text)
        {
            return @"{\rtf1\ansi " + text + @" }";
        }
        public void LoadComponentData(TreeviewBag treeviewBag)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (treeviewBag.DataBag == null) return;
            switch (treeviewBag.GrabCasterComponentType)
            {

                case GrabCasterComponentType.TriggerComponent:
                    TriggerContract triggerContract = (TriggerContract)treeviewBag.Component;
                    stringBuilder.AppendLine($"Component information");
                    stringBuilder.AppendLine($"------------------------------------------------------");
                    stringBuilder.AppendLine($"Component type - Trigger");
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
                    stringBuilder.AppendLine($"Component information");
                    stringBuilder.AppendLine($"------------------------------------------------------");
                    stringBuilder.AppendLine($"Component type - Event.");
                    stringBuilder.AppendLine($"Assembly file {treeviewBag.File}");
                    stringBuilder.AppendLine($"Name: {eventContract.Name}");
                    stringBuilder.AppendLine($"Description: {eventContract.Description}");
                    stringBuilder.AppendLine($"Component Id: {eventContract.Id}");
                    stringBuilder.AppendLine($"Shared: {eventContract.Shared}");
                    break;
                case GrabCasterComponentType.Correlation:
                    this.richTextBoxSummary.Text = treeviewBag.DataBag.ToString();
                    break;
                default:
                    break;


            }

            //if correlation not need to do anything more...
            if (treeviewBag.GrabCasterComponentType == GrabCasterComponentType.Correlation) return;

            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"------------------------------------------------------");
            stringBuilder.AppendLine($"Properties to configure and use. (Name and Description)");
            stringBuilder.AppendLine($"------------------------------------------------------");

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
