=============================
FishingForm FinalDoom Edition
=============================

--------
Overview
--------

A continuation of MisterC's FishingForm, done by FinalDoom.
This edition seeks to add additional features and fixes.
MisterC's version's support topic can be found at http://www.ffevo.net/topic/2786-fishingform-v1662-mczip/.

FishingForm is a GUI application built with Microsoft .NET 3.5 in C#.
It is meant to run along side Final Fantasy XI to automate the tedium that is fishing.
It has limited cheat capabilities, namely the ability to "kill" a fish, regardless of
player fishing skill level and reel it in. This can be useful for legendary fish
when the player is not equipped with special rings.

For the FinalDoom addition, you can check out the source and
submit bugs or ideas at https://bitbucket.org/FinalDoom/ffxi-fishingform.
Version control is done using Mercurial and TortoiseHG, and issue tracking
uses bitbucket's built-in JIRA-like system.

--------
Features
--------

- Includes fface (current as of this post)
- Includes ffacetools (current as of this post)
- Select fish or items to selectively catch or release
- Drops unknown fish unless "Unknowns?" is checked
- Uses fface fightfish to kill fish instead of tapping/holding method
- Chat through program without interrupting fishing
    - Expand/shrink chat while fishing
    - Separate logs for linkshell, tell, party, say, and fishing-related
    - General log that includes all chat
    - Timestamped similarly to windower timestamp plugin
    - Clickable URLs
- Fishing statistics tab
    - Shows total casts, time fishing, and catches per hour
    - Shows counts and percentages for catches, releases, and lost fish types
- Improved information on the Info tab (inventory/satchel/sack, skill, gil, bait count)
    - Skillups show next to skill level as they occur, including uncertainty
- Kill fish manually by clicking progress bar during fight
- Option to show fish HP and time left to reel in
- Dialog turns red/comes into focus when it stops unexpectedly
- Can be opened before FFXI, persists after logout or character change

-------
Options
-------

General
-------
- Always on top and opacity
- Sound on errors/stop
- Basic tell detection alert
- Auto-cast sneak for sneak fishing
- Cast wait resets at JP midnight

Fight
-----
- Kill fish automatically at warning (5 seconds left to reel in)
- Extend timeout option
- Kill after # of seconds on the line
- Ignore all mobs, items, small fish, or large fish
- Randomization for time on the line before release

Gear
----
- Choose gear to equip when fishing starts
- Automatically cast enchanted rings or Fisherman's Belt

Other
-----
- Move items when inventory is full using itemizer or ItemTools
- Warp then optionally log out or shut down when inventory is full or when fatigued
- GM Detection
