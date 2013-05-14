namespace Fishing {
    partial class FishingForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FishingForm));
            this.tbChangeName = new System.Windows.Forms.TextBox();
            this.contextMenuStats = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuListBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.changeNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblBait = new System.Windows.Forms.Label();
            this.lblRod = new System.Windows.Forms.Label();
            this.lblBaitHeader = new System.Windows.Forms.Label();
            this.lblZone = new System.Windows.Forms.Label();
            this.lblRodHeader = new System.Windows.Forms.Label();
            this.lblZoneHeader = new System.Windows.Forms.Label();
            this.rtbStats = new System.Windows.Forms.RichTextBox();
            this.pnlLog = new System.Windows.Forms.Panel();
            this.pnlWanted = new System.Windows.Forms.Panel();
            this.cbCatchUnknown = new System.Windows.Forms.CheckBox();
            this.lblWantedHeader = new System.Windows.Forms.Label();
            this.lbWanted = new System.Windows.Forms.ListBox();
            this.pnlUnwanted = new System.Windows.Forms.Panel();
            this.lblUnwantedHeader = new System.Windows.Forms.Label();
            this.lbUnwanted = new System.Windows.Forms.ListBox();
            this.btnRefreshLists = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tabDisplay = new System.Windows.Forms.TabControl();
            this.tabDisplayPageChat = new System.Windows.Forms.TabPage();
            this.btnStartM = new System.Windows.Forms.Button();
            this.btnResize = new System.Windows.Forms.Button();
            this.tbChat = new System.Windows.Forms.TextBox();
            this.btnChatSend = new System.Windows.Forms.Button();
            this.tabChat = new System.Windows.Forms.TabControl();
            this.tabChatPageLog = new System.Windows.Forms.TabPage();
            this.rtbChat = new System.Windows.Forms.RichTextBox();
            this.tabChatPageFishing = new System.Windows.Forms.TabPage();
            this.rtbFish = new System.Windows.Forms.RichTextBox();
            this.tabChatPageTell = new System.Windows.Forms.TabPage();
            this.rtbTell = new System.Windows.Forms.RichTextBox();
            this.tabChatPageParty = new System.Windows.Forms.TabPage();
            this.rtbParty = new System.Windows.Forms.RichTextBox();
            this.tabChatPageLS = new System.Windows.Forms.TabPage();
            this.rtbShell = new System.Windows.Forms.RichTextBox();
            this.tabChatPageSay = new System.Windows.Forms.TabPage();
            this.rtbSay = new System.Windows.Forms.RichTextBox();
            this.tabChatPageDB = new System.Windows.Forms.TabPage();
            this.rtbDB = new System.Windows.Forms.RichTextBox();
            this.tabDisplayPageStats = new System.Windows.Forms.TabPage();
            this.tabDisplayPageInfo = new System.Windows.Forms.TabPage();
            this.lblEarthTimeHeader = new System.Windows.Forms.Label();
            this.lblEarthTime = new System.Windows.Forms.Label();
            this.lblVanaTimeHeader = new System.Windows.Forms.Label();
            this.lblSackSpace = new System.Windows.Forms.Label();
            this.lblNoCatchAt = new System.Windows.Forms.Label();
            this.lblSatchelSpace = new System.Windows.Forms.Label();
            this.lblGil = new System.Windows.Forms.Label();
            this.lblInventorySpace = new System.Windows.Forms.Label();
            this.lblGilHeader = new System.Windows.Forms.Label();
            this.lblSackHeader = new System.Windows.Forms.Label();
            this.lblVanaTime = new System.Windows.Forms.Label();
            this.lblSkill = new System.Windows.Forms.Label();
            this.lblSatchelHeader = new System.Windows.Forms.Label();
            this.lblInventoryHeader = new System.Windows.Forms.Label();
            this.lblSkillHeader = new System.Windows.Forms.Label();
            this.lblNoCatchAtHeader = new System.Windows.Forms.Label();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.tabDisplayPageOptions = new System.Windows.Forms.TabPage();
            this.tabOptions = new System.Windows.Forms.TabControl();
            this.tabOptionsPageForm = new System.Windows.Forms.TabPage();
            this.numSkillCap = new System.Windows.Forms.NumericUpDown();
            this.cbSkillCap = new System.Windows.Forms.CheckBox();
            this.cbTellDetect = new System.Windows.Forms.CheckBox();
            this.cbSneakFishing = new System.Windows.Forms.CheckBox();
            this.numMaxCatch = new System.Windows.Forms.NumericUpDown();
            this.cbMaxCatch = new System.Windows.Forms.CheckBox();
            this.gbGeneralFishing = new System.Windows.Forms.GroupBox();
            this.lblCastWait = new System.Windows.Forms.Label();
            this.numMaxNoCatch = new System.Windows.Forms.NumericUpDown();
            this.btnCastReset = new System.Windows.Forms.Button();
            this.numCastIntervalLow = new System.Windows.Forms.NumericUpDown();
            this.lblMaxNoCatch = new System.Windows.Forms.Label();
            this.numCastIntervalHigh = new System.Windows.Forms.NumericUpDown();
            this.trackOpacity = new System.Windows.Forms.TrackBar();
            this.cbStopSound = new System.Windows.Forms.CheckBox();
            this.btnSettingsSave = new System.Windows.Forms.Button();
            this.btnSettingsReset = new System.Windows.Forms.Button();
            this.cbAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.tabOptionsPageChat = new System.Windows.Forms.TabPage();
            this.cbChatDetect = new System.Windows.Forms.CheckBox();
            this.panelChatDetect = new System.Windows.Forms.Panel();
            this.btnChatDetectAdd = new System.Windows.Forms.Button();
            this.gbGMDetect = new System.Windows.Forms.GroupBox();
            this.cbGMdetectAutostop = new System.Windows.Forms.CheckBox();
            this.tabOptionsPageFight = new System.Windows.Forms.TabPage();
            this.cbFishHP = new System.Windows.Forms.CheckBox();
            this.numReactionHigh = new System.Windows.Forms.NumericUpDown();
            this.numReactionLow = new System.Windows.Forms.NumericUpDown();
            this.cbReaction = new System.Windows.Forms.CheckBox();
            this.numFakeLargeIntervalLow = new System.Windows.Forms.NumericUpDown();
            this.cbReleaseSmall = new System.Windows.Forms.CheckBox();
            this.numFakeLargeIntervalHigh = new System.Windows.Forms.NumericUpDown();
            this.numFakeSmallIntervalLow = new System.Windows.Forms.NumericUpDown();
            this.cbReleaseLarge = new System.Windows.Forms.CheckBox();
            this.cbIgnoreLargeFish = new System.Windows.Forms.CheckBox();
            this.numFakeSmallIntervalHigh = new System.Windows.Forms.NumericUpDown();
            this.cbIgnoreSmallFish = new System.Windows.Forms.CheckBox();
            this.cbIgnoreItem = new System.Windows.Forms.CheckBox();
            this.cbIgnoreMonster = new System.Windows.Forms.CheckBox();
            this.lblKillSeconds = new System.Windows.Forms.Label();
            this.numQuickKill = new System.Windows.Forms.NumericUpDown();
            this.cbQuickKill = new System.Windows.Forms.CheckBox();
            this.cbAutoKill = new System.Windows.Forms.CheckBox();
            this.cbExtend = new System.Windows.Forms.CheckBox();
            this.tabOptionsPageGear = new System.Windows.Forms.TabPage();
            this.tbBaitGear = new System.Windows.Forms.ComboBox();
            this.tbRodGear = new System.Windows.Forms.ComboBox();
            this.lblBaitGear = new System.Windows.Forms.Label();
            this.lblRodGear = new System.Windows.Forms.Label();
            this.lblBodyGear = new System.Windows.Forms.Label();
            this.tbBodyGear = new System.Windows.Forms.ComboBox();
            this.lblHandsGear = new System.Windows.Forms.Label();
            this.tbHandsGear = new System.Windows.Forms.ComboBox();
            this.lblLegsGear = new System.Windows.Forms.Label();
            this.tbLegsGear = new System.Windows.Forms.ComboBox();
            this.lblFeetGear = new System.Windows.Forms.Label();
            this.tbFeetGear = new System.Windows.Forms.ComboBox();
            this.lblLRingGear = new System.Windows.Forms.Label();
            this.tbLRingGear = new System.Windows.Forms.ComboBox();
            this.cbLRingGear = new System.Windows.Forms.CheckBox();
            this.lblRRingGear = new System.Windows.Forms.Label();
            this.tbRRingGear = new System.Windows.Forms.ComboBox();
            this.cbRRingGear = new System.Windows.Forms.CheckBox();
            this.lblHeadGear = new System.Windows.Forms.Label();
            this.tbHeadGear = new System.Windows.Forms.ComboBox();
            this.lblNeckGear = new System.Windows.Forms.Label();
            this.tbNeckGear = new System.Windows.Forms.ComboBox();
            this.lblWaistGear = new System.Windows.Forms.Label();
            this.tbWaistGear = new System.Windows.Forms.ComboBox();
            this.cbWaistGear = new System.Windows.Forms.CheckBox();
            this.tabOptionsPageAdvanced = new System.Windows.Forms.TabPage();
            this.gbExhaustedBait = new System.Windows.Forms.GroupBox();
            this.cbBaitItemizerSatchel = new System.Windows.Forms.CheckBox();
            this.cbBaitItemizerSack = new System.Windows.Forms.CheckBox();
            this.numBaitactionOtherTime = new System.Windows.Forms.NumericUpDown();
            this.tbBaitactionOther = new System.Windows.Forms.TextBox();
            this.cbBaitactionOther = new System.Windows.Forms.CheckBox();
            this.cbBaitItemizerItemTools = new System.Windows.Forms.CheckBox();
            this.cbBaitActionWarp = new System.Windows.Forms.CheckBox();
            this.cbBaitActionLogout = new System.Windows.Forms.CheckBox();
            this.cbBaitActionShutdown = new System.Windows.Forms.CheckBox();
            this.gbOnFatigue = new System.Windows.Forms.GroupBox();
            this.cbFatiguedActionWarp = new System.Windows.Forms.CheckBox();
            this.cbFatiguedActionLogout = new System.Windows.Forms.CheckBox();
            this.cbFatiguedActionShutdown = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbFullactionOther = new System.Windows.Forms.TextBox();
            this.cbInventoryItemizerSatchel = new System.Windows.Forms.CheckBox();
            this.cbInventoryItemizerSack = new System.Windows.Forms.CheckBox();
            this.numFullactionOtherTime = new System.Windows.Forms.NumericUpDown();
            this.cbFullActionStop = new System.Windows.Forms.CheckBox();
            this.cbFullactionOther = new System.Windows.Forms.CheckBox();
            this.cbFullactionShutdown = new System.Windows.Forms.CheckBox();
            this.cbFullactionLogout = new System.Windows.Forms.CheckBox();
            this.cbFullactionWarp = new System.Windows.Forms.CheckBox();
            this.cbInventoryItemizerItemTools = new System.Windows.Forms.CheckBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.lblVanaDay = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVanaClock = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHP = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBarST = new System.Windows.Forms.ToolStripProgressBar();
            this.contextMenuStats.SuspendLayout();
            this.contextMenuListBox.SuspendLayout();
            this.pnlLog.SuspendLayout();
            this.pnlWanted.SuspendLayout();
            this.pnlUnwanted.SuspendLayout();
            this.tabDisplay.SuspendLayout();
            this.tabDisplayPageChat.SuspendLayout();
            this.tabChat.SuspendLayout();
            this.tabChatPageLog.SuspendLayout();
            this.tabChatPageFishing.SuspendLayout();
            this.tabChatPageTell.SuspendLayout();
            this.tabChatPageParty.SuspendLayout();
            this.tabChatPageLS.SuspendLayout();
            this.tabChatPageSay.SuspendLayout();
            this.tabChatPageDB.SuspendLayout();
            this.tabDisplayPageStats.SuspendLayout();
            this.tabDisplayPageInfo.SuspendLayout();
            this.gbInfo.SuspendLayout();
            this.tabDisplayPageOptions.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.tabOptionsPageForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSkillCap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCatch)).BeginInit();
            this.gbGeneralFishing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxNoCatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCastIntervalLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCastIntervalHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackOpacity)).BeginInit();
            this.tabOptionsPageChat.SuspendLayout();
            this.panelChatDetect.SuspendLayout();
            this.gbGMDetect.SuspendLayout();
            this.tabOptionsPageFight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReactionHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReactionLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFakeLargeIntervalLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFakeLargeIntervalHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFakeSmallIntervalLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFakeSmallIntervalHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuickKill)).BeginInit();
            this.tabOptionsPageGear.SuspendLayout();
            this.tabOptionsPageAdvanced.SuspendLayout();
            this.gbExhaustedBait.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBaitactionOtherTime)).BeginInit();
            this.gbOnFatigue.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFullactionOtherTime)).BeginInit();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbChangeName
            // 
            this.tbChangeName.Location = new System.Drawing.Point(105, 44);
            this.tbChangeName.Margin = new System.Windows.Forms.Padding(0);
            this.tbChangeName.MaxLength = 45;
            this.tbChangeName.Name = "tbChangeName";
            this.tbChangeName.ShortcutsEnabled = false;
            this.tbChangeName.Size = new System.Drawing.Size(10, 20);
            this.tbChangeName.TabIndex = 32;
            this.tbChangeName.TabStop = false;
            this.tbChangeName.Visible = false;
            this.tbChangeName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbChangeName_KeyPress);
            this.tbChangeName.LostFocus += new System.EventHandler(this.tbChangeName_LostFocus);
            // 
            // contextMenuStats
            // 
            this.contextMenuStats.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearLogToolStripMenuItem});
            this.contextMenuStats.Name = "contextMenuLog";
            this.contextMenuStats.Size = new System.Drawing.Size(130, 26);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.clearLogToolStripMenuItem.Text = "Clear Stats";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // contextMenuListBox
            // 
            this.contextMenuListBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeNameToolStripMenuItem});
            this.contextMenuListBox.Name = "contextMenuListBox";
            this.contextMenuListBox.Size = new System.Drawing.Size(151, 26);
            // 
            // changeNameToolStripMenuItem
            // 
            this.changeNameToolStripMenuItem.Name = "changeNameToolStripMenuItem";
            this.changeNameToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.changeNameToolStripMenuItem.Text = "Change Name";
            this.changeNameToolStripMenuItem.Click += new System.EventHandler(this.changeNameToolStripMenuItem_Click);
            // 
            // lblBait
            // 
            this.lblBait.AutoSize = true;
            this.lblBait.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBait.Location = new System.Drawing.Point(76, 47);
            this.lblBait.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBait.Name = "lblBait";
            this.lblBait.Size = new System.Drawing.Size(13, 13);
            this.lblBait.TabIndex = 5;
            this.lblBait.Text = "--";
            // 
            // lblRod
            // 
            this.lblRod.AutoSize = true;
            this.lblRod.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRod.Location = new System.Drawing.Point(76, 28);
            this.lblRod.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRod.Name = "lblRod";
            this.lblRod.Size = new System.Drawing.Size(13, 13);
            this.lblRod.TabIndex = 3;
            this.lblRod.Text = "--";
            // 
            // lblBaitHeader
            // 
            this.lblBaitHeader.AutoSize = true;
            this.lblBaitHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBaitHeader.Location = new System.Drawing.Point(5, 48);
            this.lblBaitHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblBaitHeader.Name = "lblBaitHeader";
            this.lblBaitHeader.Size = new System.Drawing.Size(32, 12);
            this.lblBaitHeader.TabIndex = 4;
            this.lblBaitHeader.Text = "Bait: ";
            // 
            // lblZone
            // 
            this.lblZone.AutoSize = true;
            this.lblZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZone.Location = new System.Drawing.Point(76, 9);
            this.lblZone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblZone.Name = "lblZone";
            this.lblZone.Size = new System.Drawing.Size(13, 13);
            this.lblZone.TabIndex = 1;
            this.lblZone.Text = "--";
            // 
            // lblRodHeader
            // 
            this.lblRodHeader.AutoSize = true;
            this.lblRodHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRodHeader.Location = new System.Drawing.Point(5, 29);
            this.lblRodHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblRodHeader.Name = "lblRodHeader";
            this.lblRodHeader.Size = new System.Drawing.Size(32, 12);
            this.lblRodHeader.TabIndex = 2;
            this.lblRodHeader.Text = "Rod: ";
            // 
            // lblZoneHeader
            // 
            this.lblZoneHeader.AutoSize = true;
            this.lblZoneHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZoneHeader.Location = new System.Drawing.Point(5, 10);
            this.lblZoneHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblZoneHeader.Name = "lblZoneHeader";
            this.lblZoneHeader.Size = new System.Drawing.Size(34, 12);
            this.lblZoneHeader.TabIndex = 0;
            this.lblZoneHeader.Text = "Zone:";
            // 
            // rtbStats
            // 
            this.rtbStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbStats.ContextMenuStrip = this.contextMenuStats;
            this.rtbStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbStats.Location = new System.Drawing.Point(0, 0);
            this.rtbStats.Margin = new System.Windows.Forms.Padding(0);
            this.rtbStats.Name = "rtbStats";
            this.rtbStats.ReadOnly = true;
            this.rtbStats.Size = new System.Drawing.Size(372, 202);
            this.rtbStats.TabIndex = 10;
            this.rtbStats.Text = "";
            // 
            // pnlLog
            // 
            this.pnlLog.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLog.Controls.Add(this.pnlWanted);
            this.pnlLog.Controls.Add(this.pnlUnwanted);
            this.pnlLog.Controls.Add(this.btnRefreshLists);
            this.pnlLog.Controls.Add(this.btnStart);
            this.pnlLog.Controls.Add(this.tabDisplay);
            this.pnlLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLog.Location = new System.Drawing.Point(0, 0);
            this.pnlLog.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLog.Name = "pnlLog";
            this.pnlLog.Size = new System.Drawing.Size(526, 252);
            this.pnlLog.TabIndex = 0;
            // 
            // pnlWanted
            // 
            this.pnlWanted.Controls.Add(this.cbCatchUnknown);
            this.pnlWanted.Controls.Add(this.lblWantedHeader);
            this.pnlWanted.Controls.Add(this.lbWanted);
            this.pnlWanted.Location = new System.Drawing.Point(3, 33);
            this.pnlWanted.Name = "pnlWanted";
            this.pnlWanted.Size = new System.Drawing.Size(140, 87);
            this.pnlWanted.TabIndex = 2;
            // 
            // cbCatchUnknown
            // 
            this.cbCatchUnknown.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCatchUnknown.Location = new System.Drawing.Point(59, 1);
            this.cbCatchUnknown.Margin = new System.Windows.Forms.Padding(0);
            this.cbCatchUnknown.Name = "cbCatchUnknown";
            this.cbCatchUnknown.Size = new System.Drawing.Size(81, 15);
            this.cbCatchUnknown.TabIndex = 1;
            this.cbCatchUnknown.Text = "Unknowns?";
            this.cbCatchUnknown.UseVisualStyleBackColor = true;
            // 
            // lblWantedHeader
            // 
            this.lblWantedHeader.AutoSize = true;
            this.lblWantedHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblWantedHeader.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblWantedHeader.Location = new System.Drawing.Point(0, 2);
            this.lblWantedHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblWantedHeader.Name = "lblWantedHeader";
            this.lblWantedHeader.Size = new System.Drawing.Size(45, 13);
            this.lblWantedHeader.TabIndex = 0;
            this.lblWantedHeader.Text = "Wanted";
            // 
            // lbWanted
            // 
            this.lbWanted.BackColor = System.Drawing.SystemColors.Window;
            this.lbWanted.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbWanted.FormattingEnabled = true;
            this.lbWanted.Location = new System.Drawing.Point(0, 16);
            this.lbWanted.Margin = new System.Windows.Forms.Padding(0);
            this.lbWanted.Name = "lbWanted";
            this.lbWanted.Size = new System.Drawing.Size(140, 69);
            this.lbWanted.Sorted = true;
            this.lbWanted.TabIndex = 2;
            this.lbWanted.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseDoubleClick);
            this.lbWanted.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseDown);
            // 
            // pnlUnwanted
            // 
            this.pnlUnwanted.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlUnwanted.Controls.Add(this.lblUnwantedHeader);
            this.pnlUnwanted.Controls.Add(this.lbUnwanted);
            this.pnlUnwanted.Location = new System.Drawing.Point(3, 121);
            this.pnlUnwanted.Margin = new System.Windows.Forms.Padding(0);
            this.pnlUnwanted.Name = "pnlUnwanted";
            this.pnlUnwanted.Size = new System.Drawing.Size(140, 106);
            this.pnlUnwanted.TabIndex = 3;
            // 
            // lblUnwantedHeader
            // 
            this.lblUnwantedHeader.AutoSize = true;
            this.lblUnwantedHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblUnwantedHeader.ForeColor = System.Drawing.Color.Maroon;
            this.lblUnwantedHeader.Location = new System.Drawing.Point(0, 2);
            this.lblUnwantedHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblUnwantedHeader.Name = "lblUnwantedHeader";
            this.lblUnwantedHeader.Size = new System.Drawing.Size(56, 13);
            this.lblUnwantedHeader.TabIndex = 0;
            this.lblUnwantedHeader.Text = "Unwanted";
            // 
            // lbUnwanted
            // 
            this.lbUnwanted.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbUnwanted.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbUnwanted.FormattingEnabled = true;
            this.lbUnwanted.IntegralHeight = false;
            this.lbUnwanted.Location = new System.Drawing.Point(0, 16);
            this.lbUnwanted.Margin = new System.Windows.Forms.Padding(0);
            this.lbUnwanted.Name = "lbUnwanted";
            this.lbUnwanted.Size = new System.Drawing.Size(140, 86);
            this.lbUnwanted.Sorted = true;
            this.lbUnwanted.TabIndex = 1;
            this.lbUnwanted.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseDoubleClick);
            this.lbUnwanted.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseDown);
            // 
            // btnRefreshLists
            // 
            this.btnRefreshLists.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnRefreshLists.Image = global::Fishing.Properties.Resources.refresh;
            this.btnRefreshLists.Location = new System.Drawing.Point(113, 0);
            this.btnRefreshLists.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefreshLists.Name = "btnRefreshLists";
            this.btnRefreshLists.Size = new System.Drawing.Size(31, 30);
            this.btnRefreshLists.TabIndex = 1;
            this.btnRefreshLists.UseVisualStyleBackColor = true;
            this.btnRefreshLists.Click += new System.EventHandler(this.btnRefreshLists_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(2, 0);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(112, 30);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tabDisplay
            // 
            this.tabDisplay.AccessibleName = "";
            this.tabDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabDisplay.Controls.Add(this.tabDisplayPageChat);
            this.tabDisplay.Controls.Add(this.tabDisplayPageStats);
            this.tabDisplay.Controls.Add(this.tabDisplayPageInfo);
            this.tabDisplay.Controls.Add(this.tabDisplayPageOptions);
            this.tabDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.tabDisplay.HotTrack = true;
            this.tabDisplay.Location = new System.Drawing.Point(146, 0);
            this.tabDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.Padding = new System.Drawing.Point(0, 0);
            this.tabDisplay.SelectedIndex = 0;
            this.tabDisplay.Size = new System.Drawing.Size(380, 227);
            this.tabDisplay.TabIndex = 8;
            this.tabDisplay.SelectedIndexChanged += new System.EventHandler(this.tabDisplay_SelectedIndexChanged);
            // 
            // tabDisplayPageChat
            // 
            this.tabDisplayPageChat.BackColor = System.Drawing.SystemColors.Control;
            this.tabDisplayPageChat.Controls.Add(this.btnStartM);
            this.tabDisplayPageChat.Controls.Add(this.btnResize);
            this.tabDisplayPageChat.Controls.Add(this.tbChat);
            this.tabDisplayPageChat.Controls.Add(this.btnChatSend);
            this.tabDisplayPageChat.Controls.Add(this.tabChat);
            this.tabDisplayPageChat.Location = new System.Drawing.Point(4, 21);
            this.tabDisplayPageChat.Name = "tabDisplayPageChat";
            this.tabDisplayPageChat.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisplayPageChat.Size = new System.Drawing.Size(372, 202);
            this.tabDisplayPageChat.TabIndex = 3;
            this.tabDisplayPageChat.Text = "Chat";
            // 
            // btnStartM
            // 
            this.btnStartM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartM.Image = global::Fishing.Properties.Resources.icon_play;
            this.btnStartM.Location = new System.Drawing.Point(286, -1);
            this.btnStartM.Name = "btnStartM";
            this.btnStartM.Size = new System.Drawing.Size(22, 22);
            this.btnStartM.TabIndex = 2;
            this.btnStartM.UseVisualStyleBackColor = true;
            this.btnStartM.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnResize
            // 
            this.btnResize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnResize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResize.Location = new System.Drawing.Point(350, -1);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(22, 22);
            this.btnResize.TabIndex = 3;
            this.btnResize.Text = "<";
            this.btnResize.UseVisualStyleBackColor = true;
            this.btnResize.Click += new System.EventHandler(this.btnResize_Click);
            // 
            // tbChat
            // 
            this.tbChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.tbChat.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbChat.Location = new System.Drawing.Point(0, 0);
            this.tbChat.MaxLength = 120;
            this.tbChat.Name = "tbChat";
            this.tbChat.Size = new System.Drawing.Size(286, 20);
            this.tbChat.TabIndex = 2;
            this.tbChat.WordWrap = false;
            this.tbChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbChat_KeyDown);
            this.tbChat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbChat_KeyPress);
            // 
            // btnChatSend
            // 
            this.btnChatSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChatSend.Location = new System.Drawing.Point(307, -1);
            this.btnChatSend.Name = "btnChatSend";
            this.btnChatSend.Size = new System.Drawing.Size(44, 22);
            this.btnChatSend.TabIndex = 1;
            this.btnChatSend.Text = "Send";
            this.btnChatSend.UseVisualStyleBackColor = true;
            this.btnChatSend.Click += new System.EventHandler(this.btnChatSend_Click);
            // 
            // tabChat
            // 
            this.tabChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabChat.Controls.Add(this.tabChatPageLog);
            this.tabChat.Controls.Add(this.tabChatPageFishing);
            this.tabChat.Controls.Add(this.tabChatPageTell);
            this.tabChat.Controls.Add(this.tabChatPageParty);
            this.tabChat.Controls.Add(this.tabChatPageLS);
            this.tabChat.Controls.Add(this.tabChatPageSay);
            this.tabChat.Controls.Add(this.tabChatPageDB);
            this.tabChat.Location = new System.Drawing.Point(0, 22);
            this.tabChat.Margin = new System.Windows.Forms.Padding(0);
            this.tabChat.Multiline = true;
            this.tabChat.Name = "tabChat";
            this.tabChat.SelectedIndex = 0;
            this.tabChat.Size = new System.Drawing.Size(375, 172);
            this.tabChat.TabIndex = 0;
            this.tabChat.TabStop = false;
            this.tabChat.SelectedIndexChanged += new System.EventHandler(this.tabChat_SelectedIndexChanged);
            // 
            // tabChatPageLog
            // 
            this.tabChatPageLog.BackColor = System.Drawing.Color.Transparent;
            this.tabChatPageLog.Controls.Add(this.rtbChat);
            this.tabChatPageLog.Location = new System.Drawing.Point(4, 21);
            this.tabChatPageLog.Name = "tabChatPageLog";
            this.tabChatPageLog.Size = new System.Drawing.Size(367, 147);
            this.tabChatPageLog.TabIndex = 0;
            this.tabChatPageLog.Text = "Log";
            this.tabChatPageLog.UseVisualStyleBackColor = true;
            // 
            // rtbChat
            // 
            this.rtbChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.rtbChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbChat.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbChat.ForeColor = System.Drawing.Color.White;
            this.rtbChat.Location = new System.Drawing.Point(0, 0);
            this.rtbChat.Margin = new System.Windows.Forms.Padding(0);
            this.rtbChat.Name = "rtbChat";
            this.rtbChat.ReadOnly = true;
            this.rtbChat.Size = new System.Drawing.Size(367, 147);
            this.rtbChat.TabIndex = 0;
            this.rtbChat.TabStop = false;
            this.rtbChat.Text = "";
            this.rtbChat.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbChat_LinkClicked);
            // 
            // tabChatPageFishing
            // 
            this.tabChatPageFishing.Controls.Add(this.rtbFish);
            this.tabChatPageFishing.Location = new System.Drawing.Point(4, 21);
            this.tabChatPageFishing.Name = "tabChatPageFishing";
            this.tabChatPageFishing.Size = new System.Drawing.Size(367, 147);
            this.tabChatPageFishing.TabIndex = 1;
            this.tabChatPageFishing.Text = "Fishing";
            this.tabChatPageFishing.UseVisualStyleBackColor = true;
            // 
            // rtbFish
            // 
            this.rtbFish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.rtbFish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbFish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbFish.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbFish.ForeColor = System.Drawing.Color.White;
            this.rtbFish.Location = new System.Drawing.Point(0, 0);
            this.rtbFish.Margin = new System.Windows.Forms.Padding(0);
            this.rtbFish.Name = "rtbFish";
            this.rtbFish.ReadOnly = true;
            this.rtbFish.Size = new System.Drawing.Size(367, 147);
            this.rtbFish.TabIndex = 1;
            this.rtbFish.TabStop = false;
            this.rtbFish.Text = "";
            // 
            // tabChatPageTell
            // 
            this.tabChatPageTell.Controls.Add(this.rtbTell);
            this.tabChatPageTell.Location = new System.Drawing.Point(4, 21);
            this.tabChatPageTell.Name = "tabChatPageTell";
            this.tabChatPageTell.Size = new System.Drawing.Size(367, 147);
            this.tabChatPageTell.TabIndex = 2;
            this.tabChatPageTell.Text = "Tell";
            this.tabChatPageTell.UseVisualStyleBackColor = true;
            // 
            // rtbTell
            // 
            this.rtbTell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.rtbTell.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbTell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbTell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbTell.ForeColor = System.Drawing.Color.White;
            this.rtbTell.Location = new System.Drawing.Point(0, 0);
            this.rtbTell.Margin = new System.Windows.Forms.Padding(0);
            this.rtbTell.Name = "rtbTell";
            this.rtbTell.ReadOnly = true;
            this.rtbTell.Size = new System.Drawing.Size(367, 147);
            this.rtbTell.TabIndex = 1;
            this.rtbTell.TabStop = false;
            this.rtbTell.Text = "";
            this.rtbTell.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbTell_LinkClicked);
            // 
            // tabChatPageParty
            // 
            this.tabChatPageParty.Controls.Add(this.rtbParty);
            this.tabChatPageParty.Location = new System.Drawing.Point(4, 21);
            this.tabChatPageParty.Name = "tabChatPageParty";
            this.tabChatPageParty.Size = new System.Drawing.Size(367, 147);
            this.tabChatPageParty.TabIndex = 3;
            this.tabChatPageParty.Text = "PT";
            this.tabChatPageParty.UseVisualStyleBackColor = true;
            // 
            // rtbParty
            // 
            this.rtbParty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.rtbParty.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbParty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbParty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbParty.ForeColor = System.Drawing.Color.White;
            this.rtbParty.Location = new System.Drawing.Point(0, 0);
            this.rtbParty.Margin = new System.Windows.Forms.Padding(0);
            this.rtbParty.Name = "rtbParty";
            this.rtbParty.ReadOnly = true;
            this.rtbParty.Size = new System.Drawing.Size(367, 147);
            this.rtbParty.TabIndex = 1;
            this.rtbParty.Text = "";
            this.rtbParty.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbParty_LinkClicked);
            // 
            // tabChatPageLS
            // 
            this.tabChatPageLS.Controls.Add(this.rtbShell);
            this.tabChatPageLS.Location = new System.Drawing.Point(4, 21);
            this.tabChatPageLS.Name = "tabChatPageLS";
            this.tabChatPageLS.Size = new System.Drawing.Size(367, 147);
            this.tabChatPageLS.TabIndex = 4;
            this.tabChatPageLS.Text = "LS";
            this.tabChatPageLS.UseVisualStyleBackColor = true;
            // 
            // rtbShell
            // 
            this.rtbShell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.rtbShell.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbShell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbShell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbShell.ForeColor = System.Drawing.Color.White;
            this.rtbShell.Location = new System.Drawing.Point(0, 0);
            this.rtbShell.Margin = new System.Windows.Forms.Padding(0);
            this.rtbShell.Name = "rtbShell";
            this.rtbShell.ReadOnly = true;
            this.rtbShell.Size = new System.Drawing.Size(367, 147);
            this.rtbShell.TabIndex = 1;
            this.rtbShell.TabStop = false;
            this.rtbShell.Text = "";
            this.rtbShell.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbShell_LinkClicked);
            // 
            // tabChatPageSay
            // 
            this.tabChatPageSay.Controls.Add(this.rtbSay);
            this.tabChatPageSay.Location = new System.Drawing.Point(4, 21);
            this.tabChatPageSay.Name = "tabChatPageSay";
            this.tabChatPageSay.Size = new System.Drawing.Size(367, 147);
            this.tabChatPageSay.TabIndex = 5;
            this.tabChatPageSay.Text = "Say";
            this.tabChatPageSay.UseVisualStyleBackColor = true;
            // 
            // rtbSay
            // 
            this.rtbSay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.rtbSay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbSay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSay.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbSay.ForeColor = System.Drawing.Color.White;
            this.rtbSay.Location = new System.Drawing.Point(0, 0);
            this.rtbSay.Margin = new System.Windows.Forms.Padding(0);
            this.rtbSay.Name = "rtbSay";
            this.rtbSay.ReadOnly = true;
            this.rtbSay.Size = new System.Drawing.Size(367, 147);
            this.rtbSay.TabIndex = 1;
            this.rtbSay.TabStop = false;
            this.rtbSay.Text = "";
            this.rtbSay.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbSay_LinkClicked);
            // 
            // tabChatPageDB
            // 
            this.tabChatPageDB.Controls.Add(this.rtbDB);
            this.tabChatPageDB.Location = new System.Drawing.Point(4, 21);
            this.tabChatPageDB.Name = "tabChatPageDB";
            this.tabChatPageDB.Size = new System.Drawing.Size(367, 147);
            this.tabChatPageDB.TabIndex = 5;
            this.tabChatPageDB.Text = "DB";
            this.tabChatPageDB.UseVisualStyleBackColor = true;
            // 
            // rtbDB
            // 
            this.rtbDB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.rtbDB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbDB.ForeColor = System.Drawing.Color.White;
            this.rtbDB.Location = new System.Drawing.Point(0, 0);
            this.rtbDB.Margin = new System.Windows.Forms.Padding(0);
            this.rtbDB.Name = "rtbDB";
            this.rtbDB.ReadOnly = true;
            this.rtbDB.Size = new System.Drawing.Size(367, 147);
            this.rtbDB.TabIndex = 1;
            this.rtbDB.TabStop = false;
            this.rtbDB.Text = "";
            this.rtbDB.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbDB_LinkClicked);
            // 
            // tabDisplayPageStats
            // 
            this.tabDisplayPageStats.BackColor = System.Drawing.Color.Transparent;
            this.tabDisplayPageStats.Controls.Add(this.rtbStats);
            this.tabDisplayPageStats.Location = new System.Drawing.Point(4, 21);
            this.tabDisplayPageStats.Margin = new System.Windows.Forms.Padding(0);
            this.tabDisplayPageStats.Name = "tabDisplayPageStats";
            this.tabDisplayPageStats.Size = new System.Drawing.Size(372, 202);
            this.tabDisplayPageStats.TabIndex = 0;
            this.tabDisplayPageStats.Text = "Stats";
            this.tabDisplayPageStats.UseVisualStyleBackColor = true;
            // 
            // tabDisplayPageInfo
            // 
            this.tabDisplayPageInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tabDisplayPageInfo.Controls.Add(this.lblEarthTimeHeader);
            this.tabDisplayPageInfo.Controls.Add(this.lblEarthTime);
            this.tabDisplayPageInfo.Controls.Add(this.lblVanaTimeHeader);
            this.tabDisplayPageInfo.Controls.Add(this.lblSackSpace);
            this.tabDisplayPageInfo.Controls.Add(this.lblNoCatchAt);
            this.tabDisplayPageInfo.Controls.Add(this.lblSatchelSpace);
            this.tabDisplayPageInfo.Controls.Add(this.lblGil);
            this.tabDisplayPageInfo.Controls.Add(this.lblInventorySpace);
            this.tabDisplayPageInfo.Controls.Add(this.lblGilHeader);
            this.tabDisplayPageInfo.Controls.Add(this.lblSackHeader);
            this.tabDisplayPageInfo.Controls.Add(this.lblVanaTime);
            this.tabDisplayPageInfo.Controls.Add(this.lblSkill);
            this.tabDisplayPageInfo.Controls.Add(this.lblSatchelHeader);
            this.tabDisplayPageInfo.Controls.Add(this.lblInventoryHeader);
            this.tabDisplayPageInfo.Controls.Add(this.lblSkillHeader);
            this.tabDisplayPageInfo.Controls.Add(this.lblNoCatchAtHeader);
            this.tabDisplayPageInfo.Controls.Add(this.gbInfo);
            this.tabDisplayPageInfo.Location = new System.Drawing.Point(4, 21);
            this.tabDisplayPageInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tabDisplayPageInfo.Name = "tabDisplayPageInfo";
            this.tabDisplayPageInfo.Size = new System.Drawing.Size(372, 202);
            this.tabDisplayPageInfo.TabIndex = 1;
            this.tabDisplayPageInfo.Text = "Info";
            // 
            // lblEarthTimeHeader
            // 
            this.lblEarthTimeHeader.AutoSize = true;
            this.lblEarthTimeHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEarthTimeHeader.Location = new System.Drawing.Point(5, 147);
            this.lblEarthTimeHeader.Name = "lblEarthTimeHeader";
            this.lblEarthTimeHeader.Size = new System.Drawing.Size(36, 12);
            this.lblEarthTimeHeader.TabIndex = 15;
            this.lblEarthTimeHeader.Text = "Earth:";
            // 
            // lblEarthTime
            // 
            this.lblEarthTime.AutoSize = true;
            this.lblEarthTime.Location = new System.Drawing.Point(80, 146);
            this.lblEarthTime.Name = "lblEarthTime";
            this.lblEarthTime.Size = new System.Drawing.Size(13, 13);
            this.lblEarthTime.TabIndex = 16;
            this.lblEarthTime.Text = "--";
            // 
            // lblVanaTimeHeader
            // 
            this.lblVanaTimeHeader.AutoSize = true;
            this.lblVanaTimeHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVanaTimeHeader.Location = new System.Drawing.Point(5, 129);
            this.lblVanaTimeHeader.Name = "lblVanaTimeHeader";
            this.lblVanaTimeHeader.Size = new System.Drawing.Size(56, 12);
            this.lblVanaTimeHeader.TabIndex = 13;
            this.lblVanaTimeHeader.Text = "Vana\'diel:";
            // 
            // lblSackSpace
            // 
            this.lblSackSpace.AutoSize = true;
            this.lblSackSpace.Location = new System.Drawing.Point(80, 43);
            this.lblSackSpace.Name = "lblSackSpace";
            this.lblSackSpace.Size = new System.Drawing.Size(28, 13);
            this.lblSackSpace.TabIndex = 5;
            this.lblSackSpace.Text = "-- / --";
            // 
            // lblNoCatchAt
            // 
            this.lblNoCatchAt.AutoSize = true;
            this.lblNoCatchAt.Location = new System.Drawing.Point(207, 43);
            this.lblNoCatchAt.Name = "lblNoCatchAt";
            this.lblNoCatchAt.Size = new System.Drawing.Size(13, 13);
            this.lblNoCatchAt.TabIndex = 11;
            this.lblNoCatchAt.Text = "--";
            // 
            // lblSatchelSpace
            // 
            this.lblSatchelSpace.AutoSize = true;
            this.lblSatchelSpace.Location = new System.Drawing.Point(80, 24);
            this.lblSatchelSpace.Name = "lblSatchelSpace";
            this.lblSatchelSpace.Size = new System.Drawing.Size(28, 13);
            this.lblSatchelSpace.TabIndex = 3;
            this.lblSatchelSpace.Text = "-- / --";
            // 
            // lblGil
            // 
            this.lblGil.AutoSize = true;
            this.lblGil.Location = new System.Drawing.Point(207, 24);
            this.lblGil.Name = "lblGil";
            this.lblGil.Size = new System.Drawing.Size(13, 13);
            this.lblGil.TabIndex = 9;
            this.lblGil.Text = "--";
            // 
            // lblInventorySpace
            // 
            this.lblInventorySpace.AutoSize = true;
            this.lblInventorySpace.Location = new System.Drawing.Point(80, 5);
            this.lblInventorySpace.Name = "lblInventorySpace";
            this.lblInventorySpace.Size = new System.Drawing.Size(28, 13);
            this.lblInventorySpace.TabIndex = 1;
            this.lblInventorySpace.Text = "-- / --";
            // 
            // lblGilHeader
            // 
            this.lblGilHeader.AutoSize = true;
            this.lblGilHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGilHeader.Location = new System.Drawing.Point(132, 24);
            this.lblGilHeader.Name = "lblGilHeader";
            this.lblGilHeader.Size = new System.Drawing.Size(64, 12);
            this.lblGilHeader.TabIndex = 8;
            this.lblGilHeader.Text = "Current Gil:";
            // 
            // lblSackHeader
            // 
            this.lblSackHeader.AutoSize = true;
            this.lblSackHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSackHeader.Location = new System.Drawing.Point(5, 43);
            this.lblSackHeader.Name = "lblSackHeader";
            this.lblSackHeader.Size = new System.Drawing.Size(59, 12);
            this.lblSackHeader.TabIndex = 4;
            this.lblSackHeader.Text = "Mog Sack:";
            // 
            // lblVanaTime
            // 
            this.lblVanaTime.AutoSize = true;
            this.lblVanaTime.Location = new System.Drawing.Point(80, 128);
            this.lblVanaTime.Name = "lblVanaTime";
            this.lblVanaTime.Size = new System.Drawing.Size(13, 13);
            this.lblVanaTime.TabIndex = 14;
            this.lblVanaTime.Text = "--";
            // 
            // lblSkill
            // 
            this.lblSkill.AutoSize = true;
            this.lblSkill.Location = new System.Drawing.Point(207, 5);
            this.lblSkill.Name = "lblSkill";
            this.lblSkill.Size = new System.Drawing.Size(13, 13);
            this.lblSkill.TabIndex = 7;
            this.lblSkill.Text = "--";
            // 
            // lblSatchelHeader
            // 
            this.lblSatchelHeader.AutoSize = true;
            this.lblSatchelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSatchelHeader.Location = new System.Drawing.Point(5, 24);
            this.lblSatchelHeader.Name = "lblSatchelHeader";
            this.lblSatchelHeader.Size = new System.Drawing.Size(72, 12);
            this.lblSatchelHeader.TabIndex = 2;
            this.lblSatchelHeader.Text = "Mog Satchel:";
            // 
            // lblInventoryHeader
            // 
            this.lblInventoryHeader.AutoSize = true;
            this.lblInventoryHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInventoryHeader.Location = new System.Drawing.Point(5, 5);
            this.lblInventoryHeader.Name = "lblInventoryHeader";
            this.lblInventoryHeader.Size = new System.Drawing.Size(58, 12);
            this.lblInventoryHeader.TabIndex = 0;
            this.lblInventoryHeader.Text = "Inventory:";
            // 
            // lblSkillHeader
            // 
            this.lblSkillHeader.AutoSize = true;
            this.lblSkillHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSkillHeader.Location = new System.Drawing.Point(132, 5);
            this.lblSkillHeader.Name = "lblSkillHeader";
            this.lblSkillHeader.Size = new System.Drawing.Size(71, 12);
            this.lblSkillHeader.TabIndex = 6;
            this.lblSkillHeader.Text = "Fishing Skill:";
            // 
            // lblNoCatchAtHeader
            // 
            this.lblNoCatchAtHeader.AutoSize = true;
            this.lblNoCatchAtHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoCatchAtHeader.Location = new System.Drawing.Point(132, 43);
            this.lblNoCatchAtHeader.Name = "lblNoCatchAtHeader";
            this.lblNoCatchAtHeader.Size = new System.Drawing.Size(54, 12);
            this.lblNoCatchAtHeader.TabIndex = 10;
            this.lblNoCatchAtHeader.Text = "No catch:";
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.lblBait);
            this.gbInfo.Controls.Add(this.lblZone);
            this.gbInfo.Controls.Add(this.lblZoneHeader);
            this.gbInfo.Controls.Add(this.lblRod);
            this.gbInfo.Controls.Add(this.lblRodHeader);
            this.gbInfo.Controls.Add(this.lblBaitHeader);
            this.gbInfo.Location = new System.Drawing.Point(4, 57);
            this.gbInfo.Margin = new System.Windows.Forms.Padding(0);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Padding = new System.Windows.Forms.Padding(0);
            this.gbInfo.Size = new System.Drawing.Size(267, 67);
            this.gbInfo.TabIndex = 12;
            this.gbInfo.TabStop = false;
            // 
            // tabDisplayPageOptions
            // 
            this.tabDisplayPageOptions.BackColor = System.Drawing.SystemColors.Control;
            this.tabDisplayPageOptions.Controls.Add(this.tabOptions);
            this.tabDisplayPageOptions.Location = new System.Drawing.Point(4, 21);
            this.tabDisplayPageOptions.Margin = new System.Windows.Forms.Padding(0);
            this.tabDisplayPageOptions.Name = "tabDisplayPageOptions";
            this.tabDisplayPageOptions.Size = new System.Drawing.Size(372, 202);
            this.tabDisplayPageOptions.TabIndex = 2;
            this.tabDisplayPageOptions.Text = "Options";
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.tabOptionsPageForm);
            this.tabOptions.Controls.Add(this.tabOptionsPageChat);
            this.tabOptions.Controls.Add(this.tabOptionsPageFight);
            this.tabOptions.Controls.Add(this.tabOptionsPageGear);
            this.tabOptions.Controls.Add(this.tabOptionsPageAdvanced);
            this.tabOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabOptions.Location = new System.Drawing.Point(0, 0);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new System.Drawing.Size(372, 202);
            this.tabOptions.TabIndex = 0;
            // 
            // tabOptionsPageForm
            // 
            this.tabOptionsPageForm.BackColor = System.Drawing.SystemColors.Control;
            this.tabOptionsPageForm.Controls.Add(this.numSkillCap);
            this.tabOptionsPageForm.Controls.Add(this.cbSkillCap);
            this.tabOptionsPageForm.Controls.Add(this.cbTellDetect);
            this.tabOptionsPageForm.Controls.Add(this.cbSneakFishing);
            this.tabOptionsPageForm.Controls.Add(this.numMaxCatch);
            this.tabOptionsPageForm.Controls.Add(this.cbMaxCatch);
            this.tabOptionsPageForm.Controls.Add(this.gbGeneralFishing);
            this.tabOptionsPageForm.Controls.Add(this.cbStopSound);
            this.tabOptionsPageForm.Controls.Add(this.btnSettingsSave);
            this.tabOptionsPageForm.Controls.Add(this.btnSettingsReset);
            this.tabOptionsPageForm.Controls.Add(this.cbAlwaysOnTop);
            this.tabOptionsPageForm.Location = new System.Drawing.Point(4, 21);
            this.tabOptionsPageForm.Name = "tabOptionsPageForm";
            this.tabOptionsPageForm.Size = new System.Drawing.Size(364, 177);
            this.tabOptionsPageForm.TabIndex = 3;
            this.tabOptionsPageForm.Text = "General";
            // 
            // numSkillCap
            // 
            this.numSkillCap.Location = new System.Drawing.Point(236, 45);
            this.numSkillCap.Maximum = new decimal(new int[] {
            110,
            0,
            0,
            0});
            this.numSkillCap.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSkillCap.Name = "numSkillCap";
            this.numSkillCap.Size = new System.Drawing.Size(35, 18);
            this.numSkillCap.TabIndex = 12;
            this.numSkillCap.Value = new decimal(new int[] {
            110,
            0,
            0,
            0});
            // 
            // cbSkillCap
            // 
            this.cbSkillCap.AutoSize = true;
            this.cbSkillCap.Location = new System.Drawing.Point(145, 46);
            this.cbSkillCap.Name = "cbSkillCap";
            this.cbSkillCap.Size = new System.Drawing.Size(85, 17);
            this.cbSkillCap.TabIndex = 11;
            this.cbSkillCap.Text = "Limit Skillups";
            this.cbSkillCap.UseVisualStyleBackColor = true;
            // 
            // cbTellDetect
            // 
            this.cbTellDetect.AutoSize = true;
            this.cbTellDetect.Checked = true;
            this.cbTellDetect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTellDetect.Location = new System.Drawing.Point(7, 25);
            this.cbTellDetect.Name = "cbTellDetect";
            this.cbTellDetect.Size = new System.Drawing.Size(89, 17);
            this.cbTellDetect.TabIndex = 10;
            this.cbTellDetect.Text = "Tell Detection";
            this.cbTellDetect.UseVisualStyleBackColor = true;
            // 
            // cbSneakFishing
            // 
            this.cbSneakFishing.AutoSize = true;
            this.cbSneakFishing.Location = new System.Drawing.Point(145, 25);
            this.cbSneakFishing.Name = "cbSneakFishing";
            this.cbSneakFishing.Size = new System.Drawing.Size(110, 17);
            this.cbSneakFishing.TabIndex = 3;
            this.cbSneakFishing.Text = "Use Sneak fishing";
            this.cbSneakFishing.UseVisualStyleBackColor = true;
            // 
            // numMaxCatch
            // 
            this.numMaxCatch.Location = new System.Drawing.Point(98, 45);
            this.numMaxCatch.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numMaxCatch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxCatch.Name = "numMaxCatch";
            this.numMaxCatch.Size = new System.Drawing.Size(35, 18);
            this.numMaxCatch.TabIndex = 5;
            this.numMaxCatch.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // cbMaxCatch
            // 
            this.cbMaxCatch.AutoSize = true;
            this.cbMaxCatch.Location = new System.Drawing.Point(7, 46);
            this.cbMaxCatch.Name = "cbMaxCatch";
            this.cbMaxCatch.Size = new System.Drawing.Size(88, 17);
            this.cbMaxCatch.TabIndex = 4;
            this.cbMaxCatch.Text = "Limit Catches";
            this.cbMaxCatch.UseVisualStyleBackColor = true;
            // 
            // gbGeneralFishing
            // 
            this.gbGeneralFishing.Controls.Add(this.lblCastWait);
            this.gbGeneralFishing.Controls.Add(this.numMaxNoCatch);
            this.gbGeneralFishing.Controls.Add(this.btnCastReset);
            this.gbGeneralFishing.Controls.Add(this.numCastIntervalLow);
            this.gbGeneralFishing.Controls.Add(this.lblMaxNoCatch);
            this.gbGeneralFishing.Controls.Add(this.numCastIntervalHigh);
            this.gbGeneralFishing.Controls.Add(this.trackOpacity);
            this.gbGeneralFishing.Location = new System.Drawing.Point(3, 61);
            this.gbGeneralFishing.Name = "gbGeneralFishing";
            this.gbGeneralFishing.Size = new System.Drawing.Size(216, 52);
            this.gbGeneralFishing.TabIndex = 7;
            this.gbGeneralFishing.TabStop = false;
            // 
            // lblCastWait
            // 
            this.lblCastWait.AutoSize = true;
            this.lblCastWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.lblCastWait.Location = new System.Drawing.Point(3, 11);
            this.lblCastWait.Margin = new System.Windows.Forms.Padding(0);
            this.lblCastWait.Name = "lblCastWait";
            this.lblCastWait.Size = new System.Drawing.Size(78, 13);
            this.lblCastWait.TabIndex = 0;
            this.lblCastWait.Text = "Cast wait time: ";
            this.lblCastWait.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numMaxNoCatch
            // 
            this.numMaxNoCatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numMaxNoCatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMaxNoCatch.Location = new System.Drawing.Point(83, 31);
            this.numMaxNoCatch.Margin = new System.Windows.Forms.Padding(0);
            this.numMaxNoCatch.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numMaxNoCatch.Name = "numMaxNoCatch";
            this.numMaxNoCatch.Size = new System.Drawing.Size(38, 18);
            this.numMaxNoCatch.TabIndex = 5;
            this.numMaxNoCatch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMaxNoCatch.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // btnCastReset
            // 
            this.btnCastReset.Location = new System.Drawing.Point(164, 8);
            this.btnCastReset.Name = "btnCastReset";
            this.btnCastReset.Size = new System.Drawing.Size(47, 20);
            this.btnCastReset.TabIndex = 3;
            this.btnCastReset.Text = "Reset";
            this.btnCastReset.UseVisualStyleBackColor = true;
            this.btnCastReset.Click += new System.EventHandler(this.btnCastReset_Click);
            // 
            // numCastIntervalLow
            // 
            this.numCastIntervalLow.DecimalPlaces = 1;
            this.numCastIntervalLow.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCastIntervalLow.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numCastIntervalLow.Location = new System.Drawing.Point(83, 9);
            this.numCastIntervalLow.Margin = new System.Windows.Forms.Padding(0);
            this.numCastIntervalLow.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numCastIntervalLow.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numCastIntervalLow.Name = "numCastIntervalLow";
            this.numCastIntervalLow.Size = new System.Drawing.Size(38, 18);
            this.numCastIntervalLow.TabIndex = 1;
            this.numCastIntervalLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numCastIntervalLow.Value = new decimal(new int[] {
            30,
            0,
            0,
            65536});
            this.numCastIntervalLow.ValueChanged += new System.EventHandler(this.numCastIntervalLow_ValueChanged);
            // 
            // lblMaxNoCatch
            // 
            this.lblMaxNoCatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMaxNoCatch.AutoSize = true;
            this.lblMaxNoCatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxNoCatch.Location = new System.Drawing.Point(3, 33);
            this.lblMaxNoCatch.Margin = new System.Windows.Forms.Padding(0);
            this.lblMaxNoCatch.Name = "lblMaxNoCatch";
            this.lblMaxNoCatch.Size = new System.Drawing.Size(73, 13);
            this.lblMaxNoCatch.TabIndex = 4;
            this.lblMaxNoCatch.Text = "Max no catch:";
            this.lblMaxNoCatch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numCastIntervalHigh
            // 
            this.numCastIntervalHigh.DecimalPlaces = 1;
            this.numCastIntervalHigh.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCastIntervalHigh.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numCastIntervalHigh.Location = new System.Drawing.Point(122, 9);
            this.numCastIntervalHigh.Margin = new System.Windows.Forms.Padding(0);
            this.numCastIntervalHigh.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numCastIntervalHigh.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numCastIntervalHigh.Name = "numCastIntervalHigh";
            this.numCastIntervalHigh.Size = new System.Drawing.Size(38, 18);
            this.numCastIntervalHigh.TabIndex = 2;
            this.numCastIntervalHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numCastIntervalHigh.Value = new decimal(new int[] {
            35,
            0,
            0,
            65536});
            this.numCastIntervalHigh.ValueChanged += new System.EventHandler(this.numCastIntervalHigh_ValueChanged);
            // 
            // trackOpacity
            // 
            this.trackOpacity.AutoSize = false;
            this.trackOpacity.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackOpacity.LargeChange = 1;
            this.trackOpacity.Location = new System.Drawing.Point(133, 29);
            this.trackOpacity.Margin = new System.Windows.Forms.Padding(0);
            this.trackOpacity.Minimum = 1;
            this.trackOpacity.Name = "trackOpacity";
            this.trackOpacity.Size = new System.Drawing.Size(78, 21);
            this.trackOpacity.TabIndex = 6;
            this.trackOpacity.Value = 10;
            this.trackOpacity.Scroll += new System.EventHandler(this.trackOpacity_Scroll);
            // 
            // cbStopSound
            // 
            this.cbStopSound.AutoSize = true;
            this.cbStopSound.Location = new System.Drawing.Point(145, 4);
            this.cbStopSound.Name = "cbStopSound";
            this.cbStopSound.Size = new System.Drawing.Size(106, 17);
            this.cbStopSound.TabIndex = 1;
            this.cbStopSound.Text = "Sounds on errors";
            this.cbStopSound.UseVisualStyleBackColor = true;
            // 
            // btnSettingsSave
            // 
            this.btnSettingsSave.Location = new System.Drawing.Point(7, 116);
            this.btnSettingsSave.Name = "btnSettingsSave";
            this.btnSettingsSave.Size = new System.Drawing.Size(92, 22);
            this.btnSettingsSave.TabIndex = 8;
            this.btnSettingsSave.Text = "Save Settings";
            this.btnSettingsSave.UseVisualStyleBackColor = true;
            this.btnSettingsSave.Click += new System.EventHandler(this.btnSettingsSave_Click);
            // 
            // btnSettingsReset
            // 
            this.btnSettingsReset.Location = new System.Drawing.Point(105, 116);
            this.btnSettingsReset.Name = "btnSettingsReset";
            this.btnSettingsReset.Size = new System.Drawing.Size(92, 22);
            this.btnSettingsReset.TabIndex = 9;
            this.btnSettingsReset.Text = "Reset Settings";
            this.btnSettingsReset.UseVisualStyleBackColor = true;
            this.btnSettingsReset.Click += new System.EventHandler(this.btnSettingsReset_Click);
            // 
            // cbAlwaysOnTop
            // 
            this.cbAlwaysOnTop.AutoSize = true;
            this.cbAlwaysOnTop.Checked = true;
            this.cbAlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAlwaysOnTop.Location = new System.Drawing.Point(7, 4);
            this.cbAlwaysOnTop.Name = "cbAlwaysOnTop";
            this.cbAlwaysOnTop.Size = new System.Drawing.Size(92, 17);
            this.cbAlwaysOnTop.TabIndex = 0;
            this.cbAlwaysOnTop.Text = "Always on top";
            this.cbAlwaysOnTop.UseVisualStyleBackColor = true;
            this.cbAlwaysOnTop.CheckedChanged += new System.EventHandler(this.cbAlwaysOnTop_CheckedChanged);
            // 
            // tabOptionsPageChat
            // 
            this.tabOptionsPageChat.Controls.Add(this.cbChatDetect);
            this.tabOptionsPageChat.Controls.Add(this.panelChatDetect);
            this.tabOptionsPageChat.Controls.Add(this.gbGMDetect);
            this.tabOptionsPageChat.Location = new System.Drawing.Point(4, 21);
            this.tabOptionsPageChat.Name = "tabOptionsPageChat";
            this.tabOptionsPageChat.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptionsPageChat.Size = new System.Drawing.Size(364, 177);
            this.tabOptionsPageChat.TabIndex = 6;
            this.tabOptionsPageChat.Text = "Chat";
            this.tabOptionsPageChat.UseVisualStyleBackColor = true;
            // 
            // cbChatDetect
            // 
            this.cbChatDetect.AutoSize = true;
            this.cbChatDetect.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbChatDetect.Location = new System.Drawing.Point(99, 7);
            this.cbChatDetect.Name = "cbChatDetect";
            this.cbChatDetect.Size = new System.Drawing.Size(119, 17);
            this.cbChatDetect.TabIndex = 6;
            this.cbChatDetect.Text = "Chat Detect Actions";
            // 
            // panelChatDetect
            // 
            this.panelChatDetect.AutoScroll = true;
            this.panelChatDetect.Controls.Add(this.btnChatDetectAdd);
            this.panelChatDetect.Location = new System.Drawing.Point(102, 23);
            this.panelChatDetect.Name = "panelChatDetect";
            this.panelChatDetect.Size = new System.Drawing.Size(262, 140);
            this.panelChatDetect.TabIndex = 1;
            this.panelChatDetect.Tag = "";
            this.panelChatDetect.Text = "Chat Detect";
            this.panelChatDetect.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelChatDetect_Scroll);
            // 
            // btnChatDetectAdd
            // 
            this.btnChatDetectAdd.Location = new System.Drawing.Point(220, 0);
            this.btnChatDetectAdd.Name = "btnChatDetectAdd";
            this.btnChatDetectAdd.Size = new System.Drawing.Size(23, 23);
            this.btnChatDetectAdd.TabIndex = 5;
            this.btnChatDetectAdd.Text = "+";
            this.btnChatDetectAdd.UseVisualStyleBackColor = true;
            this.btnChatDetectAdd.Click += new System.EventHandler(this.btnChatDetectAdd_Remove_click);
            // 
            // gbGMDetect
            // 
            this.gbGMDetect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbGMDetect.Controls.Add(this.cbGMdetectAutostop);
            this.gbGMDetect.Location = new System.Drawing.Point(6, 7);
            this.gbGMDetect.Name = "gbGMDetect";
            this.gbGMDetect.Size = new System.Drawing.Size(93, 148);
            this.gbGMDetect.TabIndex = 0;
            this.gbGMDetect.TabStop = false;
            this.gbGMDetect.Text = "GM Detection";
            // 
            // cbGMdetectAutostop
            // 
            this.cbGMdetectAutostop.AutoSize = true;
            this.cbGMdetectAutostop.Checked = true;
            this.cbGMdetectAutostop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGMdetectAutostop.Location = new System.Drawing.Point(6, 17);
            this.cbGMdetectAutostop.Name = "cbGMdetectAutostop";
            this.cbGMdetectAutostop.Size = new System.Drawing.Size(84, 17);
            this.cbGMdetectAutostop.TabIndex = 0;
            this.cbGMdetectAutostop.Text = "Stop Fishing";
            this.cbGMdetectAutostop.UseVisualStyleBackColor = true;
            // 
            // tabOptionsPageFight
            // 
            this.tabOptionsPageFight.BackColor = System.Drawing.SystemColors.Control;
            this.tabOptionsPageFight.Controls.Add(this.cbFishHP);
            this.tabOptionsPageFight.Controls.Add(this.numReactionHigh);
            this.tabOptionsPageFight.Controls.Add(this.numReactionLow);
            this.tabOptionsPageFight.Controls.Add(this.cbReaction);
            this.tabOptionsPageFight.Controls.Add(this.numFakeLargeIntervalLow);
            this.tabOptionsPageFight.Controls.Add(this.cbReleaseSmall);
            this.tabOptionsPageFight.Controls.Add(this.numFakeLargeIntervalHigh);
            this.tabOptionsPageFight.Controls.Add(this.numFakeSmallIntervalLow);
            this.tabOptionsPageFight.Controls.Add(this.cbReleaseLarge);
            this.tabOptionsPageFight.Controls.Add(this.cbIgnoreLargeFish);
            this.tabOptionsPageFight.Controls.Add(this.numFakeSmallIntervalHigh);
            this.tabOptionsPageFight.Controls.Add(this.cbIgnoreSmallFish);
            this.tabOptionsPageFight.Controls.Add(this.cbIgnoreItem);
            this.tabOptionsPageFight.Controls.Add(this.cbIgnoreMonster);
            this.tabOptionsPageFight.Controls.Add(this.lblKillSeconds);
            this.tabOptionsPageFight.Controls.Add(this.numQuickKill);
            this.tabOptionsPageFight.Controls.Add(this.cbQuickKill);
            this.tabOptionsPageFight.Controls.Add(this.cbAutoKill);
            this.tabOptionsPageFight.Controls.Add(this.cbExtend);
            this.tabOptionsPageFight.Location = new System.Drawing.Point(4, 21);
            this.tabOptionsPageFight.Name = "tabOptionsPageFight";
            this.tabOptionsPageFight.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptionsPageFight.Size = new System.Drawing.Size(364, 177);
            this.tabOptionsPageFight.TabIndex = 2;
            this.tabOptionsPageFight.Text = "Fight";
            // 
            // cbFishHP
            // 
            this.cbFishHP.AutoSize = true;
            this.cbFishHP.Location = new System.Drawing.Point(7, 4);
            this.cbFishHP.Name = "cbFishHP";
            this.cbFishHP.Size = new System.Drawing.Size(114, 17);
            this.cbFishHP.TabIndex = 0;
            this.cbFishHP.Text = "Show fish HP[time]";
            this.cbFishHP.UseVisualStyleBackColor = true;
            // 
            // numReactionHigh
            // 
            this.numReactionHigh.DecimalPlaces = 1;
            this.numReactionHigh.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numReactionHigh.Location = new System.Drawing.Point(98, 104);
            this.numReactionHigh.Margin = new System.Windows.Forms.Padding(0);
            this.numReactionHigh.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numReactionHigh.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numReactionHigh.Name = "numReactionHigh";
            this.numReactionHigh.Size = new System.Drawing.Size(35, 18);
            this.numReactionHigh.TabIndex = 18;
            this.numReactionHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numReactionHigh.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            this.numReactionHigh.ValueChanged += new System.EventHandler(this.numReactionHigh_ValueChanged);
            // 
            // numReactionLow
            // 
            this.numReactionLow.DecimalPlaces = 1;
            this.numReactionLow.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numReactionLow.Location = new System.Drawing.Point(62, 104);
            this.numReactionLow.Margin = new System.Windows.Forms.Padding(0);
            this.numReactionLow.Maximum = new decimal(new int[] {
            45,
            0,
            0,
            65536});
            this.numReactionLow.Name = "numReactionLow";
            this.numReactionLow.Size = new System.Drawing.Size(35, 18);
            this.numReactionLow.TabIndex = 17;
            this.numReactionLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numReactionLow.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numReactionLow.ValueChanged += new System.EventHandler(this.numReactionLow_ValueChanged);
            // 
            // cbReaction
            // 
            this.cbReaction.AutoSize = true;
            this.cbReaction.Location = new System.Drawing.Point(7, 104);
            this.cbReaction.Name = "cbReaction";
            this.cbReaction.Size = new System.Drawing.Size(56, 17);
            this.cbReaction.TabIndex = 16;
            this.cbReaction.Text = "React:";
            this.cbReaction.UseVisualStyleBackColor = true;
            // 
            // numFakeLargeIntervalLow
            // 
            this.numFakeLargeIntervalLow.Location = new System.Drawing.Point(203, 83);
            this.numFakeLargeIntervalLow.Margin = new System.Windows.Forms.Padding(0);
            this.numFakeLargeIntervalLow.Name = "numFakeLargeIntervalLow";
            this.numFakeLargeIntervalLow.Size = new System.Drawing.Size(35, 18);
            this.numFakeLargeIntervalLow.TabIndex = 14;
            this.numFakeLargeIntervalLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFakeLargeIntervalLow.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numFakeLargeIntervalLow.ValueChanged += new System.EventHandler(this.numFakeLargeIntervalLow_ValueChanged);
            // 
            // cbReleaseSmall
            // 
            this.cbReleaseSmall.AutoSize = true;
            this.cbReleaseSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbReleaseSmall.Location = new System.Drawing.Point(7, 84);
            this.cbReleaseSmall.Name = "cbReleaseSmall";
            this.cbReleaseSmall.Size = new System.Drawing.Size(52, 17);
            this.cbReleaseSmall.TabIndex = 10;
            this.cbReleaseSmall.Text = "Fake!";
            this.cbReleaseSmall.UseVisualStyleBackColor = true;
            // 
            // numFakeLargeIntervalHigh
            // 
            this.numFakeLargeIntervalHigh.Location = new System.Drawing.Point(239, 83);
            this.numFakeLargeIntervalHigh.Margin = new System.Windows.Forms.Padding(0);
            this.numFakeLargeIntervalHigh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFakeLargeIntervalHigh.Name = "numFakeLargeIntervalHigh";
            this.numFakeLargeIntervalHigh.Size = new System.Drawing.Size(35, 18);
            this.numFakeLargeIntervalHigh.TabIndex = 15;
            this.numFakeLargeIntervalHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFakeLargeIntervalHigh.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numFakeLargeIntervalHigh.ValueChanged += new System.EventHandler(this.numFakeLargeIntervalHigh_ValueChanged);
            // 
            // numFakeSmallIntervalLow
            // 
            this.numFakeSmallIntervalLow.Location = new System.Drawing.Point(62, 83);
            this.numFakeSmallIntervalLow.Margin = new System.Windows.Forms.Padding(0);
            this.numFakeSmallIntervalLow.Name = "numFakeSmallIntervalLow";
            this.numFakeSmallIntervalLow.Size = new System.Drawing.Size(35, 18);
            this.numFakeSmallIntervalLow.TabIndex = 11;
            this.numFakeSmallIntervalLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFakeSmallIntervalLow.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numFakeSmallIntervalLow.ValueChanged += new System.EventHandler(this.numFakeSmallIntervalLow_ValueChanged);
            // 
            // cbReleaseLarge
            // 
            this.cbReleaseLarge.AutoSize = true;
            this.cbReleaseLarge.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbReleaseLarge.Location = new System.Drawing.Point(142, 84);
            this.cbReleaseLarge.Name = "cbReleaseLarge";
            this.cbReleaseLarge.Size = new System.Drawing.Size(58, 17);
            this.cbReleaseLarge.TabIndex = 13;
            this.cbReleaseLarge.Text = "Fake!!!";
            this.cbReleaseLarge.UseVisualStyleBackColor = true;
            // 
            // cbIgnoreLargeFish
            // 
            this.cbIgnoreLargeFish.AutoSize = true;
            this.cbIgnoreLargeFish.Location = new System.Drawing.Point(142, 64);
            this.cbIgnoreLargeFish.Name = "cbIgnoreLargeFish";
            this.cbIgnoreLargeFish.Size = new System.Drawing.Size(108, 17);
            this.cbIgnoreLargeFish.TabIndex = 9;
            this.cbIgnoreLargeFish.Text = "Ignore Large Fish";
            this.cbIgnoreLargeFish.UseVisualStyleBackColor = true;
            // 
            // numFakeSmallIntervalHigh
            // 
            this.numFakeSmallIntervalHigh.Location = new System.Drawing.Point(98, 83);
            this.numFakeSmallIntervalHigh.Margin = new System.Windows.Forms.Padding(0);
            this.numFakeSmallIntervalHigh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFakeSmallIntervalHigh.Name = "numFakeSmallIntervalHigh";
            this.numFakeSmallIntervalHigh.Size = new System.Drawing.Size(35, 18);
            this.numFakeSmallIntervalHigh.TabIndex = 12;
            this.numFakeSmallIntervalHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFakeSmallIntervalHigh.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.numFakeSmallIntervalHigh.ValueChanged += new System.EventHandler(this.numFakeSmallIntervalHigh_ValueChanged);
            // 
            // cbIgnoreSmallFish
            // 
            this.cbIgnoreSmallFish.AutoSize = true;
            this.cbIgnoreSmallFish.Location = new System.Drawing.Point(7, 64);
            this.cbIgnoreSmallFish.Name = "cbIgnoreSmallFish";
            this.cbIgnoreSmallFish.Size = new System.Drawing.Size(106, 17);
            this.cbIgnoreSmallFish.TabIndex = 8;
            this.cbIgnoreSmallFish.Text = "Ignore Small Fish";
            this.cbIgnoreSmallFish.UseVisualStyleBackColor = true;
            // 
            // cbIgnoreItem
            // 
            this.cbIgnoreItem.AutoSize = true;
            this.cbIgnoreItem.Checked = true;
            this.cbIgnoreItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIgnoreItem.Location = new System.Drawing.Point(142, 44);
            this.cbIgnoreItem.Name = "cbIgnoreItem";
            this.cbIgnoreItem.Size = new System.Drawing.Size(84, 17);
            this.cbIgnoreItem.TabIndex = 7;
            this.cbIgnoreItem.Text = "Ignore Items";
            this.cbIgnoreItem.UseVisualStyleBackColor = true;
            // 
            // cbIgnoreMonster
            // 
            this.cbIgnoreMonster.AutoSize = true;
            this.cbIgnoreMonster.Checked = true;
            this.cbIgnoreMonster.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIgnoreMonster.Location = new System.Drawing.Point(7, 44);
            this.cbIgnoreMonster.Name = "cbIgnoreMonster";
            this.cbIgnoreMonster.Size = new System.Drawing.Size(102, 17);
            this.cbIgnoreMonster.TabIndex = 6;
            this.cbIgnoreMonster.Text = "Ignore Monsters";
            this.cbIgnoreMonster.UseVisualStyleBackColor = true;
            // 
            // lblKillSeconds
            // 
            this.lblKillSeconds.AutoSize = true;
            this.lblKillSeconds.Location = new System.Drawing.Point(246, 25);
            this.lblKillSeconds.Name = "lblKillSeconds";
            this.lblKillSeconds.Size = new System.Drawing.Size(46, 13);
            this.lblKillSeconds.TabIndex = 5;
            this.lblKillSeconds.Text = "seconds";
            // 
            // numQuickKill
            // 
            this.numQuickKill.Location = new System.Drawing.Point(205, 23);
            this.numQuickKill.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numQuickKill.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numQuickKill.Name = "numQuickKill";
            this.numQuickKill.Size = new System.Drawing.Size(35, 18);
            this.numQuickKill.TabIndex = 4;
            this.numQuickKill.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // cbQuickKill
            // 
            this.cbQuickKill.AutoSize = true;
            this.cbQuickKill.Location = new System.Drawing.Point(142, 24);
            this.cbQuickKill.Name = "cbQuickKill";
            this.cbQuickKill.Size = new System.Drawing.Size(63, 17);
            this.cbQuickKill.TabIndex = 3;
            this.cbQuickKill.Text = "Kill after";
            this.cbQuickKill.UseVisualStyleBackColor = true;
            // 
            // cbAutoKill
            // 
            this.cbAutoKill.AutoSize = true;
            this.cbAutoKill.Checked = true;
            this.cbAutoKill.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoKill.Location = new System.Drawing.Point(142, 4);
            this.cbAutoKill.Name = "cbAutoKill";
            this.cbAutoKill.Size = new System.Drawing.Size(110, 17);
            this.cbAutoKill.TabIndex = 1;
            this.cbAutoKill.Text = "Kill fish at warning";
            this.cbAutoKill.UseVisualStyleBackColor = true;
            // 
            // cbExtend
            // 
            this.cbExtend.AutoSize = true;
            this.cbExtend.Location = new System.Drawing.Point(7, 24);
            this.cbExtend.Name = "cbExtend";
            this.cbExtend.Size = new System.Drawing.Size(96, 17);
            this.cbExtend.TabIndex = 2;
            this.cbExtend.Text = "Extend timeout";
            this.cbExtend.UseVisualStyleBackColor = true;
            // 
            // tabOptionsPageGear
            // 
            this.tabOptionsPageGear.BackColor = System.Drawing.SystemColors.Control;
            this.tabOptionsPageGear.Controls.Add(this.tbBaitGear);
            this.tabOptionsPageGear.Controls.Add(this.tbRodGear);
            this.tabOptionsPageGear.Controls.Add(this.lblBaitGear);
            this.tabOptionsPageGear.Controls.Add(this.lblRodGear);
            this.tabOptionsPageGear.Controls.Add(this.lblBodyGear);
            this.tabOptionsPageGear.Controls.Add(this.tbBodyGear);
            this.tabOptionsPageGear.Controls.Add(this.lblHandsGear);
            this.tabOptionsPageGear.Controls.Add(this.tbHandsGear);
            this.tabOptionsPageGear.Controls.Add(this.lblLegsGear);
            this.tabOptionsPageGear.Controls.Add(this.tbLegsGear);
            this.tabOptionsPageGear.Controls.Add(this.lblFeetGear);
            this.tabOptionsPageGear.Controls.Add(this.tbFeetGear);
            this.tabOptionsPageGear.Controls.Add(this.lblLRingGear);
            this.tabOptionsPageGear.Controls.Add(this.tbLRingGear);
            this.tabOptionsPageGear.Controls.Add(this.cbLRingGear);
            this.tabOptionsPageGear.Controls.Add(this.lblRRingGear);
            this.tabOptionsPageGear.Controls.Add(this.tbRRingGear);
            this.tabOptionsPageGear.Controls.Add(this.cbRRingGear);
            this.tabOptionsPageGear.Controls.Add(this.lblHeadGear);
            this.tabOptionsPageGear.Controls.Add(this.tbHeadGear);
            this.tabOptionsPageGear.Controls.Add(this.lblNeckGear);
            this.tabOptionsPageGear.Controls.Add(this.tbNeckGear);
            this.tabOptionsPageGear.Controls.Add(this.lblWaistGear);
            this.tabOptionsPageGear.Controls.Add(this.tbWaistGear);
            this.tabOptionsPageGear.Controls.Add(this.cbWaistGear);
            this.tabOptionsPageGear.Location = new System.Drawing.Point(4, 21);
            this.tabOptionsPageGear.Name = "tabOptionsPageGear";
            this.tabOptionsPageGear.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptionsPageGear.Size = new System.Drawing.Size(364, 177);
            this.tabOptionsPageGear.TabIndex = 4;
            this.tabOptionsPageGear.Text = "Gear";
            // 
            // tbBaitGear
            // 
            this.tbBaitGear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbBaitGear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tbBaitGear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbBaitGear.FormattingEnabled = true;
            this.tbBaitGear.Items.AddRange(new object[] {
            ""});
            this.tbBaitGear.Location = new System.Drawing.Point(49, 30);
            this.tbBaitGear.Name = "tbBaitGear";
            this.tbBaitGear.Size = new System.Drawing.Size(97, 20);
            this.tbBaitGear.TabIndex = 3;
            // 
            // tbRodGear
            // 
            this.tbRodGear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbRodGear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tbRodGear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbRodGear.FormattingEnabled = true;
            this.tbRodGear.Items.AddRange(new object[] {
            ""});
            this.tbRodGear.Location = new System.Drawing.Point(49, 6);
            this.tbRodGear.Name = "tbRodGear";
            this.tbRodGear.Size = new System.Drawing.Size(97, 20);
            this.tbRodGear.TabIndex = 1;
            // 
            // lblBaitGear
            // 
            this.lblBaitGear.AutoSize = true;
            this.lblBaitGear.Location = new System.Drawing.Point(6, 33);
            this.lblBaitGear.Name = "lblBaitGear";
            this.lblBaitGear.Size = new System.Drawing.Size(25, 13);
            this.lblBaitGear.TabIndex = 2;
            this.lblBaitGear.Text = "Bait";
            // 
            // lblRodGear
            // 
            this.lblRodGear.AutoSize = true;
            this.lblRodGear.Location = new System.Drawing.Point(6, 9);
            this.lblRodGear.Name = "lblRodGear";
            this.lblRodGear.Size = new System.Drawing.Size(26, 13);
            this.lblRodGear.TabIndex = 0;
            this.lblRodGear.Text = "Rod";
            // 
            // lblBodyGear
            // 
            this.lblBodyGear.AutoSize = true;
            this.lblBodyGear.Location = new System.Drawing.Point(6, 56);
            this.lblBodyGear.Name = "lblBodyGear";
            this.lblBodyGear.Size = new System.Drawing.Size(31, 13);
            this.lblBodyGear.TabIndex = 4;
            this.lblBodyGear.Text = "Body";
            // 
            // tbBodyGear
            // 
            this.tbBodyGear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbBodyGear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tbBodyGear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbBodyGear.Items.AddRange(new object[] {
            ""});
            this.tbBodyGear.Location = new System.Drawing.Point(49, 54);
            this.tbBodyGear.Name = "tbBodyGear";
            this.tbBodyGear.Size = new System.Drawing.Size(97, 20);
            this.tbBodyGear.TabIndex = 5;
            // 
            // lblHandsGear
            // 
            this.lblHandsGear.AutoSize = true;
            this.lblHandsGear.Location = new System.Drawing.Point(6, 80);
            this.lblHandsGear.Name = "lblHandsGear";
            this.lblHandsGear.Size = new System.Drawing.Size(37, 13);
            this.lblHandsGear.TabIndex = 6;
            this.lblHandsGear.Text = "Hands";
            // 
            // tbHandsGear
            // 
            this.tbHandsGear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbHandsGear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tbHandsGear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbHandsGear.Items.AddRange(new object[] {
            ""});
            this.tbHandsGear.Location = new System.Drawing.Point(49, 77);
            this.tbHandsGear.Name = "tbHandsGear";
            this.tbHandsGear.Size = new System.Drawing.Size(97, 20);
            this.tbHandsGear.TabIndex = 7;
            // 
            // lblLegsGear
            // 
            this.lblLegsGear.AutoSize = true;
            this.lblLegsGear.Location = new System.Drawing.Point(6, 104);
            this.lblLegsGear.Name = "lblLegsGear";
            this.lblLegsGear.Size = new System.Drawing.Size(30, 13);
            this.lblLegsGear.TabIndex = 8;
            this.lblLegsGear.Text = "Legs";
            // 
            // tbLegsGear
            // 
            this.tbLegsGear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbLegsGear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tbLegsGear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbLegsGear.Items.AddRange(new object[] {
            ""});
            this.tbLegsGear.Location = new System.Drawing.Point(49, 101);
            this.tbLegsGear.Name = "tbLegsGear";
            this.tbLegsGear.Size = new System.Drawing.Size(97, 20);
            this.tbLegsGear.TabIndex = 9;
            // 
            // lblFeetGear
            // 
            this.lblFeetGear.AutoSize = true;
            this.lblFeetGear.Location = new System.Drawing.Point(6, 128);
            this.lblFeetGear.Name = "lblFeetGear";
            this.lblFeetGear.Size = new System.Drawing.Size(28, 13);
            this.lblFeetGear.TabIndex = 10;
            this.lblFeetGear.Text = "Feet";
            // 
            // tbFeetGear
            // 
            this.tbFeetGear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbFeetGear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tbFeetGear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbFeetGear.Items.AddRange(new object[] {
            ""});
            this.tbFeetGear.Location = new System.Drawing.Point(49, 125);
            this.tbFeetGear.Name = "tbFeetGear";
            this.tbFeetGear.Size = new System.Drawing.Size(97, 20);
            this.tbFeetGear.TabIndex = 11;
            // 
            // lblLRingGear
            // 
            this.lblLRingGear.AutoSize = true;
            this.lblLRingGear.Location = new System.Drawing.Point(161, 80);
            this.lblLRingGear.Name = "lblLRingGear";
            this.lblLRingGear.Size = new System.Drawing.Size(49, 13);
            this.lblLRingGear.TabIndex = 19;
            this.lblLRingGear.Text = "Left Ring";
            // 
            // tbLRingGear
            // 
            this.tbLRingGear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbLRingGear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tbLRingGear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbLRingGear.Items.AddRange(new object[] {
            ""});
            this.tbLRingGear.Location = new System.Drawing.Point(222, 77);
            this.tbLRingGear.Name = "tbLRingGear";
            this.tbLRingGear.Size = new System.Drawing.Size(97, 20);
            this.tbLRingGear.TabIndex = 20;
            this.tbLRingGear.SelectedIndexChanged += new System.EventHandler(this.tbLRingGear_SelectedIndexChanged);
            // 
            // cbLRingGear
            // 
            this.cbLRingGear.AutoSize = true;
            this.cbLRingGear.Enabled = false;
            this.cbLRingGear.Location = new System.Drawing.Point(325, 79);
            this.cbLRingGear.Name = "cbLRingGear";
            this.cbLRingGear.Size = new System.Drawing.Size(40, 17);
            this.cbLRingGear.TabIndex = 21;
            this.cbLRingGear.Text = "AE";
            this.cbLRingGear.UseVisualStyleBackColor = true;
            // 
            // lblRRingGear
            // 
            this.lblRRingGear.AutoSize = true;
            this.lblRRingGear.Location = new System.Drawing.Point(161, 104);
            this.lblRRingGear.Name = "lblRRingGear";
            this.lblRRingGear.Size = new System.Drawing.Size(55, 13);
            this.lblRRingGear.TabIndex = 22;
            this.lblRRingGear.Text = "Right Ring";
            // 
            // tbRRingGear
            // 
            this.tbRRingGear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbRRingGear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tbRRingGear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbRRingGear.Items.AddRange(new object[] {
            ""});
            this.tbRRingGear.Location = new System.Drawing.Point(222, 101);
            this.tbRRingGear.Name = "tbRRingGear";
            this.tbRRingGear.Size = new System.Drawing.Size(97, 20);
            this.tbRRingGear.TabIndex = 23;
            this.tbRRingGear.SelectedIndexChanged += new System.EventHandler(this.tbRRingGear_SelectedIndexChanged);
            // 
            // cbRRingGear
            // 
            this.cbRRingGear.AutoSize = true;
            this.cbRRingGear.Enabled = false;
            this.cbRRingGear.Location = new System.Drawing.Point(325, 103);
            this.cbRRingGear.Name = "cbRRingGear";
            this.cbRRingGear.Size = new System.Drawing.Size(40, 17);
            this.cbRRingGear.TabIndex = 24;
            this.cbRRingGear.Text = "AE";
            this.cbRRingGear.UseVisualStyleBackColor = true;
            // 
            // lblHeadGear
            // 
            this.lblHeadGear.AutoSize = true;
            this.lblHeadGear.Location = new System.Drawing.Point(161, 9);
            this.lblHeadGear.Name = "lblHeadGear";
            this.lblHeadGear.Size = new System.Drawing.Size(32, 13);
            this.lblHeadGear.TabIndex = 12;
            this.lblHeadGear.Text = "Head";
            // 
            // tbHeadGear
            // 
            this.tbHeadGear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbHeadGear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tbHeadGear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbHeadGear.Items.AddRange(new object[] {
            ""});
            this.tbHeadGear.Location = new System.Drawing.Point(222, 6);
            this.tbHeadGear.Name = "tbHeadGear";
            this.tbHeadGear.Size = new System.Drawing.Size(97, 20);
            this.tbHeadGear.TabIndex = 13;
            // 
            // lblNeckGear
            // 
            this.lblNeckGear.AutoSize = true;
            this.lblNeckGear.Location = new System.Drawing.Point(161, 33);
            this.lblNeckGear.Name = "lblNeckGear";
            this.lblNeckGear.Size = new System.Drawing.Size(30, 13);
            this.lblNeckGear.TabIndex = 14;
            this.lblNeckGear.Text = "Neck";
            // 
            // tbNeckGear
            // 
            this.tbNeckGear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbNeckGear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tbNeckGear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbNeckGear.Items.AddRange(new object[] {
            ""});
            this.tbNeckGear.Location = new System.Drawing.Point(222, 30);
            this.tbNeckGear.Name = "tbNeckGear";
            this.tbNeckGear.Size = new System.Drawing.Size(97, 20);
            this.tbNeckGear.TabIndex = 15;
            // 
            // lblWaistGear
            // 
            this.lblWaistGear.AutoSize = true;
            this.lblWaistGear.Location = new System.Drawing.Point(161, 56);
            this.lblWaistGear.Name = "lblWaistGear";
            this.lblWaistGear.Size = new System.Drawing.Size(32, 13);
            this.lblWaistGear.TabIndex = 16;
            this.lblWaistGear.Text = "Waist";
            // 
            // tbWaistGear
            // 
            this.tbWaistGear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbWaistGear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tbWaistGear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbWaistGear.Items.AddRange(new object[] {
            ""});
            this.tbWaistGear.Location = new System.Drawing.Point(222, 53);
            this.tbWaistGear.Name = "tbWaistGear";
            this.tbWaistGear.Size = new System.Drawing.Size(97, 20);
            this.tbWaistGear.TabIndex = 17;
            this.tbWaistGear.SelectedIndexChanged += new System.EventHandler(this.tbWaistGear_SelectedIndexChanged);
            // 
            // cbWaistGear
            // 
            this.cbWaistGear.AutoSize = true;
            this.cbWaistGear.Enabled = false;
            this.cbWaistGear.Location = new System.Drawing.Point(325, 56);
            this.cbWaistGear.Name = "cbWaistGear";
            this.cbWaistGear.Size = new System.Drawing.Size(40, 17);
            this.cbWaistGear.TabIndex = 18;
            this.cbWaistGear.Text = "AE";
            this.cbWaistGear.UseVisualStyleBackColor = true;
            // 
            // tabOptionsPageAdvanced
            // 
            this.tabOptionsPageAdvanced.BackColor = System.Drawing.SystemColors.Control;
            this.tabOptionsPageAdvanced.Controls.Add(this.gbExhaustedBait);
            this.tabOptionsPageAdvanced.Controls.Add(this.gbOnFatigue);
            this.tabOptionsPageAdvanced.Controls.Add(this.groupBox1);
            this.tabOptionsPageAdvanced.Location = new System.Drawing.Point(4, 21);
            this.tabOptionsPageAdvanced.Name = "tabOptionsPageAdvanced";
            this.tabOptionsPageAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptionsPageAdvanced.Size = new System.Drawing.Size(364, 177);
            this.tabOptionsPageAdvanced.TabIndex = 5;
            this.tabOptionsPageAdvanced.Text = "Other";
            // 
            // gbExhaustedBait
            // 
            this.gbExhaustedBait.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbExhaustedBait.Controls.Add(this.cbBaitItemizerSatchel);
            this.gbExhaustedBait.Controls.Add(this.cbBaitItemizerSack);
            this.gbExhaustedBait.Controls.Add(this.numBaitactionOtherTime);
            this.gbExhaustedBait.Controls.Add(this.tbBaitactionOther);
            this.gbExhaustedBait.Controls.Add(this.cbBaitactionOther);
            this.gbExhaustedBait.Controls.Add(this.cbBaitItemizerItemTools);
            this.gbExhaustedBait.Controls.Add(this.cbBaitActionWarp);
            this.gbExhaustedBait.Controls.Add(this.cbBaitActionLogout);
            this.gbExhaustedBait.Controls.Add(this.cbBaitActionShutdown);
            this.gbExhaustedBait.Location = new System.Drawing.Point(131, 6);
            this.gbExhaustedBait.Name = "gbExhaustedBait";
            this.gbExhaustedBait.Size = new System.Drawing.Size(119, 165);
            this.gbExhaustedBait.TabIndex = 3;
            this.gbExhaustedBait.TabStop = false;
            this.gbExhaustedBait.Text = "On Bait Exhausted";
            // 
            // cbBaitItemizerSatchel
            // 
            this.cbBaitItemizerSatchel.AutoSize = true;
            this.cbBaitItemizerSatchel.Enabled = false;
            this.cbBaitItemizerSatchel.Location = new System.Drawing.Point(55, 35);
            this.cbBaitItemizerSatchel.Name = "cbBaitItemizerSatchel";
            this.cbBaitItemizerSatchel.Size = new System.Drawing.Size(61, 17);
            this.cbBaitItemizerSatchel.TabIndex = 10;
            this.cbBaitItemizerSatchel.Text = "Satchel";
            this.cbBaitItemizerSatchel.UseVisualStyleBackColor = true;
            // 
            // cbBaitItemizerSack
            // 
            this.cbBaitItemizerSack.AutoSize = true;
            this.cbBaitItemizerSack.Enabled = false;
            this.cbBaitItemizerSack.Location = new System.Drawing.Point(6, 35);
            this.cbBaitItemizerSack.Name = "cbBaitItemizerSack";
            this.cbBaitItemizerSack.Size = new System.Drawing.Size(49, 17);
            this.cbBaitItemizerSack.TabIndex = 9;
            this.cbBaitItemizerSack.Text = "Sack";
            this.cbBaitItemizerSack.UseVisualStyleBackColor = true;
            // 
            // numBaitactionOtherTime
            // 
            this.numBaitactionOtherTime.DecimalPlaces = 1;
            this.numBaitactionOtherTime.Enabled = false;
            this.numBaitactionOtherTime.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numBaitactionOtherTime.Location = new System.Drawing.Point(67, 52);
            this.numBaitactionOtherTime.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numBaitactionOtherTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBaitactionOtherTime.Name = "numBaitactionOtherTime";
            this.numBaitactionOtherTime.Size = new System.Drawing.Size(46, 18);
            this.numBaitactionOtherTime.TabIndex = 8;
            this.numBaitactionOtherTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tbBaitactionOther
            // 
            this.tbBaitactionOther.Enabled = false;
            this.tbBaitactionOther.Location = new System.Drawing.Point(6, 71);
            this.tbBaitactionOther.Multiline = true;
            this.tbBaitactionOther.Name = "tbBaitactionOther";
            this.tbBaitactionOther.Size = new System.Drawing.Size(107, 18);
            this.tbBaitactionOther.TabIndex = 5;
            this.tbBaitactionOther.Enter += new System.EventHandler(this.tbBaitactionOther_Enter);
            this.tbBaitactionOther.Leave += new System.EventHandler(this.tbBaitactionOther_Leave);
            // 
            // cbBaitactionOther
            // 
            this.cbBaitactionOther.AutoSize = true;
            this.cbBaitactionOther.Location = new System.Drawing.Point(6, 53);
            this.cbBaitactionOther.Name = "cbBaitactionOther";
            this.cbBaitactionOther.Size = new System.Drawing.Size(52, 17);
            this.cbBaitactionOther.TabIndex = 4;
            this.cbBaitactionOther.Text = "Other";
            this.cbBaitactionOther.UseVisualStyleBackColor = true;
            this.cbBaitactionOther.CheckedChanged += new System.EventHandler(this.cbBaitactionOther_CheckedChanged);
            // 
            // cbBaitItemizerItemTools
            // 
            this.cbBaitItemizerItemTools.AutoSize = true;
            this.cbBaitItemizerItemTools.Location = new System.Drawing.Point(6, 17);
            this.cbBaitItemizerItemTools.Name = "cbBaitItemizerItemTools";
            this.cbBaitItemizerItemTools.Size = new System.Drawing.Size(109, 17);
            this.cbBaitItemizerItemTools.TabIndex = 3;
            this.cbBaitItemizerItemTools.Text = "Itemizer/ItemTools";
            this.cbBaitItemizerItemTools.UseVisualStyleBackColor = false;
            this.cbBaitItemizerItemTools.CheckedChanged += new System.EventHandler(this.cbBaitItemizerItemTools_CheckedChanged);
            // 
            // cbBaitActionWarp
            // 
            this.cbBaitActionWarp.AutoSize = true;
            this.cbBaitActionWarp.Location = new System.Drawing.Point(6, 92);
            this.cbBaitActionWarp.Name = "cbBaitActionWarp";
            this.cbBaitActionWarp.Size = new System.Drawing.Size(50, 17);
            this.cbBaitActionWarp.TabIndex = 0;
            this.cbBaitActionWarp.Text = "Warp";
            this.cbBaitActionWarp.UseVisualStyleBackColor = true;
            // 
            // cbBaitActionLogout
            // 
            this.cbBaitActionLogout.AutoSize = true;
            this.cbBaitActionLogout.Location = new System.Drawing.Point(6, 109);
            this.cbBaitActionLogout.Name = "cbBaitActionLogout";
            this.cbBaitActionLogout.Size = new System.Drawing.Size(59, 17);
            this.cbBaitActionLogout.TabIndex = 1;
            this.cbBaitActionLogout.Text = "Logout";
            this.cbBaitActionLogout.UseVisualStyleBackColor = true;
            this.cbBaitActionLogout.CheckedChanged += new System.EventHandler(this.cbBaitActionLogout_CheckedChanged);
            // 
            // cbBaitActionShutdown
            // 
            this.cbBaitActionShutdown.AutoSize = true;
            this.cbBaitActionShutdown.Location = new System.Drawing.Point(6, 127);
            this.cbBaitActionShutdown.Name = "cbBaitActionShutdown";
            this.cbBaitActionShutdown.Size = new System.Drawing.Size(74, 17);
            this.cbBaitActionShutdown.TabIndex = 2;
            this.cbBaitActionShutdown.Text = "Shutdown";
            this.cbBaitActionShutdown.UseVisualStyleBackColor = true;
            this.cbBaitActionShutdown.CheckedChanged += new System.EventHandler(this.cbBaitActionShutdown_CheckedChanged);
            // 
            // gbOnFatigue
            // 
            this.gbOnFatigue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbOnFatigue.Controls.Add(this.cbFatiguedActionWarp);
            this.gbOnFatigue.Controls.Add(this.cbFatiguedActionLogout);
            this.gbOnFatigue.Controls.Add(this.cbFatiguedActionShutdown);
            this.gbOnFatigue.Location = new System.Drawing.Point(256, 6);
            this.gbOnFatigue.Name = "gbOnFatigue";
            this.gbOnFatigue.Size = new System.Drawing.Size(102, 165);
            this.gbOnFatigue.TabIndex = 2;
            this.gbOnFatigue.TabStop = false;
            this.gbOnFatigue.Text = "On Fatigue Stop";
            // 
            // cbFatiguedActionWarp
            // 
            this.cbFatiguedActionWarp.AutoSize = true;
            this.cbFatiguedActionWarp.Location = new System.Drawing.Point(6, 17);
            this.cbFatiguedActionWarp.Name = "cbFatiguedActionWarp";
            this.cbFatiguedActionWarp.Size = new System.Drawing.Size(50, 17);
            this.cbFatiguedActionWarp.TabIndex = 0;
            this.cbFatiguedActionWarp.Text = "Warp";
            this.cbFatiguedActionWarp.UseVisualStyleBackColor = true;
            // 
            // cbFatiguedActionLogout
            // 
            this.cbFatiguedActionLogout.AutoSize = true;
            this.cbFatiguedActionLogout.Location = new System.Drawing.Point(6, 35);
            this.cbFatiguedActionLogout.Name = "cbFatiguedActionLogout";
            this.cbFatiguedActionLogout.Size = new System.Drawing.Size(59, 17);
            this.cbFatiguedActionLogout.TabIndex = 1;
            this.cbFatiguedActionLogout.Text = "Logout";
            this.cbFatiguedActionLogout.UseVisualStyleBackColor = true;
            this.cbFatiguedActionLogout.CheckedChanged += new System.EventHandler(this.cbFatiguedActionLogout_CheckedChanged);
            // 
            // cbFatiguedActionShutdown
            // 
            this.cbFatiguedActionShutdown.AutoSize = true;
            this.cbFatiguedActionShutdown.Location = new System.Drawing.Point(6, 53);
            this.cbFatiguedActionShutdown.Name = "cbFatiguedActionShutdown";
            this.cbFatiguedActionShutdown.Size = new System.Drawing.Size(74, 17);
            this.cbFatiguedActionShutdown.TabIndex = 2;
            this.cbFatiguedActionShutdown.Text = "Shutdown";
            this.cbFatiguedActionShutdown.UseVisualStyleBackColor = true;
            this.cbFatiguedActionShutdown.CheckedChanged += new System.EventHandler(this.cbFatiguedActionShutdown_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.tbFullactionOther);
            this.groupBox1.Controls.Add(this.cbInventoryItemizerSatchel);
            this.groupBox1.Controls.Add(this.cbInventoryItemizerSack);
            this.groupBox1.Controls.Add(this.numFullactionOtherTime);
            this.groupBox1.Controls.Add(this.cbFullActionStop);
            this.groupBox1.Controls.Add(this.cbFullactionOther);
            this.groupBox1.Controls.Add(this.cbFullactionShutdown);
            this.groupBox1.Controls.Add(this.cbFullactionLogout);
            this.groupBox1.Controls.Add(this.cbFullactionWarp);
            this.groupBox1.Controls.Add(this.cbInventoryItemizerItemTools);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(119, 165);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "On Full Inventory";
            // 
            // tbFullactionOther
            // 
            this.tbFullactionOther.Enabled = false;
            this.tbFullactionOther.Location = new System.Drawing.Point(6, 71);
            this.tbFullactionOther.Multiline = true;
            this.tbFullactionOther.Name = "tbFullactionOther";
            this.tbFullactionOther.Size = new System.Drawing.Size(107, 18);
            this.tbFullactionOther.TabIndex = 2;
            this.tbFullactionOther.Enter += new System.EventHandler(this.tbFullactionOther_Enter);
            this.tbFullactionOther.Leave += new System.EventHandler(this.tbFullactionOther_Leave);
            // 
            // cbInventoryItemizerSatchel
            // 
            this.cbInventoryItemizerSatchel.AutoSize = true;
            this.cbInventoryItemizerSatchel.Enabled = false;
            this.cbInventoryItemizerSatchel.Location = new System.Drawing.Point(56, 35);
            this.cbInventoryItemizerSatchel.Name = "cbInventoryItemizerSatchel";
            this.cbInventoryItemizerSatchel.Size = new System.Drawing.Size(61, 17);
            this.cbInventoryItemizerSatchel.TabIndex = 12;
            this.cbInventoryItemizerSatchel.Text = "Satchel";
            this.cbInventoryItemizerSatchel.UseVisualStyleBackColor = true;
            // 
            // cbInventoryItemizerSack
            // 
            this.cbInventoryItemizerSack.AutoSize = true;
            this.cbInventoryItemizerSack.Enabled = false;
            this.cbInventoryItemizerSack.Location = new System.Drawing.Point(6, 35);
            this.cbInventoryItemizerSack.Name = "cbInventoryItemizerSack";
            this.cbInventoryItemizerSack.Size = new System.Drawing.Size(49, 17);
            this.cbInventoryItemizerSack.TabIndex = 11;
            this.cbInventoryItemizerSack.Text = "Sack";
            this.cbInventoryItemizerSack.UseVisualStyleBackColor = true;
            // 
            // numFullactionOtherTime
            // 
            this.numFullactionOtherTime.DecimalPlaces = 1;
            this.numFullactionOtherTime.Enabled = false;
            this.numFullactionOtherTime.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numFullactionOtherTime.Location = new System.Drawing.Point(67, 52);
            this.numFullactionOtherTime.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numFullactionOtherTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFullactionOtherTime.Name = "numFullactionOtherTime";
            this.numFullactionOtherTime.Size = new System.Drawing.Size(46, 18);
            this.numFullactionOtherTime.TabIndex = 7;
            this.numFullactionOtherTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbFullActionStop
            // 
            this.cbFullActionStop.AutoSize = true;
            this.cbFullActionStop.Location = new System.Drawing.Point(6, 92);
            this.cbFullActionStop.Name = "cbFullActionStop";
            this.cbFullActionStop.Size = new System.Drawing.Size(84, 17);
            this.cbFullActionStop.TabIndex = 6;
            this.cbFullActionStop.Text = "Stop Fishing";
            this.cbFullActionStop.UseVisualStyleBackColor = true;
            this.cbFullActionStop.CheckedChanged += new System.EventHandler(this.cbFullActionStop_CheckedChanged);
            // 
            // cbFullactionOther
            // 
            this.cbFullactionOther.AutoSize = true;
            this.cbFullactionOther.Location = new System.Drawing.Point(6, 53);
            this.cbFullactionOther.Name = "cbFullactionOther";
            this.cbFullactionOther.Size = new System.Drawing.Size(52, 17);
            this.cbFullactionOther.TabIndex = 1;
            this.cbFullactionOther.Text = "Other";
            this.cbFullactionOther.UseVisualStyleBackColor = true;
            this.cbFullactionOther.CheckedChanged += new System.EventHandler(this.cbFullactionOther_CheckedChanged);
            // 
            // cbFullactionShutdown
            // 
            this.cbFullactionShutdown.AutoSize = true;
            this.cbFullactionShutdown.Location = new System.Drawing.Point(6, 145);
            this.cbFullactionShutdown.Name = "cbFullactionShutdown";
            this.cbFullactionShutdown.Size = new System.Drawing.Size(74, 17);
            this.cbFullactionShutdown.TabIndex = 5;
            this.cbFullactionShutdown.Text = "Shutdown";
            this.cbFullactionShutdown.UseVisualStyleBackColor = true;
            this.cbFullactionShutdown.CheckedChanged += new System.EventHandler(this.cbFullactionShutdown_CheckedChanged);
            // 
            // cbFullactionLogout
            // 
            this.cbFullactionLogout.AutoSize = true;
            this.cbFullactionLogout.Location = new System.Drawing.Point(6, 127);
            this.cbFullactionLogout.Name = "cbFullactionLogout";
            this.cbFullactionLogout.Size = new System.Drawing.Size(59, 17);
            this.cbFullactionLogout.TabIndex = 4;
            this.cbFullactionLogout.Text = "Logout";
            this.cbFullactionLogout.UseVisualStyleBackColor = true;
            this.cbFullactionLogout.CheckedChanged += new System.EventHandler(this.cbFullactionLogout_CheckedChanged);
            // 
            // cbFullactionWarp
            // 
            this.cbFullactionWarp.AutoSize = true;
            this.cbFullactionWarp.Location = new System.Drawing.Point(6, 109);
            this.cbFullactionWarp.Name = "cbFullactionWarp";
            this.cbFullactionWarp.Size = new System.Drawing.Size(50, 17);
            this.cbFullactionWarp.TabIndex = 3;
            this.cbFullactionWarp.Text = "Warp";
            this.cbFullactionWarp.UseVisualStyleBackColor = true;
            // 
            // cbInventoryItemizerItemTools
            // 
            this.cbInventoryItemizerItemTools.AutoSize = true;
            this.cbInventoryItemizerItemTools.Location = new System.Drawing.Point(6, 17);
            this.cbInventoryItemizerItemTools.Name = "cbInventoryItemizerItemTools";
            this.cbInventoryItemizerItemTools.Size = new System.Drawing.Size(109, 17);
            this.cbInventoryItemizerItemTools.TabIndex = 0;
            this.cbInventoryItemizerItemTools.Text = "Itemizer/ItemTools";
            this.cbInventoryItemizerItemTools.UseVisualStyleBackColor = false;
            this.cbInventoryItemizerItemTools.CheckedChanged += new System.EventHandler(this.cbEnableItemizerItemTools_CheckedChanged);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // statusStripMain
            // 
            this.statusStripMain.BackColor = System.Drawing.SystemColors.Control;
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblVanaDay,
            this.lblVanaClock,
            this.lblStatusTitle,
            this.lblStatus,
            this.lblHP,
            this.progressBarST});
            this.statusStripMain.Location = new System.Drawing.Point(0, 227);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(526, 25);
            this.statusStripMain.TabIndex = 1;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // lblVanaDay
            // 
            this.lblVanaDay.AutoSize = false;
            this.lblVanaDay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lblVanaDay.Font = new System.Drawing.Font("Segoe UI", 2.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVanaDay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lblVanaDay.Name = "lblVanaDay";
            this.lblVanaDay.Size = new System.Drawing.Size(18, 20);
            this.lblVanaDay.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // lblVanaClock
            // 
            this.lblVanaClock.AutoSize = false;
            this.lblVanaClock.Name = "lblVanaClock";
            this.lblVanaClock.Size = new System.Drawing.Size(80, 20);
            this.lblVanaClock.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.AutoSize = false;
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(48, 20);
            this.lblStatusTitle.Text = "Status:";
            this.lblStatusTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStatusTitle.Click += new System.EventHandler(this.lblStatusTitle_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(263, 20);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Idle.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHP
            // 
            this.lblHP.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHP.Name = "lblHP";
            this.lblHP.Size = new System.Drawing.Size(0, 20);
            // 
            // progressBarST
            // 
            this.progressBarST.Name = "progressBarST";
            this.progressBarST.Size = new System.Drawing.Size(100, 19);
            this.progressBarST.Click += new System.EventHandler(this.progressBarST_Click);
            // 
            // FishingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(526, 252);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.pnlLog);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(542, 290);
            this.Name = "FishingForm";
            this.Opacity = 0.99D;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FishingForm";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.FishingForm_Activated);
            this.Load += new System.EventHandler(this.FishingForm_Load);
            this.contextMenuStats.ResumeLayout(false);
            this.contextMenuListBox.ResumeLayout(false);
            this.pnlLog.ResumeLayout(false);
            this.pnlWanted.ResumeLayout(false);
            this.pnlWanted.PerformLayout();
            this.pnlUnwanted.ResumeLayout(false);
            this.pnlUnwanted.PerformLayout();
            this.tabDisplay.ResumeLayout(false);
            this.tabDisplayPageChat.ResumeLayout(false);
            this.tabDisplayPageChat.PerformLayout();
            this.tabChat.ResumeLayout(false);
            this.tabChatPageLog.ResumeLayout(false);
            this.tabChatPageFishing.ResumeLayout(false);
            this.tabChatPageTell.ResumeLayout(false);
            this.tabChatPageParty.ResumeLayout(false);
            this.tabChatPageLS.ResumeLayout(false);
            this.tabChatPageSay.ResumeLayout(false);
            this.tabChatPageDB.ResumeLayout(false);
            this.tabDisplayPageStats.ResumeLayout(false);
            this.tabDisplayPageInfo.ResumeLayout(false);
            this.tabDisplayPageInfo.PerformLayout();
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.tabDisplayPageOptions.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.tabOptionsPageForm.ResumeLayout(false);
            this.tabOptionsPageForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSkillCap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCatch)).EndInit();
            this.gbGeneralFishing.ResumeLayout(false);
            this.gbGeneralFishing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxNoCatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCastIntervalLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCastIntervalHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackOpacity)).EndInit();
            this.tabOptionsPageChat.ResumeLayout(false);
            this.tabOptionsPageChat.PerformLayout();
            this.panelChatDetect.ResumeLayout(false);
            this.gbGMDetect.ResumeLayout(false);
            this.gbGMDetect.PerformLayout();
            this.tabOptionsPageFight.ResumeLayout(false);
            this.tabOptionsPageFight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReactionHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReactionLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFakeLargeIntervalLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFakeLargeIntervalHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFakeSmallIntervalLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFakeSmallIntervalHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuickKill)).EndInit();
            this.tabOptionsPageGear.ResumeLayout(false);
            this.tabOptionsPageGear.PerformLayout();
            this.tabOptionsPageAdvanced.ResumeLayout(false);
            this.gbExhaustedBait.ResumeLayout(false);
            this.gbExhaustedBait.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBaitactionOtherTime)).EndInit();
            this.gbOnFatigue.ResumeLayout(false);
            this.gbOnFatigue.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFullactionOtherTime)).EndInit();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStats;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuListBox;
        private System.Windows.Forms.ToolStripMenuItem changeNameToolStripMenuItem;
        private System.Windows.Forms.TextBox tbChangeName;
        private System.Windows.Forms.Label lblBait;
        private System.Windows.Forms.Label lblRod;
        private System.Windows.Forms.Label lblBaitHeader;
        private System.Windows.Forms.Label lblZone;
        private System.Windows.Forms.Label lblRodHeader;
        private System.Windows.Forms.Label lblZoneHeader;
        private System.Windows.Forms.RichTextBox rtbStats;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.TabControl tabDisplay;
        private System.Windows.Forms.TabPage tabDisplayPageStats;
        private System.Windows.Forms.TabPage tabDisplayPageInfo;
        private System.Windows.Forms.TabPage tabDisplayPageOptions;
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TabControl tabOptions;
        private System.Windows.Forms.TabPage tabOptionsPageFight;
        private System.Windows.Forms.Label lblNoCatchAtHeader;
        private System.Windows.Forms.TabPage tabOptionsPageForm;
        private System.Windows.Forms.Button btnSettingsSave;
        private System.Windows.Forms.Button btnSettingsReset;
        private System.Windows.Forms.CheckBox cbAlwaysOnTop;
        private System.Windows.Forms.TabPage tabDisplayPageChat;
        private System.Windows.Forms.TabControl tabChat;
        private System.Windows.Forms.TabPage tabChatPageLog;
        private System.Windows.Forms.TabPage tabChatPageFishing;
        private System.Windows.Forms.TextBox tbChat;
        private System.Windows.Forms.Button btnChatSend;
        private System.Windows.Forms.TabPage tabChatPageTell;
        private System.Windows.Forms.TabPage tabChatPageParty;
        private System.Windows.Forms.TabPage tabChatPageLS;
        private System.Windows.Forms.Label lblVanaTime;
        private System.Windows.Forms.TrackBar trackOpacity;
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.RichTextBox rtbFish;
        private System.Windows.Forms.RichTextBox rtbTell;
        private System.Windows.Forms.RichTextBox rtbParty;
        private System.Windows.Forms.RichTextBox rtbShell;
        private System.Windows.Forms.TabPage tabOptionsPageGear;
        private System.Windows.Forms.Label lblBodyGear;
        private System.Windows.Forms.ComboBox tbBodyGear;
        private System.Windows.Forms.Label lblHandsGear;
        private System.Windows.Forms.ComboBox tbHandsGear;
        private System.Windows.Forms.Label lblLegsGear;
        private System.Windows.Forms.ComboBox tbLegsGear;
        private System.Windows.Forms.Label lblFeetGear;
        private System.Windows.Forms.ComboBox tbFeetGear;
        private System.Windows.Forms.Label lblLRingGear;
        private System.Windows.Forms.ComboBox tbLRingGear;
        private System.Windows.Forms.CheckBox cbLRingGear;
        private System.Windows.Forms.Label lblRRingGear;
        private System.Windows.Forms.ComboBox tbRRingGear;
        private System.Windows.Forms.CheckBox cbRRingGear;
        private System.Windows.Forms.Label lblHeadGear;
        private System.Windows.Forms.ComboBox tbHeadGear;
        private System.Windows.Forms.Label lblNeckGear;
        private System.Windows.Forms.ComboBox tbNeckGear;
        private System.Windows.Forms.Label lblWaistGear;
        private System.Windows.Forms.ComboBox tbWaistGear;
        private System.Windows.Forms.CheckBox cbWaistGear;
        private System.Windows.Forms.TabPage tabOptionsPageAdvanced;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbFullactionWarp;
        private System.Windows.Forms.CheckBox cbFullactionLogout;
        private System.Windows.Forms.CheckBox cbFullactionShutdown;
        private System.Windows.Forms.TextBox tbFullactionOther;
        private System.Windows.Forms.CheckBox cbFullactionOther;
        private System.Windows.Forms.GroupBox gbOnFatigue;
        private System.Windows.Forms.CheckBox cbFatiguedActionWarp;
        private System.Windows.Forms.CheckBox cbFatiguedActionLogout;
        private System.Windows.Forms.CheckBox cbFatiguedActionShutdown;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusTitle;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar progressBarST;
        private System.Windows.Forms.ToolStripStatusLabel lblHP;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox cbExtend;
        private System.Windows.Forms.CheckBox cbAutoKill;
        private System.Windows.Forms.CheckBox cbQuickKill;
        private System.Windows.Forms.NumericUpDown numQuickKill;
        private System.Windows.Forms.Button btnRefreshLists;
        private System.Windows.Forms.Panel pnlWanted;
        private System.Windows.Forms.CheckBox cbCatchUnknown;
        private System.Windows.Forms.Label lblWantedHeader;
        private System.Windows.Forms.ListBox lbWanted;
        private System.Windows.Forms.Panel pnlUnwanted;
        private System.Windows.Forms.ListBox lbUnwanted;
        private System.Windows.Forms.Label lblUnwantedHeader;
        private System.Windows.Forms.Label lblSkill;
        private System.Windows.Forms.Label lblSkillHeader;
        private System.Windows.Forms.ToolStripStatusLabel lblVanaDay;
        private System.Windows.Forms.Label lblGilHeader;
        private System.Windows.Forms.Label lblGil;
        private System.Windows.Forms.Label lblKillSeconds;
        private System.Windows.Forms.Label lblMaxNoCatch;
        private System.Windows.Forms.Label lblCastWait;
        private System.Windows.Forms.NumericUpDown numCastIntervalHigh;
        private System.Windows.Forms.NumericUpDown numCastIntervalLow;
        private System.Windows.Forms.NumericUpDown numMaxNoCatch;
        private System.Windows.Forms.Button btnCastReset;
        private System.Windows.Forms.CheckBox cbIgnoreItem;
        private System.Windows.Forms.CheckBox cbIgnoreMonster;
        private System.Windows.Forms.CheckBox cbIgnoreSmallFish;
        private System.Windows.Forms.CheckBox cbIgnoreLargeFish;
        private System.Windows.Forms.Label lblNoCatchAt;
        private System.Windows.Forms.CheckBox cbStopSound;
        private System.Windows.Forms.CheckBox cbReleaseSmall;
        private System.Windows.Forms.CheckBox cbReaction;
        private System.Windows.Forms.NumericUpDown numFakeSmallIntervalLow;
        private System.Windows.Forms.NumericUpDown numReactionLow;
        private System.Windows.Forms.NumericUpDown numFakeSmallIntervalHigh;
        private System.Windows.Forms.NumericUpDown numReactionHigh;
        private System.Windows.Forms.NumericUpDown numFakeLargeIntervalLow;
        private System.Windows.Forms.NumericUpDown numFakeLargeIntervalHigh;
        private System.Windows.Forms.CheckBox cbReleaseLarge;
        private System.Windows.Forms.Label lblSatchelHeader;
        private System.Windows.Forms.Label lblInventoryHeader;
        private System.Windows.Forms.Label lblSackHeader;
        private System.Windows.Forms.Label lblSackSpace;
        private System.Windows.Forms.Label lblSatchelSpace;
        private System.Windows.Forms.Label lblInventorySpace;
        private System.Windows.Forms.ToolStripStatusLabel lblVanaClock;
        private System.Windows.Forms.Button btnResize;
        private System.Windows.Forms.TabPage tabChatPageSay;
        private System.Windows.Forms.RichTextBox rtbSay;
        private System.Windows.Forms.GroupBox gbGeneralFishing;
        private System.Windows.Forms.Label lblVanaTimeHeader;
        private System.Windows.Forms.Label lblEarthTime;
        private System.Windows.Forms.Label lblEarthTimeHeader;
        private System.Windows.Forms.Button btnStartM;
        private System.Windows.Forms.CheckBox cbFishHP;
        private System.Windows.Forms.NumericUpDown numMaxCatch;
        private System.Windows.Forms.CheckBox cbMaxCatch;
        private System.Windows.Forms.CheckBox cbSneakFishing;
        private System.Windows.Forms.GroupBox gbExhaustedBait;
        private System.Windows.Forms.CheckBox cbBaitActionWarp;
        private System.Windows.Forms.CheckBox cbBaitActionLogout;
        private System.Windows.Forms.CheckBox cbBaitActionShutdown;
        private System.Windows.Forms.CheckBox cbInventoryItemizerItemTools;
        private System.Windows.Forms.ComboBox tbBaitGear;
        private System.Windows.Forms.ComboBox tbRodGear;
        private System.Windows.Forms.Label lblBaitGear;
        private System.Windows.Forms.Label lblRodGear;
        private System.Windows.Forms.TabPage tabOptionsPageChat;
        private System.Windows.Forms.GroupBox gbGMDetect;
        private System.Windows.Forms.CheckBox cbGMdetectAutostop;
        private System.Windows.Forms.Panel panelChatDetect;
        private System.Windows.Forms.Button btnChatDetectAdd;
        private System.Windows.Forms.CheckBox cbTellDetect;
        private System.Windows.Forms.CheckBox cbChatDetect;
        private System.Windows.Forms.NumericUpDown numSkillCap;
        private System.Windows.Forms.CheckBox cbSkillCap;
        private System.Windows.Forms.TextBox tbBaitactionOther;
        private System.Windows.Forms.CheckBox cbBaitactionOther;
        private System.Windows.Forms.CheckBox cbBaitItemizerItemTools;
        private System.Windows.Forms.CheckBox cbFullActionStop;
        private System.Windows.Forms.TabPage tabChatPageDB;
        private System.Windows.Forms.RichTextBox rtbDB;
        private System.Windows.Forms.CheckBox cbBaitItemizerSatchel;
        private System.Windows.Forms.CheckBox cbBaitItemizerSack;
        private System.Windows.Forms.NumericUpDown numBaitactionOtherTime;
        private System.Windows.Forms.CheckBox cbInventoryItemizerSatchel;
        private System.Windows.Forms.CheckBox cbInventoryItemizerSack;
        private System.Windows.Forms.NumericUpDown numFullactionOtherTime;
    }
}

