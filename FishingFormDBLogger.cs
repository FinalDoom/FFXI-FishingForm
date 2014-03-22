using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fishing
{
    internal class FishingFormDBLogger : IFishDBStatusDisplay
    {
        private const int SpamThreshold = 10;

        private FishingForm form;
        private bool InTransaction = false;
        private int uploadFish;
        private int uploadingFish = 0;
        private int downloadingRod = 0;
        private string currentRod = string.Empty;
        private int downloadFish;
        private int downloadingFish = 0;
        private string currentFish = string.Empty;

        public FishingFormDBLogger(FishingForm f)
        {
            form = f;
        }

        private void PostMessage(string message)
        {
            if (null != form && !string.IsNullOrEmpty(message))
            {
                form.UpdateDBLog(message);
            }
        }

        public bool StartDBTransaction(string message)
        {
            if (InTransaction)
            {
                return false;
            }
            InTransaction = true;
            PostMessage(message);
            return true;
        }

        public void EndDBTransaction(string message)
        {
            PostMessage(message);
            InTransaction = false;
        }

        public void SetUploadFishNumber(int fish)
        {
            if (fish > 0)
            {
                PostMessage(string.Format("Uploading {0} fish.", fish));
            }
            uploadFish = fish;
            if (uploadFish >= SpamThreshold)
            {
                PostMessage("Too many fish uploading to list individually.");
            }
            uploadingFish = 0;
        }

        public void SetUploadRodAndFish(string rod, string fish)
        {
            if (uploadFish < SpamThreshold)
            { // Prevent spam hanging the GUI
                uploadingFish++;
                PostMessage(string.Format("{0}: \"{1}\" caught with {2}.", uploadingFish, fish, rod));
            }
        }

        public void SetUploadRenameRodAndFish(string rod, string newName, string oldName)
        {
            if (uploadFish < SpamThreshold)
            { // Prevent spam hanging the GUI
                uploadingFish++;
                PostMessage(string.Format("{0}: \"{1}\" renamed to \"{2}\". ({3})", uploadingFish, oldName, newName, rod));
            }
        }

        public void SetDownloadRodNumber(int rods)
        {
            if (rods > 0)
            {
                PostMessage(string.Format("Downloading {0} rods' data.", rods));
            }
            downloadingFish = 0;
        }

        public void SetDownloadRod(string rod)
        {
            downloadingRod++;
            currentRod = rod;
            PostMessage(string.Format("Getting information for {0} (#{1}).", rod, downloadingRod));
        }

        public void SetDownloadRodFish(int fish)
        {
            downloadingFish = 0;
            downloadFish = fish;
            if (fish > 0)
            {
                PostMessage(string.Format("Downloading {0} fish and fish data caught with {1}.", fish, currentRod));
            }
            if (downloadFish >= SpamThreshold)
            { // Prevent spam hanging the GUI
                PostMessage("Too many fish downloading to list individually.");
            }
        }

        public void SetDownloadRenameRodFish(int fish)
        {
            downloadingFish = 0;
            downloadFish = fish;
            if (fish > 0)
            {
                PostMessage(string.Format("Downloading {0} renames (caught with {1}).", fish, currentRod));
            }
            if (downloadFish >= SpamThreshold)
            { // Prevent spam hanging the GUI
                PostMessage("Too many renames downloading to list individually.");
            }
        }

        public void SetDownloadFish(string fish)
        {
            if (downloadFish < SpamThreshold)
            { // Prevent spam hanging the GUI
                downloadingFish++;
                PostMessage(string.Format("{0}: got \"{1}\".", downloadingFish, fish));
            }
        }

        public void SetDownloadRenameFish(string newName, string oldName)
        {
            if (downloadFish < SpamThreshold)
            { // Prevent spam hanging the GUI
                downloadingFish++;
                PostMessage(string.Format("{0}: got \"{1}\" and renamed to \"{2}\".", downloadingFish, oldName, newName));
            }
        }

        public void SetFishBaitOrZone(string fish, string baitOrZone)
        {
            if ((downloadFish == 0 && uploadFish < SpamThreshold) || (downloadFish != 0 && downloadFish < SpamThreshold))
            { // Prevent spam hanging the GUI
                PostMessage(string.Format("Adding \"{0}\" to \"{1}\".", baitOrZone, fish));
            }
        }

        public void Error(string message)
        {
            PostMessage(string.Format("ERROR: {0}", message));
        }

        public void Warning(string message)
        {
            PostMessage(string.Format("WARNING: {0}", message));
        }

        public void Info(string message)
        {
            PostMessage(message);
        }
    }
}
