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
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.triggersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItemTrigger = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItemTrigger = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsConfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItemEvent = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItemEvent = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compTriggersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItemCompTrigger = new System.Windows.Forms.ToolStripMenuItem();
            this.delToolStripMenuItemCompTrigger = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToToolStripMenuItemCompTrigger = new System.Windows.Forms.ToolStripMenuItem();
            this.compEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItemCompEvent = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItemCompEvent = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToToolStripMenuItemCompEvent = new System.Windows.Forms.ToolStripMenuItem();
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
            this.panelUCContainer1 = new System.Windows.Forms.Panel();
            this.userControlComponent1 = new GrabCasterUI.UserControlComponent();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.splitContainerOrizRight = new System.Windows.Forms.SplitContainer();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.panelUCContainer2 = new System.Windows.Forms.Panel();
            this.userControlComponent2 = new GrabCasterUI.UserControlComponent();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.textBoxConsole = new System.Windows.Forms.TextBox();
            this.imageListMainToolbar = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripTriggers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStripEvents = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStripTriggersComponent = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStripEventComponents = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            this.panelUCContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOrizRight)).BeginInit();
            this.splitContainerOrizRight.Panel1.SuspendLayout();
            this.splitContainerOrizRight.Panel2.SuspendLayout();
            this.splitContainerOrizRight.SuspendLayout();
            this.panelUCContainer2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.eventsToolStripMenuItem});
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
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.triggersToolStripMenuItem,
            this.eventsConfToolStripMenuItem});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.configurationToolStripMenuItem.Text = "Configuration";
            // 
            // triggersToolStripMenuItem
            // 
            this.triggersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItemTrigger,
            this.deleteToolStripMenuItemTrigger});
            this.triggersToolStripMenuItem.Name = "triggersToolStripMenuItem";
            this.triggersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.triggersToolStripMenuItem.Text = "Triggers";
            // 
            // newToolStripMenuItemTrigger
            // 
            this.newToolStripMenuItemTrigger.Name = "newToolStripMenuItemTrigger";
            this.newToolStripMenuItemTrigger.Size = new System.Drawing.Size(107, 22);
            this.newToolStripMenuItemTrigger.Text = "New";
            // 
            // deleteToolStripMenuItemTrigger
            // 
            this.deleteToolStripMenuItemTrigger.Name = "deleteToolStripMenuItemTrigger";
            this.deleteToolStripMenuItemTrigger.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItemTrigger.Text = "Delete";
            // 
            // eventsConfToolStripMenuItem
            // 
            this.eventsConfToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItemEvent,
            this.deleteToolStripMenuItemEvent});
            this.eventsConfToolStripMenuItem.Name = "eventsConfToolStripMenuItem";
            this.eventsConfToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.eventsConfToolStripMenuItem.Text = "Events";
            // 
            // newToolStripMenuItemEvent
            // 
            this.newToolStripMenuItemEvent.Name = "newToolStripMenuItemEvent";
            this.newToolStripMenuItemEvent.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItemEvent.Text = "New";
            // 
            // deleteToolStripMenuItemEvent
            // 
            this.deleteToolStripMenuItemEvent.Name = "deleteToolStripMenuItemEvent";
            this.deleteToolStripMenuItemEvent.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItemEvent.Text = "Delete";
            // 
            // eventsToolStripMenuItem
            // 
            this.eventsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compTriggersToolStripMenuItem,
            this.compEventsToolStripMenuItem});
            this.eventsToolStripMenuItem.Name = "eventsToolStripMenuItem";
            this.eventsToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.eventsToolStripMenuItem.Text = "Components";
            // 
            // compTriggersToolStripMenuItem
            // 
            this.compTriggersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItemCompTrigger,
            this.delToolStripMenuItemCompTrigger,
            this.sendToToolStripMenuItemCompTrigger});
            this.compTriggersToolStripMenuItem.Name = "compTriggersToolStripMenuItem";
            this.compTriggersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.compTriggersToolStripMenuItem.Text = "Triggers";
            // 
            // newToolStripMenuItemCompTrigger
            // 
            this.newToolStripMenuItemCompTrigger.Name = "newToolStripMenuItemCompTrigger";
            this.newToolStripMenuItemCompTrigger.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItemCompTrigger.Text = "New";
            // 
            // delToolStripMenuItemCompTrigger
            // 
            this.delToolStripMenuItemCompTrigger.Name = "delToolStripMenuItemCompTrigger";
            this.delToolStripMenuItemCompTrigger.Size = new System.Drawing.Size(152, 22);
            this.delToolStripMenuItemCompTrigger.Text = "Delete";
            // 
            // sendToToolStripMenuItemCompTrigger
            // 
            this.sendToToolStripMenuItemCompTrigger.Name = "sendToToolStripMenuItemCompTrigger";
            this.sendToToolStripMenuItemCompTrigger.Size = new System.Drawing.Size(152, 22);
            this.sendToToolStripMenuItemCompTrigger.Text = "Send To";
            // 
            // compEventsToolStripMenuItem
            // 
            this.compEventsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItemCompEvent,
            this.deleteToolStripMenuItemCompEvent,
            this.sendToToolStripMenuItemCompEvent});
            this.compEventsToolStripMenuItem.Name = "compEventsToolStripMenuItem";
            this.compEventsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.compEventsToolStripMenuItem.Text = "Events";
            // 
            // newToolStripMenuItemCompEvent
            // 
            this.newToolStripMenuItemCompEvent.Name = "newToolStripMenuItemCompEvent";
            this.newToolStripMenuItemCompEvent.Size = new System.Drawing.Size(116, 22);
            this.newToolStripMenuItemCompEvent.Text = "New";
            // 
            // deleteToolStripMenuItemCompEvent
            // 
            this.deleteToolStripMenuItemCompEvent.Name = "deleteToolStripMenuItemCompEvent";
            this.deleteToolStripMenuItemCompEvent.Size = new System.Drawing.Size(116, 22);
            this.deleteToolStripMenuItemCompEvent.Text = "Delete";
            // 
            // sendToToolStripMenuItemCompEvent
            // 
            this.sendToToolStripMenuItemCompEvent.Name = "sendToToolStripMenuItemCompEvent";
            this.sendToToolStripMenuItemCompEvent.Size = new System.Drawing.Size(116, 22);
            this.sendToToolStripMenuItemCompEvent.Text = "Send To";
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
            this.splitContainerOrizLeft.Panel2.Controls.Add(this.panelUCContainer1);
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
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
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
            // panelUCContainer1
            // 
            this.panelUCContainer1.Controls.Add(this.userControlComponent1);
            this.panelUCContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUCContainer1.Location = new System.Drawing.Point(0, 0);
            this.panelUCContainer1.Name = "panelUCContainer1";
            this.panelUCContainer1.Size = new System.Drawing.Size(692, 248);
            this.panelUCContainer1.TabIndex = 0;
            // 
            // userControlComponent1
            // 
            this.userControlComponent1.BackColor = System.Drawing.SystemColors.Control;
            this.userControlComponent1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlComponent1.Location = new System.Drawing.Point(0, 0);
            this.userControlComponent1.Name = "userControlComponent1";
            this.userControlComponent1.Size = new System.Drawing.Size(692, 248);
            this.userControlComponent1.TabIndex = 0;
            this.userControlComponent1.TreeNodeSide = null;
            this.userControlComponent1.TreeViewSide = null;
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
            this.splitContainerOrizRight.Panel2.Controls.Add(this.panelUCContainer2);
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
            // panelUCContainer2
            // 
            this.panelUCContainer2.Controls.Add(this.userControlComponent2);
            this.panelUCContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUCContainer2.Location = new System.Drawing.Point(0, 0);
            this.panelUCContainer2.Name = "panelUCContainer2";
            this.panelUCContainer2.Size = new System.Drawing.Size(586, 245);
            this.panelUCContainer2.TabIndex = 0;
            // 
            // userControlComponent2
            // 
            this.userControlComponent2.BackColor = System.Drawing.SystemColors.Control;
            this.userControlComponent2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlComponent2.Location = new System.Drawing.Point(0, 0);
            this.userControlComponent2.Name = "userControlComponent2";
            this.userControlComponent2.Size = new System.Drawing.Size(586, 245);
            this.userControlComponent2.TabIndex = 1;
            this.userControlComponent2.TreeNodeSide = null;
            this.userControlComponent2.TreeViewSide = null;
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
            // contextMenuStripTriggers
            // 
            this.contextMenuStripTriggers.Name = "contextMenuStripTriggers";
            this.contextMenuStripTriggers.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStripEvents
            // 
            this.contextMenuStripEvents.Name = "contextMenuStripTriggers";
            this.contextMenuStripEvents.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStripTriggersComponent
            // 
            this.contextMenuStripTriggersComponent.Name = "contextMenuStripTriggers";
            this.contextMenuStripTriggersComponent.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStripEventComponents
            // 
            this.contextMenuStripEventComponents.Name = "contextMenuStripTriggers";
            this.contextMenuStripEventComponents.Size = new System.Drawing.Size(61, 4);
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
            this.panelUCContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainerOrizRight.Panel1.ResumeLayout(false);
            this.splitContainerOrizRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOrizRight)).EndInit();
            this.splitContainerOrizRight.ResumeLayout(false);
            this.panelUCContainer2.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panelUCContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.SplitContainer splitContainerOrizRight;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSyncronize;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Panel panelUCContainer2;
        private UserControlComponent userControlComponent1;
        private UserControlComponent userControlComponent2;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem triggersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItemTrigger;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItemTrigger;
        private System.Windows.Forms.ToolStripMenuItem eventsConfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItemEvent;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItemEvent;
        private System.Windows.Forms.ToolStripMenuItem eventsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compTriggersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItemCompTrigger;
        private System.Windows.Forms.ToolStripMenuItem delToolStripMenuItemCompTrigger;
        private System.Windows.Forms.ToolStripMenuItem sendToToolStripMenuItemCompTrigger;
        private System.Windows.Forms.ToolStripMenuItem compEventsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItemCompEvent;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItemCompEvent;
        private System.Windows.Forms.ToolStripMenuItem sendToToolStripMenuItemCompEvent;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTriggers;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripEvents;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTriggersComponent;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripEventComponents;
    }
}

