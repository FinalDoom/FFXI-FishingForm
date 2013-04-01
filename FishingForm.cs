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
        #region Members

        internal static FFACE _FFACE { get; private set; }

		private static FFACE.PlayerTools _Player;

        private static ProcessSelector.POLProcess _Process { get; set; }
        private static Random rnd = new Random();
        private static ThreadStart ts;
        private static Thread workerThread;

		private static int skillLast = 0;
		private static int skillDecimalMin = 0;
		private static int skillDecimalMax = 0;
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
        private static FFACETools.Weekday day { get; set; }
        private static bool OptionsConfigured = false;

        internal enum FishResult
        {
            Error,
            InventoryProblem,
            LackSkill,
            LineBreak,
            LostCatch,
            Monster,
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

        #endregion //Members

        #region Constructor/Destructor

        internal FishingForm(string[] arglist)
        {
            InitializeComponent();

            List<string> args = arglist.ToList();  //*golfandsurf*  Formats <player> arg to match pol process
            args[0] = args[0].Substring(0, 1).ToUpper() + args[0].Substring(1, args[0].Length - 1).ToLower();

			ChooseProcess(args[0]);

            if (args.Count > 1)
            {
                if (args[1].ToLower() == "start") { btnStart_Click(btnStart, MouseEventArgs.Empty); }
            }

            #region FormElements

            timer.Enabled = true;

            toolTip.SetToolTip(btnRefreshLists, "Refreshes the Wanted / Unwanted lists from your database, based on currently equipped rod / bait / zone.\r\rIf there are no entries after pressing this button, your database has no entries for the current combination.");
            toolTip.SetToolTip(btnCastReset, "Reset cast wait to 3.0/3.5. This is a quick reset if you become fatiged and rezone.");
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
			toolTip.SetToolTip(cbLRingGear, "Automatically use left ring enchantment.");
			toolTip.SetToolTip(cbRRingGear, "Automatically use right ring enchantment.");
			toolTip.SetToolTip(cbWaistGear, "Automatically use belt enchantment.");
            toolTip.SetToolTip(cbGMdetectAutostop, "Issue STOP on detection of a GM.");
			toolTip.SetToolTip(cbFatiguedActionShutdown, "Shut down when catch limit is reached.");
			toolTip.SetToolTip(cbFatiguedActionLogout, "Log out when catch limit is reached.");
			toolTip.SetToolTip(cbFatiguedActionWarp, "Warp when catch limit is reached.");
            toolTip.SetToolTip(cbStopSound, "When fishing terminates unexpectedly, play warning.wav sound.");
            toolTip.SetToolTip(cbTellDetect, "Changes status bar color and Tell tab name when receiving a message.");
            toolTip.SetToolTip(cbEnableItemizerItemTools, "Enables Itemizer plugin support to store fish when inventory is full.");
            toolTip.SetToolTip(cbMaxCatch, "Stops fishing when # of catches reached; value resets when limit is reached.");
            toolTip.SetToolTip(cbSneakFishing, "Will cast the spell Sneak prior to casting.");

            FishDB.OnChanged += new FishDB.DBChanged(PopulateLists);
            FishStats.OnChanged += new FishStats.FishStatsChanged(UpdateStats);

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


        }
        #endregion //Constructor/Destructor

        #region Methods

		#region Methods_Initialization

		private void ChooseProcess(string characterName)
		{
            using (ProcessSelector ChooseProcess = new ProcessSelector())
            {
                if (characterName != null && characterName != "No_args")
                {
                    for (int i = 0; i <= Process.GetProcessesByName("pol").Length; i++)
                    {
                        if (Process.GetProcessesByName("pol")[i].MainWindowTitle == characterName)
                        {
                            ChooseProcess.ThisProcess = new ProcessSelector.POLProcess(Process.GetProcessesByName("pol")[i].Id, Process.GetProcessesByName("pol")[i].MainWindowTitle);
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
                    FileVersionInfo ver = FileVersionInfo.GetVersionInfo("fishing.exe");
                    this.Text = string.Format("FishingForm v{0}-mC-FD", ver.FileVersion);
					_Process = null;
					return;
				}

                try   //if you can't create an instance, there's probably no FFACE.dll, or an old FFACE version
                {
                    _FFACE = new FFACE(ChooseProcess.ThisProcess.POLID);
					_Player = new FFACE.PlayerTools(_FFACE._InstanceID);
                    FileVersionInfo ver = FileVersionInfo.GetVersionInfo("fishing.exe");
                    this.Text = string.Format("FishingForm v{0}-mC-FD  ({1})", ver.FileVersion, ChooseProcess.ThisProcess.POLName);

                    //windower path
                    //_FFACE = new FFACE((int)PID);
                    Process pol = Process.GetProcessById((int)ChooseProcess.ThisProcess.POLID); //.GetProcessesByName("pol");
                    foreach (ProcessModule mod in pol.Modules)
                    {
                        if (mod.ModuleName.ToLower() == "hook.dll")
                        {
                            FFACE.WindowerPath = Path.GetDirectoryName(mod.FileName) + @"\plugins"; //.Substring(0, mod.FileName.Length - 8) + @"\plugins\";
                            break;
                        }
                    }
                    if (String.IsNullOrEmpty(FFACE.WindowerPath))
                        FFACE.WindowerPath = "Windower path could not be found";

                }
                catch (DllNotFoundException)   //occurs when FFACE.dll cannot be found
                {
                    MessageBox.Show("FFACE.dll was not found in the program's directory!", "Error");
                    Environment.Exit(0);
                }
                catch (EntryPointNotFoundException)   //occurs when 'CreateInstance' entry point in FFACE.dll cannot be found
                {
                    MessageBox.Show("This program uses FFACE v4.0.0.9 or higher!\r\n\r\nPlease download the latest version of FFACE and put it in this program's directory.", "Error");
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
                    Fatigued("Fatigue limit reached: Max no catches.");
                    return;
                }

				if (Status.Fishing != currentStatus && Status.FishBite != currentStatus && Status.LostCatch != currentStatus)
				{
					decimal randomCast = Math.Round((decimal)rnd.NextDouble() * (numCastIntervalHigh.Value - numCastIntervalLow.Value), 1);
					decimal castWait = numCastIntervalLow.Value + randomCast;
					SetStatus(string.Format("Casting in {0} seconds.", castWait));
					lblHP.Text = "";
					Thread.Sleep((int)castWait * 1000);
				}

                Fish();
            }

            Stop(true, "Changed zones.");

        } // @ private void BackgroundFishing()

        private void Cast()
        {
            string strBait = LastBaitName;
            string strRod = LastRodName;
            string strZone = GetZoneName(_FFACE.Player.Zone);
            string strEquipMessage = string.Format("/equip ammo \"{0}\"", LastBaitName);
            string bait = GetBaitName(_FFACE.Item.GetEquippedItemID(EquipSlot.Ammo));

            if ((string.IsNullOrEmpty(lblZone.Text)) || (lblZone.Text != strZone))
            {
                SetLblZone(strZone);
                PopulateLists();
            }

            if (IsSneakEnabled())
            {
                if (!IsStatusEffectActive(StatusEffect.Sneak))
                {
                    // Check to make sure we have enough MP
                    if (_FFACE.Player.MPCurrent >= 12)
                    {
                        _FFACE.Windower.SendString(string.Format("/ma Sneak <me>"));
                        Thread.Sleep(500);
                        // While we are casting, sleep the thread.
                        while (_FFACE.Player.CastPercentEx < 95)
                        {
                            Thread.Sleep(100);
                        }
                        // Give it time to finish casting animation
                        Thread.Sleep(2000);
                    }
                    else
                        Stop(false, "Not enough MP to cast Sneak.");
                }
                    

            }

            if (string.IsNullOrEmpty(bait))
            {
                _FFACE.Windower.SendString(strEquipMessage);
                Thread.Sleep(2000);
            }
            if (IsRodBaitEquipped())  //check to see if bait/rod changed since last loop
            {
                if (LastBaitName != strBait)
                {
                    strEquipMessage = string.Format("/equip ammo \"{0}\"", LastBaitName);
                    PopulateLists();
                }

                if ((LastRodName != strRod) || (lblZone.Text != strZone))
                {
                    PopulateLists();
                }
            }
            else  //if IsRodBaitEquipped returns false, most likely out of bait, reequip
            {
                _FFACE.Windower.SendString(strEquipMessage);
                Thread.Sleep(1000);  //pause to give the game time to equip bait

                if (!IsRodBaitEquipped())
                {
                    Stop(false, "Out of bait.");
                    return;
                }
            }

            uint baitCount = _FFACE.Item.GetEquippedItemCount(EquipSlot.Ammo);
            uint baitLeft = _FFACE.Item.GetInventoryItemCount((ushort) _FFACE.Item.GetEquippedItemID(EquipSlot.Ammo));

			CheckEnchantment();
            SetStatus(string.Format("Casting a {0} [{1}].", strBait, baitLeft));

            _FFACE.Windower.SendString("/fish");

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

        private bool IsStatusEffectActive(StatusEffect seffect)
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
        
        private void CancelSpell(StatusEffect seffect)
        {
            _FFACE.Windower.SendString(string.Format("//cancel {0}", (short)seffect));
        } // @ private void CancelSpell(StatusEffect seffect)

        private FishResult FightFish()
        {
            SetStatus(string.Format("Fighting {0}.", currentFish));
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

            SetStatus(string.Format("Reeling in {0}.", currentFish));
            lblHP.Text = "";

            string strNewFish = "Unknown";
            string strPlayerName = _FFACE.Player.Name;
            string strCaught = strPlayerName + " caught ";
            string strTempFish = strPlayerName + " obtained the temporary item: ";
            string strObtain = strPlayerName + " obtains ";
            string strMonster = strPlayerName + " caught a monster!";
            string strQuest = strPlayerName + " fishes up a large box";
            string strLostCatch = "You lost your catch.";
            string strLackSkill = "You lost your catch due to your lack of skill.";
            string strTooSmall = "Whatever caught the hook was too small";
            string strTooLarge = "Whatever caught the hook was too large";
            string strLineBreak = "Your line breaks.";
            string strRodBreak = "Your rod breaks.";
            string strInvFull = strPlayerName + " regretfully releases";
            string strZone = "We are now docking in ";
            bool foundMatch = false;
            bool caughtmonster = false;

            while (false == foundMatch)
            {
                for (int i = 0; i < 5; i++)
                {
                    if ((0 < FishChat.fishLog.Count) && (!string.IsNullOrEmpty(FishChat.fishLog[i].Text)))
                    {
                        string chatLine = FishChat.fishLog[i].Text;

                        if (chatLine.Contains(strLostCatch))
                        {
                            strNewFish = "Lost the catch";
                            result = FishResult.LostCatch;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strMonster))
                        {
                            strNewFish = "Monster";
                            result = FishResult.Monster;
                            foundMatch = true;
                            caughtmonster = true;
                            break;
                        }

                        if (chatLine.Contains(strTooSmall))
                        {
                            strNewFish = "Too small fish";
                            result = FishResult.TooSmall;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strTooLarge))
                        {
                            strNewFish = "Too large fish";
                            result = FishResult.TooLarge;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strLineBreak))
                        {
                            strNewFish = "Break line fish";
                            result = FishResult.LineBreak;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strRodBreak))
                        {
                            strNewFish = "Break rod fish";
                            result = FishResult.RodBreak;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strLackSkill))
                        {
                            strNewFish = "Lack skill fish";
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
                            strNewFish = "Brigand's Chart Box";
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strInvFull))
                        {
                            result = FishResult.InventoryProblem;
                            foundMatch = true;
                            break;
                        }

                        if (chatLine.Contains(strZone))
                        {
                            strNewFish = "Lost the catch";
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
                caughtmonster = false;
                //Stop(false, "Monster Caught!!!");
            }

            if ("Unknown" == currentFish)
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

            SetStatus(string.Format("Faking {0}.", currentFish));

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

            System.Timers.Timer progress = new System.Timers.Timer();
            progress.Elapsed += new ElapsedEventHandler(timer_DisplayProgressEvent);
            progress.Interval = 20;
            progress.Start();

            if (cbFishHP.Checked)
            {
                lblHP.Text = string.Format("{0}/{1} [{2}s]", _FFACE.Fish.HPCurrent, _FFACE.Fish.HPMax, _FFACE.Fish.Timeout);
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
                    lblHP.Text = "";
                    lblStatus.Text = string.Format("Killing {0} in {1} seconds.", currentFish, numQuickKill.Value);
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
                    lblHP.Text = string.Format("{0}/{1} [{2}s]", currentFishHP, _FFACE.Fish.HPMax, _FFACE.Fish.Timeout);
                }
                Thread.Sleep(1);
            }
            WinClear();
            progress.Stop();

            if (_FFACE.Player.Zone != currentZone)
            {
                Stop(true, "Changed zones.");
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
				SetStatus(string.Format("No catch, {0} in a row.", consecutiveNoCatchCount));
			}
			else
			{
				SetStatus("No catch.");
			}

			FishStats.totalCastCount++;
			FishStats.noCatchCount++;
			UpdateStats();
			WaitUntil(Status.Standing);
		}

        private void Fish()
        {
            FishResult fishFightResult;

            SetProgress(0);

            if ((Status.Healing == currentStatus) || (Status.Sitting == currentStatus))
            {
                Stop(false, "Healing/Sitting, please stand.");
                return;
            }

            if (_FFACE.Player.Zone != currentZone)
            {
                Stop(true, "Changed zones!");
                return;
            }

			//move items with itemizer or itemtools or custom script
            if (_FFACE.Item.InventoryCount == _FFACE.Item.InventoryMax
					&& rbFullactionOther.Checked)
            {
				if (!string.IsNullOrEmpty(tbFullactionOther.Text))
				{
					if (cbEnableItemizerItemTools.Checked)
					{
						// Get the strings, first split on semicolons for multiple fish
						string[] tempcommandstring = tbFullactionOther.Text.Split(new String[] {";"}, StringSplitOptions.RemoveEmptyEntries);
						foreach (string command in tempcommandstring) {
							string[] tempactionstring = command.Split(new String[] {" "}, StringSplitOptions.RemoveEmptyEntries);
							// Check command type
							if (!(tempactionstring[0].StartsWith("/moveitem") || tempactionstring[0].StartsWith("/put")))
							{
								Stop(false, "Unknown itemizer/itemtools command.");
							}

							string fish;
							if (command.Contains("\""))
							{
								fish = command.Split(new String[] {"\""}, StringSplitOptions.RemoveEmptyEntries)[1];
							}
							else
							{
								fish = tempactionstring[1];
							}

							string storagemedium;
							if (tempactionstring[0].StartsWith("/moveitem"))
							{
								storagemedium = tempactionstring[tempactionstring.Length-1].ToLower();
								// Command ends with a quantity. Get correct storage medium
								if (storagemedium != "satchel" && storagemedium != "sack")
								{
									storagemedium = tempactionstring[tempactionstring.Length-2].ToLower();
								}
							}
							else
							{
								storagemedium = tempactionstring[tempactionstring.Length-1].ToLower();
							}

							// Check storage location
							if (storagemedium != "satchel" && storagemedium != "sack")
							{
								Stop(false, "Unknown destination to move fish");
							}

							MoveItems(string.Join(" ", tempactionstring), ref fish, ref storagemedium);
						}
					}
					else
					{
						SetStatus("Running full inventory command.");
						_FFACE.Windower.SendString(tbFullactionOther.Text);
						Thread.Sleep(10000);
					}
				}
				else
				{
					Stop(false, "Inventory is full!");
				}
			}
            if (_FFACE.Item.InventoryCount == _FFACE.Item.InventoryMax)
            {
                if (rbFullactionWarp.Checked)
                {
                    SetStatus("Inventory is full: Warping");
                    _FFACE.Windower.SendString("/ma \"Warp\" <me>");
                    Thread.Sleep(30000);
                }
                if (rbFullactionLogout.Checked)
                {
                    SetStatus("Inventory is full: Logging out");
                    _FFACE.Windower.SendString("/logout");
                    Thread.Sleep(30000);
                }
			    else if (rbFullactionShutdown.Checked)
			    {
				    SetStatus("Inventory is full: Shutting down");
				    _FFACE.Windower.SendString("/shutdown");
				    Thread.Sleep(30000);
                }
				Stop(false, "Inventory is full!");
				return;
			}

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
                    if ((0 < FishChat.chatLog.Count) && (!string.IsNullOrEmpty(FishChat.chatLog[i].Text)))
                    {
                        string chatLine = FishChat.chatLog[i].Text;

                        if (chatLine.Equals("You must wait longer to perform that action."))
                        {
                            SetStatus("Adding one second to cast times.");
                            IncreaseCastTime();
                            Thread.Sleep(2000);
                            return;
                        }

                        if (chatLine.Equals("You cannot fish here."))
                        {
                            Stop(false, "Move to a fishing position.");
                            return;
                        }

                        if (_FFACE.Player.Zone != currentZone)
                        {
                            Stop(true, "Changed zones!");
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
					Stop(false, "Not fishing for unknown reason.");
					return;
				}
            }

			if (Status.FishBite != currentStatus)
			{
				SetStatus("Waiting for a bite.");
				lblHP.Text = "";
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

                    SetNoCatch(consecutiveNoCatchCount = 0);

                    for (int i = 0; i < 5; i++)
                    {
                        if ((0 < FishChat.fishLog.Count) && (!string.IsNullOrEmpty(FishChat.fishLog[i].Text)))
                        {
                            string chatLine = FishChat.fishLog[i].Text;

                            if (chatLine.Equals("Something caught the hook!"))
                            {
                                if (cbIgnoreSmallFish.Checked)
                                {
                                    currentFish = "Ignored Small Fish";
                                    LogResult(Release());
                                }
                                else
                                {
                                    if (FishDB.FishAccepted(out currentFish, out isNewFish, cbCatchUnknown.Checked, LastRodName, lblZone.Text, LastBaitName, ID1, ID2, ID3))
                                    {
                                        fishFightResult = FightFish();
                                        currentFish = GetFishName(currentFish);

                                        if ((isNewFish) && ("Unknown" != currentFish))
                                        {
                                            FishDB.AddNewFish(ref currentFish, lblZone.Text, LastBaitName, LastRodName, ID1, ID2, ID3, false);
                                        }

                                        LogResult(fishFightResult);
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

                            if (chatLine.Equals("Something caught the hook!!!"))
                            {
                                if (cbIgnoreLargeFish.Checked)
                                {
                                    currentFish = "Ignored Large Fish";
                                    LogResult(Release());
                                }
                                else
                                {
                                    if (FishDB.FishAccepted(out currentFish, out isNewFish, cbCatchUnknown.Checked, LastRodName, lblZone.Text, LastBaitName, ID1, ID2, ID3))
                                    {
                                        fishFightResult = FightFish();
                                        currentFish = GetFishName(currentFish);

                                        if ((isNewFish) && ("Unknown" != currentFish))
                                        {
                                            FishDB.AddNewFish(ref currentFish, lblZone.Text, LastBaitName, LastRodName, ID1, ID2, ID3, false);
                                        }

                                        LogResult(fishFightResult);
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

                            if (chatLine.Equals("Something clamps onto your line ferociously!"))
                            {
                                if (cbIgnoreMonster.Checked)
                                {
                                    currentFish = "Ignored Monster";
                                    LogResult(Release());
                                }
                                else
                                {
                                    if (FishDB.FishAccepted(out currentFish, out isNewFish, cbCatchUnknown.Checked, LastRodName, lblZone.Text, LastBaitName, ID1, ID2, ID3))
                                    {
                                        fishFightResult = FightFish();
                                        currentFish = GetFishName(currentFish);

                                        if ((isNewFish) && ("Unknown" != currentFish))
                                        {
                                            FishDB.AddNewFish(ref currentFish, lblZone.Text, LastBaitName, LastRodName, ID1, ID2, ID3, false);
                                        }

                                        LogResult(fishFightResult);
                                    }
                                    else
                                    {
                                        LogResult(Release());
                                    }
                                }
                                break;
                            }

                            if (chatLine.Equals("You feel something pulling at your line."))
                            {
                                if (cbIgnoreItem.Checked)
                                {
                                    currentFish = "Ignored Item";
                                    LogResult(Release());
                                }
                                else
                                {
                                    if (FishDB.FishAccepted(out currentFish, out isNewFish, cbCatchUnknown.Checked, LastRodName, lblZone.Text, LastBaitName, ID1, ID2, ID3))
                                    {
                                        fishFightResult = FightFish();
                                        currentFish = GetFishName(currentFish);

                                        if ((isNewFish) && ("Unknown" != currentFish))
                                        {
                                            FishDB.AddNewFish(ref currentFish, lblZone.Text, LastBaitName, LastRodName, ID1, ID2, ID3, false);
                                        }

                                        LogResult(fishFightResult);
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

        private void MoveItems(string command, ref string itemname, ref string storagearea)
        {
            // Look up item ID
            int tempitemid = FFACE.ParseResources.GetItemId(itemname);
            if (storagearea == "sack")
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
                        SetStatus(string.Format("Moving {0} to Sack: {1} remaining.", itemname, inventorycount));
                        // Send string to POL
                        _FFACE.Windower.SendString(command);
						inventorycount = _FFACE.Item.GetInventoryItemCount((ushort)tempitemid);
                        // The dreaded sleep()!
                        Thread.Sleep(rnd.Next(750,1500));
                    }
                    SetStatus(string.Format("Finished moving {0} to Sack", itemname));
                }
                else
                {
                    SetStatus("Sack is full");
					Thread.Sleep(750);
                }
            }
            if (storagearea == "satchel")
            {
                if (tempitemid > 0 && ( _FFACE.Item.SatchelCount != _FFACE.Item.SatchelMax ))
                {
                    // Get total number of the items you have in inventory.
                    uint inventorycount = _FFACE.Item.GetInventoryItemCount((ushort)tempitemid);
                    // If we have the item in inventory, move all of them till satchel is full
                    while (( _FFACE.Item.SatchelCount < _FFACE.Item.SatchelMax ) && inventorycount > 0)
                    {
                        // Update Status
                        SetStatus(string.Format("Moving {0} to Satchel: {1} remaining.", itemname, inventorycount));
                        // Send string to POL
                        _FFACE.Windower.SendString(command);
						inventorycount = _FFACE.Item.GetInventoryItemCount((ushort)tempitemid);
                        // The dreaded sleep()!
                        Thread.Sleep(rnd.Next(750, 1500));
                    }
                    SetStatus(string.Format("Finished moving {0} to Sack", itemname));
                }
                else
                {
                    SetStatus("Satchel is full");
					Thread.Sleep(750);
                }
            }
        }

        private FishResult Release()
        {
            WinClear();
            SetStatus(string.Format("Releasing {0}.", currentFish));
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

        private string GetFishName(string fish)
        {
            string name = fish;
            foreach (KeyValuePair<string, int> f in Dictionaries.fishDictionary)
            {
                if (-1 < fish.IndexOf(f.Key, StringComparison.OrdinalIgnoreCase))
                {
                    name = f.Key;
                    break;
                }
            }
            return name;

        } // @ private string GetFishName(string fish)

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

        private bool IsRodBaitEquipped()
        {
            string bait = GetBaitName(_FFACE.Item.GetEquippedItemID(EquipSlot.Ammo));
            string rod = GetRodName(_FFACE.Item.GetEquippedItemID(EquipSlot.Range));
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
                LastBaitName = LastRodName = lblZone.Text = string.Empty;
                Stop(false, "No rod or bait equipped.");
                ClearLists();

                return false;
            }

        } // @ private bool RodBaitEquipped()

        private void LogResult(FishResult result)
        {
            FishStats.totalCastCount++;

            switch (result)
            {
                case FishResult.Error:
                    Stop(false, "Error, you cannot fish.");
                    break;
                case FishResult.InventoryProblem:
                    FishStats.releasedCount++;
                    FishStats.AddFish(FishStats.releasedFishes, currentFish);
                    Stop(false, "Inventory might be full.");
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
                    Stop(false, "Monster caught!!!");
                    break;
                case FishResult.Released:
                    FishStats.releasedCount++;
                    FishStats.AddFish(FishStats.releasedFishes, currentFish);
                    break;
                case FishResult.RodBreak:
                    FishStats.rodBreakCount++;
                    FishStats.AddFish(FishStats.rodBreakFishes, currentFish);
                    Stop(false, "Your rod is broken.");
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
                            Fatigued("Fatigue limit reached: Max catch limit.");
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
                    Stop(true, "Zoned while catching a fish.");
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
                    Stop(true, "Changed zones.");
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
            int timeElapsed = 0;

            while ((status != currentStatus) && (quit > timeElapsed))
            {
                if (_FFACE.Player.Zone == currentZone)
                {
                    Thread.Sleep(timeElapsed += 100);
                }
                else
                {
                    Stop(true, "Changed zones.");
                    break;
                }
            }

        } // @ private void WaitUntil(Status status, int quit)

        /// <summary>
        /// Clears any keys sent to FFXI by this program, so they don't "lock"
        /// </summary>
        private void WinClear()
        {
            _FFACE.Windower.SendKey(KeyCode.NP_Number2, false);
            _FFACE.Windower.SendKey(KeyCode.NP_Number4, false);
            _FFACE.Windower.SendKey(KeyCode.NP_Number6, false);
            _FFACE.Windower.SendKey(KeyCode.NP_Number8, false);
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
            bool wasFishing = false;

            if ("STOP" == btnStart.Text)
            {
                wasFishing = true;
            }

            Stop(false, "Fatal chat log error, attempting to recover.");

            _FFACE = null;
            _FFACE = new FFACE(_Process.POLID);

            if (wasFishing)
            {
                SetStatus("Reattached to process, attempting to restart fishing.");
                Start();  //restart the fishing loop if the user was fishing
            }
            else
            {
                SetStatus("Reattached to process after chat error, ready to fish.");
            }

        } // @ private void Reattach()

		private bool CheckProcess()
		{
			if (_Process == null || !_Process.IsAvailable)
			{
				if (workerThread != null)
				{
					Stop(false, "Not fishing because character is not logged in.");
				}
				ChooseProcess(null);
				if (_Process == null || !_Process.IsAvailable)
				{
					SetStatus("No FFXI process available");
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

            if (IsRodBaitEquipped())
            {
                PopulateLists();

                while (_FFACE.Menu.IsOpen)
                {
                    _FFACE.Windower.SendKeyPress(KeyCode.EscapeKey);
                    Thread.Sleep(200);
                }

				GearUp();
				CheckEnchantment();

                ts = new ThreadStart(BackgroundFishing);
                workerThread = new Thread(ts);
                workerThread.IsBackground = true;
                workerThread.Start();

                btnStart.Text = "STOP";
                btnStartM.Image = Fishing.Properties.Resources.icon_stop;
            }

        } // @ private void Start()

		private void GearUp()
		{
			if (tbBodyGear.Text != "")
			{
				_FFACE.Windower.SendString("/equip body \"" + tbBodyGear.Text + "\"");
			}
			if (tbHandsGear.Text != "")
			{
				_FFACE.Windower.SendString("/equip hands \"" + tbHandsGear.Text + "\"");
			}
			if (tbLegsGear.Text != "")
			{
				_FFACE.Windower.SendString("/equip legs \"" + tbLegsGear.Text + "\"");
			}
			if (tbFeetGear.Text != "")
			{
				_FFACE.Windower.SendString("/equip feet \"" + tbFeetGear.Text + "\"");
			}
			if (tbLRingGear.Text != "")
			{
				_FFACE.Windower.SendString("/equip ring1 \"" + tbLRingGear.Text + "\"");
				if (tbLRingGear.Text == tbRRingGear.Text)
				{
					Thread.Sleep(500);
				}
			}
			if (tbRRingGear.Text != "")
			{
				_FFACE.Windower.SendString("/equip ring2 \"" + tbRRingGear.Text + "\"");
			}
			if (tbHeadGear.Text != "")
			{
				_FFACE.Windower.SendString("/equip head \"" + tbHeadGear.Text + "\"");
			}
			if (tbNeckGear.Text != "")
			{
				_FFACE.Windower.SendString("/equip neck \"" + tbNeckGear.Text + "\"");
			}
			if (tbWaistGear.SelectedIndex != 0)
			{
				_FFACE.Windower.SendString("/equip waist \"" + tbWaistGear.Items[tbWaistGear.SelectedIndex].ToString() + "\"");
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
					SetStatus("Using Fisherman's belt");
					_FFACE.Windower.SendString("/item \"" + tbWaistGear.Items[tbWaistGear.SelectedIndex].ToString() + "\" <me>");
					WaitUntil(Status.Standing);
				}
			}
            // Count enchantment effects the playe is under to compare to enabled ring count
            int enchantmentsNeeded = GetNeededEnchantments();
			// Check if left ring enchantments are available and not on
			if (cbLRingGear.Enabled && cbLRingGear.Checked && enchantmentsNeeded > 0)
			{
				SetStatus(string.Format("Using left ring ({0})", tbLRingGear.Text));
				_FFACE.Windower.SendString("/item \"" + tbLRingGear.Text + "\" <me>");
                Thread.Sleep(500);
				WaitUntil(Status.Standing);
                enchantmentsNeeded = GetNeededEnchantments();
			}
			// Check if right ring enchantments are available and not on
			if (cbRRingGear.Enabled && cbRRingGear.Checked && enchantmentsNeeded > 0)
			{
				SetStatus(string.Format("Using right ring ({0})", tbRRingGear.Text));
                _FFACE.Windower.SendString("/item \"" + tbRRingGear.Text + "\" <me>");
                Thread.Sleep(500);
				WaitUntil(Status.Standing);
			}
		}

        delegate void VoidBoolStrDelegate(bool param1, string param2);

		private void Fatigued(string message)
		{
			if (cbFatiguedActionWarp.Checked)
			{
				_FFACE.Windower.SendString("/ma \"Warp\" <me>");
				SetStatus("Fatigue limit reached: Warping");
				Thread.Sleep(30000);
			}
			if (cbFatiguedActionShutdown.Checked)
			{
				_FFACE.Windower.SendString("/shutdown");
				SetStatus("Fatigue limit reached: Shutting down");
			}
			else if (cbFatiguedActionLogout.Checked)
			{
				_FFACE.Windower.SendString("/logout");
				SetStatus("Fatigue limit reached: Logging out");
			}
			Thread.Sleep(33000);
			Stop(false, message);
		}

        private void Stop(bool zoned, string status)
        {
            if (InvokeRequired)
                Invoke(new VoidBoolStrDelegate(Stop), zoned, status);
            else
            {
				if (FishStats.startTicks != 0) {
					FishStats.ticksFished += DateTime.Now.Ticks - FishStats.startTicks;
					FishStats.startTicks = 0;
				}
                if (null != workerThread)
                {
                    string equipMessage = string.Format("/equip ammo \"{0}\"", LastBaitName);

                    SetProgressMaxValue(0);
                    SetStatus(status);

                    if (statusWarningColor)
                    {
                        statusStripMain.BackColor = Color.Red;
                        statusStripMain.ForeColor = Color.White;
                        btnStart.BackColor = Color.Red;
                        btnStart.ForeColor = Color.White;
                    }

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

                    if (stopSound && cbStopSound.Checked && File.Exists("warning.wav"))
                    {
                        SoundPlayer spWave = new SoundPlayer("warning.wav");
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
                    btnStart.Text = "START";
                    btnStartM.Image = Fishing.Properties.Resources.icon_play;
                }
            }

        } // @ private void Stop(bool zoned, string status)

        #endregion //Methods_Reattach/Start/Stop

        #region Methods_ThreadSafe

        delegate decimal DecimalNoParamDelegate();
        delegate int IntNoParamDelegate();
        delegate void VoidNoParamDelegate();
        delegate void VoidBoolDelegate(bool param);
        delegate void VoidIntDelegate(int param);
        delegate void VoidStrDelegate(string param);
        delegate void ChatLogsDelegate(RichTextBox param1, List<FFACE.ChatTools.ChatLine> param2, int param3);

        private void ClearLists()
        {
            if (InvokeRequired)
                Invoke(new VoidNoParamDelegate(ClearLists));
            else
            {
                lbWanted.Items.Clear();
                lbUnwanted.Items.Clear();
            }

        } // @ private void ClearLists()

        private decimal GetFakeLargeHigh()
        {
            if (InvokeRequired)
                return (decimal)Invoke(new DecimalNoParamDelegate(GetFakeLargeHigh), null);
            else
            {
                return this.numFakeLargeIntervalHigh.Value;
            }

        } // @ private decimal GetFakeLargeHigh()

        private decimal GetFakeLargeLow()
        {
            if (InvokeRequired)
                return (decimal)Invoke(new DecimalNoParamDelegate(GetFakeLargeLow), null);
            else
            {
                return this.numFakeLargeIntervalLow.Value;
            }

        } // @ private decimal GetFakeLargeLow()

        private decimal GetFakeSmallHigh()
        {
            if (InvokeRequired)
                return (decimal)Invoke(new DecimalNoParamDelegate(GetFakeSmallHigh), null);
            else
            {
                return this.numFakeSmallIntervalHigh.Value;
            }

        } // @ private decimal GetFakeSmallHigh()

        private decimal GetFakeSmallLow()
        {
            if (InvokeRequired)
                return (decimal)Invoke(new DecimalNoParamDelegate(GetFakeSmallLow), null);
            else
            {
                return this.numFakeSmallIntervalLow.Value;
            }

        } // @ private decimal GetFakeSmallLow()

        private void IncreaseCastTime()
        {
            if (InvokeRequired)
                Invoke(new VoidNoParamDelegate(IncreaseCastTime));
            else
            {
                numCastIntervalHigh.Value += 1;
                numCastIntervalLow.Value += 1;
            }

        } // @ private void IncreaseCastTime()

        private void PopulateLists()
        {
            if (InvokeRequired)
                Invoke(new VoidNoParamDelegate(PopulateLists));
            else
            {
                lbWanted.Items.Clear();
                lbUnwanted.Items.Clear();

                foreach (Fishie f in FishDB.GetFishes(LastRodName, lblZone.Text, LastBaitName, true))
                {
                    lbWanted.Items.Add(f);
                }

                foreach (Fishie f in FishDB.GetFishes(LastRodName, lblZone.Text, LastBaitName, false))
                {
                    lbUnwanted.Items.Add(f);
                }
            }

        } // @ private void PopulateLists()

        private void SetBait(string bait)
        {
            if (InvokeRequired)
                Invoke(new VoidStrDelegate(SetBait), bait);
            else
            {
                LastBaitName = bait;
            }

        } // @ private void SetBait(string bait)

        private void SetRod(string rod)
        {
            if (InvokeRequired)
                Invoke(new VoidStrDelegate(SetRod), rod);
            else
            {
                LastRodName = rod;
            }

        } // @ private void SetRod(string rod)

        private void SetLblZone(string zone)
        {
            if (InvokeRequired)
                Invoke(new VoidStrDelegate(SetLblZone), zone);
            else
            {
                lblZone.Text = zone;
            }

        } // @ private void SetLblZone(string zone)

        private void SetNoCatch(int releases)
        {
            if (InvokeRequired)
                Invoke(new VoidIntDelegate(SetNoCatch), releases);
            else
            {
                lblNoCatchAt.Text = string.Format("{0} / {1}", consecutiveNoCatchCount, numMaxNoCatch.Value);
            }

        } // @ private void SetNoCatch(int releases)

        private void SetProgress(int pos)
        {
            if (InvokeRequired)
                Invoke(new VoidIntDelegate(SetProgress), pos);
            else
            {
                //in case SetProgressMaxValue wasn't called, for w/e reason
                if (progressBarST.Maximum < pos)
                {
                    SetProgressMaxValue(_FFACE.Fish.HPMax);
                }

                progressBarST.Value = pos;
            }

        } // @ private void SetProgress(int pos)

        private void SetProgressMaxValue(int pos)
        {
            if (InvokeRequired)
                Invoke(new VoidIntDelegate(SetProgressMaxValue), pos);
            else
            {
                progressBarST.Maximum = pos;
                progressBarST.Value = pos;
            }

        } // @ private void SetProgressMaxValue(int pos)

        private void SetStatus(string str)
        {
            if (InvokeRequired)
                Invoke(new VoidStrDelegate(SetStatus), str);
            else
            {
                lblStatus.Text = str;
            }

        } // @ private void SetStatus(string str)

        private void UpdateChat()
        {
            if (InvokeRequired)
                Invoke(new VoidNoParamDelegate(UpdateChat));
            else
            {
                UpdateChatLogs(rtbChat, FishChat.chatLog, FishChat.chatLogAdded);
				string testLine;

                //added by golfandsurf 6/21/2010:  GMdetect
                //notes: corrects for word wrapping that may occur to always check the
                //      beginning of each message.
				for (int i = FishChat.tellLogAdded - 1; i >= 0; --i)
				{
					testLine = FishChat.tellLog[i].Text;
					if (testLine.Length >= 3 && testLine.Substring(0, 3) == "[GM")
					{
						gmDetect();
					}
				}
                //end added by golfandsurf 6/21/2010: GMdetect

				//Detect skillups
				for (int i = FishChat.chatLogAdded - 1; i >= 0; --i)
				{
					testLine = FishChat.chatLog[i].Text;
					if (testLine.StartsWith(_Player.Name + "'s fishing skill"))
					{
						if (testLine.EndsWith("points."))
						{
							if (int.TryParse(testLine.Substring(testLine.LastIndexOf(' ') - 1, 1), out skillLast))
							{
								skillDecimalMin += skillLast;
								skillDecimalMax += skillLast;
							}
							else
							{
								skillLast = 0;
							}
						}
						else // Level up
						{
							int lastSpace = testLine.LastIndexOf(' ');
							int.TryParse(testLine.Substring(lastSpace + 1, testLine.LastIndexOf('.') - lastSpace - 1), out skillLevel);
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
                }

                if (0 < FishChat.shellLogAdded)
                {
                    UpdateChatLogs(rtbShell, FishChat.shellLog, FishChat.shellLogAdded);
                }

                if (0 < FishChat.tellLogAdded)
                {
                    UpdateChatLogs(rtbTell, FishChat.tellLog, FishChat.tellLogAdded);

                    //tell detection
                    if (cbTellDetect.Checked)
                    {
                        if (tabChat.SelectedTab.Name != "tabChatPageTell" && FishChat.chatLog[FishChat.tellLogAdded - 1].Text.Length >= 2 && FishChat.chatLog[FishChat.tellLogAdded - 1].Text.Substring(0, 2) != ">>")
                        {
                            tabChatPageTell.Text = " Tell (!!!)";
                            statusStripMain.BackColor = Color.Plum;
                        }
                    }
                }
                if (0 < FishChat.sayLogAdded)
                {
                    UpdateChatLogs(rtbSay, FishChat.sayLog, FishChat.sayLogAdded);
                }
                FishChat.Clear();  //clear ___LogAdded variables for next update
            }
        } // @ private void UpdateChat()

        private void UpdateChatLogs(RichTextBox rtb, List<FFACE.ChatTools.ChatLine> log, int linesToParse)
        {
            if (InvokeRequired)
                Invoke(new ChatLogsDelegate(UpdateChatLogs), rtb, log, linesToParse);
            else
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
            }

        } // @ private void UpdateChatLogs(RichTextBox rtb, int linesToParse)

        private void UpdateStats()
        {
            if (InvokeRequired)
                Invoke(new VoidNoParamDelegate(UpdateStats));
            else
            {
                string statsRtfResult = FishStats.PrintStats();

                rtbStats.Clear();
                rtbStats.SelectedRtf = statsRtfResult;
            }

        } // @ private void UpdateStats()

        #endregion //Methods_ThreadSafe

        #region Methods_Advanced


        private void gmDetect()
        {
            if (cbGMdetectAutostop.Checked)
            {
                if (!cbStopSound.Checked && File.Exists("warning.wav"))
                {
                    SoundPlayer spWave = new SoundPlayer("warning.wav");
                    spWave.Play();
                }

                Stop(false, "[GM] DETECTED!!!");
            }
        }

        private void UpdateInfo()
        {
            //set rod and bait labels
            if (string.IsNullOrEmpty(GetRodName(_FFACE.Item.GetEquippedItemID(EquipSlot.Range))))
            {
                lblRod.ForeColor = Color.Red;
                lblRod.Text = "No rod equipped!";
            }
            else
            {
                lblRod.ForeColor = SystemColors.ControlText;
                lblRod.Text = GetRodName(_FFACE.Item.GetEquippedItemID(EquipSlot.Range));
            }

            if (string.IsNullOrEmpty(GetBaitName(_FFACE.Item.GetEquippedItemID(EquipSlot.Ammo))))
            {
                lblBait.ForeColor = Color.Red;
                lblBait.Text = "No bait equipped!";
            }
            else
            {
                lblBait.ForeColor = SystemColors.ControlText;
                lblBait.Text = string.Format("{0} [{1}]", GetBaitName(_FFACE.Item.GetEquippedItemID(EquipSlot.Ammo)),
                    _FFACE.Item.GetInventoryItemCount((ushort) _FFACE.Item.GetEquippedItemID(EquipSlot.Ammo)) );
            }

            //set day icon and time (borrowed from Xi Slacker)
            switch (vanaNow.DayType)
            {
                case Weekday.Darksday:
                    lblVanaDay.Image = Fishing.Properties.Resources.d_dark;
                    lblVanaDay.ForeColor = Color.DarkGray;
                    break;
                case Weekday.Earthsday:
                    lblVanaDay.Image = Fishing.Properties.Resources.d_earth;
                    lblVanaDay.ForeColor = Color.Yellow;
                    break;
                case Weekday.Firesday:
                    lblVanaDay.Image = Fishing.Properties.Resources.d_fire;
                    lblVanaDay.ForeColor = Color.DarkOrange;
                    break;
                case Weekday.Iceday:
                    lblVanaDay.Image = Fishing.Properties.Resources.d_ice;
                    lblVanaDay.ForeColor = Color.LightBlue;
                    break;
                case Weekday.Lightningday:
                    lblVanaDay.Image = Fishing.Properties.Resources.d_thunder;
                    lblVanaDay.ForeColor = Color.DarkMagenta;
                    break;
                case Weekday.Lightsday:
                    lblVanaDay.Image = Fishing.Properties.Resources.d_light;
                    lblVanaDay.ForeColor = Color.LightGray;
                    break;
                case Weekday.Watersday:
                    lblVanaDay.Image = Fishing.Properties.Resources.d_water;
                    lblVanaDay.ForeColor = Color.Blue;
                    break;
                case Weekday.Windsday:
                    lblVanaDay.Image = Fishing.Properties.Resources.d_wind;
                    lblVanaDay.ForeColor = Color.LightGreen;
                    break;
            }

            // the rest
            lblVanaTime.Text = string.Format("{0}/{1}/{2}, {3}, {4}:{5}, {6} Moon" + " ({7}%)", vanaNow.Month, vanaNow.Day, vanaNow.Year, vanaNow.DayType, vanaNow.Hour, vanaNow.Minute.ToString("00"), vanaNow.GetMoonPhaseName(vanaNow.MoonPhase), vanaNow.MoonPercent);
            lblEarthTime.Text = DateTime.Now.ToString("MMM. d, yyyy h:mm:ss tt");
			string skillS = Math.Max(_FFACE.Player.GetCraftDetails(Craft.Fishing).Level, skillLevel).ToString();
			if (skillDecimalMin > 0 || skillDecimalMax > 0)
			{
				skillS += " (." + skillDecimalMin.ToString();
				if (skillDecimalMin != skillDecimalMax)
				{
					skillS += " - ." + skillDecimalMax.ToString();
				}
				skillS += ")";
			}
            lblSkill.Text = skillS;

            if (_FFACE.Item.InventoryCount != -1)
            {
                lblInventorySpace.Text = string.Format("{0} / {1}", _FFACE.Item.InventoryCount, _FFACE.Item.InventoryMax);
                lblGil.Text = string.Format("{0:#,#}", _FFACE.Item.CurrentGil);
            }
            else
            {
                lblInventorySpace.Text = "N/A";
                lblGil.Text = "N/A";
            }

            if (_FFACE.Item.SatchelCount != -1)
                lblSatchelSpace.Text = string.Format("{0} / {1}", _FFACE.Item.SatchelCount, _FFACE.Item.SatchelMax);
            else
                lblSatchelSpace.Text = "N/A";

            if (_FFACE.Item.SackCount != -1)
                lblSackSpace.Text = string.Format("{0} / {1}", _FFACE.Item.SackCount, _FFACE.Item.SackMax);
            else
                lblSackSpace.Text = "N/A";

            lblVanaClock.Text = string.Format("{0}:{1}", vanaNow.Hour, vanaNow.Minute.ToString("00"));
            SetLblZone(GetZoneName(_FFACE.Player.Zone));
        }

        private void ExtendChat()
        {
            int formWidth = this.Width;
            int extrasWidth = Extras.Width;


            if (chatbig)
            {
                btnResize.Text = ">";
                pnlWanted.Visible = false;
                pnlUnwanted.Visible = false;
                btnStart.Visible = false;
                btnRefreshLists.Visible = false;
                cbCatchUnknown.Visible = false;
                Extras.Location = new Point(0, 0);
                Extras.Width = formWidth - 15;

                tbChat.Width = formWidth - 109;
                btnStartM.Visible = true;
                btnStartM.Location = new Point((formWidth - 66), -1);
                btnChatSend.Location = new Point((formWidth - 109), -1);
            }
            else
            {
                btnResize.Text = "<";
                pnlWanted.Visible = true;
                pnlUnwanted.Visible = true;
                btnStart.Visible = true;
                btnRefreshLists.Visible = true;
                cbCatchUnknown.Visible = true;
                Extras.Location = new Point(145, 0);
                Extras.Width = formWidth - 160;

                tbChat.Width = formWidth - 233;
                btnStartM.Visible = false;
                //btnStartM.Location = new Point(289, -1);
                btnChatSend.Location = new Point((formWidth - 233), -1);
            }

        }

        #endregion //Methods_Advanced

        #endregion //Methods

        #region Events

        #region Events_Chat

        private const int bufferSize = 20;
        private static int bufferPosition = 0;
        private static Regex allSpaces = new Regex("^[\\s]+$");
        private static List<string> chatBuffer = new List<string>(bufferSize);

        private void btnChatSend_Click(object sender, EventArgs e)
        {
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

                e.Handled = true;
            }

        } // @ private void tbChat_KeyPress

        #endregion //Events_Chat

        #region Events_Options

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

        private void numMaxNoCatch_ValueChanged(object sender, EventArgs e)
        {
            SetNoCatch((int)numMaxNoCatch.Value);

        } // @ private void numMaxNoCatch_ValueChanged

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

        private void rbFullactionOther_CheckedChanged(object sender, EventArgs e)
        {
            tbFullactionOther.Enabled = rbFullactionOther.Checked;
        }

        private void rbFullactionLogout_CheckedChanged(object sender, EventArgs e)
        {
			if (rbFullactionLogout.Checked)
			{
				rbFullactionShutdown.Checked = false;
			}
        }

        private void rbFullactionShutdown_CheckedChanged(object sender, EventArgs e)
        {
			if (rbFullactionShutdown.Checked)
			{
				rbFullactionLogout.Checked = false;
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

        #endregion //Events_Options

		#region Events_Gear

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

        #endregion //Events_Gear

        #region Events_Miscellaneous

        private void btnStart_Click(object sender, EventArgs e)
        {
//            SetStatus("Clicked");
//            Thread.Sleep(1000);
            if ("STOP" == btnStart.Text)
            {
                stopSound = false;
                statusWarningColor = false;
                Stop(false, "Idle.");
            }
            else  //"START" == btnStart.Text
            {
//                SetStatus("Coloring");
//                Thread.Sleep(1000);
                btnStart.BackColor = SystemColors.Control;
                btnStart.ForeColor = SystemColors.ControlText;
                statusStripMain.BackColor = SystemColors.Control;
                statusStripMain.ForeColor = SystemColors.ControlText;

//                SetStatus("Checking process 1");
//                Thread.Sleep(1000);
				if (!CheckProcess())
				{
					return;
				}

                if (!IsRodBaitEquipped())
                {
                    SetStatus("No rod or bait equipped.");
                    btnStart.BackColor = Color.Red;
                    btnStart.ForeColor = Color.White;
                    statusStripMain.BackColor = Color.Red;
                    statusStripMain.ForeColor = Color.White;
                }

                Start();
            }
        }

        private void btnStartM_Click(object sender, EventArgs e)
        {
            if ("STOP" == btnStart.Text)
            {
                stopSound = false;
                statusWarningColor = false;
                chatbig = false;
                Stop(false, "Idle.");
                chatbig = true;
            }
            else  //"START" == btnStart.Text
            {
                btnStart.BackColor = SystemColors.Control;
                btnStart.ForeColor = SystemColors.ControlText;
                statusStripMain.BackColor = SystemColors.Control;
                statusStripMain.ForeColor = SystemColors.ControlText;

                if (!IsRodBaitEquipped())
                {
                    SetStatus("No rod or bait equipped.");
                    btnStart.BackColor = Color.Red;
                    btnStart.ForeColor = Color.White;
                    statusStripMain.BackColor = Color.Red;
                    statusStripMain.ForeColor = Color.White;
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

        private void btnRefreshLists_Click(object sender, EventArgs e)
        {
            if (IsRodBaitEquipped())
            {
                PopulateLists();
                SetStatus("Wanted / Unwanted lists refreshed.");
            }
            else
            {

                SetStatus("Equip a rod / bait to refresh lists.");
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
            string message = "Do you really want to reset the settings?";
            string caption = "Reset Option Settings";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);

            if (DialogResult.Yes == result)
            {
                Settings.Default.Reset();
                Settings.Default.Reload();

                Settings.Default.AlwaysOnTop = cbAlwaysOnTop.Checked = true;
                Settings.Default.MaxCatch = cbMaxCatch.Checked = false;
                Settings.Default.MaxCatchValue = numMaxCatch.Value = 200;
                Settings.Default.CastMax = numCastIntervalHigh.Value = 3.5M;
                Settings.Default.CastMin = numCastIntervalLow.Value = 3.0M;
                Settings.Default.CatchUnknown = cbCatchUnknown.Checked = false;
                Settings.Default.Opacity = trackOpacity.Value = 10;
                this.Opacity = trackOpacity.Value;
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
                Settings.Default.FullActionOtherText = tbFullactionOther.Text = "";
                Settings.Default.FullActionOther = rbFullactionOther.Checked = false;
                Settings.Default.FullActionWarp = rbFullactionWarp.Checked = false;
                Settings.Default.FullActionLogout = rbFullactionLogout.Checked = false;
                Settings.Default.FullActionShutdown = rbFullactionShutdown.Checked = false;
                Settings.Default.IgnoreItems = cbIgnoreItem.Checked = true;
                Settings.Default.IgnoreMonsters = cbIgnoreMonster.Checked = true;
                Settings.Default.IgnoreSmallFish = cbIgnoreSmallFish.Checked = false;
                Settings.Default.IgnoreLargeFish = cbIgnoreLargeFish.Checked = false;
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
				Settings.Default.FatiguedShutdown = cbFatiguedActionShutdown.Checked = false;
				Settings.Default.FatiguedLogout = cbFatiguedActionLogout.Checked = false;
				Settings.Default.FatiguedWarp = cbFatiguedActionWarp.Checked = false;
                Settings.Default.StopSound = cbStopSound.Checked = false;
                Settings.Default.TellDetect = cbTellDetect.Checked = true;
                Settings.Default.ShowFishHP = cbFishHP.Checked = true;
                Settings.Default.ItemizerItemTools = cbEnableItemizerItemTools.Checked = false;
                Settings.Default.SneakFishing = cbSneakFishing.Checked = false;


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
            string message = "Do you really want to save the current settings?";
            string caption = "Save Option Settings";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);

            if (DialogResult.Yes == result)
            {
                Settings.Default.AlwaysOnTop = cbAlwaysOnTop.Checked;
                Settings.Default.MaxCatch = cbMaxCatch.Checked;
                Settings.Default.MaxCatchValue = numMaxCatch.Value;
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
				Settings.Default.FullActionOther = rbFullactionOther.Checked;
				Settings.Default.FullActionWarp = rbFullactionWarp.Checked;
				Settings.Default.FullActionLogout = rbFullactionLogout.Checked;
				Settings.Default.FullActionShutdown = rbFullactionShutdown.Checked;
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
                Settings.Default.ItemizerItemTools = cbEnableItemizerItemTools.Checked;
                Settings.Default.SneakFishing = cbSneakFishing.Checked;

                if (FormWindowState.Normal == this.WindowState)
                {
                    Settings.Default.WindowSize = this.Size;
                }
                else
                {
                    Settings.Default.WindowSize = this.RestoreBounds.Size;
                }

                Settings.Default.Save();
            }

        } // @ private void btnSettingsSave_Click

        private void FishingForm_Load(object sender, EventArgs e)
        {
            cbAlwaysOnTop.Checked = Settings.Default.AlwaysOnTop;
            cbMaxCatch.Checked = Settings.Default.MaxCatch;
            numMaxCatch.Value = Settings.Default.MaxCatchValue;
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
			rbFullactionOther.Checked = Settings.Default.FullActionOther;
			rbFullactionWarp.Checked = Settings.Default.FullActionWarp;
			rbFullactionLogout.Checked = Settings.Default.FullActionLogout;
			rbFullactionShutdown.Checked = Settings.Default.FullActionShutdown;
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
            cbEnableItemizerItemTools.Checked = Settings.Default.ItemizerItemTools;
            cbSneakFishing.Checked = Settings.Default.SneakFishing;

            if (false == Settings.Default.AlwaysOnTop)
            {
                this.TopMost = false;
            }

            if (null != Settings.Default.WindowSize)
            {
                this.Size = Settings.Default.WindowSize;
            }

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
			if (_FFACE == null)
			{
				return;
			}
            int workDone = FishChat.NewChat();
            currentStatus = _FFACE.Player.Status;
            vanaNow = _FFACE.Timer.GetVanaTime();

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

			DateTime japanNow = _FFACE.Timer.ServerTimeUTC.AddHours(9);
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
			DateTime midnight = _FFACE.Timer.ServerTimeUTC.AddHours(9).AddDays(1);
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

            this.selectedListBox.Parent.Controls.Add(this.tbChangeName);
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
                    FishDB.ChangeName(selectedFish, tbChangeName.Text);
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
            if (tabChat.SelectedTab.Name == "tabChatPageTell")
            {
                tabChatPageTell.Text = "Tell";
                statusStripMain.BackColor = SystemColors.Control;
            }
        }

        #endregion //Events

        private void cbEnableItemizerItemTools_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEnableItemizerItemTools.Checked && OptionsConfigured)
            {
                MessageBox.Show("NOTE: Option requires you to have the FFXI Windower Itemizer.dll plugin or Ashita ItemTools.dll extension loaded.\r\n\r\nYou must use the command under Options->Other->On Full Inventory with [Other] selected, e.g.:\r\n\r\n    /put \"Hakuryu\" sack\r\n    /puts \"Black Sole\" satchel\r\n\r\n    /moveitem \"Hakuryu\" inventory sack 1\r\n    /moveitem \"Black Sole\" inventory satchel 12");
            }
        }

    } // @ internal partial class FishingForm : Form
}
