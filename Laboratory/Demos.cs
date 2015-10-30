// --------------------------------------------------------------------------------------------------
// <copyright file = "LogMessage.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog: http://ninocrudele.me
//    
//    By accessing GrabCaster code here, you are agreeing to the following licensing terms.
//    If you do not agree to these terms, do not access the GrabCaster code.
//    Your license to the GrabCaster source and/or binaries is governed by the 
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace Lab
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    /// <summary>
    /// The demos.
    /// </summary>
    public partial class Demos : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Demos"/> class.
        /// </summary>
        public Demos()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The button power json_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void buttonPowerJson_Click(object sender, EventArgs e)
        {
            this.labellastrun.Text = string.Empty;
            var numof = int.Parse(this.textBoxNum.Text);
            for (var i = 0; i < numof; i++)
            {
                if (!EventLog.SourceExists(this.comboBoxsource.Text))
                {
                    EventLog.CreateEventSource(this.comboBoxsource.Text, "Application");
                }
                EventLog.WriteEntry(
                    this.comboBoxsource.Text,
                    "New message in the eventviewer " + DateTime.Now.ToString(),
                    EventLogEntryType.Error);
            }
            this.labellastrun.Text = DateTime.Now.ToString();
        }

        private void Demos_Load(object sender, EventArgs e)
        {
            this.comboBoxsource.SelectedIndex = 0;
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }
    }
}