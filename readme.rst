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

- **NEW**
	- Automatically sync FishDB with cloud database
	- Syncs silently at application start
	- Syncs silently after a few new fish are caught

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
	- Ctrl+ s, p, t, l, and r work like in game
- Customizable chat detection and result actions for incoming chat
- Fishing statistics tab
    - Shows total casts, time fishing, and catches per hour
    - Shows counts and percentages for catches, releases, and lost fish types
- Improved information on the Info tab (inventory/satchel/sack, skill, gil, bait count)
    - Skillups show next to skill level as they occur, including uncertainty and total skillups
- Kill fish manually by clicking progress bar during fight
- Option to show fish HP and time left to reel in
- Dialog turns red/comes into focus when it stops unexpectedly
- Can be opened before FFXI, persists after logout or character change
- Automatically equip gear, rod, and bait

------------
Instructions
------------

The program is meant to be straightforward to run. Extract the files anywhere you please and
run it (as an administrator, if necessary), get your character to a fishing spot, and click
the start button. You will probably want to take a look at the options before doing so, however.
Standard defaults should work for the most part but customization is sometimes necessary or
desirable. If run while dual boxing with two characters logged in, a window will appear asking
you to select a character. Otherwise, it will automatically attach to any logged in character,
or show a message suggesting you log in.

If bait and rod are specified in the gear options page, no bait or rod need be equipped. Otherwise,
you must equip appropriate bait and rod before clicking start. The lists under the start button
will be populated with fish that have been found in your area, with your bait and rod. As new fish
are encountered, they will be added to the list. However, unless the "unknowns?" checkbox is checked,
all fish fish not in the "wanted" list will be released. Items and monsters will also be released
according to options set in Options>Fight.

Fish may be moved between the "wanted" and "unwanted" lists by double clicking the fish name. They
may also be renamed by right clicking and selecting rename. Monsters are given sequential names, eg.
*Monster (_42_)*, so please rename them appropriately.

As you fish, the stats and info tabs will be updated to reflect fishing history and ingame
status. The stats can be cleared by right clicking and selecting *clear stats*. The wait time
between casts will be updated as fishing fatigue sets in, and fishing will stop after fatigue
is detected. It can also be set to stop after a number of catches or skillups, and will stop when
your inventory is full by default. Several options exist to handle what to do after these stops.

As of version 1.7, FishDB additions are added to a remote database, and the program will automatically
update your local FishDB with others' additions. No identifiable information is sent or accessible
to any developer or other persons. See FishDB Information below for more details.

-------
Options
-------

General
-------
- Always on top and opacity
- Sound on errors/stop
- Basic tell detection alert
- Auto-cast sneak for sneak fishing
- Stop fishing at target skill level
- Cast wait resets at JP midnight
- Maximum no catches before fatigue stop is triggered

Chat
----
- Stop fishing, flash window, note on chat log for incoming tell, PT, LS, say chat
- Enable or disable custom detection
- GM Detection

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
- Grab bait from sack or satchel when out
- Stop fishing when inventory is full
- Warp then optionally log out or shut down when inventory is full, when fatigued, or when out of bait

------------------
FishDB Information
------------------

All fish are identified by 3 IDs ingame (a fourth is ignored). These IDs, fish names, bait,
zones and their position in the Wanted or Unwanted list are stored in XML files according to
the rod used to catch them. These XML files are stored in the FishDB folder next to the
FishingForm executable. In general, it should not be necessary to modify these files. If you
do so, please be careful, as they are integral to the functioning of the program.

Rod XML
-------

All but one XML file (DBSync.xml, described later) are of the following format:

.. code:: xml
	<Rod name="Rod Item Name">
		<Fish name="Fish Name" wanted="Yes|No" ID1="INT" ID2="INT" ID3="INT"[ ID4="28"][ new=""][ rename="Old Name"]>
			<Baits>
				<Bait[ new=""]>Bait Name</Bait>
			</Baits>
			<Zones>
				<Zone[ new=""]>Windower Resources.xml Zone Name</Zone>
			</Zones>
		</Fish>
	</Rod>

All fish must have at a minimum a name, wanted status (Yes or No), and 3 integer IDs. The fourth
is sometimes present on older fish, but it is now ignored.

In the event that you are manually modifying an XML file, you will want to take note of the *new*
and *rename* attributes for fish, and the *new* attribute for baits and zones. At present, the DBs
are only additive. Any removals will only be reflected on your local system. If you add a new fish,
bait, or zone, be sure to attach the *new* attribute with any value. If you rename a fish, add the
*rename* attribute with its value set to the old fish name, exactly as it was. These attributes
are how the program knows what parts of the XML to upload to the remote database. Without the
rename attribute, duplicate fish will appear. When modifying rod xml files, you will likely also
want to examine the DBSync.xml file, described below.

DBSync XML
----------

DBSync.xml is a file used to reduce load on the remote database. Its format is as follows:

.. code:: xml
	<Updates>
		<Update host="MySQL Connection String" dbver="1.7.0.7">
			<Rod name="Rod Item Name" db="UTC Timestamp" xml="UTC Timestamp" />
		</Update>
	</Updates>

All are required elements, and there is only one you should be concerned with changing, the *xml* attribute
of a <Rod /> node. This attribute is a UTC timestamp of the format M[M]/D[D]/YYYY H[H]:mm:ss [PM|AM]. It
is used to track the last time the XML file the rod node describes, which can be referenced by comparing the
name attribute to that in another xml file. Should you edit any rod XML file and append *new* or *rename*
attributes to any fish, bait, or zone nodes, update the *xml* attribute so that its date is at least one
second larger than that in the *db* attribute. This will tell the program you have XML changes to upload.

If a rod XML is messed up, dirty, deleted, or otherwise unsatisfactory, an easy fix is to delete the file and delete
the corresponding line in DBSync.xml. The program will then download the file as it exists in the remote database.

Other attributes in the DBSync file are described below. **None of the following attributes should be modified
by hand for any reason.**

There can be multiple updates, depending on what database is in use. They are keyed by the *host* attribute,
which is set to the connection string used to connect to the remote database. If you alter this, your
whole FishDB will be resynced, which takes 20 or more seconds, depending on connection speed. Please do
not do this. FishingForm currently utilizes a free database, and the only real limitation is number of
concurrent connections (5). More, longer connections means some people can't connect. Additionally, the
access string is associated with a limited user account which can only execute routines (which you will have
to find in the code). Please don't use it to connect and screw around with things.

The *dbver* attribute denotes the version of the program the database is associated with. If this value is
below a required threshold, defined in the exe, all <Rod /> child nodes will be reset to default and all
FishDB data will be synced, so please don't alter this.

The *name* attribute corresponds to the name attribute on a similar node found in each rod XML file.

The *db* attribute of the <Rod /> nodes denotes the last time (UTC) a database sync was performed for the
noted rod. Some rods do not have any data, so the default value remains.

-------
Changes
-------

1.7.0.25
--------
- Bait fetch bug fix

1.7.0.24
--------
- Option to restart fishing at midnight

1.7.0.23
--------
- Pirates zone name fix

1.7.0.22
--------
- More intelligence when moving items and equipping things
- Minor fixes

1.7.0.21
--------
- Misspelling fix

1.7.0.20
--------
- Bug fixes

1.7.0.19
--------
- Automatic bait/fish handling when out of bait or full inventory: auto get/put from/to sack/satchel

1.7.0.18
--------
- New database

1.7.0.17
--------
- Lots more error checking for DB stuff
- DB tab after chat tabs displays info about syncs

1.6.7.49
--------
- Import useful fixes from 1.7

1.7.0.16
--------
- Manually changed XML updates correctly

1.7.0.15
--------
- Fix startup DB sync to actually check for updated XML (without loading into memory unnecessarily)

1.7.0.14
--------
- Lists should still populate even with all ignores on, if the IDs are known

1.7.0.13
--------
- Fixed the bell when using ctrl+r etc. in chat bar
- Fixed equipping correct gear from settings (overrides currently equipped gear)

1.7.0.12
--------
- Hopefully fixes inactive connections being left open

1.7.0.11
--------
- Skill level now displays total skillups as well as points into level

1.7.0.10
--------
- Fixed display issues for users with windows set to display larger fonts

1.7.0.9
-------
- Actually fixed fish renaming from the DB

1.7.0.8
-------
- Fixed fish renaming from the DB

1.7.0.7
-------
- Fixed fish naming

1.6.7.48
--------
- Fixes from 1.7 (non DB stuff) applied to 1.6

1.7.0.6
-------
- Renames should be pushed to the DB

1.7.0.5
-------
- Bug fixes

1.7.0.1
-------
- Bug fixes
- Trying to fix Windows 8 display bug

1.7.0.0
-------
- **MAJOR UPDATE**
- FishDB now syncs with a MySQL database
- Automatic sync at start
- Automatic sync after a few new fish

1.6.7.41
--------
- Option to not stop fishing when inventory is full

1.6.7.40
--------
- Bug fixes

1.6.7.39
--------
- Fish names are now required to be unique. Haven't checked XML, but it'll sort itself out
- Should grab bait from sack/satchel if configured to do so
- Bug fixes

1.6.7.36
--------
- Fixed runon chat lines activating chat detectors

1.6.7.35
--------
- Ctrl+s etc properly insert chat mode in chat box

1.6.7.34
--------
- Fixed ring equip menus

1.6.7.33
--------
- Checkbox to enable or disable chat filters

1.6.7.32
--------
- Option to stop fishing at target skill level

1.6.7.31
--------
- Bug fixes

1.6.7.30
--------
- Customizable chat detection options
  - Additional options easily added. Ask away

1.6.7.29
--------
- Bug fixes
- Tell and gm detect flashes window

1.6.7.28
--------
- Check equipment to avoid extra equip lines

1.6.7.27
--------
- Fixed rod/bait options, they save and don't break things

1.6.7.26
--------
- Automatically re-equip broken rods
- Rod and bait can be selected in options panel for easy equipping

1.6.7.25
--------
- Itemtools checkbox is on last options page

1.6.7.24
--------
- Vana'diel time is estimated from system time when not logged in

1.6.7.23
--------
- Added warp/logout-shutdown on out of bait

1.6.7.22
--------
- Error message when not run as admin is more descriptive

1.6.7.21
--------
- Rings should auto-cast somewhat intelligently
- Warp fixed for anyone not using spellcast

1.6.7.20
--------
- Fixed warp, etc. on full inventory

1.6.7.19
--------
- Fixed tab order
- Slightly redone gear options page

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
- See `MisterC's version`_.
