using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fishing.Properties;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;
using FFACETools;
using System.Diagnostics;
using System.Threading;
using System.Xml;

namespace Fishing
{
    internal struct SQLFishie
    {
        /// <summary>
        /// Class that mimics FishDB's Fishie, but also with optional zone and bait info
        /// </summary>
        /// <param name="fishName">Fish name</param>
        /// <param name="rodId">Rod ID</param>
        /// <param name="i1">Fish ID 1</param>
        /// <param name="i2">Fish ID 2</param>
        /// <param name="i3">Fish ID 3</param>
        /// <param name="zoneId">Zone ID</param>
        /// <param name="baitId">Bait ID</param>
        internal SQLFishie(string fishName, int rodId, string i1, string i2, string i3, int? zoneId, int? baitId)
        {
            name = fishName;
            rod = Dictionaries.rodDictionary.FirstOrDefault(x => x.Value == rodId).Key;
            ID1 = i1;
            ID2 = i2;
            ID3 = i3;
            if (zoneId.HasValue)
            {
                zone = FFACE.ParseResources.GetAreaName((Zone)zoneId.Value);
                if (zoneId.Value == 227 || zoneId.Value == 228)
                {// This is really silly. Is there not a proper way to do this?
                    zone += " (Pirates)";
                }
            }
            else
            {
                zone = null;
            }
            if (baitId.HasValue)
            {
                bait = Dictionaries.baitDictionary.FirstOrDefault(x => x.Value == baitId.Value).Key;
            }
            else
            {
                bait = null;
            }
        }

        public Fishie ToFishDBFishie() { return new Fishie(name, rod, ID1, ID2, ID3); }

        internal string name, rod, ID1, ID2, ID3, zone, bait;
        public override string ToString() { return name; }
    }

    /// <summary>
    /// Class that handles DB interactions
    /// </summary>
    internal static class FishSQL
    {
        #region Constants

        private static readonly string[] MessageFormatErrorBadZoneName = {
            "Broken zone name found:",
            "Rod: {0}", "Fish: {1}", "Zone: {2}",
            "",
            "Please fix it to match a Windower resources.xml name,",
            "and add an attribute to the parent tag: new=\"\""
        };

        private const string FormatConnection = "SERVER={0};PORT={1};DATABASE={2};UID={3};PASSWORD={4};";
        private const string server = "ec2-54-235-92-8.compute-1.amazonaws.com";
        private const string port = "3308";
        private const string database = "FishDB";
        // These are procedure-only credentials. Contact FinalDoom for an admin account, eg. if taking over this project.
        private const string uid = "FishClient";
        private const string password = "ThePasswordForTheFishClient";

        // MySQL Commands
        private const string MySQLCmdNewestUpdate = "CALL get_newest_update (@rodID)";
        private const string MySQLCommandAddFish = "CALL add_new_fish (@rodID, @name, @Id1, @Id2, @Id3)";
        private const string MySQLCommandRenameFish = "CALL rename_fish (@fromName, @toName, @rodID, @Id1, @Id2, @Id3)";
        private const string MySQLCommandGetFishID = "CALL check_fish (@rodID, @name, @Id1, @Id2, @Id3)";
        private const string MySQLCommandAddFishBait = "CALL add_fish_bait (@fishID, @baitID)";
        private const string MySQLCommandAddFishZone = "CALL add_fish_zone (@fishID, @zoneID)";
        private const string MySQLCommandGetNewFishSince = "CALL get_new_fish (@rodID, @time)";
        private const string MySQLCommandGetRenamedFishSince = "CALL get_renamed_fish (@rodID, @time)";
        private const string MySQLCommandIsVersionCurrent = "Call is_current_version (@major, @minor, @build, @revision)";

        // MySQL Params
        private const string MySQLParamRodID = "rodID";
        private const string MySQLParamFishName = "name";
        private const string MySQLParamId1 = "Id1";
        private const string MySQLParamId2 = "Id2";
        private const string MySQLParamId3 = "Id3";
        private const string MySQLParamFishFromName = "fromName";
        private const string MySQLParamFishToName = "toName";
        private const string MySQLParamFishID = "fishID";
        private const string MySQLParamBaitID = "baitID";
        private const string MySQLParamZoneID = "zoneID";
        private const string MySQLParamTime = "time";
        private const string MySQLParamZone = "Zone";
        private const string MySQLParamBait = "Bait";
        private const string MySQLParamVersionMajor = "major";
        private const string MySQLParamVersionMinor = "minor";
        private const string mySqlParamVersionBuild = "build";
        private const string mySqlParamVersionRevision = "revision";

        // Message constants
        private const string MessageErrorNoConnection = "Could not connect to FishDB MySQL server. Please contact program maintainer.";
        private const string MessageErrorCouldntConnect = "Could not connect to FishDB MySQL server. Error number ";
        private const string MessageWarningCloseConnection = "Could not close connection.";
        private const string MessageErrorNewestModTime = "Error getting Newest DB Modification Time";
        private const string MessageFormatErrorGettingFishID = "Error getting fish ID from DB for \"{0}\"";
        private const string MessageFormatErrorAddingFish = "Error adding new fish \"{0}\"";
        private const string MessageFormatErrorRenamingFish = "Error renaming fish \"{0}\" to \"{1}\"";
        private const string MessageFormatErrorAddingBaitFish = "Error adding bait \"{0}\" to fish \"{1}\"";
        private const string MessageFormatErrorAddingZoneFish = "Error adding zone \"{0}\" to fish \"{1}\"";
        private const string MessageFormatErrorGettingNewFishWithIDs = "Error getting new fish \"{0}\" with IDs: {1}, {2}, {3}";
        private const string MessageFormatErrorGettingRenamedFishWithIDs = "Error getting renamed fish \"{0}\" to \"{1}\" with IDs: {2}, {3}, {4}";
        private const string MessageErrorGettingVersion = "Error getting program version status.";
        private const string MessageFormatErrorUploadingFishRod = "Error uploading fish \"{0}\" for {1}.";
        private const string MessageFormatErrorUploadingBaitZones = "Error uploading bait and zones for fish \"{0}\" for {1}.";
        private const string MessageFormatErrorUploadingRename = "Error uploading rename from \"{0}\" to \"{1}\" for {2}.";
        private const string MessageFormatErrorDownloadingFishRod = "Error downloading new fish for {0}";
        private const string MessageFormatErrorDownloadingRenameRod = "Error downloading renames for {0}";
        private const string MessageUploadStart = "Starting upload to DB.";
        private const string MessageErrorUploading = "Error uploading fish";
        private const string MessageUploadFinished = "Upload to DB finished.";

        // Misc
        private const string ZoneSelbinaPirates = "Selbina (Pirates)";
        private const string ZoneMhauraPirates = "Mhaura (Pirates)";

        #endregion //Constants

        internal static IFishDBStatusDisplay StatusDisplay { private get; set; }
        internal static MySqlConnection Connection { get; private set; }
        private static bool errored = false;

        static FishSQL()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize Connection
        /// </summary>
        private static void Initialize()
        {
            Connection = new MySqlConnection(string.Format(FormatConnection, server, port, database, uid, password));
        }

        /// <summary>
        /// Open a connection to the database
        /// </summary>
        /// <returns>True if connection is open, false otherwise</returns>
        internal static bool OpenConnection()
        {
            if (Connection == null)
            {
                Initialize();
            }
            if (Connection.State == ConnectionState.Open)
            {
                return true;
            }
            try
            {
                Connection.Open();
                return Connection.State == ConnectionState.Open;
            }
            catch (MySqlException e)
            {
                if (!errored)
                {
                    string message;
                    switch (e.Number)
                    {
                        case 0:
                            message = MessageErrorNoConnection;
                            break;
                        default:
                            message = MessageErrorCouldntConnect + e.Number.ToString();
                            break;
                    }
                    MessageBox.Show(message);
                    if (StatusDisplay != null)
                    {
                        StatusDisplay.Warning(message);
                        StatusDisplay.Info(e.ToString());
                    }
                    errored = true;
                }
                return false;
            }
        }

        /// <summary>
        /// Close an open connection
        /// </summary>
        /// <returns>True if connection was closed, false if some error happened</returns>
        internal static bool CloseConnection()
        {
            try
            {
                if (Connection.State != ConnectionState.Closed)
                {
                    Connection.Close();
                }
                return true;
            }
            catch (MySqlException e)
            {
                if (StatusDisplay != null)
                {
                    StatusDisplay.Warning(MessageWarningCloseConnection);
                    StatusDisplay.Info(e.ToString());
                }
                return false;
            }
        }

        /// <summary>
        /// Connection cleanup for pooled connections.
        /// </summary>
        /// <remarks>
        /// There seems to be some issue with actually closing connections.
        /// There are an excessively large number of connections that are
        /// never closed and have to be terminated after a timeout. This
        /// is likely within the .NET code somewhere, not this program.
        /// There is probably a way to fix this, but this is the closest
        /// I've found.
        /// </remarks>
        public static void CloseAllConnections()
        {
            MySqlConnection.ClearAllPools();
        }

        /// <summary>
        /// Get the newest modified fish from the db for the passed rod.
        /// Resolves string name to ID.
        /// </summary>
        /// <param name="rod">Rod name to get newest modification time for</param>
        /// <returns>Newest modification time for passed rod</returns>
        public static DateTime NewestDBModificationTime(string rod)
        {
            return NewestDBModificationTime(Dictionaries.rodDictionary[rod]);
        }

        /// <summary>
        /// Get the newest modified fish from the db for the passed rod.
        /// </summary>
        /// <param name="rodId">Rod id to get newest modification time for</param>
        /// <returns>Newest modification time for passed rod, or epoch if it's not found.</returns>
        public static DateTime NewestDBModificationTime(int rodId)
        {
                if (OpenConnection())
                {
                    using (MySqlCommand cmd = Connection.CreateCommand())
                    {
                        cmd.CommandText = MySQLCmdNewestUpdate;
                        cmd.Parameters.AddWithValue(MySQLParamRodID, rodId);

                        object date = string.Empty;
                        try
                        {
                            date = cmd.ExecuteScalar();
                        }
                        catch (MySqlException e)
                        {
                            if (StatusDisplay != null)
                            {
                                StatusDisplay.Error(MessageErrorNewestModTime);
                                StatusDisplay.Info(e.ToString());
                            }
                        }
                        if (!string.IsNullOrEmpty(date.ToString()))
                        {
                            return DateTime.Parse(date.ToString());
                        }
                    }
                }
                return new DateTime(1970, 1, 1);
        }

        /// <summary>
        /// Upload a new fish to the DB.
        /// </summary>
        /// <param name="fish">Fish name</param>
        /// <param name="rod">Rod used</param>
        /// <param name="ID1">Fish ID 1</param>
        /// <param name="ID2">Fish ID 2</param>
        /// <param name="ID3">Fish ID 3</param>
        /// <returns>True if fish was successfully added</returns>
        public static bool UploadFish(string fish, string rod, string ID1, string ID2, string ID3)
        {
            if (OpenConnection())
            {
                int rodId = Dictionaries.rodDictionary[rod];
                int id1;
                int id2;
                int id3;

                int.TryParse(ID1, out id1);
                int.TryParse(ID2, out id2);
                int.TryParse(ID3, out id3);

                // Try adding the fish
                using (MySqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = MySQLCommandAddFish;
                    cmd.Parameters.AddWithValue(MySQLParamRodID, rodId);
                    cmd.Parameters.AddWithValue(MySQLParamFishName, fish);
                    cmd.Parameters.AddWithValue(MySQLParamId1, id1);
                    cmd.Parameters.AddWithValue(MySQLParamId2, id2);
                    cmd.Parameters.AddWithValue(MySQLParamId3, id3);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (MySqlException e)
                    {
                        switch (e.Number)
                        {
                            case 1062:
                                // Duplicate primary key. We were successful anyway
                                return true;
                            default:
                                if (StatusDisplay != null)
                                {
                                    StatusDisplay.Error(string.Format(MessageFormatErrorAddingFish, fish));
                                    StatusDisplay.Info(e.ToString());
                                }
                                break;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Register a fish rename with the DB.
        /// </summary>
        /// <param name="fish">New fish name</param>
        /// <param name="oldName">Old fish name</param>
        /// <param name="rod">Rod name</param>
        /// <param name="ID1">Fish ID 1</param>
        /// <param name="ID2">Fish ID 2</param>
        /// <param name="ID3">Fish ID 3</param>
        /// <returns>True if fish was successfully renamed</returns>
        public static bool RenameFish(string fish, string oldName, string rod, string ID1, string ID2, string ID3)
        {
            if (OpenConnection())
            {
                int rodId = Dictionaries.rodDictionary[rod];
                int id1;
                int id2;
                int id3;

                int.TryParse(ID1, out id1);
                int.TryParse(ID2, out id2);
                int.TryParse(ID3, out id3);

                // Try renaming the fish
                using (MySqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = MySQLCommandRenameFish;
                    cmd.Parameters.AddWithValue(MySQLParamFishFromName, oldName);
                    cmd.Parameters.AddWithValue(MySQLParamFishToName, fish);
                    cmd.Parameters.AddWithValue(MySQLParamRodID, rodId);
                    cmd.Parameters.AddWithValue(MySQLParamId1, id1);
                    cmd.Parameters.AddWithValue(MySQLParamId2, id2);
                    cmd.Parameters.AddWithValue(MySQLParamId3, id3);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (MySqlException e)
                    {// Don't care
                        if (StatusDisplay != null)
                        {
                            StatusDisplay.Warning(string.Format(MessageFormatErrorRenamingFish, oldName, fish));
                            StatusDisplay.Info(e.ToString());
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Helper method, gets the ID of a fish in the DB
        /// </summary>
        /// <param name="rodId">ID of the rod</param>
        /// <param name="name">Name of the fish</param>
        /// <param name="id1">Fish ID 1</param>
        /// <param name="id2">Fish ID 2</param>
        /// <param name="id3">Fish ID 3</param>
        /// <returns>DB ID of the fish</returns>
        private static int GetFishDBId(int rodId, string name, int id1, int id2, int id3)
        {
            using (MySqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = MySQLCommandGetFishID;
                cmd.Parameters.AddWithValue(MySQLParamRodID, rodId);
                cmd.Parameters.AddWithValue(MySQLParamFishName, name);
                cmd.Parameters.AddWithValue(MySQLParamId1, id1);
                cmd.Parameters.AddWithValue(MySQLParamId2, id2);
                cmd.Parameters.AddWithValue(MySQLParamId3, id3);
                try
                {
                    return int.Parse(cmd.ExecuteScalar().ToString());
                }
                catch (MySqlException e)
                {
                    if (StatusDisplay != null)
                    {
                        StatusDisplay.Error(string.Format(MessageFormatErrorGettingFishID, name));
                        StatusDisplay.Info(e.ToString());
                    }
                    return -1;
                }
            }
        }

        /// <summary>
        /// Upload fish, bait used to catch them, and zones.
        /// </summary>
        /// <remarks>This is tightly coupled with FishDB and it
        /// really shouldn't be. At present, however, this is done
        /// as well as it can be for efficiency purposes, though the
        /// coupling is undesirable.</remarks>
        /// <param name="fish">Name of the fish</param>
        /// <param name="rod">Name of the rod</param>
        /// <param name="ID1">Fish ID 1</param>
        /// <param name="ID2">Fish ID 2</param>
        /// <param name="ID3">Fish ID 3</param>
        /// <param name="bait">A list of Bait XML nodes to upload</param>
        /// <param name="zones">A list of Zone XML nodes to upload</param>
        /// <param name="fishNode">Fish XML node that's being uploaded</param>
        /// <param name="updatedBait">Pairing of bait and parent nodes to be updated</param>
        /// <param name="updatedZones">Pairing of zone and parent nodes to be updated</param>
        public static void UploadBaitAndZone(string fish, string rod, string ID1, string ID2, string ID3, List<XmlNode> bait, List<XmlNode> zones, XmlNode fishNode, ref Dictionary<XmlNode, XmlNode> updatedBait, ref Dictionary<XmlNode, XmlNode> updatedZones)
        {
            if (OpenConnection())
            {
                int rodId = Dictionaries.rodDictionary[rod];
                int id1;
                int id2;
                int id3;

                int.TryParse(ID1, out id1);
                int.TryParse(ID2, out id2);
                int.TryParse(ID3, out id3);

                // Get the db Fish ID for later
                int fishId = GetFishDBId(rodId, fish, id1, id2, id3);

                // Add baits
                foreach (XmlNode node in bait)
                {
                    String b = node.InnerText;
                    if (null != StatusDisplay)
                    {
                        StatusDisplay.SetFishBaitOrZone(fish, b);
                    }
                    using (MySqlCommand cmd = Connection.CreateCommand())
                    {
                        cmd.CommandText = MySQLCommandAddFishBait;
                        cmd.Parameters.AddWithValue(MySQLParamFishID, fishId);
                        cmd.Parameters.AddWithValue(MySQLParamBaitID, Dictionaries.baitDictionary[b]);

                        try
                        {
                            try
                            {
                                cmd.ExecuteNonQuery();
                                updatedBait.Add(fishNode, node);
                            }
                            catch (MySqlException e)
                            {
                                switch (e.Number)
                                {
                                    case 1062:
                                        // Duplicate primary key. We were successful anyway
                                        updatedBait.Add(fishNode, node);
                                        break;
                                    default:
                                        if (StatusDisplay != null)
                                        {
                                            StatusDisplay.Error(string.Format(MessageFormatErrorAddingBaitFish, b, fish));
                                            StatusDisplay.Info(e.ToString());
                                        }
                                        break;
                                }
                            }
                        }
                        catch (ArgumentException)
                        { // Just in case somehow things get added twice
                        }
                    }
                }

                // Add zones
                foreach (XmlNode node in zones)
                {
                    String z = node.InnerText;
                    if (null != StatusDisplay)
                    {
                        StatusDisplay.SetFishBaitOrZone(fish, z);
                    }
                    using (MySqlCommand cmd = Connection.CreateCommand())
                    {
                        cmd.CommandText = MySQLCommandAddFishZone;
                        cmd.Parameters.AddWithValue(MySQLParamFishID, fishId);
                        if (z.EndsWith(ZoneSelbinaPirates))
                        {
                            cmd.Parameters.AddWithValue(MySQLParamZoneID, (int)FFACETools.Zone.Ferry_between_Mhaura__Selbina_Pirates);
                        }
                        else if (z.EndsWith(ZoneMhauraPirates))
                        {
                            cmd.Parameters.AddWithValue(MySQLParamZoneID, (int)FFACETools.Zone.Ferry_between_Selbina__Mhaura_Pirates);
                        }
                        else
                        {
                            if (FFACE.ParseResources.GetAreaId(z) == 0)
                            {
                                MessageBox.Show(string.Format(string.Join(Environment.NewLine, MessageFormatErrorBadZoneName), rod, fish, z));
                                if (StatusDisplay != null)
                                {
                                    foreach (string msg in MessageFormatErrorBadZoneName)
                                    {
                                        StatusDisplay.Error(msg);
                                    }
                                }
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(MySQLParamZoneID, (int)FFACE.ParseResources.GetAreaId(z));
                            }
                        }

                        try
                        {
                            try
                            {
                                cmd.ExecuteNonQuery();
                                updatedZones.Add(fishNode, node);
                            }
                            catch (MySqlException e)
                            {
                                switch (e.Number)
                                {
                                    case 1062:
                                        // Duplicate primary key. We were successful anyway
                                        updatedZones.Add(fishNode, node);
                                        break;
                                    default:
                                        if (StatusDisplay != null)
                                        {
                                            StatusDisplay.Error(string.Format(MessageFormatErrorAddingZoneFish, z, fish));
                                            StatusDisplay.Info(e.ToString());
                                        }
                                        break;
                                }
                            }
                        }
                        catch (ArgumentException)
                        { // Just in case somehow things get added twice
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Download new fish information from the DB since last download.
        /// Resolves string rod name.
        /// </summary>
        /// <param name="rod">Rod name</param>
        /// <param name="since">Time to get new fish since</param>
        /// <returns>List of <c>SQLFishie</c> new fish, baits, and zones</returns>
        public static List<SQLFishie> DownloadNewFish(string rod, DateTime since)
        {
            return DownloadNewFish(Dictionaries.rodDictionary[rod], since);
        }

        /// <summary>
        /// Download new fish information from the DB since last download.
        /// </summary>
        /// <param name="rodId">Rod ID</param>
        /// <param name="since">Time to get new fish since</param>
        /// <returns>List of <c>SQLFishie</c> new fish, baits, and zones</returns>
        public static List<SQLFishie> DownloadNewFish(int rodId, DateTime since)
        {
            List<SQLFishie> fishies = new List<SQLFishie>();
            if (OpenConnection())
            {
                using (MySqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = MySQLCommandGetNewFishSince;
                    cmd.Parameters.AddWithValue(MySQLParamRodID, rodId);
                    cmd.Parameters.AddWithValue(MySQLParamTime, since);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string name = string.Empty;
                            string id1 = string.Empty;
                            string id2 = string.Empty;
                            string id3 = string.Empty;
                            try
                            {
                                name = rdr.GetString(MySQLParamFishName);
                                id1 = rdr.GetString(MySQLParamId1);
                                id2 = rdr.GetString(MySQLParamId2);
                                id3 = rdr.GetString(MySQLParamId3);
                                int? zone;
                                try
                                {
                                    zone = int.Parse(rdr[MySQLParamZone].ToString());
                                }
                                catch (Exception)
                                {
                                    zone = null;
                                }
                                int? bait;
                                try
                                {
                                    bait = int.Parse(rdr[MySQLParamBait].ToString());
                                }
                                catch (Exception)
                                {
                                    bait = null;
                                }
                                fishies.Add(new SQLFishie(name, rodId, id1, id2, id3, zone, bait));
                            }
                            catch (MySqlException e)
                            {
                                if (StatusDisplay != null)
                                {
                                    StatusDisplay.Error(string.Format(MessageFormatErrorGettingNewFishWithIDs, name, id1, id2, id3));
                                    StatusDisplay.Info(e.ToString());
                                }
                            }
                        }
                    }
                }
            }
            CloseConnection();
            return fishies;
        }

        /// <summary>
        /// Download all new fish renames from the DB since a passed time.
        /// Resolves rod name.
        /// </summary>
        /// <param name="rod">Rod name</param>
        /// <param name="since">Time to download renames since</param>
        /// <returns>A Dictionary of <c>SQLFishie</c> to string name renamed to</returns>
        public static Dictionary<SQLFishie, string> DownloadRenamedFish(string rod, DateTime since)
        {
            return DownloadRenamedFish(Dictionaries.rodDictionary[rod], since);
        }

        /// <summary>
        /// Download all new fish renames from the DB since a passed time.
        /// </summary>
        /// <param name="rodId">Rod ID</param>
        /// <param name="since">Time to download renames since</param>
        /// <returns>A Dictionary of <c>SQLFishie</c> to string name renamed to</returns>
        public static Dictionary<SQLFishie, string> DownloadRenamedFish(int rodId, DateTime since)
        {
            Dictionary<SQLFishie, string> fishies = new Dictionary<SQLFishie, string>();

            if (OpenConnection())
            {
                using (MySqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = MySQLCommandGetRenamedFishSince;
                    cmd.Parameters.AddWithValue(MySQLParamRodID, rodId);
                    cmd.Parameters.AddWithValue(MySQLParamTime, since);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string name = string.Empty;
                            string id1 = string.Empty;
                            string id2 = string.Empty;
                            string id3 = string.Empty;
                            string toName = string.Empty;
                            try
                            {
                                name = rdr.GetString(MySQLParamFishName);
                                id1 = rdr.GetString(MySQLParamId1);
                                id2 = rdr.GetString(MySQLParamId2);
                                id3 = rdr.GetString(MySQLParamId3);
                                fishies.Add(new SQLFishie(name, rodId, id1, id2, id3, null, null), rdr.GetString(MySQLParamFishToName));
                            }
                            catch (MySqlException e)
                            {
                                if (StatusDisplay != null)
                                {
                                    StatusDisplay.Error(string.Format(MessageFormatErrorGettingRenamedFishWithIDs, name, toName, id1, id2, id3));
                                    StatusDisplay.Info(e.ToString());
                                }
                            }
                        }

                    }
                }
            }
            CloseConnection();
            return fishies;
        }

        /// <summary>
        /// Check if the program version is current.
        /// </summary>
        /// <returns>true if the program version is current.</returns>
        public static bool IsProgramUpdated()
        {
            bool updated = false;
            if (OpenConnection())
            {
                String[] versionInfo = FileVersionInfo.GetVersionInfo(FishingForm.ProgramExeName).FileVersion.Split(new char[1] { Resources.Period });

                using (MySqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = MySQLCommandIsVersionCurrent;
                    cmd.Parameters.AddWithValue(MySQLParamVersionMajor, versionInfo[0]);
                    cmd.Parameters.AddWithValue(MySQLParamVersionMinor, versionInfo[1]);
                    cmd.Parameters.AddWithValue(mySqlParamVersionBuild, versionInfo[2]);
                    cmd.Parameters.AddWithValue(mySqlParamVersionRevision, versionInfo[3]);

                    try
                    {
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                updated = true;
                                break;
                            }
                        }
                    }
                    catch (MySqlException e)
                    {
                        if (StatusDisplay != null)
                        {
                            StatusDisplay.Error(MessageErrorGettingVersion);
                            StatusDisplay.Info(e.ToString());
                        }
                    }
                }
            }
            return updated;
        }

        /// <summary>
        /// Upload any fish marked as new or renamed.
        /// </summary>
        /// <note>This is highly coupled with FishDB, perhaps it can be done better.</note>
        public static void DoUploadFish()
        {
            if (null != StatusDisplay)
            {
                StatusDisplay.SetUploadFishNumber(FishDB.DBNewFish.Count + FishDB.DBRenamedFish.Count);
            }
            if (FishDB.DBNewFish.Count > 0 || FishDB.DBRenamedFish.Count > 0)
            {
                Dictionary<string, DateTime> updateTimes = new Dictionary<string, DateTime>();
                HashSet<string> updatedRods = new HashSet<string>();
                List<XmlNode> uploadFish = new List<XmlNode>(FishDB.DBNewFish);
                FishDB.DBNewFish.Clear();
                Dictionary<XmlNode, XmlNode> updatedNodes = new Dictionary<XmlNode, XmlNode>();
                Dictionary<XmlNode, XmlNode> updatedBait = new Dictionary<XmlNode, XmlNode>();
                Dictionary<XmlNode, XmlNode> updatedZones = new Dictionary<XmlNode, XmlNode>();
                foreach (XmlNode fishNode in uploadFish)
                {
                    List<XmlNode> baits = new List<XmlNode>();
                    List<XmlNode> zones = new List<XmlNode>();
                    string rod = fishNode.OwnerDocument.SelectSingleNode(FishDB.XPathRodNode).Attributes[FishDB.XMLAttrName].Value;
                    string fish = fishNode.Attributes[MySQLParamFishName].Value;
                    if (null != StatusDisplay)
                    {
                        StatusDisplay.SetUploadRodAndFish(rod, fish);
                    }
                    if (null != fishNode.Attributes[FishDB.XMLAttrNew])
                    {
                        foreach (XmlNode node in fishNode[FishDB.XMLNodeBaits].ChildNodes)
                        {
                            baits.Add(node);
                        }
                        foreach (XmlNode node in fishNode[FishDB.XMLNodeZones].ChildNodes)
                        {
                            zones.Add(node);
                        }
                        try
                        {
                            if (UploadFish(fish, rod, fishNode.Attributes[FishDB.XMLAttrID1].Value, fishNode.Attributes[FishDB.XMLAttrID2].Value, fishNode.Attributes[FishDB.XMLAttrID3].Value))
                            {
                                try
                                {
                                    updatedNodes.Add(fishNode, fishNode);
                                }
                                catch (ArgumentException)
                                { // In case it's been added already somehow
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            if (StatusDisplay != null)
                            {
                                StatusDisplay.Error(string.Format(MessageFormatErrorUploadingFishRod, fish, rod));
                                StatusDisplay.Info(e.ToString());
                            }
                        }
                    }
                    else
                    {
                        foreach (XmlNode node in fishNode.SelectNodes(FishDB.XPathNewBait))
                        {
                            baits.Add(node);
                        }
                        foreach (XmlNode node in fishNode.SelectNodes(FishDB.XPathNewZones))
                        {
                            zones.Add(node);
                        }
                    }
                    if (baits.Count > 0 || zones.Count > 0)
                    {
                        try
                        {
                            UploadBaitAndZone(fish, rod, fishNode.Attributes[FishDB.XMLAttrID1].Value, fishNode.Attributes[FishDB.XMLAttrID2].Value, fishNode.Attributes[FishDB.XMLAttrID3].Value, baits, zones, fishNode, ref updatedBait, ref updatedZones);
                        }
                        catch (Exception e)
                        {
                            if (StatusDisplay != null)
                            {
                                StatusDisplay.Error(string.Format(MessageFormatErrorUploadingBaitZones, fish, rod));
                                StatusDisplay.Info(e.ToString());
                            }
                        }
                    }
                    updatedRods.Add(rod);
                }
                List<XmlNode> renameFish = new List<XmlNode>(FishDB.DBRenamedFish);
                FishDB.DBRenamedFish.Clear();
                List<XmlNode> renamedNodes = new List<XmlNode>();
                foreach (XmlNode fishNode in renameFish)
                {
                    string rod = fishNode.OwnerDocument.SelectSingleNode(FishDB.XPathRodNode).Attributes[MySQLParamFishName].Value;
                    string fish = fishNode.Attributes[MySQLParamFishName].Value;
                    if (null != StatusDisplay)
                    {
                        StatusDisplay.SetUploadRenameRodAndFish(rod, fish, fishNode.Attributes[FishDB.XMLAttrRename].Value);
                    }
                    try
                    {
                        if (RenameFish(fish, fishNode.Attributes[FishDB.XMLAttrRename].Value, rod, fishNode.Attributes[FishDB.XMLAttrID1].Value, fishNode.Attributes[FishDB.XMLAttrID2].Value, fishNode.Attributes[FishDB.XMLAttrID3].Value))
                        {
                            renamedNodes.Add(fishNode);
                        }
                    }
                    catch (Exception e)
                    {
                        if (StatusDisplay != null)
                        {
                            StatusDisplay.Error(string.Format(MessageFormatErrorUploadingRename, fish, fishNode.Attributes[FishDB.XMLAttrRename].Value, rod));
                            StatusDisplay.Info(e.ToString());
                        }
                    }
                    updatedRods.Add(rod);
                }
                foreach (string rod in updatedRods)
                {
                    updateTimes[rod] = NewestDBModificationTime(rod);
                }
                CloseConnection();
                foreach (XmlNode node in updatedNodes.Keys)
                {
                    FishDB.UnsetNew(node, updatedNodes[node]);
                }
                foreach (XmlNode node in updatedBait.Keys)
                {
                    FishDB.UnsetNew(node, updatedBait[node]);
                }
                foreach (XmlNode node in updatedZones.Keys)
                {
                    FishDB.UnsetNew(node, updatedZones[node]);
                }
                foreach (XmlNode node in renamedNodes)
                {
                    FishDB.UnsetRename(node);
                }
                foreach (string rod in updateTimes.Keys)
                {
                    FishDB.DBUpdated(rod, updateTimes[rod]);
                }
                if (updateTimes.Count > 0)
                {
                    FishDB.UpdatesDBChanged();
                }
            }
        }

        /// <summary>
        /// Download any new fish and renames.
        /// </summary>
        public static void DoDownloadFish()
        {
            Dictionary<string, DateTime> updateTimes = new Dictionary<string, DateTime>();
            if (null != StatusDisplay)
            {
                StatusDisplay.SetDownloadRodNumber(Dictionaries.rodDictionary.Count);
            }
            foreach (string rod in Dictionaries.rodDictionary.Keys)
            {
                if (null != StatusDisplay)
                {
                    StatusDisplay.SetDownloadRod(rod);
                }
                DateTime newest = NewestDBModificationTime(rod);
                if (FishDB.UpdatesByRod[rod].dbDate < newest)
                {
                    try
                    {
                        List<SQLFishie> newFishies = DownloadNewFish(rod, FishDB.UpdatesByRod[rod].dbDate);
                        if (null != StatusDisplay)
                        {
                            StatusDisplay.SetDownloadRodFish(newFishies.Count);
                        }
                        foreach (SQLFishie fish in newFishies)
                        {
                            string name = fish.name;
                            if (null != StatusDisplay)
                            {
                                StatusDisplay.SetDownloadFish(name);
                                if (null == fish.zone)
                                {
                                    StatusDisplay.SetFishBaitOrZone(name, fish.bait);
                                }
                                else if (null == fish.bait)
                                {
                                    StatusDisplay.SetFishBaitOrZone(name, fish.zone);
                                }
                                else
                                {
                                    StatusDisplay.SetFishBaitOrZone(name, string.Format("{0} using {1}", fish.zone, fish.bait));
                                }
                            }
                            FishDB.AddNewFish(ref name, fish.zone, fish.bait, fish.rod, fish.ID1, fish.ID2, fish.ID3, false, true);
                        }
                    }
                    catch (Exception e)
                    {
                        if (StatusDisplay != null)
                        {
                            StatusDisplay.Error(string.Format(MessageFormatErrorDownloadingFishRod, rod));
                            StatusDisplay.Info(e.ToString());
                        }
                    }
                    try
                    {
                        Dictionary<SQLFishie, string> renamedFish = DownloadRenamedFish(rod, FishDB.UpdatesByRod[rod].dbDate);
                        if (null != StatusDisplay)
                        {
                            StatusDisplay.SetDownloadRenameRodFish(renamedFish.Count);
                        }
                        foreach (SQLFishie fish in renamedFish.Keys)
                        {
                            if (null != StatusDisplay)
                            {
                                StatusDisplay.SetDownloadRenameFish(renamedFish[fish], fish.name);
                            }
                            try
                            {
                                FishDB.ChangeName(fish.ToFishDBFishie(), renamedFish[fish], true);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (StatusDisplay != null)
                        {
                            StatusDisplay.Error(string.Format(MessageFormatErrorDownloadingRenameRod, rod));
                            StatusDisplay.Info(e.ToString());
                        }
                    }
                    updateTimes[rod] = newest;
                }
            }
            CloseConnection();
            foreach (string rod in updateTimes.Keys)
            {
                FishDB.DBUpdated(rod, updateTimes[rod]);
            }
            if (updateTimes.Count > 0)
            {
                FishDB.UpdatesDBChanged();
            }
        }

        /// <summary>
        /// Threaded method that handles uploads.
        /// </summary>
        public static void BackgroundUpload()
        {
            if (StatusDisplay != null)
            {
                while (!StatusDisplay.StartDBTransaction(MessageUploadStart))
                {
                    Thread.Sleep(250);
                }
            }
            try
            {
                DoUploadFish();
            }
            catch (Exception e)
            {
                if (StatusDisplay != null)
                {
                    StatusDisplay.Error(MessageErrorUploading);
                    StatusDisplay.Info(e.ToString());
                }
            }
            if (StatusDisplay != null)
            {
                StatusDisplay.EndDBTransaction(MessageUploadFinished);
            }
        }

        /// <summary>
        /// Method that starts a thread to upload new and renamed fish.
        /// </summary>
        public static void UploadNewFish()
        {
            Thread uploadThread = new Thread(new ThreadStart(BackgroundUpload));
            uploadThread.IsBackground = true;
            uploadThread.Start();
        }
    }
}
