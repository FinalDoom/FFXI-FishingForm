using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    internal static class FishSQL
    {
        internal static IFishDBStatusDisplay StatusDisplay { private get; set; }
        internal static MySqlConnection Connection { get; private set; }
        private static string server = "instance44985.db.xeround.com";
        private static string port = "6382";
        private static string database = "FishDB";
        // These are procedure-only credentials. Contact FinalDoom for an admin account, eg. if taking over this project.
        private static string uid = "FishClient";
        private static string password = "ThePasswordForTheFishClient";
        private static bool errorred = false;

        static FishSQL()
        {
            Initialize();
        }

        //Initialize connection
        private static void Initialize()
        {
            Connection = new MySqlConnection(string.Format("SERVER={0};PORT={1};DATABASE={2};UID={3};PASSWORD={4};", server, port, database, uid, password));
        }

        //open connection to database
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
                if (!errorred)
                {
                    switch (e.Number)
                    {
                        case 0:
                            MessageBox.Show("Could not connect to FishDB MySQL server. Please contact program maintainer.");
                            break;
                        default:
                            MessageBox.Show("Could not connect to FishDB MySQL server. Error number " + e.Number.ToString());
                            break;
                    }
                    errorred = true;
                }
                return false;
            }
        }

        //Close connection
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
            catch (MySqlException)
            {
                return false;
            }
        }

        public static void CloseAllConnections()
        {
            MySqlConnection.ClearAllPools();
        }

        public static DateTime NewestDBModificationTime(string rod)
        {
            return NewestDBModificationTime(Dictionaries.rodDictionary[rod]);
        }

        public static DateTime NewestDBModificationTime(int rodId)
        {
                if (OpenConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand("CALL get_newest_update (@rodID)", Connection))
                    {
                        cmd.Parameters.AddWithValue("rodID", rodId);

                        object date = string.Empty;
                        try
                        {
                            date = cmd.ExecuteScalar();
                        }
                        catch (MySqlException)
                        {
                        }
                        if (!string.IsNullOrEmpty(date.ToString()))
                        {
                            return DateTime.Parse(date.ToString());
                        }
                    }
                }
                return new DateTime(1970, 1, 1);
        }

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
                    cmd.CommandText = "CALL add_new_fish (@rodID, @name, @Id1, @Id2, @Id3)";
                    cmd.Parameters.AddWithValue("rodID", rodId);
                    cmd.Parameters.AddWithValue("name", fish);
                    cmd.Parameters.AddWithValue("Id1", id1);
                    cmd.Parameters.AddWithValue("Id2", id2);
                    cmd.Parameters.AddWithValue("Id3", id3);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (MySqlException e)
                    {
                        if (e.Number == 1062)
                        {
                            // Duplicate primary key. We were successful anyway
                            return true;
                        }
                    }
                }
            }
            return false;
        }

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
                    cmd.CommandText = "CALL rename_fish (@fromName, @toName, @rodID, @Id1, @Id2, @Id3)";
                    cmd.Parameters.AddWithValue("fromName", oldName);
                    cmd.Parameters.AddWithValue("toName", fish);
                    cmd.Parameters.AddWithValue("rodID", rodId);
                    cmd.Parameters.AddWithValue("Id1", id1);
                    cmd.Parameters.AddWithValue("Id2", id2);
                    cmd.Parameters.AddWithValue("Id3", id3);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (MySqlException)
                    {// Don't care
                    }
                }
            }
            return false;
        }

        private static int GetFishDBId(int rodId, string name, int id1, int id2, int id3)
        {
            using (MySqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "CALL check_fish (@rodID, @Name, @Id1, @Id2, @Id3)";
                cmd.Parameters.AddWithValue("rodID", rodId);
                cmd.Parameters.AddWithValue("Name", name);
                cmd.Parameters.AddWithValue("Id1", id1);
                cmd.Parameters.AddWithValue("Id2", id2);
                cmd.Parameters.AddWithValue("Id3", id3);
                try
                {
                    return int.Parse(cmd.ExecuteScalar().ToString());
                }
                catch (MySqlException)
                {
                    return -1;
                }
            }
        }

        public static void UploadBaitAndZone(string fish, string rod, string ID1, string ID2, string ID3, List<XmlNode> bait, List<XmlNode> zones, XmlNode fishNode, ref Dictionary<XmlNode, XmlNode> updatedNodes)
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
                        cmd.CommandText = "CALL add_fish_bait (@fishID, @baitID)";
                        cmd.Parameters.AddWithValue("fishID", fishId);
                        cmd.Parameters.AddWithValue("baitID", Dictionaries.baitDictionary[b]);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            updatedNodes.Add(fishNode, node);
                        }
                        catch (MySqlException e)
                        {
                            if (e.Number == 1062)
                            {
                                // Duplicate primary key. We were successful anyway
                                updatedNodes.Add(fishNode, node);
                            }
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
                        cmd.CommandText = "CALL add_fish_zone (@fishID, @zoneID)";
                        cmd.Parameters.AddWithValue("fishID", fishId);
                        if (z.EndsWith("Selbina (Pirates)"))
                        {
                            cmd.Parameters.AddWithValue("zoneID", (int)FFACETools.Zone.Ferry_between_Mhaura__Selbina_Pirates);
                        }
                        else if (z.EndsWith("Mhaura (Pirates)"))
                        {
                            cmd.Parameters.AddWithValue("zoneID", (int)FFACETools.Zone.Ferry_between_Selbina__Mhaura_Pirates);
                        }
                        else
                        {
                            if (FFACE.ParseResources.GetAreaId(z) == 0)
                            {
                                MessageBox.Show(string.Format("Broken zone name found:\r\nRod: {0}\r\nFish: {1}\r\nZone: {2}\r\n\r\nPlease fix it to match a Windower resources.xml name,\r\nand add an attribute to the parent tag: name", rod, fish, z));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("zoneID", (int)FFACE.ParseResources.GetAreaId(z));
                            }
                        }

                        try
                        {
                            cmd.ExecuteNonQuery();
                            updatedNodes.Add(fishNode, node);
                        }
                        catch (MySqlException e)
                        {
                            if (e.Number == 1062)
                            {
                                // Duplicate primary key. We were successful anyway
                                updatedNodes.Add(fishNode, node);
                            }
                        }
                    }
                }
            }
        }

        public static List<SQLFishie> DownloadNewFish(string rod, DateTime since)
        {
            return DownloadNewFish(Dictionaries.rodDictionary[rod], since);
        }

        public static List<SQLFishie> DownloadNewFish(int rodId, DateTime since)
        {
            List<SQLFishie> fishies = new List<SQLFishie>();
            if (OpenConnection())
            {
                using (MySqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "CALL get_new_fish (@rodID, @time)";
                    cmd.Parameters.AddWithValue("rodID", rodId);
                    cmd.Parameters.AddWithValue("time", since);
                    try
                    {
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                int? zone;
                                try
                                {
                                    zone = int.Parse(rdr["Zone"].ToString());
                                }
                                catch (Exception)
                                {
                                    zone = null;
                                }
                                int? bait;
                                try
                                {
                                    bait = int.Parse(rdr["Bait"].ToString());
                                }
                                catch (Exception)
                                {
                                    bait = null;
                                }
                                fishies.Add(new SQLFishie(rdr.GetString("Name"), rodId, rdr.GetString("Id1"), rdr.GetString("Id2"), rdr.GetString("Id3"), zone, bait));
                            }

                        }
                    }
                    catch (MySqlException)
                    {
                    }
                }
            }
            CloseConnection();
            return fishies;
        }

        public static Dictionary<SQLFishie, string> DownloadRenamedFish(string rod, DateTime since)
        {
            return DownloadRenamedFish(Dictionaries.rodDictionary[rod], since);
        }

        public static Dictionary<SQLFishie, string> DownloadRenamedFish(int rodId, DateTime since)
        {
            Dictionary<SQLFishie, string> fishies = new Dictionary<SQLFishie, string>();

            if (OpenConnection())
            {
                using (MySqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "CALL get_renamed_fish (@rodID, @time)";
                    cmd.Parameters.AddWithValue("rodID", rodId);
                    cmd.Parameters.AddWithValue("time", since);
                    try
                    {
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                fishies.Add(new SQLFishie(rdr.GetString("Name"), rodId, rdr.GetString("Id1"), rdr.GetString("Id2"), rdr.GetString("Id3"), null, null), rdr.GetString("ToName"));
                            }

                        }
                    }
                    catch (MySqlException)
                    {
                    }
                }
            }
            CloseConnection();
            return fishies;
        }

        public static bool IsProgramUpdated()
        {
            bool updated = false;
            if (OpenConnection())
            {
                String[] versionInfo = FileVersionInfo.GetVersionInfo("fishing.exe").FileVersion.Split(new char[1] { '.' });

                using (MySqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "Call is_current_version (@major, @minor, @build, @revision)";
                    cmd.Parameters.AddWithValue("major", versionInfo[0]);
                    cmd.Parameters.AddWithValue("minor", versionInfo[1]);
                    cmd.Parameters.AddWithValue("build", versionInfo[2]);
                    cmd.Parameters.AddWithValue("revision", versionInfo[3]);

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
                    catch (MySqlException)
                    {
                    }
                }
            }
            return updated;
        }

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
                foreach (XmlNode fishNode in uploadFish)
                {
                    List<XmlNode> baits = new List<XmlNode>();
                    List<XmlNode> zones = new List<XmlNode>();
                    string rod = fishNode.OwnerDocument.SelectSingleNode("/Rod").Attributes["name"].Value;
                    string fish = fishNode.Attributes["name"].Value;
                    if (null != StatusDisplay)
                    {
                        StatusDisplay.SetUploadRodAndFish(rod, fish);
                    }
                    if (null != fishNode.Attributes["new"])
                    {
                        foreach (XmlNode node in fishNode["Baits"].ChildNodes)
                        {
                            baits.Add(node);
                        }
                        foreach (XmlNode node in fishNode["Zones"].ChildNodes)
                        {
                            zones.Add(node);
                        }
                        if (UploadFish(fish, rod, fishNode.Attributes["ID1"].Value, fishNode.Attributes["ID2"].Value, fishNode.Attributes["ID3"].Value))
                        {
                            updatedNodes.Add(fishNode, fishNode);
                        }
                    }
                    else
                    {
                        foreach (XmlNode node in fishNode.SelectNodes("Baits/Bait[@new]"))
                        {
                            baits.Add(node);
                        }
                        foreach (XmlNode node in fishNode.SelectNodes("Zones/Zone[@new]"))
                        {
                            zones.Add(node);
                        }
                    }
                    if (baits.Count > 0 || zones.Count > 0)
                    {
                        UploadBaitAndZone(fish, rod, fishNode.Attributes["ID1"].Value, fishNode.Attributes["ID2"].Value, fishNode.Attributes["ID3"].Value, baits, zones, fishNode, ref updatedNodes);
                    }
                    updatedRods.Add(rod);
                }
                List<XmlNode> renameFish = new List<XmlNode>(FishDB.DBRenamedFish);
                FishDB.DBRenamedFish.Clear();
                List<XmlNode> renamedNodes = new List<XmlNode>();
                foreach (XmlNode fishNode in renameFish)
                {
                    string rod = fishNode.OwnerDocument.SelectSingleNode("/Rod").Attributes["name"].Value;
                    string fish = fishNode.Attributes["name"].Value;
                    if (null != StatusDisplay)
                    {
                        StatusDisplay.SetUploadRenameRodAndFish(rod, fish, fishNode.Attributes["rename"].Value);
                    }
                    if (RenameFish(fish, fishNode.Attributes["rename"].Value, rod, fishNode.Attributes["ID1"].Value, fishNode.Attributes["ID2"].Value, fishNode.Attributes["ID3"].Value))
                    {
                        renamedNodes.Add(fishNode);
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
                            else
                            {
                                StatusDisplay.SetFishBaitOrZone(name, fish.zone);
                            }
                        }
                        FishDB.AddNewFish(ref name, fish.zone, fish.bait, fish.rod, fish.ID1, fish.ID2, fish.ID3, false, true);
                    }
                    Dictionary<SQLFishie, string> renamedFish = DownloadRenamedFish(rod, FishDB.UpdatesByRod[rod].dbDate);
                    if (null != StatusDisplay)
                    {
                        StatusDisplay.SetDownloadRenameRodFish(renamedFish.Count);
                    }
                    foreach (SQLFishie fish in renamedFish.Keys)
                    {
                        if (null != StatusDisplay)
                        {
                            StatusDisplay.SetDownloadRenameFish(fish.name, renamedFish[fish]);
                        }
                        try
                        {
                            FishDB.ChangeName(fish.ToFishDBFishie(), renamedFish[fish], true);
                        }
                        catch (Exception)
                        {
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

        public static void UploadNewFish()
        {
            Thread uploadThread = new Thread(new ThreadStart(DoUploadFish));
            uploadThread.IsBackground = true;
            uploadThread.Start();
        }
    }
}
