using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using FFACETools;
using Fishing.Properties;

namespace Fishing
{
    internal partial class FishingForm : Form
    {
        #region Constants

        private static readonly string[] MessageErrorFFACEVersion = {
            "This program uses FFACE v4.0.0.9 or higher!",
            "",
            "Please download the latest version of FFACE and put it in this program's directory."
        };

        private static readonly string[] MessageVersionUpdate = {
            "A new version of FishingForm is available.",
            "Check https://bitbucket.org/FinalDoom/ffxi-fishingform/downloads ",
            "or http://www.ffevo.net/files/file/214-fishingform-fd-edition/ for the new version."
        };

        private static readonly string[] MessageInventoryOther = {
            "NOTE: You must enter the command below. {0} will be replaced with the any fish in the wanted list's name (as long as it's similar to the ingame name) in quotes, e.g.:",
            "",
            "    /echo Caught too many {0}!",
            "",
            "You may also enter several commands, separated by semicolons or newlines. Fishing will attempt to resume after the number of seconds indicated in the number box to the right times the number of fish in the wanted list."
        };

        private static readonly string[] MessageBaitOther = {
            "NOTE: You must enter the command below. {0} will be replaced with the bait name in quotes, e.g.:",
            "",
            "    /echo Out of {0}!",
            "",
            "You may also enter several commands, separated by semicolons or newlines. Fishing will attempt to resume after the number of seconds indicated in the number box to the right."
        };

        private const string DllNameFFACE = "FFACE.dll";
        private const string DllNameHook = "hook.dll";
        private const string DllNameItemizer = "itemizer.dll";
        private const string DllNameItemTools = "itemtools.dll";
        private const string FileWarningWav = "warning.wav";
        private const string CommandCastSneak = "/ma Sneak <me>";
        private const string CommandChatSay = "/s ";
        private const string CommandChatLinkshell = "/l ";
        private const string CommandChatParty = "/p ";
        private const string CommandChatReply = "/t <r> ";
        private const string CommandChatTell = "/t ";
        private const string CommandFish = "/fish";
        private const string CommandFormatItemMe = "/item \"{0}\" <me>";
        private const string CommandCancelStatus = "//cancel {0}";
        private const string CommandWarp = "/ma \"Warp\" <me>";
        private const string CommandLogout = "/logout";
        private const string CommandShutdown = "/shutdown";
        private const string EquipFormatRod = "/equip range \"{0}\"";
        private const string EquipFormatBait = "/equip ammo \"{0}\"";
        private const string EquipFormatBody = "/equip body \"{0}\"";
        private const string EquipFormatHands = "/equip hands \"{0}\"";
        private const string EquipFormatLegs = "/equip legs \"{0}\"";
        private const string EquipFormatFeet = "/equip feet \"{0}\"";
        private const string EquipFormatHead = "/equip head \"{0}\"";
        private const string EquipFormatNeck = "/equip neck \"{0}\"";
        private const string EquipFormatWaist = "/equip waist \"{0}\"";
        private const string EquipFormatLRing = "/equip ring1 \"{0}\"";
        private const string EquipFormatRRing = "/equip ring2 \"{0}\"";
        private const string FormatTimestampDB = "[hh:mm:ss] ";
        private const string FormatTimestampVanaMinute = "00";
        private const string FormatTimestampEarthTime = "MMM. d, yyyy h:mm:ss tt";
        private const string FormatProgramTitleNoChar = "FishingForm v{0}-mC-FD";
        private const string FormatProgramTitleLoggedIn = "FishingForm v{0}-mC-FD  ({1})";
        private const string FormatFishHP = "{0}/{1} [{2}s]";
        private const string FormatFishNameMultiple = "{0} x{1}";
        private const string GUIChatDetectButtonAdd = "+";
        private const string GUIChatDetectButtonRemove = "-";
        private const string GUIFormatNoCatch = "{0} / {1}";
        private const string GUILblBlank = "--";
        private const string GUIFormatLblBait = "{0} [{1}]";
        private const string GUIFormatVanaTime = "{0}/{1}/{2}, {3}, {4}:{5}, {6} Moon ({7}%)";
        private const string GUIFormatLblSkillDecimal = " (.{0})";
        private const string GUIFormatLblSkillDecimalRange = " (.{0} - .{1})";
        private const string GUILblNA = "N/A";
        private const string GUIFormatLblInventory = "{0} / {1}";
        private const string GUIFormatLblGil = "{0:#,#}";
        private const string GUIFormatLblVanaClock = "{0}:{1}";
        private const string GUIButtonResizeBig = ">";
        private const string GUIButtonResizeSmall = "<";
        private const string ProcessPOLName = "pol";
        internal const string ProgramExeName = "Fishing.exe";
        private const string RegexAllSpaces = @"^[\s]+$";
        private const string RegexChatIncomingTell = @"^\w+>> ";
        private const string RegexChatMode = @"(?:/(?:[slpt]|say|linkshell|party|tell)\b(?: <r>)? )?(.*)";
        private const string RegexFormatChatLinkshell = @"^<(?!{0})\w+> ";
        private const string RegexFormatChatParty = @"^\((?!{0})\w+\) ";
        private const string RegexFormatChatSay = @"^(?!{0})\w+ :";
        private const string TestChatGM = "[GM";

        #endregion //Constants

        #region Members

        internal static FFACE _FFACE { get; private set; }

		private static FFACE.PlayerTools _Player;

        private static ProcessSelector.POLProcess _Process { get; set; }
        private static Random rnd = new Random();
        private static ThreadStart ts;
        private static Thread workerThread;
        private static SizeF currentScaleFactor = new SizeF(1f, 1f);
        private static FishingFormDBLogger DBLogger;
        private static bool ItemizerAvailable = false;
        private static bool ItemToolsAvailable = false;

		private static int skillLast = 0;
		private static int skillDecimalMin = 0;
		private static int skillDecimalMax = 0;
        private static int skillDecimalTotal = 0;
		private static int skillLevel = 0;
        private static int consecutiveNoCatchCount = 0;
        private static int consecutiveCatchCount = 0;
        private static string currentFish = string.Empty;
        private static string LastRodName;
        private static string LastBaitName;
        private static Status currentStatus;
        private static Zone currentZone;
        private static bool stopSound = false;
        private static bool statusWarningColor = false;
        private static bool chatbig = false;
        private static bool OptionsConfigured = false;

        private static List<Label> chatDetectLblOnList = new List<Label>();
        private static List<ComboBox> chatDetectCmbChatTypeList = new List<ComboBox>();
        private static List<Label> chatDetectLblReceivedList = new List<Label>();
        private static List<ComboBox> chatDetectCmbChatActionList = new List<ComboBox>();
        private static List<Button> chatDetectBtnChatRemoveList = new List<Button>();

        private static int tellActions = 0;
        private static int partyActions = 0;
        private static int shellActions = 0;
        private static int sayActions = 0;

        private static Regex playerChatLinkshell;
        private static Regex playerChatTell = new Regex(RegexChatIncomingTell);
        private static Regex playerChatParty;
        private static Regex playerChatSay;
        private static Regex chatMode = new Regex(RegexChatMode);

        internal enum FishResult
        {
            Error,
            InventoryProblem,
            LackSkill,
            LineBreak,
            LostCatch,
            Monster,
            // TODO Use this Quest value somewhere. It could be useful
            Quest,
            TooLarge,
            TooSmall,
            Released,
            RodBreak,
            Success,
            Zoned
        }

        private enum FishSize
        {
            Large,
            Small
        }

        private enum ChatAction
        {
            Stop = 1,
            Note = 2,
            Flash = 4
        }

        #endregion //Members

        #region Constructor/Destructor

        internal FishingForm(string[] arglist)
        {
            InitializeComponent();

            RestoreLocation();

            List<string> args = arglist.ToList();  //*golfandsurf*  Formats <player> arg to match pol process
            args[0] = args[0].Substring(0, 1).ToUpper() + args[0].Substring(1, args[0].Length - 1).ToLower();

			ChooseProcess(args[0]);

            if (args.Count > 1)
            {
                if (args[1].ToLower() == Resources.ArgumentStart) { btnStart_Click(btnStart, MouseEventArgs.Empty); }
            }

            #region FormElements

            timer.Enabled = true;

            toolTip.SetToolTip(btnRefreshLists, "Refreshes the Wanted / Unwanted lists from your database, based on currently equipped rod / bait / zone." + Environment.NewLine + "If there are no entries after pressing this button, your database has no entries for the current combination.");
            toolTip.SetToolTip(btnCastReset, "Reset cast wait to 3.0/3.5. This is a quick reset if you become fatigued and rezone.");
            toolTip.SetToolTip(btnResize, "Resize chat log to fill dialog, click again to restore (will automatically restore if fishing terminates).");
            toolTip.SetToolTip(cbAlwaysOnTop, "Set dialog to always visible.");
            toolTip.SetToolTip(cbCatchUnknown, "Catch any unknown fish, those not shown in either the Wanted or Unwanted lists.");
            toolTip.SetToolTip(cbReleaseLarge, "Fake fight unwanted large fish.");
            toolTip.SetToolTip(cbReleaseSmall, "Fake fight unwanted small fish.");
            toolTip.SetToolTip(lblCastWait, "Seconds until recasting after cast last attempt, time increases by 1 second when needed.");
            toolTip.SetToolTip(cbReleaseLarge, "Sets the lower and upper HP% for randomly releasing a large fish.");
            toolTip.SetToolTip(cbReleaseSmall, "Sets the lower and upper HP% for randomly releasing a small fish.");
            toolTip.SetToolTip(lblMaxNoCatch, "Maximum number of no catch casts before stopping.");
            toolTip.SetToolTip(lblNoCatchAtHeader, "Displays how many times you have not caught anything in a row.");
            toolTip.SetToolTip(trackOpacity, "This bar controls the transparency of the dialog between 10% and 99%.");
            toolTip.SetToolTip(cbReaction, "Delay before starting to fight a caught fish.");
            toolTip.SetToolTip(lblUnwantedHeader, "A list of the known fish you do not want to catch for your current bait / rod / zone combination.");
            toolTip.SetToolTip(lblWantedHeader, "A list of the known fish you want to catch for your current bait / rod / zone combination.");
            toolTip.SetToolTip(cbExtend, "Extends time to reel-in by 30 seconds.");
            toolTip.SetToolTip(cbAutoKill, "Sets fish HP to 0 automatically when there's 5 seconds left on the line.");
            toolTip.SetToolTip(cbQuickKill, "Automatically kills fish after [#] of seconds elapse with fish on the line.");
            toolTip.SetToolTip(cbIgnoreItem, "Ignore and release all items.");
            toolTip.SetToolTip(cbIgnoreMonster, "Ignore and release all monsters.");
            toolTip.SetToolTip(cbIgnoreSmallFish, "Ignore and release all small fish.");
            toolTip.SetToolTip(cbIgnoreLargeFish, "Ignore and release all large fish.");
            toolTip.SetToolTip(tbRodGear, "Automatically equip rod when none equipped or when one breaks. Will use currently equipped rod if not specified.");
            toolTip.SetToolTip(tbBaitGear, "Automatically equip rod when none equipped or when exhausted. Will use currently equipped bait if not specified.");
			toolTip.SetToolTip(cbLRingGear, "Automatically use left ring enchantment.");
			toolTip.SetToolTip(cbRRingGear, "Automatically use right ring enchantment.");
			toolTip.SetToolTip(cbWaistGear, "Automatically use belt enchantment.");
            toolTip.SetToolTip(cbGMdetectAutostop, "Issue STOP on detection of a GM.");
            toolTip.SetToolTip(cbBaitActionShutdown, "Shut down when out of bait, unless above command finds bait.");
            toolTip.SetToolTip(cbBaitActionLogout, "Log out when out of bait, unless above command finds bait.");
            toolTip.SetToolTip(cbBaitActionWarp, "Warp when out of bait, unless above command finds bait.");
            toolTip.SetToolTip(cbBaitItemizerSack, "Fetch bait from sack.");
            toolTip.SetToolTip(cbBaitItemizerSatchel, "Fetch bait from satchel.");
            toolTip.SetToolTip(cbBaitItemizerItemTools, "Enables Itemizer plugin support to automatically grab current bait when none is found in inventory. This will attempt to fetch bait to prevent any warp, logout, or shutdown action.");
            toolTip.SetToolTip(cbBaitactionOther, "Execute below command when out of bait. Wait for the number of seconds to the right. Warp, logout, or shutdown will occur if bait is still not found after executing this command.");
            toolTip.SetToolTip(numBaitactionOtherTime, "Number of seconds to wait for command to execute.");
			toolTip.SetToolTip(cbFatiguedActionShutdown, "Shut down when catch limit is reached.");
			toolTip.SetToolTip(cbFatiguedActionLogout, "Log out when catch limit is reached.");
			toolTip.SetToolTip(cbFatiguedActionWarp, "Warp when catch limit is reached.");
            toolTip.SetToolTip(cbStopSound, "When fishing terminates unexpectedly, play warning.wav sound.");
            toolTip.SetToolTip(cbTellDetect, "Changes status bar color and Tell tab name when receiving a message.");
            toolTip.SetToolTip(cbFullactionShutdown, "Shut down when inventory is full.");
            toolTip.SetToolTip(cbFullactionLogout, "Log out when inventory is full.");
            toolTip.SetToolTip(cbFullactionWarp, "Warp when inventory is full.");
            toolTip.SetToolTip(cbFullActionStop, "Stop fishing when inventory is full. If disabled, fishing will continue, but no shutdown, logout, or warp will occur.");
            toolTip.SetToolTip(cbInventoryItemizerSack, "Put fish in sack.");
            toolTip.SetToolTip(cbInventoryItemizerSatchel, "Put fish in satchel.");
            toolTip.SetToolTip(cbInventoryItemizerItemTools, "Enables Itemizer plugin support to automatically store fish when inventory is full.");
            toolTip.SetToolTip(cbFullactionOther, "Execute below command when inventory is full. Wait for the number of seconds to the right for each fish in the wanted list.");
            toolTip.SetToolTip(numFullactionOtherTime, "Number of seconds to wait for command to execute.");
            toolTip.SetToolTip(cbMaxCatch, "Stops fishing when # of catches reached; value resets when limit is reached.");
            toolTip.SetToolTip(cbSneakFishing, "Will cast the spell Sneak prior to casting.");
            toolTip.SetToolTip(cbSkillCap, "Stop when skill reaches specified level.");
            toolTip.SetToolTip(cbChatDetect, "Uncheck to disable all chat detectors set below.");

            tbBaitGear.Items.AddRange(Dictionaries.baitList.ToArray());
            tbRodGear.Items.AddRange(Dictionaries.rodList.ToArray());
            tbBodyGear.Items.AddRange(Dictionaries.gearList.GetRange(Dictionaries.bodyIndex, Dictionaries.bodyCount).ToArray());
            tbHandsGear.Items.AddRange(Dictionaries.gearList.GetRange(Dictionaries.handsIndex, Dictionaries.handsCount).ToArray());
            tbLegsGear.Items.AddRange(Dictionaries.gearList.GetRange(Dictionaries.legsIndex, Dictionaries.legsCount).ToArray());
            tbFeetGear.Items.AddRange(Dictionaries.gearList.GetRange(Dictionaries.feetIndex, Dictionaries.feetCount).ToArray());
            tbHeadGear.Items.AddRange(Dictionaries.gearList.GetRange(Dictionaries.headIndex, Dictionaries.headCount).ToArray());
            tbNeckGear.Items.AddRange(Dictionaries.gearList.GetRange(Dictionaries.neckIndex, Dictionaries.neckCount).ToArray());
            tbWaistGear.Items.AddRange(Dictionaries.gearList.GetRange(Dictionaries.waistIndex, Dictionaries.waistCount).ToArray());
            tbLRingGear.Items.AddRange(Dictionaries.gearList.GetRange(Dictionaries.ringsIndex, Dictionaries.ringsCount).ToArray());
            tbRRingGear.Items.AddRange(Dictionaries.gearList.GetRange(Dictionaries.ringsIndex, Dictionaries.ringsCount).ToArray());

            FishDB.OnChanged += new FishDB.DBChanged(PopulateLists);
            FishStats.OnChanged += new FishStats.FishStatsChanged(UpdateStats);

            DBLogger = new FishingFormDBLogger(this);
            FishSQL.StatusDisplay = DBLogger;
            Thread databaseInitThread = new Thread(new ThreadStart(CheckDatabase));
            databaseInitThread.IsBackground = true;
            databaseInitThread.Start();

            #endregion //FormElements
        }
        ~FishingForm()
        {
            timer.Enabled = false;

            if (null != workerThread)
            {
                WinClear();
                workerThread.Abort();
                while (workerThread.IsAlive) { Thread.Sleep(100); }
                workerThread = null;
            }

            _FFACE = null;
            _Player = null;
            FishSQL.CloseConnection();
            FishSQL.CloseAllConnections();
        }
        #endregion //Constructor/Destructor

        #region Methods

        #region Methods_Form_Overrides

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);

            //Record the running scale factor used
            currentScaleFactor = new SizeF(
               currentScaleFactor.Width * factor.Width,
               currentScaleFactor.Height * factor.Height);
        }

        #endregion //Methods_Form_Overrides

        #region Methods_Initialization

        private void RestoreLocation()
        {
            if (Point.Empty == Settings.Default.WindowLocation)
            {
                this.Location = GetNearestConnectedScreenWindowLocation();
            }
        }

        private void CheckDatabase()
        {
            try
            {
                if (!FishSQL.OpenConnection())
                {
                    return;
                }
                while (!DBLogger.StartDBTransaction(Resources.MessageDBSyncStart))
                {
                    Thread.Sleep(250);
                }
                if (!FishSQL.IsProgramUpdated())
                {
                    MessageBox.Show(string.Join(Environment.NewLine, MessageVersionUpdate));
                    foreach (string s in MessageVersionUpdate)
                    {
                        UpdateDBLog(s);
                    }
                }

                try
                {
                    FishDB.GetUpdates();
                }
                catch (Exception e)
                {
                    DBLogger.Error(Resources.MessageDBErrorXML);
                    DBLogger.Info(e.ToString());
                }
                try{
                    FishSQL.DoUploadFish();
                }
                catch (Exception e)
                {
                    DBLogger.Error(Resources.MessageDBErrorUpload);
                    DBLogger.Info(e.ToString());
                }
                try{
                    FishSQL.DoDownloadFish();
                }
                catch (Exception e)
                {
                    DBLogger.Error(Resources.MessageDBErrorDownload);
                    DBLogger.Info(e.ToString());
                }
            }
            catch (Exception e)
            {
                DBLogger.Error(Resources.MessageDBErrorGeneral);
                DBLogger.Info(e.ToString());
            }
            finally
            {
                FishSQL.CloseConnection();
                DBLogger.EndDBTransaction(Resources.MessageDBSyncFinish);
            }
        }

		private void ChooseProcess(string characterName)
		{
            using (ProcessSelector ChooseProcess = new ProcessSelector())
            {
                if (characterName != null && characterName != Resources.ArgumentNoArgs)
                {
                    for (int i = 0; i <= Process.GetProcessesByName(ProcessPOLName).Length; i++)
                    {
                        if (Process.GetProcessesByName(ProcessPOLName)[i].MainWindowTitle == characterName)
                        {
                            ChooseProcess.ThisProcess = new ProcessSelector.POLProcess(Process.GetProcessesByName(ProcessPOLName)[i].Id, Process.GetProcessesByName(ProcessPOLName)[i].MainWindowTitle);
                            break;
                        }
                    }
                }
                if (!ChooseProcess.error && null == ChooseProcess.ThisProcess)
                {
                    ChooseProcess.ShowDialog();
                }
				if (null == ChooseProcess.ThisProcess)
				{
					_FFACE = null;
					_Player = null;
                    FileVersionInfo ver = FileVersionInfo.GetVersionInfo(ProgramExeName);
                    this.Text = string.Format(FormatProgramTitleNoChar, ver.FileVersion);
					_Process = null;
					return;
				}

                try   //if you can't create an instance, there's probably no FFACE.dll, or an old FFACE version
                {
                    _FFACE = new FFACE(ChooseProcess.ThisProcess.POLID);
                    _Player = new FFACE.PlayerTools(_FFACE._InstanceID);
                    playerChatLinkshell = new Regex(string.Format(RegexFormatChatLinkshell, _Player.Name));
                    playerChatParty = new Regex(string.Format(RegexFormatChatParty, _Player.Name));
                    playerChatSay = new Regex(string.Format(RegexFormatChatSay, _Player.Name));
                    FileVersionInfo ver = FileVersionInfo.GetVersionInfo(ProgramExeName);
                    this.Text = string.Format(FormatProgramTitleLoggedIn, ver.FileVersion, ChooseProcess.ThisProcess.POLName);

                    //windower path
                    //_FFACE = new FFACE((int)PID);
                    Process pol = Process.GetProcessById((int)ChooseProcess.ThisProcess.POLID);
                    foreach (ProcessModule mod in pol.Modules)
                    {
                        if (mod.ModuleName.ToLower() == DllNameHook)
                        {
                            FFACE.WindowerPath = Path.Combine(Path.GetDirectoryName(mod.FileName), Resources.PathWindowerResourcesFolder);
                            break;
                        }
                    }
                    foreach (ProcessModule mod in pol.Modules)
                    {
                        if (mod.ModuleName.ToLower() == DllNameItemizer)
                        {
                            ItemizerAvailable = true;
                            break;
                        }
                        if (mod.ModuleName.ToLower() == DllNameItemTools)
                        {
                            ItemToolsAvailable = true;
                            break;
                        }
                    }
                    foreach (ProcessModule mod in pol.Modules)
                    {
                        if (mod.ModuleName.ToLower() == DllNameItemizer)
                        {
                            ItemizerAvailable = true;
                            break;
                        }
                        if (mod.ModuleName.ToLower() == DllNameItemTools)
                        {
                            ItemToolsAvailable = true;
                            break;
                        }
                    }
                    if (String.IsNullOrEmpty(FFACE.WindowerPath))
                    {
                        FFACE.WindowerPath = Resources.PathWindowerResourcesError;
                    }

                }
                catch (DllNotFoundException)   //occurs when FFACE.dll cannot be found
                {
                    if (File.Exists(Path.Combine(Application.StartupPath, DllNameFFACE)))
                    {
                        MessageBox.Show(Resources.MessageErrorAdministratorNeeded, Resources.MessageTitleError);
                    }
                    else
                    {
                        MessageBox.Show(Resources.MessageErrorFFACEMissing, Resources.MessageTitleError);
                    }
                    Environment.Exit(0);
                }
                catch (EntryPointNotFoundException)   //occurs when 'CreateInstance' entry point in FFACE.dll cannot be found
                {
                    MessageBox.Show(string.Join(Environment.NewLine, MessageErrorFFACEVersion), Resources.MessageTitleError);
                    Environment.Exit(0);
                }

                _Process = ChooseProcess.ThisProcess;  //store instanced property for possible need to Reattach()

            }
		}

		#endregion

        #region Methods_Fishing

        #region Methods_Fishing_Major

        private void BackgroundFishing()
        {
            while (_FFACE.Player.Zone == currentZone)
            {
                if (((int)numMaxNoCatch.Value < consecutiveNoCatchCount))
                {
                    SetNoCatch(consecutiveNoCatchCount = 0);
                    Fatigued(Resources.StatusFatigueNoCatches);
                    return;
                }

                // Stop at skill level
                if (cbSkillCap.Checked && numSkillCap.Value <= Math.Max(_FFACE.Player.GetCraftDetails(Craft.Fishing).Level, skillLevel))
                {
                    Stop(false, Resources.StatusErrorSkillCapped);
                    return;
                }

				if (Status.Fishing != currentStatus && Status.FishBite != currentStatus && Status.LostCatch != currentStatus)
				{
					decimal randomCast = Math.Round((decimal)rnd.NextDouble() * (numCastIntervalHigh.Value - numCastIntervalLow.Value), 1);
					decimal castWait = numCastIntervalLow.Value + randomCast;
					SetStatus(string.Format(Resources.StatusFormatCastingSeconds, castWait));
                    SetLblHP(string.Empty);
					Thread.Sleep((int)(castWait * 1000));
				}

                Fish();
            }

            Stop(true, Resources.StatusErrorZoneChanged);

        } // @ private void BackgroundFishing()

        private void RetrieveBait(string bait)
        {
            if (cbBaitItemizerItemTools.Checked)
            {
                string baitLocation;
                if (cbBaitItemizerSack.Checked && _FFACE.Item.GetSackItemCount((ushort)Dictionaries.baitDictionary[bait]) > 0)
                {
                    baitLocation = "sack";
                }
                else if (cbBaitItemizerSatchel.Checked && _FFACE.Item.GetSatchelItemCount((ushort)Dictionaries.baitDictionary[bait]) > 0)
                {
                    baitLocation = "satchel";
                }
                else
                {
                    return;
                }
                bait = string.Format(Resources.FormatQuoteArg, bait);
                if (ItemizerAvailable)
                {
                    _FFACE.Windower.SendString(string.Format("/gets {0} {1}", bait, baitLocation));
                    Thread.Sleep(1000); //pause to give the game time to move bait
                }
                else if (ItemToolsAvailable)
                {
                    _FFACE.Windower.SendString(string.Format("/moveitem {0} {1} inventory 99", bait, baitLocation));
                    Thread.Sleep(1000); //pause to give the game time to move bait
                }
            }
            else if (cbBaitactionOther.Checked && !string.IsNullOrEmpty(tbBaitactionOther.Text))
            {
                bait = string.Format(Resources.FormatQuoteArg, bait);
                foreach (string command in tbBaitactionOther.Text.Split(new String[] {Resources.Semicolon, Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
                {
                    _FFACE.Windower.SendString(string.Format(command, bait));
                    Thread.Sleep((int)(numBaitactionOtherTime.Value * 1000)); //pause to give the game time to execute commands
                }
            }
        }

        private bool IsRodBaitSet()
        {
            if (_FFACE == null)
            {
                return false;
            }
            string bait = tbBaitGear.Text;
            if (string.IsNullOrEmpty(bait))
            {
                bait = GetBaitName(_FFACE.Item.GetEquippedItemID(EquipSlot.Ammo));
            }
            string rod = tbRodGear.Text;
            if (string.IsNullOrEmpty(rod))
            {
                rod = GetRodName(_FFACE.Item.GetEquippedItemID(EquipSlot.Range));
            }
            currentZone = _FFACE.Player.Zone;

            if ((!string.IsNullOrEmpty(rod)) && (!string.IsNullOrEmpty(bait)))
            {
                SetBait(bait);
                SetRod(rod);
                SetLblZone(GetZoneName(currentZone));

                return true;
            }
            else
            {
                LastBaitName = LastRodName = string.Empty;
                SetLblZone(string.Empty);
                Stop(false, Resources.StatusErrorNoBaitOrRodEquipped);
                ClearLists();

                return false;
            }
        } // @ private bool RodBaitEquipped()

        private bool IsRodBaitEquipped()
        {
            if (_FFACE == null)
            {
                return false;
            }
            string bait = GetBaitName(_FFACE.Item.GetEquippedItemID(EquipSlot.Ammo));
            string rod = GetRodName(_FFACE.Item.GetEquippedItemID(EquipSlot.Range));

            if ((!string.IsNullOrEmpty(rod)) && (!string.IsNullOrEmpty(bait)))
            {
                SetBait(bait);
                SetRod(rod);

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckRodAndBait()
        {
            string strZone = GetZoneName(_FFACE.Player.Zone);
            string strBait = LastBaitName;
            string strRod = LastRodName;
            string rod = GetRodName(_FFACE.Item.GetEquippedItemID(EquipSlot.Range));
            string bait = GetBaitName(_FFACE.Item.GetEquippedItemID(EquipSlot.Ammo));
            string strRodEquipMessage = string.Format(EquipFormatRod, LastRodName);
            string strBaitEquipMessage = string.Format(EquipFormatBait, LastBaitName);

            // Just starting or zoned
            if ((string.IsNullOrEmpty(lblZone.Text)) || (lblZone.Text != strZone))
            {
                SetLblZone(strZone);
                PopulateLists();
            }

            // No rod or bait equipped. Try equipping
            if (string.IsNullOrEmpty(rod) || string.IsNullOrEmpty(bait) || LastBaitName != bait || LastRodName != rod)
            {
                _FFACE.Windower.SendString(strRodEquipMessage);
                _FFACE.Windower.SendString(strBaitEquipMessage);
                Thread.Sleep(1500);
            }

            if (IsRodBaitEquipped())  //check to see if bait/rod changed since last loop
            {
                if ((LastBaitName != strBait) || (LastRodName != strRod) || (lblZone.Text != strZone))
                {
                    PopulateLists();
                }
            }
            else  //if IsRodBaitEquipped returns false, most likely out of bait, try to get it from sack/satchel with itemizer/itemtools
            { //if that doesn't work, return false
                RetrieveBait(LastBaitName);
                _FFACE.Windower.SendString(strRodEquipMessage);
                _FFACE.Windower.SendString(strBaitEquipMessage);
                Thread.Sleep(1500);  //pause to give the game time to equip bait

                if (!IsRodBaitEquipped())
                {
                    return false;
                }
            }
            return true;
        }

        private void Cast()
        {
            if (IsSneakEnabled())
            {
                if (!IsStatusEffectActive(StatusEffect.Sneak))
                {
                    // Check to make sure we have enough MP
                    if (_FFACE.Player.MPCurrent >= 12)
                    {
                        _FFACE.Windower.SendString(CommandCastSneak);
                        Thread.Sleep(500);
                        // While we are casting, sleep the thread.
                        while (_FFACE.Player.CastPercentEx < 95)
                        {
                            Thread.Sleep(100);
                        }
                        // Give it time to finish casting animation
                        Thread.Sleep(2000);
                        GearUp();
                    }
                    else
                    {
                        Stop(false, Resources.StatusErrorSneakLackMP);
                        return;
                    }
                }
            }

            if (!CheckRodAndBait())
            {
                Stop(false, Resources.StatusErrorNoBait);
                return;
            }

            uint baitLeft = _FFACE.Item.GetInventoryItemCount((ushort) _FFACE.Item.GetEquippedItemID(EquipSlot.Ammo));

			CheckEnchantment();
            SetStatus(string.Format(Resources.StatusFormatCastingBait, LastBaitName, baitLeft));

            _FFACE.Windower.SendString(CommandFish);

        }

        /// <summary>
        /// Check if sneak option is enabled in UI
        /// </summary>
        /// <returns>True if option is enabled</returns>
        private bool IsSneakEnabled()
        {
            return cbSneakFishing.Checked; 
        }

        /// <summary>
        /// Check if Sneak status is enabled
        /// </summary>
        /// <returns>True the statuseffect is active</returns>

        private static bool IsStatusEffectActive(StatusEffect seffect)
        {
            foreach (var statuseffects in _FFACE.Player.StatusEffects)
            {
                if (statuseffects == seffect)
                    return true;
            }
            return false;
        } // @ private bool IsSneakActive()

        /// <summary>
        /// Cancels Status effect (Requires Cancel Plugin to be active)
        /// </summary>
        
        private static void CancelSpell(StatusEffect seffect)
        {
            _FFACE.Windower.SendString(string.Format(CommandCancelStatus, (short)seffect));
        } // @ private void CancelSpell(StatusEffect seffect)

        private FishResult FightFish()
        {
            SetStatus(string.Format(Resources.StatusFormatFightingFish, currentFish));
            FishResult result = FightTo(0);
            SetProgress(0);
            Thread.Sleep(100);

            if (FishResult.LostCatch == result)
            {
                return FishResult.LostCatch;
            }

            while (_FFACE.Fish.FishOnLine)
            {
                _FFACE.Windower.SendKeyPress(KeyCode.EnterKey);
                Thread.Sleep(200);
            }

            SetStatus(string.Format(Resources.StatusFormatReelingFish, currentFish));
            SetLblHP(string.Empty);

            string strNewFish = Resources.FishNameUnknown;
            string strPlayerName = _FFACE.Player.Name;
            string strCaught = string.Format(Resources.ChatFormatCaught, strPlayerName);
            string strTempFish = string.Format(Resources.ChatFormatTempFish, strPlayerName);
            string strObtain = string.Format(Resources.ChatFormatObtains, strPlayerName);
            string strMonster = string.Format(Resources.ChatFormatMonster, strPlayerName);
            string strQuest = string.Format(Resources.ChatFormatQuestBox, strPlayerName);
            string strInvFull = string.Format(Resources.ChatFormatInventoryFull, strPlayerName);
            bool foundMatch = false;
            bool caughtmonster = false;

            while (false == foundMatch)
            {
                for (int i = 0; i < 5; i++)
                {
                    if ((0 < FishChat.fishLog.Count) && (!string.IsNullOrEmpty(FishChat.fishLog[i].Text)))
                    {
                        string chatLine = FishChat.fishLog[i].Text;

                        if (chatLine.Contains(Resources.ChatLostCatch))
                        {
                            strNewFish = Resources.FishNameLostCatch;
                            result = FishResult.LostCatch;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strMonster))
                        {
                            strNewFish = Resources.FishNameMonster;
                            result = FishResult.Monster;
                            foundMatch = true;
                            caughtmonster = true;
                            break;
                        }

                        if (chatLine.Contains(Resources.ChatLostTooSmall))
                        {
                            strNewFish = Resources.FishNameTooSmall;
                            result = FishResult.TooSmall;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(Resources.ChatLostTooLarge))
                        {
                            strNewFish = Resources.FishNameTooLarge;
                            result = FishResult.TooLarge;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(Resources.ChatHookLineBreak))
                        {
                            strNewFish = Resources.FishNameBreakLine;
                            result = FishResult.LineBreak;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(Resources.ChatRodBreak))
                        {
                            strNewFish = Resources.FishNameBreakRod;
                            result = FishResult.RodBreak;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(Resources.ChatLostLackSkill))
                        {
                            strNewFish = Resources.FishNameLackSkill;
                            result = FishResult.LackSkill;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strObtain))
                        {
                            strNewFish = chatLine.Substring(strObtain.Length);
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strQuest))
                        {
                            strNewFish = Resources.FishNameQuestBox;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strInvFull))
                        {
                            result = FishResult.InventoryProblem;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(Resources.ChatLostShipDock))
                        {
                            strNewFish = Resources.FishNameLostCatch;
                            result = FishResult.Zoned;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strCaught))
                        {
                            strNewFish = chatLine.Substring(strCaught.Length);
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strTempFish))
                        {
                            strNewFish = chatLine.Substring(strTempFish.Length);
                            foundMatch = true;
                            break;
                        }
                    }
                }
                Thread.Sleep(100);
            }

            // [Fix] Stop on no bait or caught a monster
            if (caughtmonster)
            {
                // TODO What is/was this clause for?
                caughtmonster = false;
                //Stop(false, "Monster Caught!!!");
            }

            if (Resources.FishNameUnknown == currentFish)
            {
                currentFish = strNewFish;
            }

            return result;

        } // @ private FishResult FightFish()

        private FishResult FightFishFake(FishSize size)
        {
            int max, min;
            int fishesMaxHP = _FFACE.Fish.HPMax;

            if (FishSize.Large == size) //large fish
            {
                max = (int)(fishesMaxHP * (GetFakeLargeHigh() / 100));
                min = (int)(fishesMaxHP * (GetFakeLargeLow() / 100));
            }
            else //small fish
            {
                max = (int)(fishesMaxHP * (GetFakeSmallHigh() / 100));
                min = (int)(fishesMaxHP * (GetFakeSmallLow() / 100));
            }

            SetStatus(string.Format(Resources.StatusFormatFakingFish, currentFish));

            FishResult result = FightTo(rnd.Next(min, max));

            if (FishResult.LostCatch == result)
            {
                return FishResult.LostCatch;
            }

            return Release();

        } // @ private FishResult FakeFightFish(FishSize size)

        private FishResult FightTo(int fishFinalHP)
        {
            int currentFishHP = _FFACE.Fish.HPCurrent;
            decimal randomReaction = Math.Round((decimal)rnd.NextDouble() * (numReactionHigh.Value - numReactionLow.Value), 1);
            decimal sleepInterval = numReactionLow.Value + randomReaction;
            bool extendtime = true; //prevent loop
            bool killtimer = true; //prevent loop

            SetNoCatch(consecutiveNoCatchCount = 0);
            SetProgressMaxValue(_FFACE.Fish.HPMax);

            using (System.Timers.Timer progress = new System.Timers.Timer())
            {
                progress.Elapsed += new ElapsedEventHandler(timer_DisplayProgressEvent);
                progress.Interval = 20;
                progress.Start();

                if (cbFishHP.Checked)
                {
                    SetLblHP(string.Format(FormatFishHP, _FFACE.Fish.HPCurrent, _FFACE.Fish.HPMax, _FFACE.Fish.Timeout));
                }

                if (cbReaction.Checked)
                {
                    Thread.Sleep((int)sleepInterval * 1000);
                }

                while ((_FFACE.Fish.FishOnLine) && (_FFACE.Player.Zone == currentZone) && (currentFishHP > fishFinalHP))
                {
                    if (cbExtend.Checked && _FFACE.Fish.HPCurrent > 0 && extendtime)
                    {
                        extendtime = false;
                        short extendReelTime = (short)(_FFACE.Fish.Timeout + 30);
                        _FFACE.Fish.SetFishTimeOut(extendReelTime);
                    } //extend fish timeout if option enabled

                    if (cbQuickKill.Checked && _FFACE.Fish.HPCurrent > 0 && killtimer)
                    {
                        killtimer = false;
                        int killSleep = (1000 * (int)numQuickKill.Value);
                        SetLblHP(string.Empty);
                        lblStatus.Text = string.Format(Resources.StatusFormatKillingFish, currentFish, numQuickKill.Value);
                        Thread.Sleep(killSleep);
                        _FFACE.Fish.SetHP(0);
                    } //kill fish early if option enabled

                    if (cbAutoKill.Checked && _FFACE.Fish.HPCurrent > 0 && _FFACE.Fish.Timeout <= 5)
                    {
                        _FFACE.Fish.SetHP(0);
                    } //kill fish at warning if option enabled

                    if (Status.LostCatch == currentStatus)
                    {
                        WinClear();
                        WaitUntil(Status.Standing);

                        return FishResult.LostCatch;
                    }

                    _FFACE.Fish.FightFish();
                    currentFishHP = _FFACE.Fish.HPCurrent;

                    if (cbFishHP.Checked)
                    {
                        SetLblHP(string.Format(FormatFishHP, currentFishHP, _FFACE.Fish.HPMax, _FFACE.Fish.Timeout));
                    }
                    Thread.Sleep(1);
                }
                WinClear();
                progress.Stop();
            }

            if (_FFACE.Player.Zone != currentZone)
            {
                Stop(true, Resources.StatusErrorZoneChanged);
                return FishResult.LostCatch;
            }

            return FishResult.Success;

        } // @ private FishResult FightTo(int fishFinalHP)

		private void DoLostCatch()
		{
			WinClear();
			SetNoCatch(++consecutiveNoCatchCount);

			if (1 < consecutiveNoCatchCount)
			{
				SetStatus(string.Format(Resources.StatusFormatNoCatchNumber, consecutiveNoCatchCount));
			}
			else
			{
				SetStatus(Resources.StatusFormatNoCatchFirst);
			}

			FishStats.totalCastCount++;
			FishStats.noCatchCount++;
			UpdateStats();
			WaitUntil(Status.Standing);
		}

        private string GetFishName(string fish)
        {
            string name = fish;
            // Get a better name for the fish
            foreach (KeyValuePair<string, int> f in Dictionaries.fishDictionary)
            {
                if (-1 < fish.IndexOf(f.Key, StringComparison.OrdinalIgnoreCase))
                {
                    // Remove punctuation and articles, but don't change fish name if it's only a partial match of one of the words
                    // At least one of the words must match
                    List<string> fishNameParts = new List<string>((fish.Split(new char[3] { Resources.SpaceChar, Resources.Period, Resources.Exclamation})).AsEnumerable());
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
                    if (int.TryParse(fishNameParts[0], out test))
                    {
                        string multiple = string.Format(FormatFishNameMultiple, f.Key, test);
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

        private void DoFishFighting(bool isNewFish, string ID1, string ID2, string ID3)
        {
            FishResult fishFightResult = FightFish();

            RegisterFish(isNewFish, ID1, ID2, ID3);

            LogResult(fishFightResult);
        } // @ private void DoFishFighting(bool isNewFish, string ID1, string ID2, string ID3)

        private void RegisterFish(bool isNewFish, string ID1, string ID2, string ID3)
        {
            currentFish = GetFishName(currentFish);
            if (isNewFish && Resources.FishNameUnknown != currentFish)
            {
                FishDB.AddNewFish(ref currentFish, lblZone.Text, LastBaitName, LastRodName, ID1, ID2, ID3, false, false);
            }
        }

        private void Fish()
        {

            SetProgress(0);

            if ((Status.Healing == currentStatus) || (Status.Sitting == currentStatus))
            {
                Stop(false, Resources.StatusErrorHealing);
                return;
            }

            if (_FFACE.Player.Zone != currentZone)
            {
                Stop(true, Resources.StatusErrorZoneChanged);
                return;
            }

            CheckInventory();

			if (Status.Fishing != currentStatus && Status.FishBite != currentStatus)
			{
				if (Status.LostCatch == currentStatus)
				{
					DoLostCatch();
					return;
				}
				WaitUntil(Status.Standing);
				Cast();
				Thread.Sleep(3500);
			}

			if (Status.Fishing != currentStatus && Status.FishBite != currentStatus)
            {
                for (int i = 0; i < 10; i++)
                {
                    if ((i < FishChat.chatLog.Count) && (!string.IsNullOrEmpty(FishChat.chatLog[i].Text)))
                    {
                        string chatLine = FishChat.chatLog[i].Text;

                        if (chatLine.Equals(Resources.ChatWaitLonger))
                        {
                            SetStatus(Resources.StatusInfoAddTime);
                            IncreaseCastTime();
                            Thread.Sleep(2000);
                            return;
                        }

                        if (chatLine.Equals(Resources.ChatNoBait))
                        {
                            if (!CheckRodAndBait())
                            {
                                OutOfBait(Resources.StatusErrorNoBait);
                                return;
                            }
                        }

                        if (chatLine.Equals(Resources.ChatRodBreak))
                        {
                            if (!CheckRodAndBait())
                            {
                                Stop(false, Resources.StatusErrorRodBroken);
                                return;
                            }
                        }

                        if (chatLine.Equals(Resources.ChatBadLocation))
                        {
                            Stop(false, Resources.StatusErrorMovePlayer);
                            return;
                        }

                        if (_FFACE.Player.Zone != currentZone)
                        {
                            Stop(true, Resources.StatusErrorZoneChanged);
                            return;
                        }
                    }
                }
            }

			// Leave room for lag
			if (Status.Fishing != currentStatus && Status.FishBite != currentStatus)
			{
                if (Status.LostCatch == currentStatus)
                {
                    DoLostCatch();
                    return;
                }
				WaitUntil(Status.Fishing, 10000);
			}

			if (Status.Fishing != currentStatus && Status.FishBite != currentStatus)
            {
				if (Status.LostCatch == currentStatus)
				{
					DoLostCatch();
					return;
				}
				if (!CheckProcess())
				{
					return;
				}
				else
				{
					Stop(false, Resources.StatusErrorUnknown);
					return;
				}
            }

			if (Status.FishBite != currentStatus)
			{
				SetStatus(Resources.StatusInfoWaitBite);
                SetLblHP(string.Empty);
			}

            while (_FFACE.Player.Zone == currentZone)
            {
                Thread.Sleep(100);

                //if nothing is caught
                if (Status.LostCatch == currentStatus)
                {
					DoLostCatch();
                    break;
                }

                //fish on the hook, fight if its the accepted fish, release otherwise
                if (Status.FishBite == currentStatus)
                {
                    bool isNewFish;
                    FFACE.FishTools.FishID currentID = _FFACE.Fish.ID;
                    string ID1 = currentID.ID1.ToString();
                    string ID2 = currentID.ID2.ToString();
                    string ID3 = currentID.ID3.ToString();
                    bool fishAccepted = FishDB.FishAccepted(out currentFish, out isNewFish, cbCatchUnknown.Checked, LastRodName, lblZone.Text, LastBaitName, ID1, ID2, ID3);

                    SetNoCatch(consecutiveNoCatchCount = 0);

                    for (int i = 0; i < 5; i++)
                    {
                        if ((0 < FishChat.fishLog.Count) && (!string.IsNullOrEmpty(FishChat.fishLog[i].Text)))
                        {
                            string chatLine = FishChat.fishLog[i].Text;

                            if (chatLine.Equals(Resources.ChatHookSmallFish))
                            {
                                if (cbIgnoreSmallFish.Checked)
                                {
                                    RegisterFish(fishAccepted, ID1, ID2, ID3);
                                    currentFish = Resources.FishNameIgnoredSmall;
                                    LogResult(Release());
                                }
                                else
                                {
                                    if (fishAccepted)
                                    {
                                        DoFishFighting(isNewFish, ID1, ID2, ID3);
                                    }
                                    else
                                    {
                                        if (cbReleaseSmall.Checked)
                                        {
                                            LogResult(FightFishFake(FishSize.Small));
                                        }
                                        else
                                        {
                                            LogResult(Release());
                                        }
                                    }
                                }
                                break;
                            }

                            if (chatLine.Equals(Resources.ChatHookLargeFish))
                            {
                                if (cbIgnoreLargeFish.Checked)
                                {
                                    RegisterFish(fishAccepted, ID1, ID2, ID3);
                                    currentFish = Resources.FishNameIgnoredLarge;
                                    LogResult(Release());
                                }
                                else
                                {
                                    if (fishAccepted)
                                    {
                                        DoFishFighting(isNewFish, ID1, ID2, ID3);
                                    }
                                    else
                                    {
                                        if (cbReleaseLarge.Checked)
                                        {
                                            LogResult(FightFishFake(FishSize.Large));
                                        }
                                        else
                                        {
                                            LogResult(Release());
                                        }
                                    }
                                }
                                break;
                            }

                            if (chatLine.Equals(Resources.ChatHookMonster))
                            {
                                if (cbIgnoreMonster.Checked)
                                {
                                    RegisterFish(fishAccepted, ID1, ID2, ID3);
                                    currentFish = Resources.FishNameIgnoredMonster;
                                    LogResult(Release());
                                }
                                else
                                {
                                    if (fishAccepted)
                                    {
                                        DoFishFighting(isNewFish, ID1, ID2, ID3);
                                    }
                                    else
                                    {
                                        LogResult(Release());
                                    }
                                }
                                break;
                            }

                            if (chatLine.Equals(Resources.ChatHookItem))
                            {
                                if (cbIgnoreItem.Checked)
                                {
                                    RegisterFish(fishAccepted, ID1, ID2, ID3);
                                    currentFish = Resources.FishNameIgnoredItem;
                                    LogResult(Release());
                                }
                                else
                                {
                                    if (fishAccepted)
                                    {
                                        DoFishFighting(isNewFish, ID1, ID2, ID3);
                                    }
                                    else
                                    {
                                        LogResult(Release());
                                    }
                                }
                                break;
                            }
                        }
                    }
                    break;
                }

				if (Status.Standing == currentStatus)
				{
					break;
				}
            }
            WaitUntil(Status.Standing);
        } // @ private void Fish()

        private void CheckInventory()
        {
            //move items with itemizer or itemtools or custom script
            if (_FFACE.Item.InventoryCount == _FFACE.Item.InventoryMax
                    && (cbInventoryItemizerItemTools.Checked || cbFullactionOther.Checked))
            {
                if (cbInventoryItemizerItemTools.Checked)
                {
                    foreach (Fishie fishie in lbWanted.Items)
                    {
                        if (!(cbInventoryItemizerSack.Checked && _FFACE.Item.SackCount < _FFACE.Item.SackMax) &&
                            !(cbInventoryItemizerSatchel.Checked && _FFACE.Item.SatchelCount < _FFACE.Item.SatchelMax))
                        {
                            break;
                        }
                        // Get best guess for the fish name
                        string name = GetFishName(fishie.name);
                        if (!Dictionaries.fishDictionary.ContainsKey(name) || _FFACE.Item.GetInventoryItemCount((ushort)Dictionaries.fishDictionary[name]) == 0)
                        {
                            continue;
                        }
                        string quoteName = string.Format(Resources.FormatQuoteArg, name);
                        string storagemedium;
                        if (_FFACE.Item.GetInventoryItemCount((ushort)Dictionaries.fishDictionary[name]) > 0 &&
                            cbInventoryItemizerSack.Checked && _FFACE.Item.SackCount < _FFACE.Item.SackMax)
                        {
                            storagemedium = Resources.CommandPartSack;
                            if (ItemizerAvailable)
                            {
                                MoveItems(string.Format("/puts {0} {1}", quoteName, storagemedium), ref name, ref storagemedium);
                            }
                            else if (ItemToolsAvailable)
                            {
                                MoveItems(string.Format("/moveitem {0} inventory {1} 12", quoteName, storagemedium), ref name, ref storagemedium);
                            }
                        }
                        if (_FFACE.Item.GetInventoryItemCount((ushort)Dictionaries.fishDictionary[name]) > 0 &&
                            cbInventoryItemizerSatchel.Checked && _FFACE.Item.SatchelCount < _FFACE.Item.SatchelMax)
                        {
                            storagemedium = Resources.CommandPartSatchel;
                            if (ItemizerAvailable)
                            {
                                MoveItems(string.Format("/puts {0} {1}", quoteName, storagemedium), ref name, ref storagemedium);
                            }
                            else if (ItemToolsAvailable)
                            {
                                MoveItems(string.Format("/moveitem {0} inventory {1} 12", quoteName, storagemedium), ref name, ref storagemedium);
                            }
                        }
                    }
                }
                else if (cbFullactionOther.Checked && !string.IsNullOrEmpty(tbFullactionOther.Text))
                {
                    SetStatus(Resources.StatusInfoFullInventoryCommand);

                    foreach (Fishie fishie in lbWanted.Items)
                    {
                        // Get best guess for the fish name
                        string name = GetFishName(fishie.name);
                        if (!Dictionaries.fishDictionary.ContainsKey(name) || _FFACE.Item.GetInventoryItemCount((ushort)Dictionaries.fishDictionary[name]) == 0)
                        {
                            continue;
                        }
                        name = string.Format(Resources.FormatQuoteArg, name);
                        _FFACE.Windower.SendString(string.Format(tbFullactionOther.Text, name));
                        Thread.Sleep((int)(numFullactionOtherTime.Value * 1000));
                    }
                }
            }
            if (_FFACE.Item.InventoryCount == _FFACE.Item.InventoryMax && cbFullActionStop.Checked)
            {
                if (cbFullactionWarp.Checked)
                {
                    SetStatus(Resources.StatusInfoFullInventoryWarp);
                    _FFACE.Windower.SendString(CommandWarp);
                    Thread.Sleep(30000);
                }
                if (cbFullactionLogout.Checked)
                {
                    SetStatus(Resources.StatusInfoFullInventoryLogout);
                    _FFACE.Windower.SendString(CommandLogout);
                    Thread.Sleep(30000);
                }
                else if (cbFullactionShutdown.Checked)
                {
                    SetStatus(Resources.StatusInfoFullInventoryShutdown);
                    _FFACE.Windower.SendString(CommandShutdown);
                    Thread.Sleep(30000);
                }
                Stop(false, Resources.StatusErrorFullInventory);
            }
        }

        private void MoveItems(string command, ref string itemname, ref string storagearea)
        {
            // Look up item ID
            int tempitemid = FFACE.ParseResources.GetItemId(itemname);
            if (storagearea == Resources.CommandPartSack)
            {
                // If we find the itemid and the sack is not full move item to sack
                if (tempitemid > 0 && ( _FFACE.Item.SackCount != _FFACE.Item.SackMax ))
                {
                    // Get total number of the items you have in inventory.
                    uint inventorycount = _FFACE.Item.GetInventoryItemCount((ushort)tempitemid);
                    // If we have the item in inventory, move all of them till satchel is full
                    while (( _FFACE.Item.SackCount < _FFACE.Item.SackMax ) && inventorycount > 0)
                    {
                        // Update Status
                        SetStatus(string.Format(Resources.StatusFormatMoveToSack, itemname, inventorycount));
                        // Send string to POL
                        _FFACE.Windower.SendString(command);
						inventorycount = _FFACE.Item.GetInventoryItemCount((ushort)tempitemid);
                        // The dreaded sleep()!
                        Thread.Sleep(rnd.Next(750,1500));
                    }
                    SetStatus(string.Format(Resources.StatusFormatMoveSackFinished, itemname));
                }
                else
                {
                    SetStatus(Resources.StatusInfoFullSack);
					Thread.Sleep(750);
                }
            }
            if (storagearea == Resources.CommandPartSatchel)
            {
                if (tempitemid > 0 && ( _FFACE.Item.SatchelCount != _FFACE.Item.SatchelMax ))
                {
                    // Get total number of the items you have in inventory.
                    uint inventorycount = _FFACE.Item.GetInventoryItemCount((ushort)tempitemid);
                    // If we have the item in inventory, move all of them till satchel is full
                    while (( _FFACE.Item.SatchelCount < _FFACE.Item.SatchelMax ) && inventorycount > 0)
                    {
                        // Update Status
                        SetStatus(string.Format(Resources.StatusFormatMoveToSatchel, itemname, inventorycount));
                        // Send string to POL
                        _FFACE.Windower.SendString(command);
						inventorycount = _FFACE.Item.GetInventoryItemCount((ushort)tempitemid);
                        // The dreaded sleep()!
                        Thread.Sleep(rnd.Next(750, 1500));
                    }
                    SetStatus(string.Format(Resources.StatusFormatMoveSatchelFinished, itemname));
                }
                else
                {
                    SetStatus(Resources.StatusInfoFullSatchel);
					Thread.Sleep(750);
                }
            }
        }

        private FishResult Release()
        {
            WinClear();
            SetStatus(string.Format(Resources.StatusFormatReleaseFish, currentFish));
            decimal randomReaction = Math.Round((decimal)rnd.NextDouble() * (numReactionHigh.Value - numReactionLow.Value), 1);
            decimal sleepInterval = numReactionLow.Value + randomReaction + 0.01m;
            
            if (cbReaction.Checked)
            {
                Thread.Sleep((int)sleepInterval * 1000);
            }

            while (_FFACE.Fish.FishOnLine)
            {
                _FFACE.Windower.SendKeyPress(KeyCode.EscapeKey);
                Thread.Sleep(200);
            }

            return FishResult.Released;

        } // @ private FishResult Release()

        #endregion //// @ Methods_Fishing_Major

        #region Methods_Fishing_Minor

        private string GetBaitName(int id)
        {
            string name = string.Empty;
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

        private string GetRodName(int id)
        {
            string name = string.Empty;
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

        private string GetGearName(int id)
        {
            string name = string.Empty;
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

        private string GetZoneName(Zone zone)
        {
            return FFACE.ParseResources.GetAreaName(zone);

        } // @ private string GetPlayerZoneName(Zone zone)

        private void LogResult(FishResult result)
        {
            FishStats.totalCastCount++;

            switch (result)
            {
                case FishResult.Error:
                    Stop(false, Resources.StatusErrorCantFish);
                    break;
                case FishResult.InventoryProblem:
                    FishStats.releasedCount++;
                    FishStats.AddFish(FishStats.releasedFishes, currentFish);
                    if (cbFullActionStop.Checked)
                    {
                        Stop(false, Resources.StatusErrorInventoryProblem);
                    }
                    break;
                case FishResult.LackSkill:
                    FishStats.lackSkillCount++;
                    FishStats.AddFish(FishStats.lackSkillFishes, currentFish);
                    break;
                case FishResult.LineBreak:
                    FishStats.lineBreakCount++;
                    FishStats.AddFish(FishStats.lineBreakFishes, currentFish);
                    break;
                case FishResult.LostCatch:
                    FishStats.lostCatchCount++;
                    FishStats.AddFish(FishStats.lostCatchFishes, currentFish);
                    break;
                case FishResult.Monster:
                    FishStats.monsterCount++;
                    Stop(false, Resources.StatusErrorMonster);
                    break;
                case FishResult.Released:
                    FishStats.releasedCount++;
                    FishStats.AddFish(FishStats.releasedFishes, currentFish);
                    break;
                case FishResult.RodBreak:
                    FishStats.rodBreakCount++;
                    FishStats.AddFish(FishStats.rodBreakFishes, currentFish);
                    SetStatus(Resources.StatusInfoRodBroken);
                    if (!CheckRodAndBait())
                    {
                        Stop(false, Resources.StatusErrorRodBroken);
                    }
                    break;
                case FishResult.Success:
                    FishStats.caughtCount++;
                    FishStats.AddFish(FishStats.caughtFishes, currentFish);
                    // Check max catch count
                    if (cbMaxCatch.Checked)
                    {
                        consecutiveCatchCount++;
                        if (consecutiveCatchCount == (int)numMaxCatch.Value)
                        {
                            consecutiveCatchCount = 0;
                            Fatigued(Resources.StatusFatiguedMaxCatchLimit);
                        }
                    }
                    break;
                case FishResult.TooLarge:
                    FishStats.tooLargeCount++;
                    FishStats.AddFish(FishStats.tooLargeFishes, currentFish);
                    break;
                case FishResult.TooSmall:
                    FishStats.tooSmallCount++;
                    FishStats.AddFish(FishStats.tooSmallFishes, currentFish);
                    break;
                case FishResult.Zoned:
                    FishStats.lostCatchCount++;
                    FishStats.AddFish(FishStats.lostCatchFishes, currentFish);
                    Stop(true, Resources.StatusErrorZoned);
                    break;
            }

        } // @ private void LogResult(FishResult result)

        /// <summary>
        /// Pauses the execution of the thread until the player meets the passed Status
        /// </summary>
        /// <param name="status">enum FFACETools.Status</param>
        private void WaitUntil(Status status)
        {
            while (status != currentStatus)
            {
                if (_FFACE.Player.Zone == currentZone)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    Stop(true, Resources.StatusErrorZoneChanged);
                    break;
                }
            }

        } // @ private void WaitUntil(Status status)

        /// <summary>
        /// Pauses the execution of the thread until the player meets the passed Status
        /// or until the second param a time (in milliseconds), has elapsed
        /// </summary>
        /// <param name="status">enum FFACETools.Status</param>
        /// <param name="quit">amount of time (in milliseconds)</param>
        private void WaitUntil(Status status, int quit)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            while ((status != currentStatus) && (sw.ElapsedMilliseconds < quit))
            {
                if (_FFACE.Player.Zone == currentZone)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    Stop(true, Resources.StatusErrorZoneChanged);
                    break;
                }
            }
            sw.Stop();

        } // @ private void WaitUntil(Status status, int quit)

        /// <summary>
        /// Clears any keys sent to FFXI by this program, so they don't "lock"
        /// </summary>
        private void WinClear()
        {
            _FFACE.Windower.SendKey(KeyCode.EscapeKey, false);
            _FFACE.Windower.SendKey(KeyCode.EnterKey, false);

        } // @ private void WinClear()

        #endregion //Methods_Fishing_Minor

        #endregion //Methods_Fishing

        #region Methods_Reattach/Start/Stop

        /// <summary>
        /// Function called when FFACETools, or FFACE, chat fails.
        /// Releases the current instance of FFACETools,
        /// then reattaches to the originally chosen PID.
        /// </summary>
        public void Reattach()
        {
            bool wasFishing = Resources.GUIButtonStop == btnStart.Text;

            Stop(false, Resources.StatusErrorChatGet);

            _FFACE = null;
            _Player = null;
            ChooseProcess(null);
            if (_Process != null)
            {
                _FFACE = new FFACE(_Process.POLID);
                _Player = _FFACE.Player;

                if (wasFishing)
                {
                    SetStatus(Resources.StatusInfoReattachedRestart);
                    Start();  //restart the fishing loop if the user was fishing
                }
                else
                {
                    SetStatus(Resources.StatusInfoReattachSuccess);
                }
            }

        } // @ private void Reattach()

		private bool CheckProcess()
		{
			if (_Process == null || !_Process.IsAvailable)
			{
				if (workerThread != null)
				{
					Stop(false, Resources.StatusErrorLoggedOut);
				}
				ChooseProcess(null);
				if (_Process == null || !_Process.IsAvailable)
				{
					SetStatus(Resources.StatusInfoNoProcess);
					return false;
				}
			}
			return true;
		}

        private void Start()
        {
            LastBaitName = LastRodName = lblZone.Text = string.Empty;
            ClearLists();
			if (!CheckProcess())
			{
				return;
			}
			FishStats.startTicks = DateTime.Now.Ticks;
			japanNextMidnight = GetNextMidnight();
            UpdateStats();
            stopSound = true;
            statusWarningColor = true;

            if (IsRodBaitSet())
            {
                while (_FFACE.Menu.IsOpen)
                {
                    _FFACE.Windower.SendKeyPress(KeyCode.EscapeKey);
                    Thread.Sleep(200);
                }

                GearUp();
                PopulateLists();
                CheckEnchantment();

                ts = new ThreadStart(BackgroundFishing);
                workerThread = new Thread(BackgroundFishing);
                workerThread.IsBackground = true;
                workerThread.Start();

                btnStart.Text = Resources.GUIButtonStop;
                btnStartM.Image = Resources.icon_stop;
            }

        } // @ private void Start()

		private void GearUp()
		{
			if (!string.IsNullOrEmpty(tbBodyGear.Text) && GetGearName(_FFACE.Item.GetEquippedItemID(EquipSlot.Body)) != tbBodyGear.Text)
			{
				_FFACE.Windower.SendString(string.Format(EquipFormatBody, tbBodyGear.Text));
            }
            if (!string.IsNullOrEmpty(tbHandsGear.Text) && GetGearName(_FFACE.Item.GetEquippedItemID(EquipSlot.Hands)) != tbHandsGear.Text)
			{
				_FFACE.Windower.SendString(string.Format(EquipFormatHands, tbHandsGear.Text));
            }
            if (!string.IsNullOrEmpty(tbLegsGear.Text) && GetGearName(_FFACE.Item.GetEquippedItemID(EquipSlot.Legs)) != tbLegsGear.Text)
			{
                _FFACE.Windower.SendString(string.Format(EquipFormatLegs, tbLegsGear.Text));
            }
            if (!string.IsNullOrEmpty(tbFeetGear.Text) && GetGearName(_FFACE.Item.GetEquippedItemID(EquipSlot.Feet)) != tbFeetGear.Text)
			{
                _FFACE.Windower.SendString(string.Format(EquipFormatFeet, tbFeetGear.Text));
            }
            if (!string.IsNullOrEmpty(tbHeadGear.Text) && GetGearName(_FFACE.Item.GetEquippedItemID(EquipSlot.Head)) != tbHeadGear.Text)
			{
                _FFACE.Windower.SendString(string.Format(EquipFormatHead, tbHeadGear.Text));
            }
            if (!string.IsNullOrEmpty(tbNeckGear.Text) && GetGearName(_FFACE.Item.GetEquippedItemID(EquipSlot.Neck)) != tbNeckGear.Text)
			{
                _FFACE.Windower.SendString(string.Format(EquipFormatNeck, tbNeckGear.Text));
            }
            if (!string.IsNullOrEmpty(tbWaistGear.Text) && GetGearName(_FFACE.Item.GetEquippedItemID(EquipSlot.Waist)) != tbWaistGear.Text)
			{
                _FFACE.Windower.SendString(string.Format(EquipFormatWaist, tbWaistGear.Text));
			}
            if (!string.IsNullOrEmpty(tbLRingGear.Text) && GetGearName(_FFACE.Item.GetEquippedItemID(EquipSlot.RingLeft)) != tbLRingGear.Text)
			{
                _FFACE.Windower.SendString(string.Format(EquipFormatLRing, tbLRingGear.Text));
				if (tbLRingGear.Text == tbRRingGear.Text)
				{
					Thread.Sleep(500);
				}
            }
            if (!string.IsNullOrEmpty(tbRRingGear.Text) && GetGearName(_FFACE.Item.GetEquippedItemID(EquipSlot.RingRight)) != tbRRingGear.Text)
			{
                _FFACE.Windower.SendString(string.Format(EquipFormatRRing, tbRRingGear.Text));
            }
        } // @ private void GearUp()

        private int GetNeededEnchantments()
        {
            int enchantmentsNeeded = 0;
            if (cbLRingGear.Enabled && cbLRingGear.Checked)
            {
                enchantmentsNeeded++;
            }
            if (cbRRingGear.Enabled && cbRRingGear.Checked)
            {
                enchantmentsNeeded++;
            }
            foreach (StatusEffect se in _Player.StatusEffects)
            {
                if (StatusEffect.Enchantment == se)
                {
                    enchantmentsNeeded--;
                }
            }
            return enchantmentsNeeded;
        }

		private void CheckEnchantment() {
			// Check if fishing support is available and not on (Fisherman's belt)
			if (cbWaistGear.Enabled && cbWaistGear.Checked)
			{
				bool enchantInactive = true;
				foreach (StatusEffect se in _Player.StatusEffects)
                {
					if (StatusEffect.Fishing_Imagery == se)
                    {
						enchantInactive = false;
					}
				}
				if (enchantInactive)
				{
					SetStatus(Resources.StatusInfoEnchantBelt);
					_FFACE.Windower.SendString(string.Format(CommandFormatItemMe, tbWaistGear.Text));
                    Thread.Sleep(500);
					WaitUntil(Status.Standing);
				}
			}
            // Count enchantment effects the playe is under to compare to enabled ring count
            int enchantmentsNeeded = GetNeededEnchantments();
			// Check if left ring enchantments are available and not on
			if (cbLRingGear.Enabled && cbLRingGear.Checked && enchantmentsNeeded > 0)
			{
				SetStatus(string.Format(Resources.StatusInfoEnchantLRing, tbLRingGear.Text));
				_FFACE.Windower.SendString(string.Format(CommandFormatItemMe, tbLRingGear.Text));
                Thread.Sleep(500);
				WaitUntil(Status.Standing);
                enchantmentsNeeded = GetNeededEnchantments();
			}
			// Check if right ring enchantments are available and not on
			if (cbRRingGear.Enabled && cbRRingGear.Checked && enchantmentsNeeded > 0)
			{
				SetStatus(string.Format(Resources.StatusInfoEnchantRRing, tbRRingGear.Text));
                _FFACE.Windower.SendString(string.Format(CommandFormatItemMe, tbRRingGear.Text));
                Thread.Sleep(500);
				WaitUntil(Status.Standing);
			}
		}

        private void OutOfBait(string message)
        {
            if (cbBaitActionWarp.Checked)
            {
                _FFACE.Windower.SendString(CommandWarp);
                SetStatus(Resources.StatusInfoBaitWarping);
                Thread.Sleep(30000);
            }
            if (cbBaitActionShutdown.Checked)
            {
                _FFACE.Windower.SendString(CommandShutdown);
                SetStatus(Resources.StatusInfoBaitShutdown);
                Thread.Sleep(33000);
            }
            else if (cbBaitActionLogout.Checked)
            {
                _FFACE.Windower.SendString(CommandLogout);
                SetStatus(Resources.StatusInfoBaitLogout);
                Thread.Sleep(33000);
            }
            Stop(false, message);
        }

        private void Fatigued(string message)
        {
            if (cbFatiguedActionWarp.Checked)
            {
                _FFACE.Windower.SendString(CommandWarp);
                SetStatus(Resources.StatusInfoFatiguedWarping);
                Thread.Sleep(30000);
            }
            if (cbFatiguedActionShutdown.Checked)
            {
                _FFACE.Windower.SendString(CommandShutdown);
                SetStatus(Resources.StatusInfoFatiguedShutdown);
            }
            else if (cbFatiguedActionLogout.Checked)
            {
                _FFACE.Windower.SendString(CommandLogout);
                SetStatus(Resources.StatusInfoFatiguedLogout);
            }
            Thread.Sleep(33000);
            Stop(false, message);
        }

        private void Stop(bool zoned, string status)
        {
            this.UIThread(delegate
            {
                if (FishStats.startTicks != 0)
                {
                    FishStats.ticksFished += DateTime.Now.Ticks - FishStats.startTicks;
                    FishStats.startTicks = 0;
                }
                SetStatus(status);

                if (statusWarningColor)
                {
                    statusStripMain.BackColor = Color.Red;
                    statusStripMain.ForeColor = Color.White;
                    btnStart.BackColor = Color.Red;
                    btnStart.ForeColor = Color.White;
                }
                if (null != workerThread)
                {
                    string equipMessage = string.Format(EquipFormatBait, LastBaitName);

                    SetProgressMaxValue(0);

                    WinClear();

                    if (Status.Fishing == currentStatus)
                    {
                        FishStats.totalCastCount++;
                        FishStats.noCatchCount++;
                        _FFACE.Windower.SendKeyPress(KeyCode.EscapeKey);
                        UpdateStats();
                    }

                    if (Status.FishBite == currentStatus)
                    {
                        FishStats.totalCastCount++;
                        FishStats.releasedCount++;
                        FishStats.AddFish(FishStats.releasedFishes, currentFish);
                        _FFACE.Windower.SendKeyPress(KeyCode.EscapeKey);
                        UpdateStats();
                    }

                    if (!zoned)
                    {
                        // in case the last consumable bait was used after stopping
                        if (string.IsNullOrEmpty(GetBaitName(_FFACE.Item.GetEquippedItemID(EquipSlot.Ammo))))
                        {
                            WaitUntil(Status.Standing);
                            _FFACE.Windower.SendString(equipMessage);
                        }
                    }

                    if (stopSound && cbStopSound.Checked && File.Exists(FileWarningWav))
                    {
                        SoundPlayer spWave = new SoundPlayer(FileWarningWav);
                        spWave.Play();
                    }

                    this.WindowState = FormWindowState.Normal;

                    if (chatbig)
                    {
                        chatbig = false;
                        ExtendChat();
                    }

                    workerThread.Abort();
                    // while(workerThread.IsAlive) { Thread.Sleep(100); }  //*golfandsurf* removed to correct freeze problem.
                    workerThread = null;
                    btnStart.Text = Resources.GUIButtonStart;
                    btnStartM.Image = Resources.icon_play;
                }
            });
        } // @ private void Stop(bool zoned, string status)

        #endregion //Methods_Reattach/Start/Stop

        #region Methods_ThreadSafe

        private void ClearLists()
        {
            this.UIThread(delegate
            {
                lbWanted.Items.Clear();
                lbUnwanted.Items.Clear();
            });

        } // @ private void ClearLists()

        private decimal GetFakeLargeHigh()
        {
            decimal ret = decimal.Zero;
            this.UIThreadInvoke(delegate
            {
                ret = numFakeLargeIntervalHigh.Value;
            });
            return ret;
        } // @ private decimal GetFakeLargeHigh()

        private decimal GetFakeLargeLow()
        {
            decimal ret = decimal.Zero;
            this.UIThreadInvoke(delegate
            {
                ret = numFakeLargeIntervalLow.Value;
            });
            return ret;
        } // @ private decimal GetFakeLargeLow()

        private decimal GetFakeSmallHigh()
        {
            decimal ret = decimal.Zero;
            this.UIThreadInvoke(delegate
            {
                ret = numFakeSmallIntervalHigh.Value;
            });
            return ret;
        } // @ private decimal GetFakeSmallHigh()

        private decimal GetFakeSmallLow()
        {
            decimal ret = decimal.Zero;
            this.UIThreadInvoke(delegate
            {
                ret = numFakeSmallIntervalLow.Value;
            });
            return ret;
        } // @ private decimal GetFakeSmallLow()

        private void IncreaseCastTime()
        {
            this.UIThread(delegate
            {
                numCastIntervalHigh.Value += 1;
                numCastIntervalLow.Value += 1;
            });
        } // @ private void IncreaseCastTime()

        private void PopulateLists()
        {
            this.UIThread(delegate
            {
                lbWanted.Items.Clear();
                lbUnwanted.Items.Clear();

                if (_FFACE == null || string.IsNullOrEmpty(LastRodName) || string.IsNullOrEmpty(LastBaitName))
                {
                    return;
                }

                foreach (Fishie f in FishDB.GetFishes(LastRodName, lblZone.Text, LastBaitName, true))
                {
                    lbWanted.Items.Add(f);
                }

                foreach (Fishie f in FishDB.GetFishes(LastRodName, lblZone.Text, LastBaitName, false))
                {
                    lbUnwanted.Items.Add(f);
                }
            });
        } // @ private void PopulateLists()

        private void SetBait(string bait)
        {
            this.UIThread(delegate
            {
                LastBaitName = bait;
            });
        } // @ private void SetBait(string bait)

        private void SetRod(string rod)
        {
            this.UIThread(delegate
            {
                LastRodName = rod;
            });
        } // @ private void SetRod(string rod)

        private void SetLblZone(string zone)
        {
            this.UIThread(delegate
            {
                lblZone.Text = zone;
            });
        } // @ private void SetLblZone(string zone)

        private void SetNoCatch(int releases)
        {
            this.UIThread(delegate
            {
                lblNoCatchAt.Text = string.Format(GUIFormatNoCatch, consecutiveNoCatchCount, numMaxNoCatch.Value);
            });
        } // @ private void SetNoCatch(int releases)

        private void SetLblHP(string text)
        {
            this.UIThread(delegate
            {
                lblHP.Text = text;
            });
        } // @ private void SetLblHP(string text)

        private void SetProgress(int pos)
        {
            this.UIThread(delegate
            {
                //in case SetProgressMaxValue wasn't called, for w/e reason
                if (progressBarST.Maximum < pos)
                {
                    SetProgressMaxValue(_FFACE.Fish.HPMax);
                }

                progressBarST.Value = pos;
            });
        } // @ private void SetProgress(int pos)

        private void SetProgressMaxValue(int pos)
        {
            this.UIThread(delegate
            {
                progressBarST.Maximum = pos;
                progressBarST.Value = pos;
            });
        } // @ private void SetProgressMaxValue(int pos)

        private void SetStatus(string str)
        {
            this.UIThread(delegate
            {
                lblStatus.Text = str;
            });
        } // @ private void SetStatus(string str)

        private void UpdateChat()
        {
            this.UIThread(delegate
            {
                UpdateChatLogs(rtbChat, FishChat.chatLog, FishChat.chatLogAdded);
                string testLine;

                //added by golfandsurf 6/21/2010:  GMdetect
                //notes: corrects for word wrapping that may occur to always check the
                //      beginning of each message.
                for (int i = FishChat.tellLogAdded - 1; i >= 0; --i)
                {
                    testLine = FishChat.tellLog[i].Text;
                    if (testLine.Length >= 3 && testLine.Substring(0, 3) == TestChatGM)
                    {
                        GMDetect();
                    }
                }
                //end added by golfandsurf 6/21/2010: GMdetect

                //Detect skillups
                for (int i = FishChat.chatLogAdded - 1; i >= 0; --i)
                {
                    testLine = FishChat.chatLog[i].Text;
                    if (testLine.StartsWith(string.Format(Resources.TestChatFishingSkill, _Player.Name)))
                    {
                        if (testLine.EndsWith(Resources.TestChatFishingSkillPoints))
                        {
                            if (int.TryParse(testLine.Substring(testLine.LastIndexOf(Resources.SpaceChar) - 1, 1), out skillLast))
                            {
                                skillDecimalMin += skillLast;
                                skillDecimalMax += skillLast;
                                skillDecimalTotal += skillLast;
                            }
                            else
                            {
                                skillLast = 0;
                            }
                        }
                        else // Level up
                        {
                            int lastSpace = testLine.LastIndexOf(Resources.SpaceChar);
                            int.TryParse(testLine.Substring(lastSpace + 1, testLine.LastIndexOf(Resources.Period) - lastSpace - 1), out skillLevel);
                            skillDecimalMin = 0;
                            if (skillDecimalMax < 10) // Partial levelup previously recorded
                            {
                                skillDecimalMax = skillLast - 1;
                            }
                            else
                            {
                                skillDecimalMax %= 10;
                            }
                            skillLast = 0;
                        }
                    }
                    else if (skillLast > 0)
                    {
                        // Adjust for overestimated skill fraction when there's not a level up
                        if (skillDecimalMax >= 10)
                        {
                            skillDecimalMax = 9;
                        }
                        if (skillDecimalMin >= 10)
                        {
                            skillDecimalMin = 9;
                        }
                        skillLast = 0;
                    }
                }


                if (0 < FishChat.fishLogAdded)
                {
                    UpdateChatLogs(rtbFish, FishChat.fishLog, FishChat.fishLogAdded);
                }

                if (0 < FishChat.partyLogAdded)
                {
                    UpdateChatLogs(rtbParty, FishChat.partyLog, FishChat.partyLogAdded);

                    DoCustomChatActions(partyActions, FishChat.partyLogAdded, FishChat.partyLog, playerChatParty, Resources.StatusErrorChatParty, tabChatPageParty, Resources.GUIChatTabAlertParty);
                }

                if (0 < FishChat.shellLogAdded)
                {
                    UpdateChatLogs(rtbShell, FishChat.shellLog, FishChat.shellLogAdded);

                    DoCustomChatActions(shellActions, FishChat.shellLogAdded, FishChat.shellLog, playerChatLinkshell, Resources.StatusErrorChatLinkshell, tabChatPageLS, Resources.GUIChatTabAlertLinkshell);
                }

                if (0 < FishChat.tellLogAdded)
                {
                    UpdateChatLogs(rtbTell, FishChat.tellLog, FishChat.tellLogAdded);

                    int oldTellActions = tellActions;
                    if (cbTellDetect.Checked)
                    {
                        tellActions |= (int)ChatAction.Note;
                    }
                    DoCustomChatActions(tellActions, FishChat.tellLogAdded, FishChat.tellLog, playerChatTell, Resources.StatusErrorChatTell, tabChatPageTell, Resources.GUIChatTabAlertTell);
                    tellActions = oldTellActions;
                }
                if (0 < FishChat.sayLogAdded)
                {
                    UpdateChatLogs(rtbSay, FishChat.sayLog, FishChat.sayLogAdded);

                    DoCustomChatActions(sayActions, FishChat.sayLogAdded, FishChat.sayLog, playerChatSay, Resources.StatusErrorChatSay, tabChatPageSay, Resources.GUIChatTabAlertSay);
                }
                FishChat.Clear();  //clear ___LogAdded variables for next update
            });
        }

        private void DoCustomChatActions(int actions, int newCount, List<FFACE.ChatTools.ChatLine> chatLines, Regex testPrefix, string stopText, TabPage tabPage, string tabText)
        {
            // Do any custom say actions
            if (cbChatDetect.Checked && actions > 0)
            {
                string testLine;
                for (int i = newCount - 1; i >= 0; --i)
                {
                    testLine = chatLines[i].Text;
                    if (testPrefix.IsMatch(testLine))
                    {
                        if ((actions & (int)ChatAction.Stop) == (int)ChatAction.Stop)
                        {
                            Stop(false, stopText);
                        }
                        if ((actions & (int)ChatAction.Note) == (int)ChatAction.Note)
                        {
                            tabPage.Text = tabText;
                            statusStripMain.BackColor = chatLines[0].Color;
                        }
                        if ((actions & (int)ChatAction.Flash) == (int)ChatAction.Flash)
                        {
                            DoFlashWindow();
                        }
                        break;
                    }
                }
            }
        } // @ private void UpdateChat()

        private void UpdateChatLogs(RichTextBox rtb, List<FFACE.ChatTools.ChatLine> log, int linesToParse)
        {
            this.UIThread(delegate
            {
                if (0 < linesToParse)
                {
                    for (int i = (linesToParse - 1); i >= 0; --i)
                    {
                        try
                        {
                            rtb.SelectionStart = rtb.Text.Length;
                            rtb.SelectionColor = Color.SlateBlue;
                            rtb.SelectedText = log[i].Now;
                            rtb.SelectionColor = FishChat.BrightenColor(log[i]);
                            rtb.SelectedText = log[i].Text + Environment.NewLine;
                            rtb.SelectionStart = rtb.Text.Length - 1;
                            rtb.ScrollToCaret();
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            break;
                        }
                    }
                }
            });
        } // @ private void UpdateChatLogs(RichTextBox rtb, int linesToParse)

        public void UpdateDBLog(string line)
        {
            rtbDB.UIThread(delegate
            {
                if (!string.IsNullOrEmpty(line))
                {
                    rtbDB.SelectionStart = rtbDB.Text.Length;
                    rtbDB.SelectionColor = Color.SlateBlue;
                    rtbDB.SelectedText = DateTime.Now.ToString(FormatTimestampDB);
                    rtbDB.SelectionColor = Color.White;
                    rtbDB.SelectedText = line + Environment.NewLine;
                    rtbDB.SelectionStart = rtbDB.Text.Length - 1;
                    rtbDB.ScrollToCaret();
                }
            });
        } // private void UpdateDBLog(string line)

        private void UpdateStats()
        {
            this.UIThread(delegate
            {
                string statsRtfResult = FishStats.PrintStats();

                rtbStats.Clear();
                rtbStats.SelectedRtf = statsRtfResult;
            });
        } // @ private void UpdateStats()

        #endregion //Methods_ThreadSafe

        #region Methods_Advanced

        private int ScaleWidth(int width)
        {
            return (int)(width * currentScaleFactor.Width);
        }

        private int ScaleHeight(int height)
        {
            return (int)(height * currentScaleFactor.Height);
        }

        private void DoFlashWindow()
        {
            FlashWindow.Flash(this);
        }

        private void GMDetect()
        {
            if (cbGMdetectAutostop.Checked)
            {
                if (!cbStopSound.Checked && File.Exists(FileWarningWav))
                {
                    DoFlashWindow();
                    SoundPlayer spWave = new SoundPlayer(FileWarningWav);
                    spWave.Play();
                }

                Stop(false, Resources.StatusErrorGM);
            }
        }

        private void UpdateInfo()
        {
            //set rod and bait labels
            if (_FFACE == null)
            {
                lblRod.ForeColor = SystemColors.ControlText;
                lblRod.Text = GUILblBlank;
            }
            else if (string.IsNullOrEmpty(GetRodName(_FFACE.Item.GetEquippedItemID(EquipSlot.Range))))
            {
                lblRod.ForeColor = Color.Red;
                lblRod.Text = Resources.GUILblRodNone;
            }
            else
            {
                lblRod.ForeColor = SystemColors.ControlText;
                lblRod.Text = GetRodName(_FFACE.Item.GetEquippedItemID(EquipSlot.Range));
            }

            if (_FFACE == null)
            {
                lblBait.ForeColor = SystemColors.ControlText;
                lblBait.Text = GUILblBlank;
            }
            else if (string.IsNullOrEmpty(GetBaitName(_FFACE.Item.GetEquippedItemID(EquipSlot.Ammo))))
            {
                lblBait.ForeColor = Color.Red;
                lblBait.Text = Resources.GUILblBaitNone;
            }
            else
            {
                lblBait.ForeColor = SystemColors.ControlText;
                lblBait.Text = string.Format(GUIFormatLblBait, GetBaitName(_FFACE.Item.GetEquippedItemID(EquipSlot.Ammo)),
                    _FFACE.Item.GetInventoryItemCount((ushort) _FFACE.Item.GetEquippedItemID(EquipSlot.Ammo)) );
            }

            //set day icon and time (borrowed from Xi Slacker)
            switch (vanaNow.DayType)
            {
                case Weekday.Darksday:
                    lblVanaDay.Image = Resources.d_dark;
                    lblVanaDay.ForeColor = Color.DarkGray;
                    break;
                case Weekday.Earthsday:
                    lblVanaDay.Image = Resources.d_earth;
                    lblVanaDay.ForeColor = Color.Yellow;
                    break;
                case Weekday.Firesday:
                    lblVanaDay.Image = Resources.d_fire;
                    lblVanaDay.ForeColor = Color.DarkOrange;
                    break;
                case Weekday.Iceday:
                    lblVanaDay.Image = Resources.d_ice;
                    lblVanaDay.ForeColor = Color.LightBlue;
                    break;
                case Weekday.Lightningday:
                    lblVanaDay.Image = Resources.d_thunder;
                    lblVanaDay.ForeColor = Color.DarkMagenta;
                    break;
                case Weekday.Lightsday:
                    lblVanaDay.Image = Resources.d_light;
                    lblVanaDay.ForeColor = Color.LightGray;
                    break;
                case Weekday.Watersday:
                    lblVanaDay.Image = Resources.d_water;
                    lblVanaDay.ForeColor = Color.Blue;
                    break;
                case Weekday.Windsday:
                    lblVanaDay.Image = Resources.d_wind;
                    lblVanaDay.ForeColor = Color.LightGreen;
                    break;
            }

            // the rest
            lblVanaTime.Text = string.Format(GUIFormatVanaTime, vanaNow.Month, vanaNow.Day, vanaNow.Year, vanaNow.DayType, vanaNow.Hour, vanaNow.Minute.ToString(FormatTimestampVanaMinute), vanaNow.GetMoonPhaseName(vanaNow.MoonPhase), vanaNow.MoonPercent);
            lblEarthTime.Text = DateTime.Now.ToString(FormatTimestampEarthTime);
			string skillS = _FFACE == null ? GUILblBlank : Math.Max(_FFACE.Player.GetCraftDetails(Craft.Fishing).Level, skillLevel).ToString();
			if (_FFACE != null && (skillDecimalMin > 0 || skillDecimalMax > 0))
			{
				if (skillDecimalMin == skillDecimalMax)
				{
				    skillS += string.Format(GUIFormatLblSkillDecimal, skillDecimalMax);
				}
				else
				{
                    skillS += string.Format(GUIFormatLblSkillDecimalRange, skillDecimalMin, skillDecimalMax);
				}
			}
            if (_FFACE != null && skillDecimalTotal > 0)
            {
                skillS += string.Format(Resources.GUIFormatLblSkillTotal, skillDecimalTotal / 10, skillDecimalTotal % 10);
            }
            lblSkill.Text = skillS;

            if (_FFACE == null)
            {
                lblInventorySpace.Text = string.Format(GUIFormatLblInventory, GUILblBlank, GUILblBlank);
                lblGil.Text = GUILblBlank;
            }
            else if (_FFACE.Item.InventoryCount != -1)
            {
                lblInventorySpace.Text = string.Format(GUIFormatLblInventory, _FFACE.Item.InventoryCount, _FFACE.Item.InventoryMax);
                try
                {
                    lblGil.Text = string.Format(GUIFormatLblGil, _FFACE.Item.CurrentGil);
                }
                catch
                {
                    lblGil.Text = GUILblNA;
                }
            }
            else
            {
                lblInventorySpace.Text = GUILblNA;
                lblGil.Text = GUILblNA;
            }

            if (_FFACE == null)
            {
                lblSatchelSpace.Text = string.Format(GUIFormatLblInventory, GUILblBlank, GUILblBlank);
            }
            else if (_FFACE.Item.SatchelCount != -1)
            {
                lblSatchelSpace.Text = string.Format(GUIFormatLblInventory, _FFACE.Item.SatchelCount, _FFACE.Item.SatchelMax);
            }
            else
            {
                lblSatchelSpace.Text = GUILblNA;
            }

            if (_FFACE == null)
            {
                lblSackSpace.Text = string.Format(GUIFormatLblInventory, GUILblBlank, GUILblBlank);
            }
            else if (_FFACE.Item.SackCount != -1)
            {
                lblSackSpace.Text = string.Format(GUIFormatLblInventory, _FFACE.Item.SackCount, _FFACE.Item.SackMax);
            }
            else
            {
                lblSackSpace.Text = GUILblNA;
            }

            lblVanaClock.Text = string.Format(GUIFormatLblVanaClock, vanaNow.Hour, vanaNow.Minute.ToString(FormatTimestampVanaMinute));
            if (_FFACE != null)
            {
                SetLblZone(GetZoneName(_FFACE.Player.Zone));
            }
        }

        private void ExtendChat()
        {
            int formWidth = this.Width;

            if (chatbig)
            {
                btnResize.Text = GUIButtonResizeBig;
                pnlWanted.Visible = false;
                pnlUnwanted.Visible = false;
                btnStart.Visible = false;
                btnRefreshLists.Visible = false;
                cbCatchUnknown.Visible = false;
                tabDisplay.Location = new Point(0, 0);
                tabDisplay.Width = formWidth - ScaleWidth(15);

                tbChat.Width = formWidth - ScaleWidth(109);
                btnStartM.Visible = true;
                btnStartM.Location = new Point((formWidth - ScaleWidth(66)), -1);
                btnChatSend.Location = new Point((formWidth - ScaleWidth(109)), -1);
            }
            else
            {
                btnResize.Text = GUIButtonResizeSmall;
                pnlWanted.Visible = true;
                pnlUnwanted.Visible = true;
                btnStart.Visible = true;
                btnRefreshLists.Visible = true;
                cbCatchUnknown.Visible = true;
                // These have to be adjusted based on font scaling
                tabDisplay.Location = new Point(ScaleWidth(145), 0);
                tabDisplay.Width = formWidth - ScaleWidth(160);
                tbChat.Width = formWidth - ScaleWidth(233);
                btnStartM.Visible = false;
                btnChatSend.Location = new Point((formWidth - ScaleWidth(233)), -1);
            }

        }

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

        public static Point GetClosestOnScreenOffsetPoint(Point target)
        {
            double smallestDistance = double.NaN;
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

        public static Point GetNearestConnectedScreenWindowLocation()
        {
            Point location = Settings.Default.WindowLocation;
            Point lowerRight = Settings.Default.WindowLocation;
            lowerRight.Offset(Settings.Default.WindowSize.Width, Settings.Default.WindowSize.Height);
            // Adjust lower right to be on screen
            if (!ThisPointIsOnOneOfTheConnectedScreens(lowerRight))
            {
                Point offset1 = GetClosestOnScreenOffsetPoint(lowerRight);
                location.Offset(offset1);
            }
            // Adjust upper left to be on screen
            if (!ThisPointIsOnOneOfTheConnectedScreens(location))
            {
                Point offset2 = GetClosestOnScreenOffsetPoint(location);
                location.Offset(offset2);
            }

            return location;
        }

        #endregion //Methods_Advanced

        #endregion //Methods

        #region Events

        #region Events_Chat

        private const int bufferSize = 20;
        private static int bufferPosition = 0;
        private static Regex allSpaces = new Regex(RegexAllSpaces);
        private static List<string> chatBuffer = new List<string>(bufferSize);

        private void btnChatSend_Click(object sender, EventArgs e)
        {
            if (_FFACE == null)
            {
                SetStatus(Resources.StatusInfoNoProcess);
                return;
            }
            if ((0 < tbChat.Text.Length) && (!tbChat.Text.Equals(allSpaces)))
            {
                _FFACE.Windower.SendString(tbChat.Text);
                chatBuffer.Insert(0, tbChat.Text);
                bufferPosition = 0;
                tbChat.Clear();

                if (chatBuffer.Count > bufferSize)
                {
                    chatBuffer.RemoveRange(bufferSize, (chatBuffer.Count - bufferSize));
                    chatBuffer.TrimExcess();
                }
            }

        } // @ private void btnChatSend_Click

        private void tbChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Up == e.KeyCode)
            {
                if ((chatBuffer.Count > bufferPosition))
                {
                    switch (bufferPosition)
                    {
                        case 0:
                            if ((0 < tbChat.Text.Length) && (!tbChat.Text.Equals(allSpaces)))
                            {
                                chatBuffer.Insert(0, tbChat.Text);
                                tbChat.Text = chatBuffer[(bufferPosition + 1)];
                                bufferPosition += 2;
                            }
                            else
                            {
                                tbChat.Text = chatBuffer[bufferPosition];
                                bufferPosition++;
                            }

                            if (chatBuffer.Count > bufferSize)
                            {
                                chatBuffer.RemoveRange(bufferSize, (chatBuffer.Count - bufferSize));
                                chatBuffer.TrimExcess();
                            }

                            break;
                        case (bufferSize - 1):
                            break;
                        default:
                            tbChat.Text = chatBuffer[bufferPosition];
                            bufferPosition++;
                            break;
                    }
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            if (Keys.Down == e.KeyCode)
            {
                switch (bufferPosition)
                {
                    case 0:
                        tbChat.Clear();
                        break;
                    case 1:
                        tbChat.Clear();
                        bufferPosition--;
                        break;
                    default:
                        tbChat.Text = chatBuffer[(bufferPosition - 2)];
                        bufferPosition--;
                        break;
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            if (e.Modifiers == Keys.Control)
            {
                int caretPos = tbChat.SelectionStart;
                int selectionLength = tbChat.SelectionLength;
                int textLength = tbChat.TextLength;
                switch (e.KeyCode)
                {
                    case Keys.S:
                        tbChat.Text = CommandChatSay + chatMode.Replace(tbChat.Text, Resources.RegexGroup1);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.L:
                        tbChat.Text = CommandChatLinkshell + chatMode.Replace(tbChat.Text, Resources.RegexGroup1);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.P:
                        tbChat.Text = CommandChatParty + chatMode.Replace(tbChat.Text, Resources.RegexGroup1);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.R:
                        tbChat.Text = CommandChatReply + chatMode.Replace(tbChat.Text, Resources.RegexGroup1);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.T:
                        tbChat.Text = CommandChatTell + chatMode.Replace(tbChat.Text, Resources.RegexGroup1);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                }
                tbChat.SelectionStart = caretPos + tbChat.TextLength - textLength;
                tbChat.SelectionLength = selectionLength;
            }

        } // @ private void tbChat_KeyDown

        private void tbChat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char)Keys.Escape == e.KeyChar)
            {
                tbChat.Clear();
                e.Handled = true;
            }

            if ((char)Keys.Enter == e.KeyChar)
            {
                btnChatSend_Click(sender, EventArgs.Empty);

                e.Handled = true;
            }

        } // @ private void tbChat_KeyPress



        private void btnResize_Click(object sender, EventArgs e)
        {
            if (chatbig)
            {
                chatbig = false;
                ExtendChat();
            }
            else
            {
                chatbig = true;
                ExtendChat();
            }
        }

        private void tabChat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabChat.SelectedTab == tabChatPageTell)
            {
                tabChatPageTell.Text = Resources.GUIChatTabTell;
                statusStripMain.BackColor = SystemColors.Control;
            }
            else if (tabChat.SelectedTab == tabChatPageParty)
            {
                tabChatPageParty.Text = Resources.GUIChatTabParty;
                statusStripMain.BackColor = SystemColors.Control;
            }
            else if (tabChat.SelectedTab == tabChatPageLS)
            {
                tabChatPageLS.Text = Resources.GUIChatTabLinkshell;
                statusStripMain.BackColor = SystemColors.Control;
            }
            else if (tabChat.SelectedTab == tabChatPageSay)
            {
                tabChatPageSay.Text = Resources.GUIChatTabSay;
                statusStripMain.BackColor = SystemColors.Control;
            }
        }

        private void FishingForm_Activated(object sender, EventArgs e)
        {
            if (tabDisplay.SelectedTab == tabDisplayPageChat)
            {
                tabChat_SelectedIndexChanged(tabDisplayPageChat, EventArgs.Empty);
            }
        }

        private void tabDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabDisplay.SelectedTab == tabDisplayPageChat)
            {
                tabChat_SelectedIndexChanged(tabDisplayPageChat, EventArgs.Empty);
            }
        }

        #endregion //Events_Chat

        #region Events_Options

        #region General

        private void cbAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = cbAlwaysOnTop.Checked;

        } // @ private void cbAlwaysOnTop_CheckedChanged

        private void numCastIntervalHigh_ValueChanged(object sender, EventArgs e)
        {
            if (numCastIntervalHigh.Value < numCastIntervalLow.Value)
            {
                numCastIntervalLow.Value = numCastIntervalHigh.Value;
            }

        } // @ private void numCastIntervalHigh_ValueChanged

        private void numCastIntervalLow_ValueChanged(object sender, EventArgs e)
        {
            if (numCastIntervalLow.Value > numCastIntervalHigh.Value)
            {
                numCastIntervalHigh.Value = numCastIntervalLow.Value;
            }

        } // @ private void numCastIntervalLow_ValueChanged

        private void numFakeLargeIntervalHigh_ValueChanged(object sender, EventArgs e)
        {
            if (numFakeLargeIntervalLow.Value > numFakeLargeIntervalHigh.Value)
            {
                numFakeLargeIntervalLow.Value = numFakeLargeIntervalHigh.Value;
            }

        } // @ private void numFakeLargeIntervalHigh_ValueChanged

        private void numMaxNoCatch_ValueChanged(object sender, EventArgs e)
        {
            SetNoCatch((int)numMaxNoCatch.Value);

        } // @ private void numMaxNoCatch_ValueChanged

        private void trackOpacity_Scroll(object sender, EventArgs e)
        {
            if (10 == trackOpacity.Value)
            {
                this.Opacity = 0.99;
            }
            else
            {
                this.Opacity = (double)trackOpacity.Value / 10;
            }

        } // @ private void trackOpacity_Scroll

        #endregion //General

        #region Fight

        private void numFakeLargeIntervalLow_ValueChanged(object sender, EventArgs e)
        {
            if (numFakeLargeIntervalLow.Value > numFakeLargeIntervalHigh.Value)
            {
                numFakeLargeIntervalHigh.Value = numFakeLargeIntervalLow.Value;
            }

        } // @ private void numFakeLargeIntervalLow_ValueChanged

        private void numFakeSmallIntervalHigh_ValueChanged(object sender, EventArgs e)
        {
            if (numFakeSmallIntervalLow.Value > numFakeSmallIntervalHigh.Value)
            {
                numFakeSmallIntervalLow.Value = numFakeSmallIntervalHigh.Value;
            }

        } // @ private void numFakeSmallIntervalHigh_ValueChanged

        private void numFakeSmallIntervalLow_ValueChanged(object sender, EventArgs e)
        {
            if (numFakeSmallIntervalLow.Value > numFakeSmallIntervalHigh.Value)
            {
                numFakeSmallIntervalHigh.Value = numFakeSmallIntervalLow.Value;
            }

        } // @ private void numFakeSmallIntervalLow_ValueChanged

        private void numReactionHigh_ValueChanged(object sender, EventArgs e)
        {
            if (numReactionLow.Value > numReactionHigh.Value)
            {
                numReactionLow.Value = numReactionHigh.Value;
            }

        } // @ private void numReactionHigh_ValueChanged

        private void numReactionLow_ValueChanged(object sender, EventArgs e)
        {
            if (numReactionLow.Value < 0.5M)
            {
                numReactionHigh.Minimum = 0.5M;
            }
            else if (numReactionLow.Value > numReactionHigh.Value)
            {
                numReactionHigh.Value = numReactionLow.Value;
            }

        } // @ private void numReactionLow_ValueChanged

        #endregion //Fight

        #region Gear

        private void tbLRingGear_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLRingGear.Enabled = (tbLRingGear.SelectedIndex > 0 && tbLRingGear.SelectedIndex < 4);
        }

        private void tbRRingGear_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbRRingGear.Enabled = (tbRRingGear.SelectedIndex > 0 && tbRRingGear.SelectedIndex < 4);
        }

        private void tbWaistGear_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbWaistGear.Enabled = tbWaistGear.SelectedIndex == 2;
        }

        #endregion //Gear

        #region Other

        private void cbFullactionOther_CheckedChanged(object sender, EventArgs e)
        {
            tbFullactionOther.Enabled = numFullactionOtherTime.Enabled = cbFullactionOther.Checked;
            if (cbFullactionOther.Checked)
            {
                cbInventoryItemizerItemTools.Checked = false;
                if (OptionsConfigured)
                {
                    MessageBox.Show(string.Join(Environment.NewLine, MessageInventoryOther));
                }
            }
        }

        private void cbFullactionLogout_CheckedChanged(object sender, EventArgs e)
        {
			if (cbFullactionLogout.Checked)
			{
				cbFullactionShutdown.Checked = false;
			}
        }

        private void cbFullactionShutdown_CheckedChanged(object sender, EventArgs e)
        {
			if (cbFullactionShutdown.Checked)
			{
				cbFullactionLogout.Checked = false;
			}
        }

        private void cbBaitActionLogout_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBaitActionLogout.Checked)
            {
                cbBaitActionShutdown.Checked = false;
            }
        }

        private void cbBaitActionShutdown_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBaitActionShutdown.Checked)
            {
                cbBaitActionLogout.Checked = false;
            }
        }

        private void cbFatiguedActionLogout_CheckedChanged(object sender, EventArgs e)
        {
			if (cbFatiguedActionLogout.Checked)
			{
				cbFatiguedActionShutdown.Checked = false;
			}
        }

        private void cbFatiguedActionShutdown_CheckedChanged(object sender, EventArgs e)
        {
			if (cbFatiguedActionShutdown.Checked)
			{
				cbFatiguedActionLogout.Checked = false;
			}
        }

        private void cbEnableItemizerItemTools_CheckedChanged(object sender, EventArgs e)
        {
            cbInventoryItemizerSack.Enabled = cbInventoryItemizerSatchel.Enabled = cbInventoryItemizerItemTools.Checked;
            if (cbInventoryItemizerItemTools.Checked)
            {
                cbFullactionOther.Checked = false;
            }
        }

        private void cbBaitItemizerItemTools_CheckedChanged(object sender, EventArgs e)
        {
            cbBaitItemizerSack.Enabled = cbBaitItemizerSatchel.Enabled = cbBaitItemizerItemTools.Checked;
            if (cbBaitItemizerItemTools.Checked)
            {
                cbBaitactionOther.Checked = false;
            }
        }

        private void cbBaitactionOther_CheckedChanged(object sender, EventArgs e)
        {
            tbBaitactionOther.Enabled = numBaitactionOtherTime.Enabled = cbBaitactionOther.Checked;
            if (cbBaitactionOther.Checked)
            {
                cbBaitItemizerItemTools.Checked = false;
                if (OptionsConfigured)
                {
                    MessageBox.Show(string.Join(Environment.NewLine, MessageBaitOther));
                }
            }
        }

        private void cbFullActionStop_CheckedChanged(object sender, EventArgs e)
        {
            cbFullactionWarp.Enabled = cbFullActionStop.Checked;
            cbFullactionLogout.Enabled = cbFullActionStop.Checked;
            cbFullactionShutdown.Enabled = cbFullActionStop.Checked;
        }

        private static Size tbFullactionOtherSize;
        private static Size tbBaitactionOtherSize;

        private void tbFullactionOther_Enter(object sender, EventArgs e)
        {
            tbFullactionOtherSize = tbFullactionOther.Size;
            tbFullactionOther.Size = new Size(tbFullactionOtherSize.Width, tbFullactionOtherSize.Height + ScaleHeight(71));
        }

        private void tbFullactionOther_Leave(object sender, EventArgs e)
        {
            tbFullactionOther.Size = tbFullactionOtherSize;
        }

        private void tbBaitactionOther_Enter(object sender, EventArgs e)
        {
            tbBaitactionOtherSize = tbBaitactionOther.Size;
            tbBaitactionOther.Size = new Size(tbBaitactionOtherSize.Width, tbBaitactionOtherSize.Height + ScaleHeight(71));
        }

        private void tbBaitactionOther_Leave(object sender, EventArgs e)
        {
            tbBaitactionOther.Size = tbBaitactionOtherSize;
        }

        #endregion //Other

        #region Chat

        private void btnChatDetectAdd_Remove_click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text.Equals(GUIChatDetectButtonAdd))
            {
                int offset = 23 * chatDetectBtnChatRemoveList.Count;
                int taboffset = 5 * chatDetectBtnChatRemoveList.Count;
                // 
                // cmbChatAction
                // 
                ComboBox cmbChatAction = new ComboBox();
                cmbChatAction.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbChatAction.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbChatAction.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbChatAction.FormattingEnabled = true;
                cmbChatAction.Items.AddRange(new object[] {
            Resources.GUIChatDetectActionStop,
            Resources.GUIChatDetectActionNote,
            Resources.GUIChatDetectActionFlash});
                cmbChatAction.Location = new Point(ScaleWidth(136), ScaleHeight(2 + offset + panelChatDetect.AutoScrollPosition.Y));
                cmbChatAction.Name = "cmbChatAction";
                cmbChatAction.Size = new Size(ScaleWidth(83), ScaleHeight(20));
                cmbChatAction.TabIndex = 3 + taboffset;
                cmbChatAction.SelectedIndexChanged += new System.EventHandler(this.cmbChatDetection_SelectedIndexChanged);
                chatDetectCmbChatActionList.Add(cmbChatAction);
                // 
                // cmbChatType
                // 
                ComboBox cmbChatType = new ComboBox();
                cmbChatType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbChatType.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbChatType.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbChatType.FormattingEnabled = true;
                cmbChatType.Items.AddRange(new object[] {
            Resources.GUIChatDetectTypeTell,
            Resources.GUIChatDetectTypeParty,
            Resources.GUIChatDetectTypeLinkshell,
            Resources.GUIChatDetectTypeSay});
                cmbChatType.Location = new Point(ScaleWidth(23), ScaleHeight(2 + offset + panelChatDetect.AutoScrollPosition.Y));
                cmbChatType.Name = "cmbChatType";
                cmbChatType.Size = new Size(ScaleWidth(61), ScaleHeight(20));
                cmbChatType.TabIndex = 1 + taboffset;
                cmbChatType.SelectedIndexChanged += new System.EventHandler(this.cmbChatDetection_SelectedIndexChanged);
                chatDetectCmbChatTypeList.Add(cmbChatType);
                // 
                // lblChatOn
                // 
                Label lblChatOn = new Label();
                lblChatOn.AutoSize = true;
                lblChatOn.Location = new Point(ScaleWidth(2), ScaleHeight(5 + offset + panelChatDetect.AutoScrollPosition.Y));
                lblChatOn.Name = "lblChatOn";
                lblChatOn.Size = new Size(ScaleWidth(21), ScaleHeight(13));
                lblChatOn.TabIndex = 0 + taboffset;
                lblChatOn.Text = "On";
                chatDetectLblOnList.Add(lblChatOn);
                // 
                // btnRemoveChatDetect
                // 
                Button btnRemoveChatDetect = new Button();
                btnRemoveChatDetect.Location = new Point(ScaleWidth(220), ScaleHeight(offset + panelChatDetect.AutoScrollPosition.Y));
                btnRemoveChatDetect.Name = "btnRemoveChatDetect";
                btnRemoveChatDetect.Size = new Size(ScaleWidth(23), ScaleHeight(23));
                btnRemoveChatDetect.TabIndex = 4 + taboffset;
                btnRemoveChatDetect.Text = GUIChatDetectButtonRemove;
                btnRemoveChatDetect.UseVisualStyleBackColor = true;
                btnRemoveChatDetect.Click += new System.EventHandler(this.btnChatDetectAdd_Remove_click);
                chatDetectBtnChatRemoveList.Add(btnRemoveChatDetect);
                // 
                // lblChatReceived
                // 
                Label lblChatReceived = new Label();
                lblChatReceived.AutoSize = true;
                lblChatReceived.Location = new Point(ScaleWidth(84), ScaleHeight(5 + offset + panelChatDetect.AutoScrollPosition.Y));
                lblChatReceived.Name = "lblChatReceived";
                lblChatReceived.Size = new Size(ScaleWidth(54), ScaleHeight(13));
                lblChatReceived.TabIndex = 2 + taboffset;
                lblChatReceived.Text = "Received,";
                chatDetectLblReceivedList.Add(lblChatReceived);

                panelChatDetect.SuspendLayout();
                // Move Add button down
                btnChatDetectAdd.Location = new Point(btnChatDetectAdd.Location.X, ScaleHeight(23 + offset + panelChatDetect.AutoScrollPosition.Y));
                btnChatDetectAdd.TabIndex = 5 + taboffset;

                panelChatDetect.Controls.Add(cmbChatAction);
                panelChatDetect.Controls.Add(cmbChatType);
                panelChatDetect.Controls.Add(lblChatOn);
                panelChatDetect.Controls.Add(btnRemoveChatDetect);
                panelChatDetect.Controls.Add(lblChatReceived);
                lblChatOn.Focus();
                panelChatDetect.ResumeLayout();
            }
            else
            {
                int index = (int)Math.Round(((double)btn.Location.Y - 17) / 23);

                panelChatDetect.SuspendLayout();
                for (int i = index + 1; i < chatDetectBtnChatRemoveList.Count; ++i)
                {
                    chatDetectLblOnList[i].Location = new Point(chatDetectLblOnList[i].Location.X, chatDetectLblOnList[i].Location.Y - ScaleHeight(23));
                    chatDetectCmbChatTypeList[i].Location = new Point(chatDetectCmbChatTypeList[i].Location.X, chatDetectCmbChatTypeList[i].Location.Y - ScaleHeight(23));
                    chatDetectLblReceivedList[i].Location = new Point(chatDetectLblReceivedList[i].Location.X, chatDetectLblReceivedList[i].Location.Y - ScaleHeight(23));
                    chatDetectCmbChatActionList[i].Location = new Point(chatDetectCmbChatActionList[i].Location.X, chatDetectCmbChatActionList[i].Location.Y - ScaleHeight(23));
                    chatDetectBtnChatRemoveList[i].Location = new Point(chatDetectBtnChatRemoveList[i].Location.X, chatDetectBtnChatRemoveList[i].Location.Y - ScaleHeight(23));
                }
                btnChatDetectAdd.Location = new Point(btnChatDetectAdd.Location.X, btnChatDetectAdd.Location.Y - ScaleHeight(23));

                panelChatDetect.Controls.Remove(chatDetectLblOnList[index]);
                panelChatDetect.Controls.Remove(chatDetectCmbChatTypeList[index]);
                panelChatDetect.Controls.Remove(chatDetectLblReceivedList[index]);
                panelChatDetect.Controls.Remove(chatDetectCmbChatActionList[index]);
                panelChatDetect.Controls.Remove(chatDetectBtnChatRemoveList[index]);
                panelChatDetect.ResumeLayout();

                chatDetectLblOnList.Remove(chatDetectLblOnList[index]);
                chatDetectCmbChatTypeList.Remove(chatDetectCmbChatTypeList[index]);
                chatDetectLblReceivedList.Remove(chatDetectLblReceivedList[index]);
                chatDetectCmbChatActionList.Remove(chatDetectCmbChatActionList[index]);
                chatDetectBtnChatRemoveList.Remove(chatDetectBtnChatRemoveList[index]);

                cmbChatDetection_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        private void panelChatDetect_Scroll(object sender, ScrollEventArgs e)
        {
            panelChatDetect.Focus();
        }

        private void cmbChatDetection_SelectedIndexChanged(object sender, EventArgs e)
        {
            tellActions = 0;
            partyActions = 0;
            shellActions = 0;
            sayActions = 0;
            string type;
            string action;
            ChatAction tmpAction = 0;
            for (int i = 0; i < chatDetectBtnChatRemoveList.Count; ++i)
            {
                if (chatDetectCmbChatTypeList[i].SelectedItem == null)
                {
                    continue;
                }
                type = chatDetectCmbChatTypeList[i].SelectedItem.ToString();
                if (chatDetectCmbChatActionList[i].SelectedItem == null)
                {
                    continue;
                }
                action = chatDetectCmbChatActionList[i].SelectedItem.ToString();
                if (action.Equals(Resources.GUIChatDetectActionStop))
                {
                    tmpAction = ChatAction.Stop;
                }
                else if (action.Equals(Resources.GUIChatDetectActionNote))
                {
                    tmpAction = ChatAction.Note;
                }
                else if (action.Equals(Resources.GUIChatDetectActionFlash))
                {
                    tmpAction = ChatAction.Flash;
                }
                if (type.Equals(Resources.GUIChatDetectTypeTell))
                {
                    tellActions |= (int)tmpAction;
                }
                else if (type.Equals(Resources.GUIChatDetectTypeLinkshell))
                {
                    shellActions |= (int)tmpAction;
                }
                else if (type.Equals(Resources.GUIChatDetectTypeParty))
                {
                    partyActions |= (int)tmpAction;
                }
                else if (type.Equals(Resources.GUIChatDetectTypeSay))
                {
                    sayActions |= (int)tmpAction;
                }
            }
        }

        #endregion //Chat

        #endregion //Events_Options

        #region Events_Miscellaneous

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (Resources.GUIButtonStop == btnStart.Text)
            {
                stopSound = false;
                statusWarningColor = false;
                bool storeChatBig = chatbig;
                Stop(false, Resources.StatusErrorIdle);
                chatbig = storeChatBig;
            }
            else
            {
                btnStart.BackColor = SystemColors.Control;
                btnStart.ForeColor = SystemColors.ControlText;
                statusStripMain.BackColor = SystemColors.Control;
                statusStripMain.ForeColor = SystemColors.ControlText;

				if (!CheckProcess())
				{
					return;
				}

                Start();
            }
        }

        private void lblStatusTitle_Click(object sender, EventArgs e)
        {
            if (btnStart.BackColor == Color.Red && statusStripMain.BackColor == Color.Red)
            {
                btnStart.BackColor = SystemColors.Control;
                btnStart.ForeColor = SystemColors.ControlText;
                statusStripMain.BackColor = SystemColors.Control;
                statusStripMain.ForeColor = SystemColors.ControlText;
            }
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FishStats.Clear();
            UpdateStats();

        } // @ private void clearLogToolStripMenuItem_Click

        private void progressBarST_Click(object sender, EventArgs e)
        {
            if (_FFACE.Player.Status == Status.FishBite && _FFACE.Fish.HPCurrent > 0 && _FFACE.Fish.FishOnLine)
            {
                _FFACE.Fish.SetHP(0);
            }
        }

        private void rtbChat_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void rtbTell_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void rtbParty_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void rtbShell_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void rtbSay_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void rtbDB_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void btnRefreshLists_Click(object sender, EventArgs e)
        {
            if (IsRodBaitSet())
            {
                PopulateLists();
                SetStatus(Resources.StatusInfoListsRefreshed);
            }
            else
            {

                SetStatus(Resources.StatusInfoEquipToRefresh);
            }
        }

        private void btnCastReset_Click(object sender, EventArgs e)
        {
            Settings.Default.CastMax = numCastIntervalHigh.Value = 3.5M;
            Settings.Default.CastMin = numCastIntervalLow.Value = 3.0M;
        }

        #endregion //Events_Miscellaneous

        #region Events_Settings

        private void btnSettingsReset_Click(object sender, EventArgs e)
        {
            string message = Resources.MessageSettingsReset;
            string caption = Resources.MessageTitleSettingsReset;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);

            if (DialogResult.Yes == result)
            {
                Settings.Default.Reset();
                Settings.Default.Reload();

                Settings.Default.AlwaysOnTop = cbAlwaysOnTop.Checked = true;
                Settings.Default.MaxCatch = cbMaxCatch.Checked = false;
                Settings.Default.MaxCatchValue = numMaxCatch.Value = 200;
                Settings.Default.SkillCap = cbSkillCap.Checked = false;
                Settings.Default.SkillCapLevel = numSkillCap.Value = 110;
                Settings.Default.CastMax = numCastIntervalHigh.Value = 3.5M;
                Settings.Default.CastMin = numCastIntervalLow.Value = 3.0M;
                Settings.Default.CatchUnknown = cbCatchUnknown.Checked = false;
                Settings.Default.Opacity = trackOpacity.Value = 10;
                this.Opacity = (double)trackOpacity.Value / 10;
                Settings.Default.FakeLarge = cbReleaseLarge.Checked = false;
                Settings.Default.FakeLargeMax = 100;
                Settings.Default.FakeLargeMin = 90;
                Settings.Default.FakeSmall = cbReleaseSmall.Checked = false;
                Settings.Default.FakeSmallMax = 75;
                Settings.Default.FakeSmallMin = 50;
                Settings.Default.MaxNoCatch = 20;
                Settings.Default.ReactionMax = numReactionHigh.Value = 2.0M;
                Settings.Default.ReactionMin = numReactionLow.Value = 0.5M;
                Settings.Default.Reaction = cbReaction.Checked = false;
                Settings.Default.AutoKill = cbAutoKill.Checked = true;
                Settings.Default.Extend = cbExtend.Checked = false;
                Settings.Default.QuickKill = cbQuickKill.Checked = false;
                Settings.Default.QuickKillValue = numQuickKill.Value = 15;
                Settings.Default.FullActionItemizer = cbInventoryItemizerItemTools.Checked = false;
                Settings.Default.FullActionSack = cbInventoryItemizerSack.Checked = false;
                Settings.Default.FullActionSatchel = cbInventoryItemizerSatchel.Checked = false;
                Settings.Default.FullActionOtherText = tbFullactionOther.Text = "";
                Settings.Default.FullActionOtherTime = numFullactionOtherTime.Value = 1.0m;
                Settings.Default.FullActionOther = cbFullactionOther.Checked = false;
                Settings.Default.FullActionWarp = cbFullactionWarp.Checked = false;
                Settings.Default.FullActionStop = cbFullActionStop.Checked = true;
                Settings.Default.FullActionLogout = cbFullactionLogout.Checked = false;
                Settings.Default.FullActionShutdown = cbFullactionShutdown.Checked = false;
                Settings.Default.IgnoreItems = cbIgnoreItem.Checked = true;
                Settings.Default.IgnoreMonsters = cbIgnoreMonster.Checked = true;
                Settings.Default.IgnoreSmallFish = cbIgnoreSmallFish.Checked = false;
                Settings.Default.IgnoreLargeFish = cbIgnoreLargeFish.Checked = false;
                Settings.Default.RodGear = tbRodGear.SelectedIndex = 0;
                Settings.Default.BaitGear = tbBaitGear.SelectedIndex = 0;
				Settings.Default.BodyGear = tbBodyGear.SelectedIndex = 0;
				Settings.Default.HandsGear = tbHandsGear.SelectedIndex = 0;
				Settings.Default.LegsGear = tbLegsGear.SelectedIndex = 0;
				Settings.Default.FeetGear = tbFeetGear.SelectedIndex = 0;
				Settings.Default.LRingGear = tbLRingGear.SelectedIndex = 0;
				Settings.Default.LRingEnchantment = cbLRingGear.Checked = true;
				Settings.Default.RRingGear = tbRRingGear.SelectedIndex = 0;
				Settings.Default.RRingEnchantment = cbRRingGear.Checked = true;
				Settings.Default.HeadGear = tbHeadGear.SelectedIndex = 0;
				Settings.Default.NeckGear = tbNeckGear.SelectedIndex = 0;
				Settings.Default.WaistGear = tbWaistGear.SelectedIndex = 0;
				Settings.Default.WaistEnchantment = cbWaistGear.Checked = true;
                Settings.Default.GMAutoStop = cbGMdetectAutostop.Checked = true;
                Settings.Default.BaitShutdown = cbBaitActionShutdown.Checked = false;
                Settings.Default.BaitLogout = cbBaitActionLogout.Checked = false;
                Settings.Default.BaitWarp = cbBaitActionWarp.Checked = false;
                Settings.Default.BaitItemizer = cbBaitItemizerItemTools.Checked = false;
                Settings.Default.BaitSack = cbBaitItemizerSack.Checked = false;
                Settings.Default.BaitSatchel = cbBaitItemizerSatchel.Checked = false;
                Settings.Default.BaitOther = cbBaitactionOther.Checked = false;
                Settings.Default.BaitOtherText = tbBaitactionOther.Text = "";
                Settings.Default.BaitOtherTime = numBaitactionOtherTime.Value = 1.0m;
				Settings.Default.FatiguedShutdown = cbFatiguedActionShutdown.Checked = false;
				Settings.Default.FatiguedLogout = cbFatiguedActionLogout.Checked = false;
				Settings.Default.FatiguedWarp = cbFatiguedActionWarp.Checked = false;
                Settings.Default.StopSound = cbStopSound.Checked = false;
                Settings.Default.TellDetect = cbTellDetect.Checked = true;
                Settings.Default.ShowFishHP = cbFishHP.Checked = true;
                Settings.Default.SneakFishing = cbSneakFishing.Checked = false;

                // Chat detect settings
                Settings.Default.ChatDetectEnabled = cbChatDetect.Checked = true;
                Settings.Default.ChatDetect = new int[] {};
                for (int i = 0; i < chatDetectBtnChatRemoveList.Count; ++i)
                {
                    btnChatDetectAdd_Remove_click(chatDetectBtnChatRemoveList[0], MouseEventArgs.Empty);
                }

                if (null != Settings.Default.WindowSize)
                {
                    this.Size = Settings.Default.WindowSize = new Size(540, 255);
                }

                SetNoCatch((int)Settings.Default.MaxNoCatch);
                this.TopMost = true;
            }

        } // @ private void btnSettingsReset_Click

        private void btnSettingsSave_Click(object sender, EventArgs e)
        {
            string message = Resources.MessageSettingsSave;
            string caption = Resources.MessageTitleSettingsSave;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);

            if (DialogResult.Yes == result)
            {
                Settings.Default.AlwaysOnTop = cbAlwaysOnTop.Checked;
                Settings.Default.MaxCatch = cbMaxCatch.Checked;
                Settings.Default.MaxCatchValue = numMaxCatch.Value;
                Settings.Default.SkillCap = cbSkillCap.Checked;
                Settings.Default.SkillCapLevel = numSkillCap.Value;
                Settings.Default.CastMax = numCastIntervalHigh.Value;
                Settings.Default.CastMin = numCastIntervalLow.Value;
                Settings.Default.CatchUnknown = cbCatchUnknown.Checked;
                Settings.Default.Opacity = trackOpacity.Value;
                Settings.Default.FakeLarge = cbReleaseLarge.Checked;
                Settings.Default.FakeLargeMax = numFakeLargeIntervalHigh.Value;
                Settings.Default.FakeLargeMin = numFakeLargeIntervalLow.Value;
                Settings.Default.FakeSmall = cbReleaseSmall.Checked;
                Settings.Default.FakeSmallMax = numFakeSmallIntervalHigh.Value;
                Settings.Default.FakeSmallMin = numFakeSmallIntervalLow.Value;
                Settings.Default.MaxNoCatch = numMaxNoCatch.Value;
                Settings.Default.Reaction = cbReaction.Checked;
                Settings.Default.ReactionMax = numReactionHigh.Value;
                Settings.Default.ReactionMin = numReactionLow.Value;
                Settings.Default.FullActionOtherText = tbFullactionOther.Text;
                Settings.Default.FullActionItemizer = cbInventoryItemizerItemTools.Checked;
                Settings.Default.FullActionSack = cbInventoryItemizerSack.Checked;
                Settings.Default.FullActionSatchel = cbInventoryItemizerSatchel.Checked;
				Settings.Default.FullActionOther = cbFullactionOther.Checked;
                Settings.Default.FullActionOtherTime = numFullactionOtherTime.Value;
                Settings.Default.FullActionStop = cbFullActionStop.Checked;
				Settings.Default.FullActionWarp = cbFullactionWarp.Checked;
				Settings.Default.FullActionLogout = cbFullactionLogout.Checked;
                Settings.Default.FullActionShutdown = cbFullactionShutdown.Checked;
                Settings.Default.RodGear = tbRodGear.SelectedIndex;
                Settings.Default.BaitGear = tbBaitGear.SelectedIndex;
				Settings.Default.BodyGear = tbBodyGear.SelectedIndex;
				Settings.Default.HandsGear = tbHandsGear.SelectedIndex;
				Settings.Default.LegsGear = tbLegsGear.SelectedIndex;
				Settings.Default.FeetGear = tbFeetGear.SelectedIndex;
				Settings.Default.LRingGear = tbLRingGear.SelectedIndex;
				Settings.Default.LRingEnchantment = cbLRingGear.Checked;
				Settings.Default.RRingGear = tbRRingGear.SelectedIndex;
				Settings.Default.RRingEnchantment = cbRRingGear.Checked;
				Settings.Default.HeadGear = tbHeadGear.SelectedIndex;
				Settings.Default.NeckGear = tbNeckGear.SelectedIndex;
				Settings.Default.WaistGear = tbWaistGear.SelectedIndex;
				Settings.Default.WaistEnchantment = cbWaistGear.Checked;
                Settings.Default.GMAutoStop = cbGMdetectAutostop.Checked;
                Settings.Default.BaitShutdown = cbBaitActionShutdown.Checked;
                Settings.Default.BaitLogout = cbBaitActionLogout.Checked;
                Settings.Default.BaitWarp = cbBaitActionWarp.Checked;
                Settings.Default.BaitItemizer = cbBaitItemizerItemTools.Checked;
                Settings.Default.BaitSack = cbBaitItemizerSack.Checked;
                Settings.Default.BaitSatchel = cbBaitItemizerSatchel.Checked;
                Settings.Default.BaitOther = cbBaitactionOther.Checked;
                Settings.Default.BaitOtherText = tbBaitactionOther.Text;
                Settings.Default.BaitOtherTime = numBaitactionOtherTime.Value;
				Settings.Default.FatiguedShutdown = cbFatiguedActionShutdown.Checked;
				Settings.Default.FatiguedLogout = cbFatiguedActionLogout.Checked;
				Settings.Default.FatiguedWarp = cbFatiguedActionWarp.Checked;
                Settings.Default.StopSound = cbStopSound.Checked;
                Settings.Default.AutoKill = cbAutoKill.Checked;
                Settings.Default.Extend = cbExtend.Checked;
                Settings.Default.QuickKill = cbQuickKill.Checked;
                Settings.Default.QuickKillValue = numQuickKill.Value;
                Settings.Default.IgnoreItems = cbIgnoreItem.Checked;
                Settings.Default.IgnoreMonsters = cbIgnoreMonster.Checked;
                Settings.Default.IgnoreSmallFish = cbIgnoreSmallFish.Checked;
                Settings.Default.IgnoreLargeFish = cbIgnoreLargeFish.Checked;
                Settings.Default.TellDetect = cbTellDetect.Checked;
                Settings.Default.ShowFishHP = cbFishHP.Checked;
                Settings.Default.SneakFishing = cbSneakFishing.Checked;

                // Chat detect settings
                List<int> chatDetects = new List<int>(chatDetectBtnChatRemoveList.Count * 2);
                for (int i = 0; i < chatDetectBtnChatRemoveList.Count; ++i)
                {
                    chatDetects.Add(chatDetectCmbChatTypeList[i].SelectedIndex);
                    chatDetects.Add(chatDetectCmbChatActionList[i].SelectedIndex);
                }
                Settings.Default.ChatDetect = chatDetects.ToArray();
                Settings.Default.ChatDetectEnabled = cbChatDetect.Checked;

                if (FormWindowState.Normal == this.WindowState)
                {
                    Settings.Default.WindowSize = this.Size;
                    Settings.Default.WindowLocation = this.Location;
                }
                else
                {
                    Settings.Default.WindowSize = this.RestoreBounds.Size;
                    Settings.Default.WindowLocation = this.RestoreBounds.Location;
                }

                Settings.Default.Save();
            }

        } // @ private void btnSettingsSave_Click

        private void FishingForm_Load(object sender, EventArgs e)
        {
            cbAlwaysOnTop.Checked = Settings.Default.AlwaysOnTop;
            cbMaxCatch.Checked = Settings.Default.MaxCatch;
            numMaxCatch.Value = Settings.Default.MaxCatchValue;
            cbSkillCap.Checked = Settings.Default.SkillCap;
            numSkillCap.Value = Settings.Default.SkillCapLevel;
            numCastIntervalHigh.Value = Settings.Default.CastMax;
            numCastIntervalLow.Value = Settings.Default.CastMin;
            cbCatchUnknown.Checked = Settings.Default.CatchUnknown;
            trackOpacity.Value = Settings.Default.Opacity;
            this.Opacity = (double)trackOpacity.Value / 10;
            cbReleaseLarge.Checked = Settings.Default.FakeLarge;
            numFakeLargeIntervalHigh.Value = Settings.Default.FakeLargeMax;
            numFakeLargeIntervalLow.Value = Settings.Default.FakeLargeMin;
            cbReleaseSmall.Checked = Settings.Default.FakeSmall;
            numFakeSmallIntervalHigh.Value = Settings.Default.FakeSmallMax;
            numFakeSmallIntervalLow.Value = Settings.Default.FakeSmallMin;
            numMaxNoCatch.Value = Settings.Default.MaxNoCatch;
            cbReaction.Checked = Settings.Default.Reaction;
            numReactionHigh.Value = Settings.Default.ReactionMax;
            numReactionLow.Value = Settings.Default.ReactionMin;
            tbFullactionOther.Text = Settings.Default.FullActionOtherText;
            numFullactionOtherTime.Value = Settings.Default.FullActionOtherTime;
            cbInventoryItemizerItemTools.Checked = Settings.Default.FullActionItemizer;
            cbInventoryItemizerSack.Checked = Settings.Default.FullActionSack;
            cbInventoryItemizerSatchel.Checked = Settings.Default.FullActionSatchel;
			cbFullactionOther.Checked = Settings.Default.FullActionOther;
            cbFullActionStop.Checked = Settings.Default.FullActionStop;
			cbFullactionWarp.Checked = Settings.Default.FullActionWarp;
			cbFullactionLogout.Checked = Settings.Default.FullActionLogout;
            cbFullactionShutdown.Checked = Settings.Default.FullActionShutdown;
            tbRodGear.SelectedIndex = Settings.Default.RodGear;
            tbBaitGear.SelectedIndex = Settings.Default.BaitGear;
			tbBodyGear.SelectedIndex = Settings.Default.BodyGear;
			tbHandsGear.SelectedIndex = Settings.Default.HandsGear;
			tbLegsGear.SelectedIndex = Settings.Default.LegsGear;
			tbFeetGear.SelectedIndex = Settings.Default.FeetGear;
			tbLRingGear.SelectedIndex = Settings.Default.LRingGear;
			cbLRingGear.Checked = Settings.Default.LRingEnchantment;
			tbRRingGear.SelectedIndex = Settings.Default.RRingGear;
			cbRRingGear.Checked = Settings.Default.RRingEnchantment;
			tbHeadGear.SelectedIndex = Settings.Default.HeadGear;
			tbNeckGear.SelectedIndex = Settings.Default.NeckGear;
			tbWaistGear.SelectedIndex = Settings.Default.WaistGear;
			cbWaistGear.Checked = Settings.Default.WaistEnchantment;
            cbGMdetectAutostop.Checked = Settings.Default.GMAutoStop;
            cbBaitActionShutdown.Checked = Settings.Default.BaitShutdown;
            cbBaitActionLogout.Checked = Settings.Default.BaitLogout;
            cbBaitActionWarp.Checked = Settings.Default.BaitWarp;
            cbBaitItemizerItemTools.Checked = Settings.Default.BaitItemizer;
            cbBaitItemizerSack.Checked = Settings.Default.BaitSack;
            cbBaitItemizerSatchel.Checked = Settings.Default.BaitSatchel;
            cbBaitactionOther.Checked = Settings.Default.BaitOther;
            tbBaitactionOther.Text = Settings.Default.BaitOtherText;
            numBaitactionOtherTime.Value = Settings.Default.BaitOtherTime;
			cbFatiguedActionShutdown.Checked = Settings.Default.FatiguedShutdown;
			cbFatiguedActionLogout.Checked = Settings.Default.FatiguedLogout;
			cbFatiguedActionWarp.Checked = Settings.Default.FatiguedWarp;
            cbStopSound.Checked = Settings.Default.StopSound;
            cbAutoKill.Checked = Settings.Default.AutoKill;
            cbExtend.Checked = Settings.Default.Extend;
            cbQuickKill.Checked = Settings.Default.QuickKill;
            numQuickKill.Value = Settings.Default.QuickKillValue;
            cbIgnoreItem.Checked = Settings.Default.IgnoreItems;
            cbIgnoreMonster.Checked = Settings.Default.IgnoreMonsters;
            cbIgnoreSmallFish.Checked = Settings.Default.IgnoreSmallFish;
            cbIgnoreLargeFish.Checked = Settings.Default.IgnoreLargeFish;
            cbTellDetect.Checked = Settings.Default.TellDetect;
            cbSneakFishing.Checked = Settings.Default.SneakFishing;

            // Chat detect settings
            cbChatDetect.Checked = Settings.Default.ChatDetectEnabled;
            List<int> chatDetects = Settings.Default.ChatDetect.ToList();
            for (int i = 0; i < chatDetects.Count / 2; ++i)
            {
                btnChatDetectAdd_Remove_click(btnChatDetectAdd, MouseEventArgs.Empty);
                chatDetectCmbChatTypeList[i].SelectedIndex = chatDetects[i * 2];
                chatDetectCmbChatActionList[i].SelectedIndex = chatDetects[i * 2 + 1];
            }

            if (false == Settings.Default.AlwaysOnTop)
            {
                this.TopMost = false;
            }

            if (null != Settings.Default.WindowSize)
            {
                this.Size = Settings.Default.WindowSize;
            }

            RestoreLocation();

            SetNoCatch((int)numMaxNoCatch.Value);

            ExtendChat();

            OptionsConfigured = true;

        } // @ private void FishingForm_Load

        #endregion //Events_Settings

        #region Events_Timers

        private static FFACE.TimerTools.VanaTime vanaNow = new FFACE.TimerTools.VanaTime();
		private static DateTime japanNextMidnight;

        private void timer_Tick(object sender, EventArgs e)
        {
            int workDone = FishChat.NewChat();
            if (_FFACE != null && _Process != null && _Process.IsAvailable)
            {
                currentStatus = _FFACE.Player.Status;
                vanaNow = _FFACE.Timer.GetVanaTime();
            }
            // No FFACE, so calculate vanaNow based on system clock
            else
            {
                // Reset FFACE
                if (_FFACE != null || _Process != null)
                {
                    workDone = -1;
                }

                // get the server time (in seconds) based on local clock (inaccurate)
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                TimeSpan diff = DateTime.UtcNow - origin;
                long baseTime = (long)Math.Floor(diff.TotalSeconds);

                // calculate the difference between server time and 
                // 1/1/1970 00:00:00 unix time -> "Vana'Diel time in seconds"
                long timeInSeconds = ((long)baseTime + 92514960) * 25;

                // how many days
                decimal dayOfYear = Math.Floor((decimal)(timeInSeconds / (decimal)86400));

                vanaNow = new FFACETools.FFACE.TimerTools.VanaTime();
                vanaNow.DayType = (Weekday)(dayOfYear % 8);
                vanaNow.Day = (byte)((dayOfYear % 30) + 1);
                vanaNow.Month = (byte)(((dayOfYear % 360) / 30) + 1);
                vanaNow.Year = (short)(dayOfYear / 360);
                vanaNow.Hour = (byte)((timeInSeconds / 3600) % 24);
                vanaNow.Minute = (byte)((timeInSeconds / 60) % 60);
                // can't floor on a long, so need to shrink it first
                vanaNow.Second = (byte)((timeInSeconds - (System.Math.Floor((decimal)(timeInSeconds / 60)) * 60)));

                // calculate moon phase/percent
                decimal moonPhase = (dayOfYear + (decimal)26) % 84;

                // calculate moon percent
                decimal moonPercent = (((42 - moonPhase) * 100) / 42);
                if (0 > moonPercent)
                    moonPercent = Math.Abs(moonPercent);

                // get final moon percent
                vanaNow.MoonPercent = (byte)Math.Floor((moonPercent + (decimal)0.5));

                // get final moon phase
                if (38 <= moonPhase)
                {
                    vanaNow.MoonPhase = (MoonPhase)Math.Floor((moonPhase - (decimal)38) / (decimal)7);
                }
                else
                {
                    vanaNow.MoonPhase = (MoonPhase)Math.Floor((moonPhase + (decimal)46) / (decimal)7);
                }
            }

            if (-1 == workDone)  //error code from FishChat.NewChat() means chat failed
            {
                timer.Stop();
                Reattach();  //start a new instance of FFACETools to recover
                timer.Start();
            }
            else if (0 < workDone)  //some work was done
            {
                UpdateChat();
            }

			DateTime japanNow = _FFACE != null ? _FFACE.Timer.ServerTimeUTC : DateTime.UtcNow;
            japanNow = japanNow.AddHours(9);
			if (japanNow.CompareTo(japanNextMidnight) > 0)
			{
				// Reset cast wait time
				btnCastReset_Click(btnCastReset, MouseEventArgs.Empty);
				japanNextMidnight = GetNextMidnight();
			}

            UpdateInfo();

        } // @ private void timer_Tick

		private DateTime GetNextMidnight()
		{
			DateTime midnight = _FFACE != null ? _FFACE.Timer.ServerTimeUTC : DateTime.UtcNow;
            midnight = midnight.AddHours(9).AddDays(1);
			midnight = new DateTime(midnight.Year, midnight.Month, midnight.Day);
			return midnight;
		}

        private void timer_DisplayProgressEvent(object sender, ElapsedEventArgs e)
        {
            SetProgress(_FFACE.Fish.HPCurrent);

        } // @ private void DisplayProgressEvent

        #endregion //Events_Timers

        #region Events_Wanted/UnwantedLists

        ListBox selectedListBox;

        private void changeNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedIndex = selectedListBox.SelectedIndex;
            Rectangle r = selectedListBox.GetItemRectangle(selectedIndex);
            int x = selectedListBox.Location.X + r.X + 1;
            int y = selectedListBox.Location.Y + r.Y - 2;

            selectedListBox.Parent.Controls.Add(this.tbChangeName);
            tbChangeName.Location = new Point(x, y);
            tbChangeName.Size = new Size(r.Width, r.Height);
            tbChangeName.Text = selectedListBox.SelectedItem.ToString();
            tbChangeName.SelectAll();
            tbChangeName.Show();
            tbChangeName.BringToFront();
            tbChangeName.Focus();

        } // @ private void changeNameToolStripMenuItem_Click

        private void listBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (null != ((ListBox)sender).SelectedItem)
            {
                Fishie selectedFish = (Fishie)((ListBox)sender).SelectedItem;
                FishDB.ToggleWanted(selectedFish);
            }

        } // @ private void listBox_MouseDoubleClick

        private void listBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Right == e.Button)
            {
                selectedListBox = (ListBox)sender;
                int selectedIndex = selectedListBox.IndexFromPoint(e.X, e.Y);

                selectedListBox.SelectedIndex = selectedIndex;

                if (-1 == selectedIndex)
                {
                    selectedListBox.ContextMenu = null;
                }
                else
                {
                    selectedListBox.ContextMenuStrip = contextMenuListBox;
                }
            }

        } // @ private void listBox_MouseDown

        private void tbChangeName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char)Keys.Enter == e.KeyChar)
            {
                string strSelectedFish = selectedListBox.SelectedItem.ToString();

                if (tbChangeName.Text == strSelectedFish)
                {
                    tbChangeName.Hide();
                    e.Handled = true;
                }
                else
                {
                    Fishie selectedFish = (Fishie)selectedListBox.SelectedItem;
                    FishDB.ChangeName(selectedFish, tbChangeName.Text, false);
                    tbChangeName.Hide();
                    e.Handled = true;
                }
            }

            if ((char)Keys.Escape == e.KeyChar)
            {
                tbChangeName.Hide();
                e.Handled = true;
            }

        } // @ private void tbChangeName_KeyPress

        private void tbChangeName_LostFocus(object sender, EventArgs e)
        {
            tbChangeName.Hide();
        }

        #endregion //Events_Wanted/UnwantedLists

        #endregion //Events

    } // @ internal partial class FishingForm : Form
}
