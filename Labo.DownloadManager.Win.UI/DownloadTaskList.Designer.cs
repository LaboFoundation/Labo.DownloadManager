namespace Labo.DownloadManager.Win.UI
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
            this.lvwDownloadTasks = new System.Windows.Forms.ListView();
            this.columnFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCompleted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnResume = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnUrl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnRemaining = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbDownloadTaskInfo = new System.Windows.Forms.TabControl();
            this.tpDownloadTaskInfo = new System.Windows.Forms.TabPage();
            this.tpDownloadTaskSegments = new System.Windows.Forms.TabPage();
            this.tpDownloadTaskLogs = new System.Windows.Forms.TabPage();
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
            this.tbDownloadTaskInfo.SuspendLayout();
            this.tpDownloadTaskSegments.SuspendLayout();
            this.SuspendLayout();
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
            this.columnDate,
            this.columnState,
            this.columnResume,
            this.columnUrl});
            this.lvwDownloadTasks.Dock = System.Windows.Forms.DockStyle.Top;
            this.lvwDownloadTasks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwDownloadTasks.HideSelection = false;
            this.lvwDownloadTasks.Location = new System.Drawing.Point(0, 0);
            this.lvwDownloadTasks.Name = "lvwDownloadTasks";
            this.lvwDownloadTasks.ShowGroups = false;
            this.lvwDownloadTasks.ShowItemToolTips = true;
            this.lvwDownloadTasks.Size = new System.Drawing.Size(700, 176);
            this.lvwDownloadTasks.TabIndex = 0;
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
            this.columnFileSize.Width = 75;
            // 
            // columnCompleted
            // 
            this.columnCompleted.Text = "Completed";
            this.columnCompleted.Width = 80;
            // 
            // columnProgress
            // 
            this.columnProgress.Text = "Progress";
            // 
            // columnSpeed
            // 
            this.columnSpeed.Text = "Speed";
            // 
            // columnDate
            // 
            this.columnDate.Text = "Date";
            this.columnDate.Width = 80;
            // 
            // columnState
            // 
            this.columnState.Text = "State";
            // 
            // columnResume
            // 
            this.columnResume.Text = "Resume";
            // 
            // columnUrl
            // 
            this.columnUrl.Text = "Url";
            this.columnUrl.Width = 150;
            // 
            // columnRemaining
            // 
            this.columnRemaining.Text = "Remaining";
            this.columnRemaining.Width = 90;
            // 
            // tbDownloadTaskInfo
            // 
            this.tbDownloadTaskInfo.Controls.Add(this.tpDownloadTaskInfo);
            this.tbDownloadTaskInfo.Controls.Add(this.tpDownloadTaskSegments);
            this.tbDownloadTaskInfo.Controls.Add(this.tpDownloadTaskLogs);
            this.tbDownloadTaskInfo.Location = new System.Drawing.Point(0, 179);
            this.tbDownloadTaskInfo.Name = "tbDownloadTaskInfo";
            this.tbDownloadTaskInfo.SelectedIndex = 0;
            this.tbDownloadTaskInfo.Size = new System.Drawing.Size(697, 128);
            this.tbDownloadTaskInfo.TabIndex = 1;
            // 
            // tpDownloadTaskInfo
            // 
            this.tpDownloadTaskInfo.Location = new System.Drawing.Point(4, 22);
            this.tpDownloadTaskInfo.Name = "tpDownloadTaskInfo";
            this.tpDownloadTaskInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpDownloadTaskInfo.Size = new System.Drawing.Size(689, 102);
            this.tpDownloadTaskInfo.TabIndex = 0;
            this.tpDownloadTaskInfo.Text = "Task Info";
            this.tpDownloadTaskInfo.UseVisualStyleBackColor = true;
            // 
            // tpDownloadTaskSegments
            // 
            this.tpDownloadTaskSegments.Controls.Add(this.lvwDownloadTaskSegments);
            this.tpDownloadTaskSegments.Location = new System.Drawing.Point(4, 22);
            this.tpDownloadTaskSegments.Name = "tpDownloadTaskSegments";
            this.tpDownloadTaskSegments.Padding = new System.Windows.Forms.Padding(3);
            this.tpDownloadTaskSegments.Size = new System.Drawing.Size(689, 102);
            this.tpDownloadTaskSegments.TabIndex = 1;
            this.tpDownloadTaskSegments.Text = "Segments";
            this.tpDownloadTaskSegments.UseVisualStyleBackColor = true;
            // 
            // tpDownloadTaskLogs
            // 
            this.tpDownloadTaskLogs.Location = new System.Drawing.Point(4, 22);
            this.tpDownloadTaskLogs.Name = "tpDownloadTaskLogs";
            this.tpDownloadTaskLogs.Padding = new System.Windows.Forms.Padding(3);
            this.tpDownloadTaskLogs.Size = new System.Drawing.Size(689, 102);
            this.tpDownloadTaskLogs.TabIndex = 2;
            this.tpDownloadTaskLogs.Text = "Logs";
            this.tpDownloadTaskLogs.UseVisualStyleBackColor = true;
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
            this.lvwDownloadTaskSegments.Size = new System.Drawing.Size(683, 96);
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
            // DownloadTaskList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbDownloadTaskInfo);
            this.Controls.Add(this.lvwDownloadTasks);
            this.Name = "DownloadTaskList";
            this.Size = new System.Drawing.Size(700, 310);
            this.tbDownloadTaskInfo.ResumeLayout(false);
            this.tpDownloadTaskSegments.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvwDownloadTasks;
        private System.Windows.Forms.ColumnHeader columnFileName;
        private System.Windows.Forms.ColumnHeader columnFileSize;
        private System.Windows.Forms.ColumnHeader columnCompleted;
        private System.Windows.Forms.ColumnHeader columnProgress;
        private System.Windows.Forms.ColumnHeader columnSpeed;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.ColumnHeader columnState;
        private System.Windows.Forms.ColumnHeader columnResume;
        private System.Windows.Forms.ColumnHeader columnUrl;
        private System.Windows.Forms.ColumnHeader columnRemaining;
        private System.Windows.Forms.TabControl tbDownloadTaskInfo;
        private System.Windows.Forms.TabPage tpDownloadTaskInfo;
        private System.Windows.Forms.TabPage tpDownloadTaskSegments;
        private System.Windows.Forms.TabPage tpDownloadTaskLogs;
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
    }
}
