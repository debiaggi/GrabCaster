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
    using System.Reflection;

    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Engine;

    public partial class FormListComponent : Form
    {

        public BubblingEvent bubblingEvent { get; set; }

        public List<componentTrigger> ComponentTriggers { get; set; }
        public List<componentEvent> ComponentEvents { get; set; }

        public GrabCasterComponentType grabCasterComponentType { get; set; }

        public FormListComponent()
        {
            InitializeComponent();
        }

        public void LoadTriggers()
        {
            foreach (var item in ComponentTriggers)
            {
                this.listBox1.Items.Add(string.Concat(item.triggerContract.Name,"\t",item.triggerContract.Description, "\t", item.triggerContract.Id));
            }
;
        }
        public void LoadEvents()
        {
            foreach (var item in ComponentEvents)
            {
                this.listBox1.Items.Add(string.Concat(item.eventContract.Name, "\t", item.eventContract.Description, "\t", item.eventContract.Id));
            }
        }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex >= 0)
            {
                switch (this.grabCasterComponentType)
                {
                    case GrabCasterComponentType.TriggerConfiguration:
                        componentTrigger _componentTrigger = ComponentTriggers[listBox1.SelectedIndex];
                        BubblingEvent bubliBubblingTrigger =
                            EventsEngine.CreateBubblingTrigger(
                                _componentTrigger.triggerClass,
                                Assembly.GetExecutingAssembly(),
                                "");
                        bubblingEvent = bubliBubblingTrigger;
                        break;
                    case GrabCasterComponentType.EventConfiguration:
                        componentEvent _componentEvent = ComponentEvents[listBox1.SelectedIndex];
                        BubblingEvent bubliBubblingEvent = EventsEngine.CreateBubblingTrigger(
                                                                _componentEvent.eventClass,
                                                                Assembly.GetExecutingAssembly(),
                                                                "");
                        bubblingEvent = bubliBubblingEvent;
                        break;
                }

                
            }
        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {

            this.Close();
        }
    }
}
