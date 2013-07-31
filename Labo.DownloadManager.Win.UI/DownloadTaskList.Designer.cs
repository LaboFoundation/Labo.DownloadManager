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
            this.columnPreview = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnResume = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnUrl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvwDownloadTasks
            // 
            this.lvwDownloadTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFileName,
            this.columnFileSize,
            this.columnPreview,
            this.columnProgress,
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
            // columnPreview
            // 
            this.columnPreview.Text = "Preview";
            this.columnPreview.Width = 80;
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
            this.columnResume.Width = 50;
            // 
            // columnUrl
            // 
            this.columnUrl.Text = "Url";
            this.columnUrl.Width = 150;
            // 
            // DownloadTaskList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvwDownloadTasks);
            this.Name = "DownloadTaskList";
            this.Size = new System.Drawing.Size(700, 279);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvwDownloadTasks;
        private System.Windows.Forms.ColumnHeader columnFileName;
        private System.Windows.Forms.ColumnHeader columnFileSize;
        private System.Windows.Forms.ColumnHeader columnPreview;
        private System.Windows.Forms.ColumnHeader columnProgress;
        private System.Windows.Forms.ColumnHeader columnSpeed;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.ColumnHeader columnState;
        private System.Windows.Forms.ColumnHeader columnResume;
        private System.Windows.Forms.ColumnHeader columnUrl;
    }
}
