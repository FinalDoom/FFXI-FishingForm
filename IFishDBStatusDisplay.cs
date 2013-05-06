using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fishing
{
    interface IFishDBStatusDisplay
    {
        /**
         * <summary>Start interaction with remote database. Returns false if transaction already in progress.
         * This should be handled by the sync handler, not FishSQL.</summary>
         * <param name="message">Message that may be displayed</param>
         * <returns>true if no transaction in progress</returns>
         */
        bool StartDBTransaction(string message);
        /**
         * <summary>Signal an end to interaction with remote database.
         * This should be handled by the sync handler, not FishSQL.</summary>
         * <param name="message">Message that may be displayed</param>
         */
        void EndDBTransaction(string message);
        /**
         * <summary>Set the total number of fish to upload. For tracking progress</summary>
         * <param name="fish">Total number of fish to be uploaded or renamed</param>
         */
        void SetUploadFishNumber(int fish);
        /**
         * <summary>Set the current rod and fish names being uploaded.
         * Use with <c>SetUploadFishNumber</c> to track upload progress.</summary>
         * <param name="rod">Name of the rod being updated</param>
         * <param name="fish">Name of the fish being uploaded</param>
         */
        void SetUploadRodAndFish(string rod, string fish);
        /**
         * <summary>Set the current rod and fish names being renamed.
         * Use with <c>SetUploadFishNumber</c> to track upload progress.</summary>
         * <param name="rod">Name of the rod being updated</param>
         * <param name="newName">New name of the fish being renamed</param>
         * <param name="oldName">Old name of the fish being renamed</param>
         */
        void SetUploadRenameRodAndFish(string rod, string newName, string oldName);
        /**
         * <summary>Set the number of rods to be downloaded</summary>
         * <param name="rods">Number of rods to be downloaded</param>
         */
        void SetDownloadRodNumber(int rods);
        /**
         * <summary>Set the number of fish being downloaded for the current rod.</summary>
         * <param name="fish">Number of fish being downloaded for the current rod.</param>
         */
        void SetDownloadRodFish(int fish);
        /**
         * <summary>Set the number of fish being downloaded to be renamed for the current rod.</summary>
         * <param name="fish">Number of fish being downloaded to be renamed for the current rod.</param>
         */
        void SetDownloadRenameRodFish(int fish);
        /**
         * <summary>Set the name of the rod being downloaded.
         * Use with <c>SetDownloadRodNumber</c> to track download progress.</summary>
         * <param name="rod">The name of the rod being downloaded</param>
         */
        void SetDownloadRod(string rod);
        /**
         * <summary>Set the name of the fish being downloaded.
         * Use with <c>SetDownloadRodFish</c> to track download progress.</summary>
         * <param name="rod">The name of the fish being downloaded</param>
         */
        void SetDownloadFish(string fish);
        /**
         * <summary>Set the name of the fish being renamed.
         * Use with <c>SetDownloadRenamedRodFish</c> to track download progress.</summary>
         * <param name="newName">New name of the fish being renamed</param>
         * <param name="oldName">Old name of the fish being renamed</param>
         */
        void SetDownloadRenameFish(string newName, string oldName);
        /**
         * <summary>Set the name of the bait or zone being synced.</summary>
         * <param name="rod">The name of the fish being synced</param>
         * <param name="baitOrZone">The name of the bait or zone being synced</param>
         */
        void SetFishBaitOrZone(string fish, string baitOrZone);
    }
}
