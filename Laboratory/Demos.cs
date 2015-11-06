// --------------------------------------------------------------------------------------------------
// <copyright file = "Demos.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Copyright (c) 2013 - 2015 Nino Crudele
//    Blog: http://ninocrudele.me
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License. 
// </summary>
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