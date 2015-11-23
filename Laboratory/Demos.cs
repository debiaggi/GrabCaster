// --------------------------------------------------------------------------------------------------
// <copyright file = "Demos.cs" company="Nino Crudele">
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
//    
//    This work is registered with the UK Copyright Service.
//    Registration No:284695248  
//  </summary>
// --------------------------------------------------------------------------------------------------
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

                EventLog.WriteEntry(
                    this.comboBoxsource.Text,
                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    "New message in the eventviewer " + DateTime.Now.ToString(),
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