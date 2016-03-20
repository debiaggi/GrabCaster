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
            this.labelFile = new System.Windows.Forms.Label();
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.dataGridViewProperties = new System.Windows.Forms.DataGridView();
            this.labelProperties = new System.Windows.Forms.Label();
            this.buttonChange = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(3, 7);
            this.labelFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(30, 17);
            this.labelFile.TabIndex = 0;
            this.labelFile.Text = "File";
            // 
            // textBoxFile
            // 
            this.textBoxFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFile.Enabled = false;
            this.textBoxFile.Location = new System.Drawing.Point(115, 4);
            this.textBoxFile.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.Size = new System.Drawing.Size(782, 22);
            this.textBoxFile.TabIndex = 1;
            // 
            // dataGridViewProperties
            // 
            this.dataGridViewProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProperties.Location = new System.Drawing.Point(7, 55);
            this.dataGridViewProperties.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewProperties.Name = "dataGridViewProperties";
            this.dataGridViewProperties.Size = new System.Drawing.Size(892, 376);
            this.dataGridViewProperties.TabIndex = 8;
            this.dataGridViewProperties.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProperties_CellContentClick);
            this.dataGridViewProperties.Resize += new System.EventHandler(this.dataGridViewProperties_Resize);
            // 
            // labelProperties
            // 
            this.labelProperties.AutoSize = true;
            this.labelProperties.Location = new System.Drawing.Point(3, 36);
            this.labelProperties.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProperties.Name = "labelProperties";
            this.labelProperties.Size = new System.Drawing.Size(73, 17);
            this.labelProperties.TabIndex = 9;
            this.labelProperties.Text = "Properties";
            // 
            // buttonChange
            // 
            this.buttonChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChange.Location = new System.Drawing.Point(798, 439);
            this.buttonChange.Margin = new System.Windows.Forms.Padding(4);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(100, 28);
            this.buttonChange.TabIndex = 10;
            this.buttonChange.Text = "Change";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // UserControlConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelProperties);
            this.Controls.Add(this.textBoxFile);
            this.Controls.Add(this.labelFile);
            this.Controls.Add(this.dataGridViewProperties);
            this.Controls.Add(this.buttonChange);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UserControlConfiguration";
            this.Size = new System.Drawing.Size(909, 476);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.DataGridView dataGridViewProperties;
        private System.Windows.Forms.Label labelProperties;
        private System.Windows.Forms.Button buttonChange;
    }
}
