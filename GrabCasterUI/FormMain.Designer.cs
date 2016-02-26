namespace GrabCasterUI
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSyncronize = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerMainHorrizzontal = new System.Windows.Forms.SplitContainer();
            this.splitContainerMainVertical = new System.Windows.Forms.SplitContainer();
            this.splitContainerOrizLeft = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageListTreeview = new System.Windows.Forms.ImageList(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.splitContainerOrizRight = new System.Windows.Forms.SplitContainer();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.textBoxConsole = new System.Windows.Forms.TextBox();
            this.imageListMainToolbar = new System.Windows.Forms.ImageList(this.components);
            this.userControlComponent1 = new GrabCasterUI.UserControlComponent();
            this.menuStrip1.SuspendLayout();
            this.toolStripMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainHorrizzontal)).BeginInit();
            this.splitContainerMainHorrizzontal.Panel1.SuspendLayout();
            this.splitContainerMainHorrizzontal.Panel2.SuspendLayout();
            this.splitContainerMainHorrizzontal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainVertical)).BeginInit();
            this.splitContainerMainVertical.Panel1.SuspendLayout();
            this.splitContainerMainVertical.Panel2.SuspendLayout();
            this.splitContainerMainVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOrizLeft)).BeginInit();
            this.splitContainerOrizLeft.Panel1.SuspendLayout();
            this.splitContainerOrizLeft.Panel2.SuspendLayout();
            this.splitContainerOrizLeft.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOrizRight)).BeginInit();
            this.splitContainerOrizRight.Panel1.SuspendLayout();
            this.splitContainerOrizRight.Panel2.SuspendLayout();
            this.splitContainerOrizRight.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1284, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(26, 26);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSyncronize,
            this.toolStripButtonRefresh,
            this.toolStripButtonSettings,
            this.toolStripButton1});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 24);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(1284, 33);
            this.toolStripMenu.TabIndex = 1;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // toolStripButtonSyncronize
            // 
            this.toolStripButtonSyncronize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSyncronize.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSyncronize.Image")));
            this.toolStripButtonSyncronize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSyncronize.Name = "toolStripButtonSyncronize";
            this.toolStripButtonSyncronize.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonSyncronize.Text = "toolStripButtonSyncronize";
            this.toolStripButtonSyncronize.Click += new System.EventHandler(this.toolStripButtonSyncronize_Click);
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonRefresh.Text = "toolStripButton1";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
            // 
            // toolStripButtonSettings
            // 
            this.toolStripButtonSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSettings.Image")));
            this.toolStripButtonSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSettings.Name = "toolStripButtonSettings";
            this.toolStripButtonSettings.Size = new System.Drawing.Size(30, 30);
            this.toolStripButtonSettings.Text = "toolStripButtonSettings";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(30, 30);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 742);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1284, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessage
            // 
            this.toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
            this.toolStripStatusLabelMessage.Size = new System.Drawing.Size(158, 17);
            this.toolStripStatusLabelMessage.Text = "toolStripStatusLabelMessage";
            this.toolStripStatusLabelMessage.Click += new System.EventHandler(this.toolStripStatusLabelMessage_Click);
            // 
            // splitContainerMainHorrizzontal
            // 
            this.splitContainerMainHorrizzontal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMainHorrizzontal.Location = new System.Drawing.Point(0, 57);
            this.splitContainerMainHorrizzontal.Name = "splitContainerMainHorrizzontal";
            this.splitContainerMainHorrizzontal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMainHorrizzontal.Panel1
            // 
            this.splitContainerMainHorrizzontal.Panel1.Controls.Add(this.splitContainerMainVertical);
            // 
            // splitContainerMainHorrizzontal.Panel2
            // 
            this.splitContainerMainHorrizzontal.Panel2.Controls.Add(this.textBoxConsole);
            this.splitContainerMainHorrizzontal.Size = new System.Drawing.Size(1284, 685);
            this.splitContainerMainHorrizzontal.SplitterDistance = 611;
            this.splitContainerMainHorrizzontal.TabIndex = 3;
            // 
            // splitContainerMainVertical
            // 
            this.splitContainerMainVertical.BackColor = System.Drawing.Color.Black;
            this.splitContainerMainVertical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMainVertical.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMainVertical.Name = "splitContainerMainVertical";
            // 
            // splitContainerMainVertical.Panel1
            // 
            this.splitContainerMainVertical.Panel1.Controls.Add(this.splitContainerOrizLeft);
            this.splitContainerMainVertical.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainerMainVertical.Panel2
            // 
            this.splitContainerMainVertical.Panel2.Controls.Add(this.splitContainerOrizRight);
            this.splitContainerMainVertical.Panel2.Controls.Add(this.panel2);
            this.splitContainerMainVertical.Size = new System.Drawing.Size(1284, 611);
            this.splitContainerMainVertical.SplitterDistance = 692;
            this.splitContainerMainVertical.SplitterWidth = 6;
            this.splitContainerMainVertical.TabIndex = 0;
            // 
            // splitContainerOrizLeft
            // 
            this.splitContainerOrizLeft.BackColor = System.Drawing.Color.Black;
            this.splitContainerOrizLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOrizLeft.Location = new System.Drawing.Point(0, 22);
            this.splitContainerOrizLeft.Name = "splitContainerOrizLeft";
            this.splitContainerOrizLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerOrizLeft.Panel1
            // 
            this.splitContainerOrizLeft.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainerOrizLeft.Panel2
            // 
            this.splitContainerOrizLeft.Panel2.Controls.Add(this.panel3);
            this.splitContainerOrizLeft.Size = new System.Drawing.Size(692, 589);
            this.splitContainerOrizLeft.SplitterDistance = 331;
            this.splitContainerOrizLeft.SplitterWidth = 10;
            this.splitContainerOrizLeft.TabIndex = 3;
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageListTreeview;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(692, 331);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // imageListTreeview
            // 
            this.imageListTreeview.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeview.ImageStream")));
            this.imageListTreeview.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeview.Images.SetKeyName(0, "BUBBLING");
            this.imageListTreeview.Images.SetKeyName(1, "COMPONENT");
            this.imageListTreeview.Images.SetKeyName(2, "COMPONENTS");
            this.imageListTreeview.Images.SetKeyName(3, "EVENT");
            this.imageListTreeview.Images.SetKeyName(4, "EVENTS");
            this.imageListTreeview.Images.SetKeyName(5, "TRIGGERS");
            this.imageListTreeview.Images.SetKeyName(6, "FOLDER");
            this.imageListTreeview.Images.SetKeyName(7, "POINT");
            this.imageListTreeview.Images.SetKeyName(8, "CORRELATION");
            this.imageListTreeview.Images.SetKeyName(9, "EVENTOFF");
            this.imageListTreeview.Images.SetKeyName(10, "EVENTON");
            this.imageListTreeview.Images.SetKeyName(11, "TRIGGERON");
            this.imageListTreeview.Images.SetKeyName(12, "TRIGGEROFF");
            this.imageListTreeview.Images.SetKeyName(13, "TRIGGER");
            this.imageListTreeview.Images.SetKeyName(14, "EVENTCOMPONENT");
            this.imageListTreeview.Images.SetKeyName(15, "TRIGGERCOMPONENT");
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.userControlComponent1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(692, 248);
            this.panel3.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(692, 22);
            this.panel1.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(0, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(692, 24);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // splitContainerOrizRight
            // 
            this.splitContainerOrizRight.BackColor = System.Drawing.Color.Black;
            this.splitContainerOrizRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOrizRight.Location = new System.Drawing.Point(0, 22);
            this.splitContainerOrizRight.Name = "splitContainerOrizRight";
            this.splitContainerOrizRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerOrizRight.Panel1
            // 
            this.splitContainerOrizRight.Panel1.Controls.Add(this.treeView2);
            // 
            // splitContainerOrizRight.Panel2
            // 
            this.splitContainerOrizRight.Panel2.Controls.Add(this.propertyGrid2);
            this.splitContainerOrizRight.Size = new System.Drawing.Size(586, 589);
            this.splitContainerOrizRight.SplitterDistance = 334;
            this.splitContainerOrizRight.SplitterWidth = 10;
            this.splitContainerOrizRight.TabIndex = 1;
            // 
            // treeView2
            // 
            this.treeView2.AllowDrop = true;
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView2.ImageIndex = 0;
            this.treeView2.ImageList = this.imageListTreeview;
            this.treeView2.Location = new System.Drawing.Point(0, 0);
            this.treeView2.Name = "treeView2";
            this.treeView2.SelectedImageIndex = 0;
            this.treeView2.Size = new System.Drawing.Size(586, 334);
            this.treeView2.TabIndex = 1;
            this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGrid2.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(586, 245);
            this.propertyGrid2.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(586, 22);
            this.panel2.TabIndex = 0;
            // 
            // comboBox2
            // 
            this.comboBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(0, 0);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(586, 24);
            this.comboBox2.TabIndex = 2;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // textBoxConsole
            // 
            this.textBoxConsole.BackColor = System.Drawing.Color.Black;
            this.textBoxConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxConsole.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConsole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBoxConsole.Location = new System.Drawing.Point(0, 0);
            this.textBoxConsole.Multiline = true;
            this.textBoxConsole.Name = "textBoxConsole";
            this.textBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxConsole.Size = new System.Drawing.Size(1284, 70);
            this.textBoxConsole.TabIndex = 0;
            this.textBoxConsole.Text = "test message";
            // 
            // imageListMainToolbar
            // 
            this.imageListMainToolbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMainToolbar.ImageStream")));
            this.imageListMainToolbar.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMainToolbar.Images.SetKeyName(0, "bar_refresh.fw.png");
            this.imageListMainToolbar.Images.SetKeyName(1, "bar_setting.fw.png");
            this.imageListMainToolbar.Images.SetKeyName(2, "barr_add.fw.png");
            // 
            // userControlComponent1
            // 
            this.userControlComponent1.BackColor = System.Drawing.SystemColors.Control;
            this.userControlComponent1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlComponent1.Location = new System.Drawing.Point(0, 0);
            this.userControlComponent1.Name = "userControlComponent1";
            this.userControlComponent1.Size = new System.Drawing.Size(692, 248);
            this.userControlComponent1.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 764);
            this.Controls.Add(this.splitContainerMainHorrizzontal);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStripMenu);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "GrabCaster";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainerMainHorrizzontal.Panel1.ResumeLayout(false);
            this.splitContainerMainHorrizzontal.Panel2.ResumeLayout(false);
            this.splitContainerMainHorrizzontal.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainHorrizzontal)).EndInit();
            this.splitContainerMainHorrizzontal.ResumeLayout(false);
            this.splitContainerMainVertical.Panel1.ResumeLayout(false);
            this.splitContainerMainVertical.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainVertical)).EndInit();
            this.splitContainerMainVertical.ResumeLayout(false);
            this.splitContainerOrizLeft.Panel1.ResumeLayout(false);
            this.splitContainerOrizLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOrizLeft)).EndInit();
            this.splitContainerOrizLeft.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainerOrizRight.Panel1.ResumeLayout(false);
            this.splitContainerOrizRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOrizRight)).EndInit();
            this.splitContainerOrizRight.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage;
        private System.Windows.Forms.SplitContainer splitContainerMainHorrizzontal;
        private System.Windows.Forms.SplitContainer splitContainerMainVertical;
        private System.Windows.Forms.TextBox textBoxConsole;
        private System.Windows.Forms.ImageList imageListMainToolbar;
        private System.Windows.Forms.ImageList imageListTreeview;
        private System.Windows.Forms.ToolStripButton toolStripButtonSettings;
        private System.Windows.Forms.SplitContainer splitContainerOrizLeft;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.SplitContainer splitContainerOrizRight;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSyncronize;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private UserControlComponent userControlComponent1;
    }
}

