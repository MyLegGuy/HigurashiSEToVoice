### PlaySE -> PlayVoice for Higurashi

Chapter 5 finally fixes the PlayVoice command that was leftover from d2b VS Deardrops, I think. The PlayVoice command is still broken in chapters 1-4 as of 7/17/17 because the developers are too lazy to update the old games to use the improved engine. 

This program will convert any PlaySE commands that don't play files with "wa_" in the name to PlayVoice commands while keeping the same arguments. You need the .NET Framework or Mono as this was made with C#.

Example:

	PlaySE( 4, "s05/13/101300469", 128, 64 ); -> PlayVoice( 4, "s05/13/101300469", 128 );
	PlaySE( 3, "wa_037", 128, 64 ); -> Not changed.

Put script files in a folder called "PlzFixThese" that is in the same directory as the exe file.

PlayVoice works the same as PlaySE, but it will wait for the voice to finish playing if the user is on auto mode. It also has 8 more channels that are separate from the SE and BGM ones. **Most importantly, it looks for the files in HigurashiEpXX_Data/StreamingAssets/voice/ instead of HigurashiEpXX_Data/StreamingAssets/SE/**. After using this, please move all the voice files to the voice folder.

For reference:

	PlayVoice(channel, filename, volume)
	PlaySE(channel, filename, volume, pan)

Chapters 1 through 4 could easily be modded to fix the command, but I don't know how to legally distribute a dll mod.