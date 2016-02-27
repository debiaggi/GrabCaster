namespace GrabCasterUI
{
    partial class UserControlConfiguration
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelIdConfiguration = new System.Windows.Forms.Label();
            this.textBoxIdConfiguration = new System.Windows.Forms.TextBox();
            this.dataGridViewProperties = new System.Windows.Forms.DataGridView();
            this.labelProperties = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelIdConfiguration
            // 
            this.labelIdConfiguration.AutoSize = true;
            this.labelIdConfiguration.Location = new System.Drawing.Point(2, 6);
            this.labelIdConfiguration.Name = "labelIdConfiguration";
            this.labelIdConfiguration.Size = new System.Drawing.Size(78, 13);
            this.labelIdConfiguration.TabIndex = 0;
            this.labelIdConfiguration.Text = "IdConfiguration";
            // 
            // textBoxIdConfiguration
            // 
            this.textBoxIdConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxIdConfiguration.Location = new System.Drawing.Point(86, 3);
            this.textBoxIdConfiguration.Name = "textBoxIdConfiguration";
            this.textBoxIdConfiguration.Size = new System.Drawing.Size(431, 20);
            this.textBoxIdConfiguration.TabIndex = 1;
            // 
            // dataGridViewProperties
            // 
            this.dataGridViewProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProperties.Location = new System.Drawing.Point(5, 45);
            this.dataGridViewProperties.Name = "dataGridViewProperties";
            this.dataGridViewProperties.Size = new System.Drawing.Size(512, 347);
            this.dataGridViewProperties.TabIndex = 8;
            this.dataGridViewProperties.Resize += new System.EventHandler(this.dataGridViewProperties_Resize);
            // 
            // labelProperties
            // 
            this.labelProperties.AutoSize = true;
            this.labelProperties.Location = new System.Drawing.Point(2, 29);
            this.labelProperties.Name = "labelProperties";
            this.labelProperties.Size = new System.Drawing.Size(54, 13);
            this.labelProperties.TabIndex = 9;
            this.labelProperties.Text = "Properties";
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(442, 398);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 10;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
    
            // 
            // UserControlConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelProperties);
            this.Controls.Add(this.dataGridViewProperties);
            this.Controls.Add(this.textBoxIdConfiguration);
            this.Controls.Add(this.labelIdConfiguration);
            this.Name = "UserControlConfiguration";
            this.Size = new System.Drawing.Size(525, 428);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelIdConfiguration;
        private System.Windows.Forms.TextBox textBoxIdConfiguration;
        private System.Windows.Forms.DataGridView dataGridViewProperties;
        private System.Windows.Forms.Label labelProperties;
        private System.Windows.Forms.Button buttonSave;
    }
}
