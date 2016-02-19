namespace GrabCasterUI
{
    partial class FormPoint
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeViewBubbling = new System.Windows.Forms.TreeView();
            this.buttonAskAll = new System.Windows.Forms.Button();
            this.buttonAskSpecific = new System.Windows.Forms.Button();
            this.listBoxGCPoints = new System.Windows.Forms.ListBox();
            this.buttonLoadBubbling = new System.Windows.Forms.Button();
            this.buttonSelecteditem = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeViewBubbling
            // 
            this.treeViewBubbling.Location = new System.Drawing.Point(46, 77);
            this.treeViewBubbling.Name = "treeViewBubbling";
            this.treeViewBubbling.Size = new System.Drawing.Size(271, 290);
            this.treeViewBubbling.TabIndex = 0;
            // 
            // buttonAskAll
            // 
            this.buttonAskAll.Location = new System.Drawing.Point(523, 61);
            this.buttonAskAll.Name = "buttonAskAll";
            this.buttonAskAll.Size = new System.Drawing.Size(103, 23);
            this.buttonAskAll.TabIndex = 1;
            this.buttonAskAll.Text = "Ask All Points";
            this.buttonAskAll.UseVisualStyleBackColor = true;
            this.buttonAskAll.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonAskSpecific
            // 
            this.buttonAskSpecific.Location = new System.Drawing.Point(523, 32);
            this.buttonAskSpecific.Name = "buttonAskSpecific";
            this.buttonAskSpecific.Size = new System.Drawing.Size(103, 23);
            this.buttonAskSpecific.TabIndex = 2;
            this.buttonAskSpecific.Text = "Ask specific";
            this.buttonAskSpecific.UseVisualStyleBackColor = true;
            this.buttonAskSpecific.Click += new System.EventHandler(this.buttonAskSpecific_Click);
            // 
            // listBoxGCPoints
            // 
            this.listBoxGCPoints.FormattingEnabled = true;
            this.listBoxGCPoints.Location = new System.Drawing.Point(476, 373);
            this.listBoxGCPoints.Name = "listBoxGCPoints";
            this.listBoxGCPoints.Size = new System.Drawing.Size(497, 160);
            this.listBoxGCPoints.TabIndex = 3;
            // 
            // buttonLoadBubbling
            // 
            this.buttonLoadBubbling.Location = new System.Drawing.Point(961, 344);
            this.buttonLoadBubbling.Name = "buttonLoadBubbling";
            this.buttonLoadBubbling.Size = new System.Drawing.Size(103, 23);
            this.buttonLoadBubbling.TabIndex = 4;
            this.buttonLoadBubbling.Text = "Load Bubbling";
            this.buttonLoadBubbling.UseVisualStyleBackColor = true;
            this.buttonLoadBubbling.Click += new System.EventHandler(this.buttonLoadBubbling_Click);
            // 
            // buttonSelecteditem
            // 
            this.buttonSelecteditem.Location = new System.Drawing.Point(530, 421);
            this.buttonSelecteditem.Name = "buttonSelecteditem";
            this.buttonSelecteditem.Size = new System.Drawing.Size(74, 27);
            this.buttonSelecteditem.TabIndex = 5;
            this.buttonSelecteditem.Text = "buttonselectit";
            this.buttonSelecteditem.UseVisualStyleBackColor = true;
            this.buttonSelecteditem.Click += new System.EventHandler(this.buttonSelecteditem_Click);
            // 
            // FormPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 682);
            this.Controls.Add(this.buttonSelecteditem);
            this.Controls.Add(this.buttonLoadBubbling);
            this.Controls.Add(this.listBoxGCPoints);
            this.Controls.Add(this.buttonAskSpecific);
            this.Controls.Add(this.buttonAskAll);
            this.Controls.Add(this.treeViewBubbling);
            this.Name = "FormPoint";
            this.Text = "FormPoint";
            this.Load += new System.EventHandler(this.FormPoint_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewBubbling;
        private System.Windows.Forms.Button buttonAskAll;
        private System.Windows.Forms.Button buttonAskSpecific;
        private System.Windows.Forms.ListBox listBoxGCPoints;
        private System.Windows.Forms.Button buttonLoadBubbling;
        private System.Windows.Forms.Button buttonSelecteditem;
    }
}