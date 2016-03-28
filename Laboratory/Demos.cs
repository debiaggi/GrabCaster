// -----------------------------------------------------------------------------------
// 
// GRABCASTER LTD CONFIDENTIAL
// ___________________________
// 
// Copyright © 2013 - 2016 GrabCaster Ltd. All rights reserved.
// This work is registered with the UK Copyright Service: Registration No:284701085
// 
// 
// NOTICE:  All information contained herein is, and remains
// the property of GrabCaster Ltd and its suppliers,
// if any.  The intellectual and technical concepts contained
// herein are proprietary to GrabCaster Ltd
// and its suppliers and may be covered by UK and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from GrabCaster Ltd.
// 
// -----------------------------------------------------------------------------------
namespace Laboratory
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
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
        private void ButtonPowerJsonClick(object sender, EventArgs e)
        {
            this.labellastrun.Text = string.Empty;
            var numof = int.Parse(this.textBoxNum.Text);
            for (var i = 0; i < numof; i++)
            {
                if (!EventLog.SourceExists(this.comboBoxsource.Text))
                {
                    EventLog.CreateEventSource(this.comboBoxsource.Text, "Application");
                }
                string message = $"{Environment.MachineName}: {textBoxMessage.Text} {DateTime.Now.ToString()}";
                EventLog.WriteEntry(
                    this.comboBoxsource.Text,
                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    message,
                    EventLogEntryType.Error);
            }

            this.labellastrun.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The demos_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DemosLoad(object sender, EventArgs e)
        {
            this.comboBoxsource.SelectedIndex = 0;
        }

        /// <summary>
        /// The label 2_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Label2Click(object sender, EventArgs e)
        {
        }
    }
}