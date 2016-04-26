namespace Labo.DownloadManager.Win.Controls
{
    partial class DownloadTaskList
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
            this.components = new System.ComponentModel.Container();
            this.downloadTaskMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lvwDownloadTasks = new System.Windows.Forms.ListView();
            this.columnFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCompleted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnRemaining = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnAvgSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnResume = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnUrl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblDownloadTasksInfo = new System.Windows.Forms.Label();
            this.tbDownloadTaskInfo = new System.Windows.Forms.TabControl();
            this.tpDownloadTaskInfo = new System.Windows.Forms.TabPage();
            this.pnlTaskInfo = new System.Windows.Forms.Panel();
            this.lblUrlValue = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.lblAverageSpeedValue = new System.Windows.Forms.Label();
            this.lblAverageSpeed = new System.Windows.Forms.Label();
            this.lblSizeValue = new System.Windows.Forms.Label();
            this.lblDownloadLocation = new System.Windows.Forms.LinkLabel();
            this.picFileType = new System.Windows.Forms.PictureBox();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblFileNameValue = new System.Windows.Forms.Label();
            this.tpDownloadTaskSegments = new System.Windows.Forms.TabPage();
            this.lvwDownloadTaskSegments = new System.Windows.Forms.ListView();
            this.columnSegmentProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSegmentCompleted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSegmentSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSegmentStartPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSegmentEndPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSegmentDownloadSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSegmentRemaining = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSegmentState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSegmentUrl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpDownloadTaskLogs = new System.Windows.Forms.TabPage();
            this.lblCopyUrl = new System.Windows.Forms.LinkLabel();
            this.downloadTaskMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tbDownloadTaskInfo.SuspendLayout();
            this.tpDownloadTaskInfo.SuspendLayout();
            this.pnlTaskInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFileType)).BeginInit();
            this.tpDownloadTaskSegments.SuspendLayout();
            this.SuspendLayout();
            // 
            // downloadTaskMenuStrip
            // 
            this.downloadTaskMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pauseToolStripMenuItem,
            this.startToolStripMenuItem});
            this.downloadTaskMenuStrip.Name = "downloadTaskMenuStrip";
            this.downloadTaskMenuStrip.Size = new System.Drawing.Size(106, 48);
            this.downloadTaskMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.DownloadTaskMenuStripOpening);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.pauseToolStripMenuItem.Text = "Pause";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.PauseToolStripMenuItemClick);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.StartToolStripMenuItemClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1MinSize = 176;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbDownloadTaskInfo);
            this.splitContainer1.Panel2MinSize = 130;
            this.splitContainer1.Size = new System.Drawing.Size(700, 310);
            this.splitContainer1.SplitterDistance = 176;
            this.splitContainer1.TabIndex = 4;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lvwDownloadTasks);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lblDownloadTasksInfo);
            this.splitContainer2.Size = new System.Drawing.Size(700, 176);
            this.splitContainer2.SplitterDistance = 147;
            this.splitContainer2.TabIndex = 0;
            // 
            // lvwDownloadTasks
            // 
            this.lvwDownloadTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFileName,
            this.columnFileSize,
            this.columnCompleted,
            this.columnProgress,
            this.columnRemaining,
            this.columnSpeed,
            this.columnAvgSpeed,
            this.columnDate,
            this.columnState,
            this.columnResume,
            this.columnUrl});
            this.lvwDownloadTasks.ContextMenuStrip = this.downloadTaskMenuStrip;
            this.lvwDownloadTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwDownloadTasks.FullRowSelect = true;
            this.lvwDownloadTasks.GridLines = true;
            this.lvwDownloadTasks.HideSelection = false;
            this.lvwDownloadTasks.Location = new System.Drawing.Point(0, 0);
            this.lvwDownloadTasks.MultiSelect = false;
            this.lvwDownloadTasks.Name = "lvwDownloadTasks";
            this.lvwDownloadTasks.ShowGroups = false;
            this.lvwDownloadTasks.ShowItemToolTips = true;
            this.lvwDownloadTasks.Size = new System.Drawing.Size(700, 147);
            this.lvwDownloadTasks.TabIndex = 1;
            this.lvwDownloadTasks.UseCompatibleStateImageBehavior = false;
            this.lvwDownloadTasks.View = System.Windows.Forms.View.Details;
            // 
            // columnFileName
            // 
            this.columnFileName.Text = "File Name";
            this.columnFileName.Width = 150;
            // 
            // columnFileSize
            // 
            this.columnFileSize.Text = "Size";
            this.columnFileSize.Width = 100;
            // 
            // columnCompleted
            // 
            this.columnCompleted.Text = "Completed";
            this.columnCompleted.Width = 100;
            // 
            // columnProgress
            // 
            this.columnProgress.Text = "Progress";
            this.columnProgress.Width = 100;
            // 
            // columnRemaining
            // 
            this.columnRemaining.Text = "Remaining";
            this.columnRemaining.Width = 90;
            // 
            // columnSpeed
            // 
            this.columnSpeed.Text = "Speed";
            this.columnSpeed.Width = 80;
            // 
            // columnAvgSpeed
            // 
            this.columnAvgSpeed.Text = "Avg. Speed";
            this.columnAvgSpeed.Width = 100;
            // 
            // columnDate
            // 
            this.columnDate.Text = "Date";
            this.columnDate.Width = 80;
            // 
            // columnState
            // 
            this.columnState.Text = "State";
            this.columnState.Width = 80;
            // 
            // columnResume
            // 
            this.columnResume.Text = "Resume";
            this.columnResume.Width = 80;
            // 
            // columnUrl
            // 
            this.columnUrl.Text = "Url";
            this.columnUrl.Width = 150;
            // 
            // lblDownloadTasksInfo
            // 
            this.lblDownloadTasksInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDownloadTasksInfo.Location = new System.Drawing.Point(0, 0);
            this.lblDownloadTasksInfo.Name = "lblDownloadTasksInfo";
            this.lblDownloadTasksInfo.Size = new System.Drawing.Size(700, 25);
            this.lblDownloadTasksInfo.TabIndex = 0;
            this.lblDownloadTasksInfo.Text = "Waiting...";
            // 
            // tbDownloadTaskInfo
            // 
            this.tbDownloadTaskInfo.Controls.Add(this.tpDownloadTaskInfo);
            this.tbDownloadTaskInfo.Controls.Add(this.tpDownloadTaskSegments);
            this.tbDownloadTaskInfo.Controls.Add(this.tpDownloadTaskLogs);
            this.tbDownloadTaskInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDownloadTaskInfo.Location = new System.Drawing.Point(0, 0);
            this.tbDownloadTaskInfo.Name = "tbDownloadTaskInfo";
            this.tbDownloadTaskInfo.SelectedIndex = 0;
            this.tbDownloadTaskInfo.Size = new System.Drawing.Size(700, 130);
            this.tbDownloadTaskInfo.TabIndex = 2;
            // 
            // tpDownloadTaskInfo
            // 
            this.tpDownloadTaskInfo.Controls.Add(this.pnlTaskInfo);
            this.tpDownloadTaskInfo.Location = new System.Drawing.Point(4, 22);
            this.tpDownloadTaskInfo.Name = "tpDownloadTaskInfo";
            this.tpDownloadTaskInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpDownloadTaskInfo.Size = new System.Drawing.Size(692, 104);
            this.tpDownloadTaskInfo.TabIndex = 0;
            this.tpDownloadTaskInfo.Text = "Task Info";
            this.tpDownloadTaskInfo.UseVisualStyleBackColor = true;
            // 
            // pnlTaskInfo
            // 
            this.pnlTaskInfo.Controls.Add(this.lblCopyUrl);
            this.pnlTaskInfo.Controls.Add(this.lblUrlValue);
            this.pnlTaskInfo.Controls.Add(this.lblUrl);
            this.pnlTaskInfo.Controls.Add(this.lblAverageSpeedValue);
            this.pnlTaskInfo.Controls.Add(this.lblAverageSpeed);
            this.pnlTaskInfo.Controls.Add(this.lblSizeValue);
            this.pnlTaskInfo.Controls.Add(this.lblDownloadLocation);
            this.pnlTaskInfo.Controls.Add(this.picFileType);
            this.pnlTaskInfo.Controls.Add(this.lblSize);
            this.pnlTaskInfo.Controls.Add(this.lblFileNameValue);
            this.pnlTaskInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTaskInfo.Location = new System.Drawing.Point(3, 3);
            this.pnlTaskInfo.Name = "pnlTaskInfo";
            this.pnlTaskInfo.Size = new System.Drawing.Size(686, 98);
            this.pnlTaskInfo.TabIndex = 2;
            // 
            // lblUrlValue
            // 
            this.lblUrlValue.AutoSize = true;
            this.lblUrlValue.Location = new System.Drawing.Point(82, 77);
            this.lblUrlValue.Name = "lblUrlValue";
            this.lblUrlValue.Size = new System.Drawing.Size(35, 13);
            this.lblUrlValue.TabIndex = 8;
            this.lblUrlValue.Text = "[URL]";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblUrl.Location = new System.Drawing.Point(3, 77);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(36, 13);
            this.lblUrl.TabIndex = 7;
            this.lblUrl.Text = "URL:";
            // 
            // lblAverageSpeedValue
            // 
            this.lblAverageSpeedValue.AutoSize = true;
            this.lblAverageSpeedValue.Location = new System.Drawing.Point(82, 58);
            this.lblAverageSpeedValue.Name = "lblAverageSpeedValue";
            this.lblAverageSpeedValue.Size = new System.Drawing.Size(84, 13);
            this.lblAverageSpeedValue.TabIndex = 6;
            this.lblAverageSpeedValue.Text = "[AverageSpeed]";
            // 
            // lblAverageSpeed
            // 
            this.lblAverageSpeed.AutoSize = true;
            this.lblAverageSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAverageSpeed.Location = new System.Drawing.Point(3, 58);
            this.lblAverageSpeed.Name = "lblAverageSpeed";
            this.lblAverageSpeed.Size = new System.Drawing.Size(73, 13);
            this.lblAverageSpeed.TabIndex = 5;
            this.lblAverageSpeed.Text = "Avg Speed:";
            // 
            // lblSizeValue
            // 
            this.lblSizeValue.AutoSize = true;
            this.lblSizeValue.Location = new System.Drawing.Point(82, 39);
            this.lblSizeValue.Name = "lblSizeValue";
            this.lblSizeValue.Size = new System.Drawing.Size(33, 13);
            this.lblSizeValue.TabIndex = 4;
            this.lblSizeValue.Text = "[Size]";
            // 
            // lblDownloadLocation
            // 
            this.lblDownloadLocation.AutoSize = true;
            this.lblDownloadLocation.Location = new System.Drawing.Point(44, 21);
            this.lblDownloadLocation.Name = "lblDownloadLocation";
            this.lblDownloadLocation.Size = new System.Drawing.Size(105, 13);
            this.lblDownloadLocation.TabIndex = 3;
            this.lblDownloadLocation.TabStop = true;
            this.lblDownloadLocation.Text = "[Download Location]";
            // 
            // picFileType
            // 
            this.picFileType.Location = new System.Drawing.Point(6, 3);
            this.picFileType.Name = "picFileType";
            this.picFileType.Size = new System.Drawing.Size(32, 32);
            this.picFileType.TabIndex = 2;
            this.picFileType.TabStop = false;
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSize.Location = new System.Drawing.Point(3, 39);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(35, 13);
            this.lblSize.TabIndex = 0;
            this.lblSize.Text = "Size:";
            // 
            // lblFileNameValue
            // 
            this.lblFileNameValue.AutoSize = true;
            this.lblFileNameValue.Location = new System.Drawing.Point(44, 3);
            this.lblFileNameValue.Name = "lblFileNameValue";
            this.lblFileNameValue.Size = new System.Drawing.Size(57, 13);
            this.lblFileNameValue.TabIndex = 1;
            this.lblFileNameValue.Text = "[FileName]";
            // 
            // tpDownloadTaskSegments
            // 
            this.tpDownloadTaskSegments.Controls.Add(this.lvwDownloadTaskSegments);
            this.tpDownloadTaskSegments.Location = new System.Drawing.Point(4, 22);
            this.tpDownloadTaskSegments.Name = "tpDownloadTaskSegments";
            this.tpDownloadTaskSegments.Padding = new System.Windows.Forms.Padding(3);
            this.tpDownloadTaskSegments.Size = new System.Drawing.Size(692, 104);
            this.tpDownloadTaskSegments.TabIndex = 1;
            this.tpDownloadTaskSegments.Text = "Segments";
            this.tpDownloadTaskSegments.UseVisualStyleBackColor = true;
            // 
            // lvwDownloadTaskSegments
            // 
            this.lvwDownloadTaskSegments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnSegmentProgress,
            this.columnSegmentCompleted,
            this.columnSegmentSize,
            this.columnSegmentStartPosition,
            this.columnSegmentEndPosition,
            this.columnSegmentDownloadSpeed,
            this.columnSegmentRemaining,
            this.columnSegmentState,
            this.columnSegmentUrl});
            this.lvwDownloadTaskSegments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwDownloadTaskSegments.Location = new System.Drawing.Point(3, 3);
            this.lvwDownloadTaskSegments.Name = "lvwDownloadTaskSegments";
            this.lvwDownloadTaskSegments.Size = new System.Drawing.Size(686, 98);
            this.lvwDownloadTaskSegments.TabIndex = 0;
            this.lvwDownloadTaskSegments.UseCompatibleStateImageBehavior = false;
            this.lvwDownloadTaskSegments.View = System.Windows.Forms.View.Details;
            // 
            // columnSegmentProgress
            // 
            this.columnSegmentProgress.Text = "Progress";
            // 
            // columnSegmentCompleted
            // 
            this.columnSegmentCompleted.Text = "Completed";
            this.columnSegmentCompleted.Width = 70;
            // 
            // columnSegmentSize
            // 
            this.columnSegmentSize.Text = "Size";
            // 
            // columnSegmentStartPosition
            // 
            this.columnSegmentStartPosition.Text = "Start Position";
            this.columnSegmentStartPosition.Width = 80;
            // 
            // columnSegmentEndPosition
            // 
            this.columnSegmentEndPosition.Text = "End Position";
            this.columnSegmentEndPosition.Width = 80;
            // 
            // columnSegmentDownloadSpeed
            // 
            this.columnSegmentDownloadSpeed.Text = "Speed";
            // 
            // columnSegmentRemaining
            // 
            this.columnSegmentRemaining.Text = "Remaining";
            this.columnSegmentRemaining.Width = 70;
            // 
            // columnSegmentState
            // 
            this.columnSegmentState.Text = "State";
            // 
            // columnSegmentUrl
            // 
            this.columnSegmentUrl.Text = "Url";
            this.columnSegmentUrl.Width = 100;
            // 
            // tpDownloadTaskLogs
            // 
            this.tpDownloadTaskLogs.Location = new System.Drawing.Point(4, 22);
            this.tpDownloadTaskLogs.Name = "tpDownloadTaskLogs";
            this.tpDownloadTaskLogs.Padding = new System.Windows.Forms.Padding(3);
            this.tpDownloadTaskLogs.Size = new System.Drawing.Size(692, 104);
            this.tpDownloadTaskLogs.TabIndex = 2;
            this.tpDownloadTaskLogs.Text = "Logs";
            this.tpDownloadTaskLogs.UseVisualStyleBackColor = true;
            // 
            // lblCopyUrl
            // 
            this.lblCopyUrl.AutoSize = true;
            this.lblCopyUrl.Location = new System.Drawing.Point(44, 77);
            this.lblCopyUrl.Name = "lblCopyUrl";
            this.lblCopyUrl.Size = new System.Drawing.Size(31, 13);
            this.lblCopyUrl.TabIndex = 9;
            this.lblCopyUrl.TabStop = true;
            this.lblCopyUrl.Text = "Copy";
            this.lblCopyUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblCopyUrl_LinkClicked);
            // 
            // DownloadTaskList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(700, 310);
            this.Name = "DownloadTaskList";
            this.Size = new System.Drawing.Size(700, 310);
            this.downloadTaskMenuStrip.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tbDownloadTaskInfo.ResumeLayout(false);
            this.tpDownloadTaskInfo.ResumeLayout(false);
            this.pnlTaskInfo.ResumeLayout(false);
            this.pnlTaskInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFileType)).EndInit();
            this.tpDownloadTaskSegments.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip downloadTaskMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView lvwDownloadTasks;
        private System.Windows.Forms.ColumnHeader columnFileName;
        private System.Windows.Forms.ColumnHeader columnFileSize;
        private System.Windows.Forms.ColumnHeader columnCompleted;
        private System.Windows.Forms.ColumnHeader columnProgress;
        private System.Windows.Forms.ColumnHeader columnRemaining;
        private System.Windows.Forms.ColumnHeader columnSpeed;
        private System.Windows.Forms.ColumnHeader columnAvgSpeed;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.ColumnHeader columnState;
        private System.Windows.Forms.ColumnHeader columnResume;
        private System.Windows.Forms.ColumnHeader columnUrl;
        private System.Windows.Forms.Label lblDownloadTasksInfo;
        private System.Windows.Forms.TabControl tbDownloadTaskInfo;
        private System.Windows.Forms.TabPage tpDownloadTaskInfo;
        private System.Windows.Forms.Panel pnlTaskInfo;
        private System.Windows.Forms.Label lblUrlValue;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.Label lblAverageSpeedValue;
        private System.Windows.Forms.Label lblAverageSpeed;
        private System.Windows.Forms.Label lblSizeValue;
        private System.Windows.Forms.LinkLabel lblDownloadLocation;
        private System.Windows.Forms.PictureBox picFileType;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblFileNameValue;
        private System.Windows.Forms.TabPage tpDownloadTaskSegments;
        private System.Windows.Forms.ListView lvwDownloadTaskSegments;
        private System.Windows.Forms.ColumnHeader columnSegmentProgress;
        private System.Windows.Forms.ColumnHeader columnSegmentCompleted;
        private System.Windows.Forms.ColumnHeader columnSegmentSize;
        private System.Windows.Forms.ColumnHeader columnSegmentStartPosition;
        private System.Windows.Forms.ColumnHeader columnSegmentEndPosition;
        private System.Windows.Forms.ColumnHeader columnSegmentDownloadSpeed;
        private System.Windows.Forms.ColumnHeader columnSegmentRemaining;
        private System.Windows.Forms.ColumnHeader columnSegmentState;
        private System.Windows.Forms.ColumnHeader columnSegmentUrl;
        private System.Windows.Forms.TabPage tpDownloadTaskLogs;
        private System.Windows.Forms.LinkLabel lblCopyUrl;

    }
}
