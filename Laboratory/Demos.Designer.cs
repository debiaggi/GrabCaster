﻿namespace Lab
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class Demos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonPowerJson = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNum = new System.Windows.Forms.TextBox();
            this.comboBoxsource = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labellastrun = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonPowerJson
            // 
            this.buttonPowerJson.Location = new System.Drawing.Point(77, 195);
            this.buttonPowerJson.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPowerJson.Name = "buttonPowerJson";
            this.buttonPowerJson.Size = new System.Drawing.Size(141, 30);
            this.buttonPowerJson.TabIndex = 0;
            this.buttonPowerJson.Text = "Write in Event Vierwer";
            this.buttonPowerJson.UseVisualStyleBackColor = true;
            this.buttonPowerJson.Click += new System.EventHandler(this.buttonPowerJson_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "How many";
            // 
            // textBoxNum
            // 
            this.textBoxNum.Location = new System.Drawing.Point(77, 11);
            this.textBoxNum.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxNum.Name = "textBoxNum";
            this.textBoxNum.Size = new System.Drawing.Size(143, 20);
            this.textBoxNum.TabIndex = 3;
            this.textBoxNum.Text = "1";
            // 
            // comboBoxsource
            // 
            this.comboBoxsource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxsource.FormattingEnabled = true;
            this.comboBoxsource.Items.AddRange(new object[] {
            "DEMOPOWERSHELLJOB",
            "DEMOTEXTMESSAGE",
            "DEMONOTEPAD",
            "DEMOEV"});
            this.comboBoxsource.Location = new System.Drawing.Point(77, 36);
            this.comboBoxsource.Name = "comboBoxsource";
            this.comboBoxsource.Size = new System.Drawing.Size(302, 21);
            this.comboBoxsource.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 39);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Type";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // labellastrun
            // 
            this.labellastrun.AutoSize = true;
            this.labellastrun.Location = new System.Drawing.Point(234, 210);
            this.labellastrun.Name = "labellastrun";
            this.labellastrun.Size = new System.Drawing.Size(10, 13);
            this.labellastrun.TabIndex = 6;
            this.labellastrun.Text = " ";
            // 
            // Demos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 254);
            this.Controls.Add(this.labellastrun);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxsource);
            this.Controls.Add(this.textBoxNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonPowerJson);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Demos";
            this.Text = "BizTalk_London_Summit";
            this.Load += new System.EventHandler(this.Demos_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button buttonPowerJson;
        private Label label1;
        private TextBox textBoxNum;
        private ComboBox comboBoxsource;
        private Label label2;
        private Label labellastrun;
    }
}