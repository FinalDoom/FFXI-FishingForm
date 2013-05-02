using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Fishing
{
    internal struct Fishie
    {
        internal Fishie(string fishName, string rodName, string i1, string i2, string i3)
        {
            name = fishName;
            rod = rodName;
            ID1 = i1;
            ID2 = i2;
            ID3 = i3;
        }

        internal string name, rod, ID1, ID2, ID3;
        public override string ToString() { return name; }

    } // @ internal struct Fishie

    internal struct DBUpdate
    {
        internal DBUpdate(string dbTime, string xmlTime)
        {
            dbDate = DateTime.Parse(dbTime);
            xmlDate = DateTime.Parse(xmlTime);
        }

        internal DateTime dbDate, xmlDate;
        public void XmlUpdated() { xmlDate = DateTime.UtcNow; }
        public void DBUpdated(DateTime time) { dbDate = time; }
    }

    internal static class FishDB
    {
        #region Members

        private const string dbFolder = "FishDB";
        private static Dictionary<string, XmlDocument> DBByRod = new Dictionary<string, XmlDocument>();
        private static XmlDocument ChangeDB;
        private static XmlNode UpdateNode;
        internal static Dictionary<string, DBUpdate> UpdatesByRod = new Dictionary<string, DBUpdate>();
        internal static List<XmlNode> DBNewFish = new List<XmlNode>();
        internal static List<XmlNode> DBRenamedFish = new List<XmlNode>();

        #endregion //Members

        #region Methods

        #region Methods_DBRelated

        private static XmlDocument GetUpdatesDB()
        {
            if (null != ChangeDB)
            {
                return ChangeDB;
            }
            string updateFile = dbFolder + @"\DBSync.xml";

            if (!File.Exists(updateFile))
            {
                //file does not exist, create and add the root node
                if (!Directory.Exists(dbFolder))
                {
                    Directory.CreateDirectory(dbFolder);
                }

                TextWriter writer = new StreamWriter(System.IO.File.Create(updateFile));

                try
                {
                    writer.Write("<Updates>\n</Updates>");
                    writer.Flush();
                }
                finally
                {
                    writer.Close();
                }
            }

            //xml file is ready, load it into the dictionary
            ChangeDB = new XmlDocument();
            ChangeDB.Load(updateFile);
            return ChangeDB;
        }

        internal static XmlNode GetUpdatesNode()
        {
            if (null != UpdateNode)
            {
                return UpdateNode;
            }
            XmlDocument upDoc = GetUpdatesDB();
            XmlNode updateNode = upDoc.SelectSingleNode(string.Format("/Updates/Update[@host=\"{0}\"]", FishSQL.Connection.ConnectionString));
            if (null == updateNode)
            {
                updateNode = upDoc.DocumentElement.AppendChild(upDoc.CreateNode(XmlNodeType.Element, "Update", upDoc.NamespaceURI));
                XmlAttribute hostAttr = updateNode.Attributes.Append(upDoc.CreateAttribute("host"));
                hostAttr.Value = FishSQL.Connection.ConnectionString;
                UpdatesDBChanged();
            }
            UpdateNode = updateNode;
            return updateNode;
        }

        internal static Dictionary<string, DBUpdate> GetUpdates()
        {
            XmlDocument upDoc = GetUpdatesDB();
            XmlNode updateNode = GetUpdatesNode();
            bool changed = false;
            foreach (string rod in Dictionaries.rodDictionary.Keys)
            {
                if (!UpdatesByRod.ContainsKey(rod))
                {
                    XmlNode rodNode = updateNode.SelectSingleNode(string.Format("Rod[@name=\"{0}\"]", rod));
                    if (rodNode == null)
                    {
                        rodNode = updateNode.AppendChild(upDoc.CreateNode(XmlNodeType.Element, "Rod", upDoc.NamespaceURI));
                        XmlAttribute rodName = rodNode.Attributes.Append(upDoc.CreateAttribute("name"));
                        XmlAttribute dbTime = rodNode.Attributes.Append(upDoc.CreateAttribute("db"));
                        XmlAttribute xmlTime = rodNode.Attributes.Append(upDoc.CreateAttribute("xml"));
                        rodName.Value = rod;
                        dbTime.Value = (new DateTime(1970, 1, 1, 0, 0, 1)).ToString();
                        xmlTime.Value = DateTime.UtcNow.ToString();
                        changed = true;
                    }
                    UpdatesByRod[rod] = new DBUpdate(rodNode.Attributes["db"].Value, rodNode.Attributes["xml"].Value);
                }
            }
            if (changed)
            {
                UpdatesDBChanged();
            }
            return UpdatesByRod;
        }

        internal static void XmlUpdated(string rod)
        {
            if (!UpdatesByRod.ContainsKey(rod))
            {
                GetUpdates();
            }
            UpdatesByRod[rod].XmlUpdated();
            XmlNode rodNode = GetUpdatesNode().SelectSingleNode(string.Format("Rod[@name=\"{0}\"]", rod));
            rodNode.Attributes["xml"].Value = UpdatesByRod[rod].xmlDate.ToString();
        }

        internal static void DBUpdated(string rod, DateTime time)
        {
            if (!UpdatesByRod.ContainsKey(rod))
            {
                GetUpdates();
            }
            UpdatesByRod[rod].XmlUpdated();
            XmlNode rodNode = GetUpdatesNode().SelectSingleNode(string.Format("Rod[@name=\"{0}\"]", rod));
            rodNode.Attributes["db"].Value = time.ToString();
            FishDBChanged(rod);
        }

        internal static void UpdatesDBChanged()
        {
            GetUpdatesDB().Save(dbFolder + @"\DBSync.xml");
        }

        internal static void MarkAllFishNew()
        {
            if (DialogResult.Yes == MessageBox.Show("About to mark all fish in all XML as new.\r\nPlease only use this if you are a developer.\r\n\r\nIf running against the xeround server, please think twice.\r\n\r\nAre you sure you want to proceed?", "WARNING", MessageBoxButtons.YesNo))
            {
                foreach (string rod in Dictionaries.rodDictionary.Keys)
                {
                    XmlDocument xmlDoc = GetFishDB(rod);
                    foreach (XmlNode fishNode in xmlDoc.SelectNodes("/Rod/Fish"))
                    {
                        SetNew(rod, fishNode, fishNode);
                    }
                }
            }
        }

        #endregion //DBRelated

        internal static void AddNewFish(ref string fish, string zone, string bait, string rod, string ID1, string ID2, string ID3, bool wanted, bool fromDB)
        {
            XmlDocument xmlDoc = GetFishDB(rod);

            //generate non duplicate name if it is an unknown monster
            if ("Monster" == fish)
            {
                int count = 1;
                fish = "Mob (_" + (count++).ToString() + "_)";

                while (xmlDoc.SelectSingleNode(string.Format("/Rod/Fish[@name=\"{1}\"]", rod, fish)) != null)
                {
                    fish = "Mob (_" + (count++).ToString() + "_)";
                }
            }
            else
            {
                // Generate non-duplicate name for other fish, if IDs don't match another with the same name
                string basefish = fish;
                int count = 1;
                while (xmlDoc.SelectSingleNode(string.Format("/Rod/Fish[@name=\"{1}\"]", rod, fish)) != null &&
                    xmlDoc.SelectSingleNode(string.Format("/Rod/Fish[@name=\"{0}\"][@ID1=\"{1}\"][@ID2=\"{2}\"][@ID3=\"{3}\"]", fish, ID1, ID2, ID3)) == null)
                {
                    fish = basefish + " (_" + (count++).ToString() + "_)";
                }
            }

            XmlNode fishNode = xmlDoc.SelectSingleNode(string.Format("/Rod/Fish[@name=\"{0}\"][@ID1=\"{1}\"][@ID2=\"{2}\"][@ID3=\"{3}\"]", fish, ID1, ID2, ID3));

            if(null == fishNode)
            {
                fishNode = xmlDoc["Rod"].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Fish", xmlDoc.NamespaceURI));
                XmlAttribute fishName = fishNode.Attributes.Append(xmlDoc.CreateAttribute("name"));
                XmlAttribute fishWanted = fishNode.Attributes.Append(xmlDoc.CreateAttribute("wanted"));
                XmlAttribute ID1Node = fishNode.Attributes.Append(xmlDoc.CreateAttribute("ID1"));
                XmlAttribute ID2Node = fishNode.Attributes.Append(xmlDoc.CreateAttribute("ID2"));
                XmlAttribute ID3Node = fishNode.Attributes.Append(xmlDoc.CreateAttribute("ID3"));
                fishName.Value = fish;
                fishWanted.Value = wanted ? "Yes" : "No";
                ID1Node.Value = ID1;
                ID2Node.Value = ID2;
                ID3Node.Value = ID3;
                // Mark it as new, but don't add it to DB until restart or rename
                if (!fromDB)
                { // If it's not being added from the DB anyway
                    fishNode.Attributes.Append(fishNode.OwnerDocument.CreateAttribute("new"));
                    XmlUpdated(rod);
                }

                fishNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Zones", xmlDoc.NamespaceURI));
                fishNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Baits", xmlDoc.NamespaceURI));
            }

            if(null != zone)
            {
                if(null == fishNode.SelectSingleNode("Zones/Zone[text()=\"" + zone + "\"]"))
                {
                    XmlNode zoneNode = fishNode["Zones"].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Zone", xmlDoc.NamespaceURI));
                    zoneNode.InnerText = zone;
                    if (!fromDB)
                    {
                        SetNew(rod, fishNode, zoneNode);
                    }
                }
            }

            if(null != bait)
            {
                if(null == fishNode.SelectSingleNode("Baits/Bait[text()=\"" + bait + "\"]"))
                {
                    XmlNode baitNode = fishNode["Baits"].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Bait", xmlDoc.NamespaceURI));
                    baitNode.InnerText = bait;
                    if (!fromDB)
                    {
                        SetNew(rod, fishNode, baitNode);
                    }
                }
            }

            if (!fromDB)
            {
                FishDBChanged(rod);
            }

        } // @ internal static void AddNewFish(ref string fish, string zone, string bait, string rod, string ID1, string ID2, string ID3, string ID4, bool wanted)

        internal static bool ChangeName(Fishie fish, string newName, bool fromDB)
        {
            XmlDocument xmlDoc = GetFishDB(fish.rod);
            XmlNode sameNameNode = xmlDoc.SelectSingleNode(string.Format("/Rod/Fish[@name=\"{0}\"]", newName));
            XmlNode oldFishNode = xmlDoc.SelectSingleNode(string.Format("/Rod/Fish[@name=\"{0}\"][@ID1=\"{1}\"][@ID2=\"{2}\"][@ID3=\"{3}\"]", newName, fish.ID1, fish.ID2, fish.ID3));
            XmlNode fishNode = xmlDoc.SelectSingleNode(string.Format("/Rod/Fish[@name=\"{0}\"][@ID1=\"{1}\"][@ID2=\"{2}\"][@ID3=\"{3}\"]", fish.name, fish.ID1, fish.ID2, fish.ID3));

            //check if there is already a fish with the same name but different IDs, if there is, the rename fails
            if (null != sameNameNode && null == oldFishNode)
            {
                return false;
            }
            //check if there is already an entry with same ID and name = newName, if there is, merge the 2 entries
            if(null == oldFishNode)
            {
                fishNode.Attributes["name"].Value = newName;
                // Mark for adding to DB if it's a new fish being renamed
                if (null != fishNode.Attributes["new"])
                {
                    SetNew(fish.rod, fishNode, fishNode);
                }
                else
                {
                    SetRenamed(fish.rod, fish.name, fishNode);
                }
                if (!fromDB)
                {
                    FishDBChanged(fish.rod);
                }
            }
            else
            {
                //merging (union of Zones and Baits)
                XmlNodeList zones = fishNode["Zones"].ChildNodes;
                XmlNodeList baits = fishNode["Baits"].ChildNodes;

                foreach(XmlNode zone in zones)
                {
                    if(null == oldFishNode.SelectSingleNode("Zones/Zone[text()=\"" + zone.InnerText + "\"]"))
                    {
                        XmlNode zoneNode = oldFishNode["Zones"].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Zone", xmlDoc.NamespaceURI));
                        zoneNode.InnerText = zone.InnerText;
                    }
                }

                foreach(XmlNode bait in baits)
                {
                    if(null == oldFishNode.SelectSingleNode("Baits/Bait[text()=\"" + bait.InnerText + "\"]"))
                    {
                        XmlNode baitNode = oldFishNode["Baits"].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Bait", xmlDoc.NamespaceURI));
                        baitNode.InnerText = bait.InnerText;
                    }
                }

                // This shouldn't really happen, and the duplicates aren't allowed in the DB, so no worries there
                xmlDoc["Rod"].RemoveChild(fishNode);
                SetRenamed(fish.rod, fish.name, fishNode);
                if (!fromDB)
                {
                    FishDBChanged(fish.rod);
                }
            }
            return true;

        } // @ internal static void ChangeName(Fishie fish, string newName)

        internal static void SetRenamed(string rod, string oldName, XmlNode fishNode)
        {
            if (fishNode.Attributes["rename"] == null)
            {
                fishNode.Attributes.Append(fishNode.OwnerDocument.CreateAttribute("rename"));
            }
            fishNode.Attributes["rename"].Value = oldName;
            DBRenamedFish.Add(fishNode);
        }

        internal static void SetNew(string rod, XmlNode fishNode, XmlNode newNode)
        {
            if (newNode.Attributes["new"] == null)
            {
                newNode.Attributes.Append(fishNode.OwnerDocument.CreateAttribute("new"));
                DBNewFish.Add(fishNode);
                XmlUpdated(rod);
            }
        }

        internal static void UnsetRename(XmlNode fishNode)
        {
            if (fishNode.Attributes["rename"] != null)
            {
                fishNode.Attributes.Remove(fishNode.Attributes["rename"]);
                DBRenamedFish.Remove(fishNode);
            }
        }

        internal static void UnsetNew(XmlNode fishNode, XmlNode newNode)
        {
            if (newNode.Attributes["new"] != null)
            {
                newNode.Attributes.Remove(newNode.Attributes["new"]);
                DBNewFish.Remove(fishNode);
            }
        }

        internal static bool FishAccepted(out string name, out bool isNew, bool fishUnknown, string rod, string zone, string bait, string ID1, string ID2, string ID3)
        {
            XmlDocument xmlDoc = GetFishDB(rod);
            string xpathQuery = string.Format("/Rod/Fish[@ID1=\"{0}\"][@ID2=\"{1}\"][@ID3=\"{2}\"][Zones/Zone=\"{3}\"]", ID1, ID2, ID3, zone);
            XmlNode fishNode = xmlDoc.SelectSingleNode(xpathQuery);

            if(null == fishNode)
            {
                name = "Unknown";
                isNew = true;

                return fishUnknown;
            }
            else
            {
                isNew = false;
                name = fishNode.Attributes["name"].Value;

                if(null == fishNode.SelectSingleNode("Baits/Bait[text()=\"" + bait + "\"]"))
                {
                    XmlNode newBaitNode = fishNode["Baits"].AppendChild(xmlDoc.CreateElement("Bait"));
                    newBaitNode.InnerText = bait;
                    SetNew(rod, fishNode, newBaitNode);
                    FishDBChanged(rod);
                }

                return ("Yes" == fishNode.Attributes["wanted"].Value) ? true : false;
            }

        } // @ internal static bool FishAccepted(out string name, out bool isNew, bool fishUnknown, string rod, string zone, string bait, string ID1, string ID2, string ID3, string ID4)

        private static string GetFileName(string rod)
        {
            switch(rod)
            {
                case "Comp. Fishing Rod":
                    return dbFolder + @"\composite.xml";
                case "Bamboo Fish. Rod":
                    return dbFolder + @"\bamboo.xml";
                case "Carbon Fish. Rod":
                    return dbFolder + @"\carbon.xml";
                case "Ebisu Fishing Rod":
                    return dbFolder + @"\ebisu.xml";
                case "Clothespole":
                    return dbFolder + @"\clothespole.xml";
                case "Fastwater F. Rod":
                    return dbFolder + @"\fastwater.xml";
                case "Glass Fiber F. Rod":
                    return dbFolder + @"\glassfiber.xml";
                case "Halcyon Rod":
                    return dbFolder + @"\halcyon.xml";
                case "Hume Fishing Rod":
                    return dbFolder + @"\hume.xml";
                case "Lu Shang's F. Rod":
                    return dbFolder + @"\lushang.xml";
                case "MMM Fishing Rod":
                    return dbFolder + @"\mmm.xml";
                case "Mithran Fish. Rod":
                    return dbFolder + @"\mithran.xml";
                case "S.H. Fishing Rod":
                    return dbFolder + @"\singlehook.xml";
                case "Tarutaru F. Rod":
                    return dbFolder + @"\tarutaru.xml";
                case "Willow Fish. Rod":
                    return dbFolder + @"\willow.xml";
                case "Yew Fishing Rod":
                    return dbFolder + @"\yew.xml";
                default:
                    return null;
            }

        } // @ private static string GetFileName(string rod)

        private static XmlDocument GetFishDB(string rod)
        {
            if(!DBByRod.ContainsKey(rod))
            {
                //fish db for this rod is not loaded, check if the xml file is available, create if needed
                string fishDBFile = GetFileName(rod);

                if(!File.Exists(fishDBFile))
                {
                    //file does not exist, create and add the root node
                    if(!Directory.Exists(dbFolder))
                    {
                        Directory.CreateDirectory(dbFolder);
                    }

                    TextWriter writer = new StreamWriter(System.IO.File.Create(fishDBFile));

                    try
                    {
                        writer.WriteLine(string.Format("<Rod name=\"{0}\">\n</Rod>", rod));
                        writer.Flush();
                    }
                    finally
                    {
                        writer.Close();
                    }
                }

                //xml file is ready, load it into the dictionary
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fishDBFile);
                DBByRod.Add(rod, xmlDoc);
                //Check any fish previously marked new
                foreach (XmlNode fishNode in xmlDoc.SelectNodes("/Rod/Fish[@new]"))
                {
                    DBNewFish.Add(fishNode);
                }
                //Check any bait or zones previously marked new
                foreach (XmlNode baitorZoneNode in xmlDoc.SelectNodes("/Rod/Fish/Baits/Bait[@new] | /Rod/Fish/Zones/Zone[@new]"))
                {
                    DBNewFish.Add(baitorZoneNode.ParentNode.ParentNode);
                }
            }

            return DBByRod[rod];

        } // @ private static XmlDocument GetFishDB(string rod)

        internal static Fishie[] GetFishes(string rod, string zone, string bait, bool wanted)
        {
            XmlDocument xmlDoc = GetFishDB(rod);
            string xpathQuery = string.Format("/Rod/Fish[Zones/Zone=\"{0}\"][Baits/Bait=\"{1}\"][@wanted=\"{2}\"]", zone, bait, wanted ? "Yes" : "No");
            XmlNodeList nodes = xmlDoc.SelectNodes(xpathQuery);
            Fishie[] fishes = new Fishie[nodes.Count];
            int i = 0;

            foreach(XmlNode node in nodes)
            {
                fishes[i++] = new Fishie(node.Attributes["name"].Value, rod, node.Attributes["ID1"].Value, node.Attributes["ID2"].Value, node.Attributes["ID3"].Value);
            }

            return fishes;

        } // @ internal static Fishie[] GetFishes(string rod, string zone, string bait, bool wanted)

        internal static void ToggleWanted(Fishie fish)
        {
            XmlDocument xmlDoc = GetFishDB(fish.rod);
            string xpathQuery = string.Format("/Rod/Fish[@name=\"{0}\"]", fish.name);
            XmlNode fishNode = xmlDoc.SelectSingleNode(xpathQuery);
            fishNode.Attributes["wanted"].Value = ("Yes" == fishNode.Attributes["wanted"].Value) ? "No" : "Yes";
            FishDBChanged(fish.rod);

        } // @ internal static void ToggleWanted(Fishie fish)

        #endregion //Methods

        #region Events_Custom

        internal delegate void DBChanged();
        internal static event DBChanged OnChanged;

        private static void FishDBChanged(string rod)
        {
            DBByRod[rod].Save(GetFileName(rod));
            // Cache a few before doing a DB upload
            if (DBNewFish.Count > 4)
            {
                FishSQL.UploadNewFish();
            }

            if(null != OnChanged)
            {
                OnChanged();
            }

        } // @ private static void FishDBChanged(string rod)

        #endregion //Events_Custom

    } // @ internal static class FishDB
}
