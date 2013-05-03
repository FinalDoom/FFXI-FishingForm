namespace Fishing
{
    partial class FishingDBSyncForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FishingDBSyncForm));
            this.progress = new System.Windows.Forms.ProgressBar();
            this.lblRod = new System.Windows.Forms.Label();
            this.lblFish = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(12, 38);
            this.progress.Maximum = 1000;
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(330, 23);
            this.progress.TabIndex = 2;
            // 
            // lblRod
            // 
            this.lblRod.AutoSize = true;
            this.lblRod.Location = new System.Drawing.Point(12, 9);
            this.lblRod.Name = "lblRod";
            this.lblRod.Size = new System.Drawing.Size(0, 13);
            this.lblRod.TabIndex = 0;
            // 
            // lblFish
            // 
            this.lblFish.AutoSize = true;
            this.lblFish.Location = new System.Drawing.Point(12, 22);
            this.lblFish.Name = "lblFish";
            this.lblFish.Size = new System.Drawing.Size(0, 13);
            this.lblFish.TabIndex = 1;
            // 
            // FishingDBSyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 73);
            this.Controls.Add(this.lblFish);
            this.Controls.Add(this.lblRod);
            this.Controls.Add(this.progress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(370, 111);
            this.Name = "FishingDBSyncForm";
            this.Text = "Syncing FishDB to Database";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Label lblRod;
        private System.Windows.Forms.Label lblFish;
    }
}