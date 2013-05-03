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
    internal partial class FishingDBSyncForm : Form
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

        public FishingDBSyncForm()
        {
            InitializeComponent();

            Thread syncThread = new Thread(new ThreadStart(DoSync));
            syncThread.IsBackground = true;
            FishSQL.SyncForm = this;
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

            FishDB.MarkAllFishNew();
            FishDB.GetUpdates();
            FishSQL.DoUploadFish();
            FishSQL.DoDownloadFish();
            Done();
        }

        public void Done()
        {
            if (InvokeRequired)
            {
                Invoke(new VoidDelegate(Done));
            }
            else
            {
                lblRod.Text = "Done!";
                lblFish.Text = String.Empty;
                progress.Value = progress.Maximum;
            }
        }

        public void SetUploadFishNumber(int fish)
        {
            uploadFish = fish;
        }

        public void SetUploadRodAndFish(string rod, string fish)
        {
            if (InvokeRequired)
            {
                Invoke(new VoidDoubleStringDelegate(SetUploadRodAndFish), rod, fish);
            }
            else
            {
                lblRod.Text = string.Format("Uploading {0}:", rod);
                lblFish.Text = fish;
                uploadingFish++;
                ProgressUpdate();
            }
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

        public void SetDownloadRod(string rod)
        {
            if (InvokeRequired)
            {
                Invoke(new VoidStringDelegate(SetDownloadRod), rod);
            }
            else
            {
                lblRod.Text = string.Format("Downloading {0}:", rod);
                downloadingRod++;
                ProgressUpdate();
            }
        }

        public void SetDownloadFish(string fish)
        {
            if (InvokeRequired)
            {
                Invoke(new VoidStringDelegate(SetDownloadFish), fish);
            }
            else
            {
                lblFish.Text = fish;
                downloadingFish++;
                ProgressUpdate();
            }
        }

        public void SetFishBaitOrZone(string fish, string baitOrZone)
        {
            if (InvokeRequired)
            {
                Invoke(new VoidDoubleStringDelegate(SetFishBaitOrZone), fish, baitOrZone);
            }
            else
            {
                lblFish.Text = string.Format("{0} - {1}", fish, baitOrZone);
            }
        }

        delegate void VoidStringDelegate(string s);
        delegate void VoidDoubleStringDelegate(string s1, string s2);
        delegate void VoidDelegate();

        private void ProgressUpdate()
        {
            int prog = 0;
            int half = progress.Maximum / 2;
            if (uploadingFish > 0)
            {
                prog = (int)Math.Ceiling(Math.Min((double)(uploadingFish - 1) / uploadFish, 1.0D) * half);
            }
            if (downloadingRod > 0)
            {
                prog = half / 2;
                prog += (int)Math.Ceiling(Math.Min((double)(downloadingRod - 1) / downloadRods, 1.0D) * half / downloadRods);
                if (downloadingFish > 0)
                {
                    prog += (int)Math.Ceiling(Math.Min((double)(downloadingFish - 1) / downloadFish, 1.0D) * half / downloadRods);
                }
            }
            progress.Value = prog;
        }
    }
}
