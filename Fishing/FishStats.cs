﻿using System.Collections.Generic;
using System.Text;
using System;

namespace Fishing
{
    internal static class FishStats
    {
        #region Members

        internal delegate void FishStatsChanged();
        internal static event FishStatsChanged OnChanged;

        internal static int totalCastCount = 0;
        internal static int noCatchCount = 0;
        internal static int caughtCount = 0;
        internal static int monsterCount = 0;
        internal static int lostCatchCount = 0;
        internal static int lackSkillCount = 0;
        internal static int tooSmallCount = 0;
        internal static int tooLargeCount = 0;
        internal static int lineBreakCount = 0;
        internal static int rodBreakCount = 0;
        internal static int releasedCount = 0;

		internal static long ticksFished = 0;
		internal static long startTicks = 0;

        internal static SortedDictionary<string, int> caughtFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> lostCatchFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> lackSkillFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> tooSmallFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> tooLargeFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> lineBreakFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> rodBreakFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> releasedFishes = new SortedDictionary<string, int>();

        #endregion //Members

        #region Methods

        /// <summary>
        /// Add fish to stats dictionary and execute any change event.
        /// </summary>
        /// <param name="dictionary">Dictionary to add to</param>
        /// <param name="fish">fish name to add</param>
        internal static void AddFish(SortedDictionary<string, int> dictionary, string fish)
        {
            if(!dictionary.ContainsKey(fish))
            {
                dictionary.Add(fish, 0);
            }

            dictionary[fish]++;

            if(null != OnChanged)
            {
                OnChanged();
            }

        } // @ internal static void AddFish(SortedDictionary<string, int> dictionary, string fish)

        /// <summary>
        /// Clear the fish stats
        /// </summary>
        internal static void Clear()
        {
            totalCastCount = 0;
            noCatchCount = 0;
            caughtCount = 0;
            monsterCount = 0;
            lostCatchCount = 0;
            lackSkillCount = 0;
            tooSmallCount = 0;
            tooLargeCount = 0;
            lineBreakCount = 0;
            rodBreakCount = 0;
            releasedCount = 0;

			ticksFished = 0;
            startTicks = DateTime.Now.Ticks;

            caughtFishes.Clear();
            lostCatchFishes.Clear();
            lackSkillFishes.Clear();
            tooSmallFishes.Clear();
            tooLargeFishes.Clear();
            lineBreakFishes.Clear();
            rodBreakFishes.Clear();
            releasedFishes.Clear();

        } // @ internal static void Clear()

        /// <summary>
        /// Get a percentage string from passed values.
        /// </summary>
        /// <param name="value">numerator in fractional value</param>
        /// <param name="total">total/denominator in fractional value</param>
        /// <param name="places">decimal places to format string to</param>
        /// <returns>string percentage value</returns>
        private static string GetPercentage(int value, int total, int places)
        {
            decimal percent;
            string stringPercentage = string.Empty;
            string stringPlaces = new string('0', places);

            if((0 == value) || (0 == total))
            {
                percent = 0;
            }
            else
            {
                percent = decimal.Divide(value, total) * 100;

                if(0 < places)
                {
                    stringPlaces = "." + stringPlaces;
                }
            }

            stringPercentage = percent.ToString("#" + stringPlaces);

            return stringPercentage;

        } // @ private static string GetPercentage(int value, int total, int places)

        /// <summary>
        /// Get a formatted output of all fish stats.
        /// </summary>
        /// <returns>Formatted stats string</returns>
        internal static string PrintStats()
        {
            // stats log is built from scratch each time PrintLog() is called 
            StringBuilder stats = new StringBuilder();

            stats.AppendLine(@"{\rtf1\ansi\deff0{\fonttbl{\f0\fswiss Microsoft Sans Serif;}}{\colortbl;}\uc1\pard\f0\fs14");
            stats.AppendLine(@"\b Total casts:  " + totalCastCount + @"\b0\par");

			if(ticksFished > 0 || startTicks > 0)
			{
				stats.AppendLine("   Time fished : " + getTimeFished() + @"\par");
				stats.AppendLine("   Catches per hour : " + getCatchesPerHour() + @"\par\par");
			}
            if(noCatchCount > 0)
            {
                stats.AppendLine(@"\b No catch  ( " + noCatchCount + " / " + 
                            GetPercentage(noCatchCount, totalCastCount, 1) + @"% )\b0\par\par");
            }
            if(caughtCount > 0)
            {
                stats.AppendLine(StatsSection("Caught", caughtCount, caughtFishes));
            }
            if(lostCatchCount > 0)
            {
                stats.AppendLine(StatsSection("Lost catch", lostCatchCount, lostCatchFishes));
            }
            if(lackSkillCount > 0)
            {
                stats.AppendLine(StatsSection("Lack skill", lackSkillCount, lackSkillFishes));
            }
            if(tooSmallCount > 0)
            {
                stats.AppendLine(StatsSection("Too small", tooSmallCount, tooSmallFishes));
            }
            if(tooLargeCount > 0)
            {
                stats.AppendLine(StatsSection("Too large", tooLargeCount, tooLargeFishes));
            }
            if(lineBreakCount > 0)
            {
                stats.AppendLine(StatsSection("Line break", lineBreakCount, lineBreakFishes));
            }
            if(rodBreakCount > 0)
            {
                stats.AppendLine(StatsSection("Rod break", rodBreakCount, rodBreakFishes));
            }
            if(releasedCount > 0)
            {
                stats.AppendLine(StatsSection("Released", releasedCount, releasedFishes));
            }
            if(monsterCount > 0)
            {
                stats.AppendLine("Monster  ( " + monsterCount + " / " + GetPercentage(monsterCount, totalCastCount, 1) + "% )");
            }

            stats.AppendLine(@"}");
            
            return stats.ToString();

        } // @ internal static string PrintStats()

        /// <summary>
        /// Create a formatted section for the stats output
        /// </summary>
        /// <param name="header">section header</param>
        /// <param name="count">section count</param>
        /// <param name="dictionary">dictionary of sub elements</param>
        /// <returns>Formatted string with section stats</returns>
        private static string StatsSection(string header, int count, SortedDictionary<string, int> dictionary)
        {
            if(dictionary.Count > 0)
            {
                StringBuilder statsSection = new StringBuilder();
                header = header + "  ( " + count + " / " + GetPercentage(count, totalCastCount, 1) + "% )";

                statsSection.AppendLine(@"\b " + header + @"\b0\par");

                foreach (KeyValuePair<string, int> fishStats in dictionary)
                {
                    string items = "   " + fishStats.Key + " : " + fishStats.Value;
                    statsSection.AppendLine(items + @"\par");
                }

                statsSection.AppendLine(@"\par");

                return statsSection.ToString();
            }

            return string.Empty;

        } // @ private static string StatsSection(string header, int count, SortedDictionary<string, int> dictionary)

        /// <summary>
        /// Get a string formatted that displays the amount of time fished.
        /// </summary>
        /// <returns>string formatted of amount of time fished</returns>
        internal static string getTimeFished()
        {
            TimeSpan ts;
            if (startTicks > 0)
            {
                ts = TimeSpan.FromTicks(ticksFished + DateTime.Now.Ticks - startTicks);
            }
            else
            {
                ts = TimeSpan.FromTicks(ticksFished);
            }
            string timeText = "";
            if (ts.Hours > 0)
            {
                timeText = ts.Hours + " hour";
                if (ts.Hours != 1)
                {
                    timeText += "s";
                }
                if (ts.Minutes > 0)
                {
                    timeText += ", ";
                }
            }
            if (ts.Minutes > 0)
            {
                timeText += ts.Minutes + " minute";
                if (ts.Minutes != 1)
                {
                    timeText += "s";
                }
                if (ts.Hours == 0)
                {
                    timeText += ", ";
                }
            }
            if (ts.Hours == 0)
            {
                if (ts.Seconds == 1)
                {
                    timeText += "1 second";
                }
                else
                {
                    timeText += ts.Seconds + " seconds";
                }
            }
            return timeText;
        }

        /// <summary>
        /// Get a string showing the number of catches per hour
        /// </summary>
        /// <returns>a string showing the number of catches per hour</returns>
        internal static string getCatchesPerHour()
		{
			TimeSpan ts;
			if (startTicks > 0)
			{
				ts = TimeSpan.FromTicks(ticksFished + DateTime.Now.Ticks - startTicks);
			}
			else
			{
				ts = TimeSpan.FromTicks(ticksFished);
			}
			return string.Format("{0:0.0}", (double)caughtCount / ts.TotalHours);
		}

        #endregion //Methods
    }
}
