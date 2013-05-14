using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Fishing
{
    internal partial class FishingDBSyncForm : Form, IFishDBStatusDisplay
    {
        // 50% upload 50% download
        // Rods or fish divide that half evenly
        // Fish divide rods further (in download)
        private int uploadFish;
        private int uploadingFish = 0;
        private int downloadRods;
        private int downloadingRod = 0;
        private int downloadFish;
        private int downloadingFish = 0;
        private int renameFish;
        private int renamingFish = 0;

        public FishingDBSyncForm()
        {
            InitializeComponent();

            Thread syncThread = new Thread(new ThreadStart(DoSync));
            syncThread.IsBackground = true;
            FishSQL.StatusDisplay = this;
            syncThread.Start();
        }

        ~FishingDBSyncForm()
        {
            FishSQL.CloseConnection();
            FishSQL.CloseAllConnections();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void DoSync()
        {
            if (!FishSQL.OpenConnection())
            {
                return;
            }

            try
            {
                StartDBTransaction("Starting Sync...");
                FishDB.MarkAllFishNew();
                FishDB.GetUpdates();
                FishSQL.BackgroundUpload();
                FishSQL.DoDownloadFish();
            }
            catch (Exception e)
            {
                Error(e.ToString());
            }
            EndDBTransaction("Done!");
        }

        public bool StartDBTransaction(string message)
        {
            bool ret = false;
            this.UIThreadInvoke(delegate
            {
                lblRod.Text = message;
                lblFish.Text = String.Empty;
                progress.Value = progress.Minimum;
                ret = true;
            });
            return ret;
        }

        public void EndDBTransaction(string message)
        {
            this.UIThread(delegate
            {
                lblRod.Text = message;
                lblFish.Text = String.Empty;
                progress.Value = progress.Maximum;
            });
        }

        public void SetUploadFishNumber(int fish)
        {
            uploadFish = fish;
        }

        public void SetUploadRodAndFish(string rod, string fish)
        {
            this.UIThread(delegate
            {
                lblRod.Text = string.Format("Uploading {0}:", rod);
                lblFish.Text = fish;
                uploadingFish++;
                ProgressUpdate();
            });
        }

        public void SetUploadRenameRodAndFish(string rod, string fromName, string toName)
        {
            this.UIThread(delegate
            {
                lblRod.Text = string.Format("Uploading {0}:", rod);
                lblFish.Text = string.Format("Renaming {0} to {1}", fromName, toName);
                uploadingFish++;
                ProgressUpdate();
            });
        }


        public void SetDownloadRodNumber(int rods)
        {
            downloadRods = rods;
        }

        public void SetDownloadRodFish(int fish)
        {
            downloadingFish = 0;
            downloadFish = fish;
        }

        public void SetDownloadRenameRodFish(int fish)
        {
            renamingFish = 0;
            renameFish = fish;
        }

        public void SetDownloadRod(string rod)
        {
            this.UIThread(delegate
            {
                lblRod.Text = string.Format("Downloading {0}:", rod);
                downloadingRod++;
                ProgressUpdate();
            });
        }

        public void SetDownloadFish(string fish)
        {
            this.UIThread(delegate
            {
                lblFish.Text = fish;
                downloadingFish++;
                ProgressUpdate();
            });
        }

        public void SetDownloadRenameFish(string fromName, string toName)
        {
            this.UIThread(delegate
            {
                lblFish.Text = string.Format("Renaming {0} to {1}", fromName, toName);
                renamingFish++;
                ProgressUpdate();
            });
        }

        public void SetFishBaitOrZone(string fish, string baitOrZone)
        {
            this.UIThread(delegate
            {
                lblFish.Text = string.Format("{0} - {1}", fish, baitOrZone);
            });
        }

        private void ProgressUpdate()
        {
            int prog = 0;
            int half = 0;
            this.UIThreadInvoke(delegate
            {
                half = progress.Maximum / 2;
            });
            if (uploadingFish > 0)
            {
                prog = (int)Math.Ceiling(Math.Min((double)(uploadingFish - 1) / uploadFish, 1.0D) * half);
            }
            if (downloadingRod > 0)
            {
                prog = half / 2;
                prog += (int)Math.Ceiling(Math.Min((double)(downloadingRod - 1) / downloadRods, 1.0D) * half / downloadRods / 2);
                if (downloadingFish > 0)
                {
                    prog += (int)Math.Ceiling(Math.Min((double)(downloadingFish - 1) / downloadFish, 1.0D) * half / downloadRods / 2);
                }
                if (renamingFish > 0)
                {
                    prog += (int)Math.Ceiling(Math.Min((double)(renamingFish - 1) / renameFish, 1.0D) * half / downloadRods / 2);
                }
            }
            this.UIThread(delegate
            {
                progress.Value = prog;
            });
        }

        public void Error(string message)
        {
            this.UIThread(delegate
            {
                lblRod.Text = string.Format("ERROR: {0}", message);
                lblFish.Text = string.Empty;
                MessageBox.Show(message, "ERROR");
                Thread.Sleep(500);
            });
        }

        public void Warning(string message)
        {
            this.UIThread(delegate
            {
                lblRod.Text = string.Format("WARNING: {0}", message);
                lblFish.Text = string.Empty;
                MessageBox.Show(message, "WARNING");
                Thread.Sleep(500);
            });
        }

        public void Info(string message)
        {
            this.UIThread(delegate
            {
                lblRod.Text = string.Format("Info: {0}", message);
                lblFish.Text = string.Empty;
                MessageBox.Show(message, "Info");
                Thread.Sleep(500);
            });
        }
    }
}
