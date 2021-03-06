﻿namespace GrabCaster.Framework.DialogBoxEvent
{
    using System.Text;
    using System.Windows.Forms;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    [EventContract("{39AD14F3-009E-45EE-83B6-CECD51E6A242}", "DialogBox Event", "Show a DialogBox", true)]
    public class DialogBoxEvent : IEventType
    {
        public EventActionContext Context { get; set; }

        public SetEventActionEvent SetEventActionEvent { get; set; }

        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        [EventActionContract("{6908E16A-6763-435C-B7C9-8FDD9F333FB9}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                var rfidtag = Encoding.UTF8.GetString(this.DataContext);
                var dialogResult = MessageBox.Show(
                    $"Authorization for TAG code {rfidtag}.",
                    "Authorization TAG",
                    MessageBoxButtons.YesNo);
                this.DataContext = Encoding.UTF8.GetBytes(dialogResult == DialogResult.Yes ? true.ToString() : false.ToString());

                setEventActionEvent(this, context);
            }

            catch
            {
                // ignored
            }
        }
    }
}