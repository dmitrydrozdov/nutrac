namespace Player
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.curStateTextBox = new System.Windows.Forms.TextBox();
            this.statesTotalTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTraceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statesVisualizationPanel = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.prevStateButton = new System.Windows.Forms.Button();
            this.nextStateButton = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.panel4 = new System.Windows.Forms.Panel();
            this.stopButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.stateVarsListBox = new System.Windows.Forms.ListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.setAsInputButton = new System.Windows.Forms.Button();
            this.unsetUnputButton = new System.Windows.Forms.Button();
            this.setAsOutputButton = new System.Windows.Forms.Button();
            this.unsetAsOutputButton = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.inputVarsListBox = new System.Windows.Forms.ListBox();
            this.outputVarsListBox = new System.Windows.Forms.ListBox();
            this.stopOpcServerButton = new System.Windows.Forms.Button();
            this.startOpcServer = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.messageRichTextBox = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.curStateTextBox);
            this.panel1.Controls.Add(this.statesTotalTextBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 101);
            this.panel1.TabIndex = 0;
            // 
            // curStateTextBox
            // 
            this.curStateTextBox.Enabled = false;
            this.curStateTextBox.Location = new System.Drawing.Point(71, 40);
            this.curStateTextBox.Name = "curStateTextBox";
            this.curStateTextBox.Size = new System.Drawing.Size(100, 20);
            this.curStateTextBox.TabIndex = 2;
            // 
            // statesTotalTextBox
            // 
            this.statesTotalTextBox.Enabled = false;
            this.statesTotalTextBox.Location = new System.Drawing.Point(71, 14);
            this.statesTotalTextBox.Name = "statesTotalTextBox";
            this.statesTotalTextBox.Size = new System.Drawing.Size(100, 20);
            this.statesTotalTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Current state";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "States total";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(606, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openTraceToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openTraceToolStripMenuItem
            // 
            this.openTraceToolStripMenuItem.Name = "openTraceToolStripMenuItem";
            this.openTraceToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.openTraceToolStripMenuItem.Text = "Open trace";
            this.openTraceToolStripMenuItem.Click += new System.EventHandler(this.openTraceToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.statesVisualizationPanel);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Location = new System.Drawing.Point(6, 113);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(589, 155);
            this.panel2.TabIndex = 2;
            // 
            // statesVisualizationPanel
            // 
            this.statesVisualizationPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statesVisualizationPanel.Location = new System.Drawing.Point(0, 0);
            this.statesVisualizationPanel.Name = "statesVisualizationPanel";
            this.statesVisualizationPanel.Size = new System.Drawing.Size(589, 64);
            this.statesVisualizationPanel.TabIndex = 5;
            this.statesVisualizationPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.statesVisualizationPanel_Paint);
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.prevStateButton);
            this.panel5.Controls.Add(this.nextStateButton);
            this.panel5.Controls.Add(this.trackBar1);
            this.panel5.Location = new System.Drawing.Point(1, 66);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(585, 50);
            this.panel5.TabIndex = 4;
            // 
            // prevStateButton
            // 
            this.prevStateButton.Location = new System.Drawing.Point(7, 4);
            this.prevStateButton.Name = "prevStateButton";
            this.prevStateButton.Size = new System.Drawing.Size(36, 23);
            this.prevStateButton.TabIndex = 1;
            this.prevStateButton.Text = "<<";
            this.prevStateButton.UseVisualStyleBackColor = true;
            // 
            // nextStateButton
            // 
            this.nextStateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nextStateButton.Location = new System.Drawing.Point(552, 4);
            this.nextStateButton.Name = "nextStateButton";
            this.nextStateButton.Size = new System.Drawing.Size(30, 23);
            this.nextStateButton.TabIndex = 2;
            this.nextStateButton.Text = ">>";
            this.nextStateButton.UseVisualStyleBackColor = true;
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Location = new System.Drawing.Point(49, 4);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(501, 45);
            this.trackBar1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.stopButton);
            this.panel4.Controls.Add(this.resetButton);
            this.panel4.Controls.Add(this.playButton);
            this.panel4.Location = new System.Drawing.Point(147, 122);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(245, 30);
            this.panel4.TabIndex = 3;
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(165, 3);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(3, 3);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 3;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(84, 3);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(75, 23);
            this.playButton.TabIndex = 3;
            this.playButton.Text = "Start";
            this.playButton.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(606, 411);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(598, 385);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Variables";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.stateVarsListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.stopOpcServerButton);
            this.splitContainer1.Panel2.Controls.Add(this.startOpcServer);
            this.splitContainer1.Size = new System.Drawing.Size(592, 379);
            this.splitContainer1.SplitterDistance = 233;
            this.splitContainer1.TabIndex = 2;
            // 
            // stateVarsListBox
            // 
            this.stateVarsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stateVarsListBox.FormattingEnabled = true;
            this.stateVarsListBox.Location = new System.Drawing.Point(0, 0);
            this.stateVarsListBox.Name = "stateVarsListBox";
            this.stateVarsListBox.Size = new System.Drawing.Size(233, 379);
            this.stateVarsListBox.TabIndex = 0;
            this.stateVarsListBox.SelectedIndexChanged += new System.EventHandler(this.stateVarsListBox_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel3.Controls.Add(this.setAsInputButton);
            this.panel3.Controls.Add(this.unsetUnputButton);
            this.panel3.Controls.Add(this.setAsOutputButton);
            this.panel3.Controls.Add(this.unsetAsOutputButton);
            this.panel3.Location = new System.Drawing.Point(3, 43);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(40, 213);
            this.panel3.TabIndex = 4;
            // 
            // setAsInputButton
            // 
            this.setAsInputButton.Location = new System.Drawing.Point(3, 3);
            this.setAsInputButton.Name = "setAsInputButton";
            this.setAsInputButton.Size = new System.Drawing.Size(32, 23);
            this.setAsInputButton.TabIndex = 1;
            this.setAsInputButton.Text = ">>";
            this.setAsInputButton.UseVisualStyleBackColor = true;
            this.setAsInputButton.Click += new System.EventHandler(this.setAsInputButton_Click);
            // 
            // unsetUnputButton
            // 
            this.unsetUnputButton.Location = new System.Drawing.Point(3, 32);
            this.unsetUnputButton.Name = "unsetUnputButton";
            this.unsetUnputButton.Size = new System.Drawing.Size(32, 23);
            this.unsetUnputButton.TabIndex = 1;
            this.unsetUnputButton.Text = "<<";
            this.unsetUnputButton.UseVisualStyleBackColor = true;
            this.unsetUnputButton.Click += new System.EventHandler(this.unsetUnputButton_Click);
            // 
            // setAsOutputButton
            // 
            this.setAsOutputButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setAsOutputButton.Location = new System.Drawing.Point(3, 158);
            this.setAsOutputButton.Name = "setAsOutputButton";
            this.setAsOutputButton.Size = new System.Drawing.Size(32, 23);
            this.setAsOutputButton.TabIndex = 1;
            this.setAsOutputButton.Text = ">>";
            this.setAsOutputButton.UseVisualStyleBackColor = true;
            this.setAsOutputButton.Click += new System.EventHandler(this.setAsOutputButton_Click);
            // 
            // unsetAsOutputButton
            // 
            this.unsetAsOutputButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.unsetAsOutputButton.Location = new System.Drawing.Point(3, 187);
            this.unsetAsOutputButton.Name = "unsetAsOutputButton";
            this.unsetAsOutputButton.Size = new System.Drawing.Size(32, 23);
            this.unsetAsOutputButton.TabIndex = 1;
            this.unsetAsOutputButton.Text = "<<";
            this.unsetAsOutputButton.UseVisualStyleBackColor = true;
            this.unsetAsOutputButton.Click += new System.EventHandler(this.unsetAsOutputButton_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(44, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.inputVarsListBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.outputVarsListBox);
            this.splitContainer2.Size = new System.Drawing.Size(306, 337);
            this.splitContainer2.SplitterDistance = 153;
            this.splitContainer2.TabIndex = 3;
            // 
            // inputVarsListBox
            // 
            this.inputVarsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputVarsListBox.FormattingEnabled = true;
            this.inputVarsListBox.Location = new System.Drawing.Point(0, 0);
            this.inputVarsListBox.Name = "inputVarsListBox";
            this.inputVarsListBox.Size = new System.Drawing.Size(306, 153);
            this.inputVarsListBox.TabIndex = 0;
            // 
            // outputVarsListBox
            // 
            this.outputVarsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputVarsListBox.FormattingEnabled = true;
            this.outputVarsListBox.Location = new System.Drawing.Point(0, 0);
            this.outputVarsListBox.Name = "outputVarsListBox";
            this.outputVarsListBox.Size = new System.Drawing.Size(306, 180);
            this.outputVarsListBox.TabIndex = 0;
            // 
            // stopOpcServerButton
            // 
            this.stopOpcServerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopOpcServerButton.Enabled = false;
            this.stopOpcServerButton.Location = new System.Drawing.Point(126, 351);
            this.stopOpcServerButton.Name = "stopOpcServerButton";
            this.stopOpcServerButton.Size = new System.Drawing.Size(109, 23);
            this.stopOpcServerButton.TabIndex = 1;
            this.stopOpcServerButton.Text = "Stop OPC Server";
            this.stopOpcServerButton.UseVisualStyleBackColor = true;
            this.stopOpcServerButton.Click += new System.EventHandler(this.stopOpcServerButton_Click);
            // 
            // startOpcServer
            // 
            this.startOpcServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startOpcServer.Location = new System.Drawing.Point(241, 351);
            this.startOpcServer.Name = "startOpcServer";
            this.startOpcServer.Size = new System.Drawing.Size(109, 23);
            this.startOpcServer.TabIndex = 1;
            this.startOpcServer.Text = "Start OPC Server";
            this.startOpcServer.UseVisualStyleBackColor = true;
            this.startOpcServer.Click += new System.EventHandler(this.startOpcServer_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(598, 531);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Player";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 24);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.messageRichTextBox);
            this.splitContainer3.Size = new System.Drawing.Size(606, 557);
            this.splitContainer3.SplitterDistance = 411;
            this.splitContainer3.TabIndex = 4;
            // 
            // messageRichTextBox
            // 
            this.messageRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.messageRichTextBox.Name = "messageRichTextBox";
            this.messageRichTextBox.Size = new System.Drawing.Size(606, 142);
            this.messageRichTextBox.TabIndex = 0;
            this.messageRichTextBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 581);
            this.Controls.Add(this.splitContainer3);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(563, 499);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox curStateTextBox;
        private System.Windows.Forms.TextBox statesTotalTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button nextStateButton;
        private System.Windows.Forms.Button prevStateButton;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem openTraceToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button unsetUnputButton;
        private System.Windows.Forms.Button setAsInputButton;
        private System.Windows.Forms.Button startOpcServer;
        private System.Windows.Forms.ListBox outputVarsListBox;
        private System.Windows.Forms.ListBox inputVarsListBox;
        private System.Windows.Forms.ListBox stateVarsListBox;
        private System.Windows.Forms.Button unsetAsOutputButton;
        private System.Windows.Forms.Button setAsOutputButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel statesVisualizationPanel;
        private System.Windows.Forms.Button stopOpcServerButton;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.RichTextBox messageRichTextBox;
    }
}

