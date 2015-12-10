using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratory
{
    using System.Threading;

    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Library;

    public partial class WindowsStartPointTest : Form
    {
        private Embedded.SetEventActionEventEmbedded setEventActionEventEmbedded; 

        public WindowsStartPointTest()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            setEventActionEventEmbedded = eventReceivedFromEmbedded;
            Embedded.setEventActionEventEmbedded = setEventActionEventEmbedded;
            GrabCaster.Framework.Library.Embedded.StartEngine();
        }
        private SynchronizationContext synchronizationContext;

        private void eventReceivedFromEmbedded(IEventType eventType, EventActionContext context)
        {
            string s = Encoding.UTF8.GetString(eventType.DataContext);
            //synchronizationContext.Post(
            //    delegate
            //        {
            //            listBoxEvents.Items.Add(s);
            //        },
            //    null); //as the worker thread cannot access the UI thread resources
            MessageBox.Show(s);

            //AddListBoxItem(Encoding.UTF8.GetString(eventType.DataContext));
        }

        private delegate void AddListBoxItemDelegate(object item);

        private void AddListBoxItem(object item)
        {
            if (this.listBoxEvents.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                this.listBoxEvents.Invoke(new AddListBoxItemDelegate(this.AddListBoxItem), item);
            }
            else
            {
                // This is the UI thread so perform the task.
                this.listBoxEvents.Items.Add(item);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listBoxEvents.Items.Clear();
        }

        private void WindowsStartPointTest_Load(object sender, EventArgs e)
        {
            synchronizationContext = System.Threading.SynchronizationContext.Current;
        }
    }
}
