=============================
FishingForm FinalDoom Edition
=============================

--------
Overview
--------

.. _MisterC's version:
.. _its FFEvo download page: http://www.ffevo.net/files/file/171-fishingform-v1662-mczip/.

A continuation of MisterC's FishingForm, done by FinalDoom.
This edition seeks to add additional features and fixes.
`MisterC's version`_ can be found at `its FFEvo download page`_.

FishingForm is a GUI application built with Microsoft .NET 3.5 in C#.
It is meant to run along side Final Fantasy XI to automate the tedium that is fishing.
It has limited cheat capabilities, namely the ability to "kill" a fish, regardless of
player fishing skill level and reel it in. This can be useful for legendary fish
when the player is not equipped with special rings.

.. _the bitbucket:
.. _the FinalDoom edition bitbucket : https://bitbucket.org/FinalDoom/ffxi-fishingform/
.. _the FFEvo download page: http://www.ffevo.net/files/file/214-fishingform-fd-edition/
.. _TortoiseHG: http://tortoisehg.bitbucket.org/

For the FinalDoom edition, you can check out the source and
submit bugs or ideas at `the FinalDoom edition bitbucket`_.
Version control is done using Mercurial (try TortoiseHG_), and issue tracking
uses bitbucket's built-in JIRA-like system. The "official" site for downloading
the most recent version is `the bitbucket`_ or at `the FFEvo download page`_.

.. _guide here: http://www.howtogeek.com/howto/windows-vista/make-user-account-control-uac-stop-blacking-out-the-screen-in-windows-vista/

.. IMPORTANT:: **Windows Vista+ Notice**

    If you are running a system which uses UAC, there are a
    few extra steps that are necessary to run this (and many other
    related applications). Firstly, the program must be run as an admin.
    It currently displays an error about not being able to find FFACE.dll
    if it is not. Secondly, your UAC settings must be modified to not
    dim the desktop when requesting administrator privileges. The dimming
    is actually Windows switching to another desktop, which breaks
    Final Fantasy for whatever reason. (Note that locking your screen will
    do the same thing). See the `guide here`_ for how to disable the
    screen dimming thing. There are a variety of ways to satisfy whatever
    level of technical expertise you like to practice.

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

-------
Changes
-------

1.6.7.18
--------
- Fixed ring equipping

1.6.7.17
--------
- Full inventory "other" allows custom commands. They have 10 seconds to reduce inventory
- On full inventory, warp and logout or shutdown will be executed after other command, if inventory continues to be full

1.6.7.16
--------
- Itemizer/itemtools accepts multi-word fish

1.6.7.13
--------
- Now persists between login/logout
  - Will attach to single logged in character (beware multiboxers)
- Minor related bugs TODO

1.6.7.8
-------
- Now tracks skillups (including uncertainty on 0.2 or 0.3 level up) in info tab
- Options includes gear tab for gear equipped when fishing
- Belts will auto-cast when equipped. Rings TODO
- When fatigue is reached, can optionally warp then optionally logout or shutdown

1.6.7.3
-------
- Cast wait time resets at Japanese midnight
- Stats tab shows amout of time fished and catches per hour
- Full inventory "other" command accepts multiple itemizer commands, semicolon separated
- Fixes stopping for "unknown reason" when there is slight lag on /fish
- START can be clicked any time, fishing will resume from game state

Previous
--------
See `MisterC's version`_.
