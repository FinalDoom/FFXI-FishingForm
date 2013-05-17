using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Fishing.Properties;

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
        public void XmlUpdated(DateTime time) { xmlDate = time; }
        private void DBUpdated(DateTime time) { dbDate = time; }
    }

    internal static class FishDB
    {
        #region Members

        private const string dbFolder = "FishDB";
        private const string dbMinVersion = "1.7.0.7";
        private static readonly string[] MessageMarkAllNew = {
            "About to mark all fish in all XML as new.",
            "Please only use this if you are a developer.",
            "",
            "If running against the xeround server, please think twice.",
            "",
            "Are you sure you want to proceed?"
        };

        // XML constants for DBSync.xml
        private const string DBSyncFile = dbFolder + @"\DBSync.xml";
        private const string XMLUpdates = "<Updates>\n</Updates>";
        private const string XPathFormatUpdateByHost = "/Updates/Update[@host=\"{0}\"]";
        private const string XMLNodeUpdate = "Update";
        private const string XMLAttrDBVer = "dbver";
        private const string XMLAttrHost = "host";
        private const string XMLAttrDbTime = "db";
        private const string XMLAttrXMLTime = "xml";
        private const string XPathFormatRodByName = "Rod[@name=\"{0}\"]";

        // XML constants for rod.xml files
        private const string RodItemGlassFiber = "Glass Fiber F. Rod";
        private const string RodItemComposite = "Comp. Fishing Rod";
        private const string RodItemBamboo = "Bamboo Fish. Rod";
        private const string RodItemCarbon = "Carbon Fish. Rod";
        private const string EbisuFishingRod = "Ebisu Fishing Rod";
        private const string RodItemClothespole = "Clothespole";
        private const string RodItemFastwater = "Fastwater F. Rod";
        private const string RodItemHalcyon = "Halcyon Rod";
        private const string RodItemHume = "Hume Fishing Rod";
        private const string RodItemLuShangs = "Lu Shang's F. Rod";
        private const string RodItemMMM = "MMM Fishing Rod";
        private const string RodItemMithran = "Mithran Fish. Rod";
        private const string RodItemSH = "S.H. Fishing Rod";
        private const string RodItemTarutaru = "Tarutaru F. Rod";
        private const string RodItemWillow = "Willow Fish. Rod";
        private const string RodItemYew = "Yew Fishing Rod";
        private const string FileXMLBamboo = dbFolder + @"\bamboo.xml";
        private const string FileXMLCarbon = dbFolder + @"\carbon.xml";
        private const string FileXMLClothespole = dbFolder + @"\clothespole.xml";
        private const string FileXMLComposite = dbFolder + @"\composite.xml";
        private const string FileXMLEbisu = dbFolder + @"\ebisu.xml";
        private const string FileXMLFastwater = dbFolder + @"\fastwater.xml";
        private const string FileXMLGlass = dbFolder + @"\glassfiber.xml";
        private const string FileXMLHalcyon = dbFolder + @"\halcyon.xml";
        private const string FileXMLHume = dbFolder + @"\hume.xml";
        private const string FileXMLLuShangs = dbFolder + @"\lushang.xml";
        private const string FileXMLMMM = dbFolder + @"\mmm.xml";
        private const string FileXMLMithran = dbFolder + @"\mithran.xml";
        private const string FileXMLSH = dbFolder + @"\singlehook.xml";
        private const string FileXMLTarutaru = dbFolder + @"\tarutaru.xml";
        private const string FileXMLWillow = dbFolder + @"\willow.xml";
        private const string FileXMLYew = dbFolder + @"\yew.xml";
        private const string XMLFormatRod = "<Rod name=\"{0}\">\n</Rod>";
        private const string XMLNodeRod = "Rod";
        private const string XMLNodeFish = "Fish";
        internal const string XMLNodeZones = "Zones";
        private const string XMLNodeZone = "Zone";
        internal const string XMLNodeBaits = "Baits";
        private const string XMLNodeBait = "Bait";
        internal const string XMLAttrName = "name";
        private const string XMLAttrWanted = "wanted";
        internal const string XMLAttrNew = "new";
        internal const string XMLAttrRename = "rename";
        internal const string XMLAttrID1 = "ID1";
        internal const string XMLAttrID2 = "ID2";
        internal const string XMLAttrID3 = "ID3";
        internal const string XPathRodNode = "/Rod";
        private const string XPathFormatFishByName = "/Rod/Fish[@name=\"{0}\"]";
        private const string XPathFormatFishByIDsAndZone = "/Rod/Fish[@ID1=\"{0}\"][@ID2=\"{1}\"][@ID3=\"{2}\"][Zones/Zone=\"{3}\"]";
        private const string XPathFormatFishByZoneBaitAndWanted = "/Rod/Fish[Zones/Zone=\"{0}\"][Baits/Bait=\"{1}\"][@wanted=\"{2}\"]";
        private const string XPathFormatFishByNameAndIDs = "/Rod/Fish[@name=\"{0}\"][@ID1=\"{1}\"][@ID2=\"{2}\"][@ID3=\"{3}\"]";
        private const string XPathFormatBaitByName = "Baits/Bait[text() = \"{0}\"]";
        private const string XPathFormatZoneByName = "Zones/Zone[text()=\"{0}\"]";
        private const string XPathNewFish = "/Rod/Fish[@new]";
        internal const string XPathNewBait = "Baits/Bait[@new]";
        internal const string XPathNewZones = "Zones/Zone[@new]";
        private const string XPathNewBaitsAndZones = "/Rod/Fish/Baits/Bait[@new]|/Rod/Fish/Zones/Zone[@new]";
        private const string XPathRenamedFish = "/Rod/Fish[@rename]";
        private const string XPathAllFish = "/Rod/Fish";

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

            if (!File.Exists(DBSyncFile))
            {
                //file does not exist, create and add the root node
                if (!Directory.Exists(dbFolder))
                {
                    Directory.CreateDirectory(dbFolder);
                }

                using (TextWriter writer = new StreamWriter(System.IO.File.Create(DBSyncFile)))
                {

                    try
                    {
                        writer.Write(XMLUpdates);
                        writer.Flush();
                    }
                    finally
                    {
                        writer.Close();
                    }
                }
            }

            //xml file is ready, load it into the dictionary
            ChangeDB = new XmlDocument();
            ChangeDB.Load(DBSyncFile);
            return ChangeDB;
        }

        internal static bool IsDBVersionAcceptable(string ver)
        {
            string[] verSplit = ver.Split(new char[1] { Resources.Period });
            string[] dbMin = dbMinVersion.Split(new char[1] { Resources.Period });
            for (int i = 0; i < verSplit.Length; ++i)
            {
                if (int.Parse(verSplit[i]) > int.Parse(dbMin[i]))
                {
                    return true;
                }
                else if (i == verSplit.Length - 1 && verSplit[i] == dbMin[i])
                {
                    return true;
                }
            }
            return false;
        }

        internal static XmlNode GetUpdatesNode()
        {
            if (null != UpdateNode)
            {
                return UpdateNode;
            }
            XmlDocument upDoc = GetUpdatesDB();
            XmlNode updateNode = upDoc.SelectSingleNode(string.Format(XPathFormatUpdateByHost, FishSQL.Connection.ConnectionString));
            if (null == updateNode)
            {
                updateNode = upDoc.DocumentElement.AppendChild(upDoc.CreateNode(XmlNodeType.Element, XMLNodeUpdate, upDoc.NamespaceURI));
                XmlAttribute hostAttr = updateNode.Attributes.Append(upDoc.CreateAttribute(XMLAttrHost));
                hostAttr.Value = FishSQL.Connection.ConnectionString;
                UpdatesDBChanged();
            }
            else if (updateNode.Attributes[XMLAttrDBVer] == null || !IsDBVersionAcceptable(updateNode.Attributes[XMLAttrDBVer].Value))
            {
                updateNode.RemoveAll();
                XmlAttribute hostAttr = updateNode.Attributes.Append(upDoc.CreateAttribute(XMLAttrHost));
                hostAttr.Value = FishSQL.Connection.ConnectionString;
                XmlAttribute dbverAttr = updateNode.Attributes.Append(upDoc.CreateAttribute(XMLAttrDBVer));
                dbverAttr.Value = dbMinVersion;
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
                    XmlNode rodNode = updateNode.SelectSingleNode(string.Format(XPathFormatRodByName, rod));
                    if (rodNode == null)
                    {
                        rodNode = updateNode.AppendChild(upDoc.CreateNode(XmlNodeType.Element, XMLNodeRod, upDoc.NamespaceURI));
                        XmlAttribute rodName = rodNode.Attributes.Append(upDoc.CreateAttribute(XMLAttrName));
                        XmlAttribute dbTime = rodNode.Attributes.Append(upDoc.CreateAttribute(XMLAttrDbTime));
                        XmlAttribute xmlTime = rodNode.Attributes.Append(upDoc.CreateAttribute(XMLAttrXMLTime));
                        rodName.Value = rod;
                        dbTime.Value = (new DateTime(1970, 1, 1, 0, 0, 1)).ToString();
                        xmlTime.Value = (new DateTime(1970, 1, 1, 0, 0, 1)).ToString();
                        changed = true;
                    }
                    UpdatesByRod[rod] = new DBUpdate(rodNode.Attributes[XMLAttrDbTime].Value, rodNode.Attributes[XMLAttrXMLTime].Value);
                }
            }
            if (changed)
            {
                UpdatesDBChanged();
            }
            foreach (string rod in UpdatesByRod.Keys)
            {
                if (UpdatesByRod[rod].xmlDate > UpdatesByRod[rod].dbDate)
                {
                    GetFishDB(rod);
                }
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
            XmlNode rodNode = GetUpdatesNode().SelectSingleNode(string.Format(XPathFormatRodByName, rod));
            rodNode.Attributes[XMLAttrXMLTime].Value = UpdatesByRod[rod].xmlDate.ToString();
        }

        internal static void DBUpdated(string rod, DateTime time)
        {
            if (!UpdatesByRod.ContainsKey(rod))
            {
                GetUpdates();
            }
            UpdatesByRod[rod].XmlUpdated(time);
            XmlNode rodNode = GetUpdatesNode().SelectSingleNode(string.Format(XPathFormatRodByName, rod));
            rodNode.Attributes[XMLAttrDbTime].Value = time.ToString();
            FishDBChanged(rod);
        }

        internal static void UpdatesDBChanged()
        {
            GetUpdatesDB().Save(DBSyncFile);
        }

        internal static void MarkAllFishNew()
        {
            if (DialogResult.Yes == MessageBox.Show(string.Join(Environment.NewLine, MessageMarkAllNew), Resources.MessageTitleWarning, MessageBoxButtons.YesNo))
            {
                foreach (string rod in Dictionaries.rodDictionary.Keys)
                {
                    XmlDocument xmlDoc = GetFishDB(rod);
                    foreach (XmlNode fishNode in xmlDoc.SelectNodes(XPathAllFish))
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
            if (Resources.FishNameMonster == fish)
            {
                int count = 1;
                fish = string.Format(Resources.FishNameFormatMob, (count++));

                while (xmlDoc.SelectSingleNode(string.Format(XPathFormatFishByName, fish)) != null)
                {
                    fish = string.Format(Resources.FishNameFormatMob, (count++));
                }
            }

            XmlNode fishNode = xmlDoc.SelectSingleNode(string.Format(XPathFormatFishByNameAndIDs, fish, ID1, ID2, ID3));

            if(null == fishNode)
            {
                fishNode = xmlDoc[XMLNodeRod].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, XMLNodeFish, xmlDoc.NamespaceURI));
                XmlAttribute fishName = fishNode.Attributes.Append(xmlDoc.CreateAttribute(XMLAttrName));
                XmlAttribute fishWanted = fishNode.Attributes.Append(xmlDoc.CreateAttribute(XMLAttrWanted));
                XmlAttribute ID1Node = fishNode.Attributes.Append(xmlDoc.CreateAttribute(XMLAttrID1));
                XmlAttribute ID2Node = fishNode.Attributes.Append(xmlDoc.CreateAttribute(XMLAttrID2));
                XmlAttribute ID3Node = fishNode.Attributes.Append(xmlDoc.CreateAttribute(XMLAttrID3));
                fishName.Value = fish;
                fishWanted.Value = wanted ? Resources.Yes : Resources.No;
                ID1Node.Value = ID1;
                ID2Node.Value = ID2;
                ID3Node.Value = ID3;
                // Mark it as new, but don't add it to DB until restart or rename
                if (!fromDB)
                { // If it's not being added from the DB anyway
                    SetNew(rod, fishNode, fishNode);
                }

                fishNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, XMLNodeZones, xmlDoc.NamespaceURI));
                fishNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, XMLNodeBaits, xmlDoc.NamespaceURI));
            }

            if(null != zone)
            {
                if(null == fishNode.SelectSingleNode(string.Format(XPathFormatZoneByName, zone)))
                {
                    XmlNode zoneNode = fishNode[XMLNodeZones].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, XMLNodeZone, xmlDoc.NamespaceURI));
                    zoneNode.InnerText = zone;
                    if (!fromDB)
                    {
                        SetNew(rod, fishNode, zoneNode);
                    }
                }
            }

            if(null != bait)
            {
                if(null == fishNode.SelectSingleNode(string.Format(XPathFormatBaitByName, bait)))
                {
                    XmlNode baitNode = fishNode[XMLNodeBaits].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, XMLNodeBait, xmlDoc.NamespaceURI));
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

        internal static void ChangeName(Fishie fish, string newName, bool fromDB)
        {
            XmlDocument xmlDoc = GetFishDB(fish.rod);
            XmlNode oldFishNode = xmlDoc.SelectSingleNode(string.Format(XPathFormatFishByNameAndIDs, newName, fish.ID1, fish.ID2, fish.ID3));
            XmlNode fishNode = xmlDoc.SelectSingleNode(string.Format(XPathFormatFishByNameAndIDs, fish.name, fish.ID1, fish.ID2, fish.ID3));

            //check if there is already an entry with same ID and name = newName, if there is, merge the 2 entries
            if(null == oldFishNode)
            {
                // Not an entry with the same ID and name
                fishNode.Attributes[XMLAttrName].Value = newName;
                // Mark for adding to DB if it's a new fish being renamed
                if (null != fishNode.Attributes[XMLAttrNew])
                {
                    SetNew(fish.rod, fishNode, fishNode);
                }
                else
                {
                    SetRenamed(fish.name, fishNode);
                }
                if (!fromDB)
                {
                    FishDBChanged(fish.rod);
                }
            }
            else
            {
                //merging (union of Zones and Baits)
                XmlNodeList zones = fishNode[XMLNodeZones].ChildNodes;
                XmlNodeList baits = fishNode[XMLNodeBaits].ChildNodes;

                foreach(XmlNode zone in zones)
                {
                    if(null == oldFishNode.SelectSingleNode(string.Format(XPathFormatZoneByName, zone.InnerText)))
                    {
                        XmlNode zoneNode = oldFishNode[XMLNodeZones].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, XMLNodeZone, xmlDoc.NamespaceURI));
                        zoneNode.InnerText = zone.InnerText;
                    }
                }

                foreach(XmlNode bait in baits)
                {
                    if(null == oldFishNode.SelectSingleNode(string.Format(XPathFormatBaitByName, bait.InnerText)))
                    {
                        XmlNode baitNode = oldFishNode[XMLNodeBaits].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, XMLNodeBait, xmlDoc.NamespaceURI));
                        baitNode.InnerText = bait.InnerText;
                    }
                }

                xmlDoc[XMLNodeRod].RemoveChild(fishNode);
                if (!fromDB)
                {
                    SetRenamed(fish.name, fishNode);
                    FishDBChanged(fish.rod);
                }
            }

        } // @ internal static void ChangeName(Fishie fish, string newName)

        internal static void SetRenamed(string oldName, XmlNode fishNode)
        {
            if (fishNode.Attributes[XMLAttrRename] == null)
            {
                fishNode.Attributes.Append(fishNode.OwnerDocument.CreateAttribute(XMLAttrRename));
            }
            fishNode.Attributes[XMLAttrRename].Value = oldName;
            DBRenamedFish.Add(fishNode);
        }

        internal static void SetNew(string rod, XmlNode fishNode, XmlNode newNode)
        {
            if (newNode.Attributes[XMLAttrNew] == null)
            {
                newNode.Attributes.Append(fishNode.OwnerDocument.CreateAttribute(XMLAttrNew));
                DBNewFish.Add(fishNode);
                XmlUpdated(rod);
            }
        }

        internal static void UnsetRename(XmlNode fishNode)
        {
            if (fishNode.Attributes[XMLAttrRename] != null)
            {
                fishNode.Attributes.Remove(fishNode.Attributes[XMLAttrRename]);
                DBRenamedFish.Remove(fishNode);
            }
        }

        internal static void UnsetNew(XmlNode fishNode, XmlNode newNode)
        {
            if (newNode.Attributes[XMLAttrNew] != null)
            {
                newNode.Attributes.Remove(newNode.Attributes[XMLAttrNew]);
                DBNewFish.Remove(fishNode);
            }
        }

        internal static bool FishAccepted(out string name, out bool isNew, bool fishUnknown, string rod, string zone, string bait, string ID1, string ID2, string ID3)
        {
            XmlDocument xmlDoc = GetFishDB(rod);
            string xpathQuery = string.Format(XPathFormatFishByIDsAndZone, ID1, ID2, ID3, zone);
            XmlNode fishNode = xmlDoc.SelectSingleNode(xpathQuery);

            if(null == fishNode)
            {
                name = Resources.FishNameUnknown;
                isNew = true;

                return fishUnknown;
            }
            else
            {
                isNew = false;
                name = fishNode.Attributes[XMLAttrName].Value;

                if(null == fishNode.SelectSingleNode(string.Format(XPathFormatBaitByName, bait)))
                {
                    XmlNode newBaitNode = fishNode[XMLNodeBaits].AppendChild(xmlDoc.CreateElement(XMLNodeBait));
                    newBaitNode.InnerText = bait;
                    SetNew(rod, fishNode, newBaitNode);
                    FishDBChanged(rod);
                }

                return Resources.Yes == fishNode.Attributes[XMLAttrWanted].Value;
            }

        } // @ internal static bool FishAccepted(out string name, out bool isNew, bool fishUnknown, string rod, string zone, string bait, string ID1, string ID2, string ID3, string ID4)

        private static string GetFileName(string rod)
        {
            switch(rod)
            {
                case RodItemBamboo:
                    return FileXMLBamboo;
                case RodItemCarbon:
                    return FileXMLCarbon;
                case RodItemClothespole:
                    return FileXMLClothespole;
                case RodItemComposite:
                    return FileXMLComposite;
                case EbisuFishingRod:
                    return FileXMLEbisu;
                case RodItemFastwater:
                    return FileXMLFastwater;
                case RodItemGlassFiber:
                    return FileXMLGlass;
                case RodItemHalcyon:
                    return FileXMLHalcyon;
                case RodItemHume:
                    return FileXMLHume;
                case RodItemLuShangs:
                    return FileXMLLuShangs;
                case RodItemMMM:
                    return FileXMLMMM;
                case RodItemMithran:
                    return FileXMLMithran;
                case RodItemSH:
                    return FileXMLSH;
                case RodItemTarutaru:
                    return FileXMLTarutaru;
                case RodItemWillow:
                    return FileXMLWillow;
                case RodItemYew:
                    return FileXMLYew;
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

                    using (TextWriter writer = new StreamWriter(System.IO.File.Create(fishDBFile)))
                    {

                        try
                        {
                            writer.WriteLine(string.Format(XMLFormatRod, rod));
                            writer.Flush();
                        }
                        finally
                        {
                            writer.Close();
                        }
                    }
                }

                //xml file is ready, load it into the dictionary
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fishDBFile);
                DBByRod.Add(rod, xmlDoc);
                //Check any fish previously marked new
                foreach (XmlNode fishNode in xmlDoc.SelectNodes(XPathNewFish))
                {
                    DBNewFish.Add(fishNode);
                }
                //Check any bait or zones previously marked new
                foreach (XmlNode baitorZoneNode in xmlDoc.SelectNodes(XPathNewBaitsAndZones))
                {
                    DBNewFish.Add(baitorZoneNode.ParentNode.ParentNode);
                }
                //Check any fish marked renamed
                foreach (XmlNode fishNode in xmlDoc.SelectNodes(XPathRenamedFish))
                {
                    DBRenamedFish.Add(fishNode);
                }
            }

            return DBByRod[rod];

        } // @ private static XmlDocument GetFishDB(string rod)

        internal static Fishie[] GetFishes(string rod, string zone, string bait, bool wanted)
        {
            XmlDocument xmlDoc = GetFishDB(rod);
            string xpathQuery = string.Format(XPathFormatFishByZoneBaitAndWanted, zone, bait, wanted ? Resources.Yes : Resources.No);
            XmlNodeList nodes = xmlDoc.SelectNodes(xpathQuery);
            Fishie[] fishes = new Fishie[nodes.Count];
            int i = 0;

            foreach(XmlNode node in nodes)
            {
                fishes[i++] = new Fishie(node.Attributes[XMLAttrName].Value, rod, node.Attributes[XMLAttrID1].Value, node.Attributes[XMLAttrID2].Value, node.Attributes[XMLAttrID3].Value);
            }

            return fishes;

        } // @ internal static Fishie[] GetFishes(string rod, string zone, string bait, bool wanted)

        internal static void ToggleWanted(Fishie fish)
        {
            XmlDocument xmlDoc = GetFishDB(fish.rod);
            string xpathQuery = string.Format(XPathFormatFishByNameAndIDs, fish.name, fish.ID1, fish.ID2, fish.ID3);
            XmlNode fishNode = xmlDoc.SelectSingleNode(xpathQuery);
            fishNode.Attributes[XMLAttrWanted].Value = (Resources.Yes == fishNode.Attributes[XMLAttrWanted].Value) ? Resources.No : Resources.Yes;
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
