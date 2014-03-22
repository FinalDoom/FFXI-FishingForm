using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FFACETools;
using Fishing.Properties;

namespace Fishing
{
    public class FishUtils
    {
        #region Constructor
        private FishUtils()
        {
        }
        #endregion //Constructor

        private const string FormatFishNameMultiple = "{0} x{1}";

        /// <summary>
        /// Get a normalized name for a fish or item fished up.
        /// </summary>
        /// <remarks>Be careful modifying this, so that inputs and outputs
        /// remain consistent. These names are integral to XML arrangement
        /// and database integrity. These require that <c>Dictionaries</c>.<c>fishDictionary</c>
        /// is updated with current fish and catchable items, should the
        /// game be updated as such.</remarks>
        /// <param name="fish">String to normalize to a standard fish name.
        /// Generally this is from ingame chat and has characters that need
        /// to be removed.</param>
        /// <returns>Normalized fish name</returns>
        public static string GetFishName(string fish)
        {
            string name = fish;
            // Get a better name for the fish
            foreach (KeyValuePair<string, int> f in Dictionaries.fishDictionary)
            {
                if (-1 < fish.IndexOf(f.Key, StringComparison.OrdinalIgnoreCase))
                {
                    // Remove punctuation and articles, but don't change fish name if it's only a partial match of one of the words
                    // At least one of the words must match
                    List<string> fishNameParts = new List<string>((fish.Split(new char[3] { Resources.SpaceChar, Resources.Period, Resources.Exclamation })).AsEnumerable());
                    List<string> fishKeyParts = new List<string>((f.Key.Split(new char[1] { Resources.SpaceChar })).AsEnumerable());
                    bool found = false;
                    foreach (string p in fishKeyParts)
                    {
                        if (fishNameParts.Contains(p, StringComparer.OrdinalIgnoreCase))
                        {
                            found = true;
                            break;
                        }
                    }
                    int test;
                    if (Int32.TryParse(fishNameParts[0], out test))
                    {
                        string multiple = String.Format(FormatFishNameMultiple, f.Key, test);
                        if (Dictionaries.fishDictionary.Keys.Contains(multiple))
                        {
                            name = multiple;
                            break;
                        }
                    }
                    if (found)
                    {
                        name = f.Key;
                        break;
                    }
                }
            }

            return name;

        } // @ private string GetFishName(string fish)

        /// <summary>
        /// Get the name of a bait from its ID.
        /// </summary>
        /// <remarks>Requires that <c>Dictionaries</c>.<c>baitDictionary</c>
        /// be updated with current bait, if the game is ever updated.</remarks>
        /// <param name="id">ingame bait ID</param>
        /// <returns>String name of the bait equivalent to passed ID</returns>
        public static string GetBaitName(int id)
        {
            string name = String.Empty;
            foreach (KeyValuePair<string, int> b in Dictionaries.baitDictionary)
            {
                if (b.Value == id)
                {
                    name = b.Key;
                    break;
                }
            }
            return name;
        } // @ private string GetBaitName(ushort id)

        /// <summary>
        /// Get the name of a rod from its ID.
        /// </summary>
        /// <remarks>Requires that <c>Dictionaries</c>.<c>rodDictionary</c>
        /// be updated with current rods, if the game is ever updated.</remarks>
        /// <param name="id">ingame rod ID</param>
        /// <returns>String name of the rod equivalent to passed ID</returns>
        public static string GetRodName(int id)
        {
            string name = String.Empty;
            foreach (KeyValuePair<string, int> r in Dictionaries.rodDictionary)
            {
                if (r.Value == id)
                {
                    name = r.Key;
                    break;
                }
            }
            return name;
        } // @ private string GetRodName(ushort id)

        /// <summary>
        /// Get the name of a piece of fishing gear from its ID.
        /// </summary>
        /// <remarks>Requires that <c>Dictionaries</c>.<c>gearDictionary</c>
        /// be updated with current fishing gear, if the game is ever updated.</remarks>
        /// <param name="id">ingame gear ID</param>
        /// <returns>String name of the fishing gear equivalent to passed ID</returns>
        public static string GetGearName(int id)
        {
            string name = String.Empty;
            foreach (KeyValuePair<string, int> r in Dictionaries.gearDictionary)
            {
                if (r.Value == id)
                {
                    name = r.Key;
                    break;
                }
            }
            return name;
        } // @ private string GetGearName(ushort id)

        /// <summary>
        /// Get the string name of a zone from its FFACE <c>FFACEToos.Zone</c>.
        /// </summary>
        /// <remarks>Gets the name of a zone, resolving it from FFACE,
        /// which uses the windower resources.xml file. Contains special
        /// logic for the Selbina-Mhaura ferry, since it has 2 IDs based on
        /// if there are pirates or not. Windower does not regard the names
        /// as different, so they must have " (Pirates)" appended here for
        /// clarity and accuracy. Should other fishable dual-ID zones be
        /// added, logic should be added here.</remarks>
        /// <param name="zone">FFACE zone to resolve</param>
        /// <returns>string representation of passed FFACE zone</returns>
        public static string GetZoneName(Zone zone)
        {
            if (zone == Zone.Ferry_between_Mhaura__Selbina_Pirates || zone == Zone.Ferry_between_Selbina__Mhaura_Pirates)
            {
                string zoneName = FFACE.ParseResources.GetAreaName(zone);
                return zoneName.EndsWith(" (Pirates)") ? zoneName : zoneName + " (Pirates)";
            }
            else
            {
                return FFACE.ParseResources.GetAreaName(zone);
            }

        } // @ private string GetPlayerZoneName(Zone zone)
        /// <summary>
        /// Helper method to check if a point is displayable on a screen.
        /// </summary>
        /// <param name="thePoint">Point to check</param>
        /// <returns>true if the point is on one of the connected screens</returns>
        public static bool ThisPointIsOnOneOfTheConnectedScreens(Point thePoint)
        {
            bool FoundAScreenThatContainsThePoint = false;

            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                if (Screen.AllScreens[i].Bounds.Contains(thePoint))
                    FoundAScreenThatContainsThePoint = true;
            }
            return FoundAScreenThatContainsThePoint;
        }

        /// <summary>
        /// Get the nearest point that is displayable on a screen.
        /// </summary>
        /// <param name="target">Point to get a near point for</param>
        /// <returns>Point nearest to the passed point, displayable on a screen</returns>
        public static Point GetClosestOnScreenOffsetPoint(Point target)
        {
            double smallestDistance = Double.NaN;
            Point smallestOffset = Point.Empty;
            for (int i = 0; i < Screen.AllScreens.Length; ++i)
            {
                Rectangle screenRect = Screen.AllScreens[i].Bounds;
                int dx = 0;
                int dy = 0;
                if (target.X < screenRect.Left)
                {
                    dx = screenRect.Left - target.X;
                }
                else if (target.X > screenRect.Right)
                {
                    dx = screenRect.Right - target.X;
                }
                if (target.Y < screenRect.Top)
                {
                    dy = screenRect.Top - target.Y;
                }
                else if (target.Y > screenRect.Bottom)
                {
                    dy = screenRect.Bottom - target.Y;
                }
                Point tmpOffset = new Point(dx, dy);
                double tmpDistance = Math.Pow(dx, 2) + Math.Pow(dy, 2);
                // Get smallest offset
                if (smallestOffset == Point.Empty)
                {
                    smallestDistance = tmpDistance;
                    smallestOffset = tmpOffset;
                }
                else if (tmpDistance < smallestDistance)
                {
                    smallestDistance = tmpDistance;
                    smallestOffset = tmpOffset;
                }
            }
            return smallestOffset;
        }
    }
}
