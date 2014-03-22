using System;
using System.Drawing;

namespace Fishing
{
    internal class FishingFormDBLogger : IFishDBStatusDisplay, ILogger
    {
        private const int SpamThreshold = 10;

        private readonly Action<string, Color> Log;
        private bool InTransaction = false;
        private int uploadFish;
        private int uploadingFish = 0;
        private int downloadingRod = 0;
        private string currentRod = string.Empty;
        private int downloadFish;
        private int downloadingFish = 0;

        public FishingFormDBLogger(Action<string, Color> logFunc)
        {
            Log = logFunc;
        }

        public bool StartDBTransaction(string message)
        {
            if (InTransaction)
            {
                return false;
            }
            InTransaction = true;
            Info(message);
            return true;
        }

        public void EndDBTransaction(string message)
        {
            Info(message);
            InTransaction = false;
        }

        public void SetUploadFishNumber(int fish)
        {
            if (fish > 0)
            {
                Info(string.Format("Uploading {0} fish.", fish));
            }
            uploadFish = fish;
            if (uploadFish >= SpamThreshold)
            {
                Info("Too many fish uploading to list individually.");
            }
            uploadingFish = 0;
        }

        public void SetUploadRodAndFish(string rod, string fish)
        {
            if (uploadFish < SpamThreshold)
            { // Prevent spam hanging the GUI
                uploadingFish++;
                Info(string.Format("{0}: \"{1}\" caught with {2}.", uploadingFish, fish, rod));
            }
        }

        public void SetUploadRenameRodAndFish(string rod, string newName, string oldName)
        {
            if (uploadFish < SpamThreshold)
            { // Prevent spam hanging the GUI
                uploadingFish++;
                Info(string.Format("{0}: \"{1}\" renamed to \"{2}\". ({3})", uploadingFish, oldName, newName, rod));
            }
        }

        public void SetDownloadRodNumber(int rods)
        {
            if (rods > 0)
            {
                Info(string.Format("Downloading {0} rods' data.", rods));
            }
            downloadingFish = 0;
        }

        public void SetDownloadRod(string rod)
        {
            downloadingRod++;
            currentRod = rod;
            Info(string.Format("Getting information for {0} (#{1}).", rod, downloadingRod));
        }

        public void SetDownloadRodFish(int fish)
        {
            downloadingFish = 0;
            downloadFish = fish;
            if (fish > 0)
            {
                Info(string.Format("Downloading {0} fish and fish data caught with {1}.", fish, currentRod));
            }
            if (downloadFish >= SpamThreshold)
            { // Prevent spam hanging the GUI
                Info("Too many fish downloading to list individually.");
            }
        }

        public void SetDownloadRenameRodFish(int fish)
        {
            downloadingFish = 0;
            downloadFish = fish;
            if (fish > 0)
            {
                Info(string.Format("Downloading {0} renames (caught with {1}).", fish, currentRod));
            }
            if (downloadFish >= SpamThreshold)
            { // Prevent spam hanging the GUI
                Info("Too many renames downloading to list individually.");
            }
        }

        public void SetDownloadFish(string fish)
        {
            if (downloadFish < SpamThreshold)
            { // Prevent spam hanging the GUI
                downloadingFish++;
                Info(string.Format("{0}: got \"{1}\".", downloadingFish, fish));
            }
        }

        public void SetDownloadRenameFish(string newName, string oldName)
        {
            if (downloadFish < SpamThreshold)
            { // Prevent spam hanging the GUI
                downloadingFish++;
                Info(string.Format("{0}: got \"{1}\" and renamed to \"{2}\".", downloadingFish, oldName, newName));
            }
        }

        public void SetFishBaitOrZone(string fish, string baitOrZone)
        {
            if ((downloadFish == 0 && uploadFish < SpamThreshold) || (downloadFish != 0 && downloadFish < SpamThreshold))
            { // Prevent spam hanging the GUI
                Info(string.Format("Adding \"{0}\" to \"{1}\".", baitOrZone, fish));
            }
        }

        public void Error(string message)
        {
            Log(string.Format("ERROR: {0}", message), Color.Red);
        }

        public void Warning(string message)
        {
            Log(string.Format("WARNING: {0}", message), Color.Yellow);
        }

        public void Info(string message)
        {
            Log(message, Color.White);
        }
    }
}
