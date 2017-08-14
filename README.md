## PlaySE -> PlayVoice for Higurashi

---

There is a PlayVoice command probably leftover from d2b VS Deardrops. Chapter 1 can use the command, the developers broke it for chapters 2 through 4, and then fixed it for chapter 5.

### How to use

---

This program will convert any PlaySE commands that don't play files with "wa_" in the name to PlayVoice commands while keeping the same arguments. You need the .NET Framework or Mono as this was made with C#.

Example:

	PlaySE( 4, "s05/13/101300469", 128, 64 ); -> PlayVoice( 4, "s05/13/101300469", 128 );
	PlaySE( 3, "wa_037", 128, 64 ); -> Not changed.

Put script files in a folder called "PlzFixThese" that is in the same directory as the exe file.

PlayVoice works the same as PlaySE, but it will wait for the voice to finish playing if the user is on auto mode. It also has 8 more channels that are separate from the SE and BGM ones. **Most importantly, it looks for the files in HigurashiEpXX_Data/StreamingAssets/voice/ instead of HigurashiEpXX_Data/StreamingAssets/SE/**. After using this, please move all the voice files to the voice folder.

**You will need to set some global flags too.** Edit HigurashiEpXX_Data/StreamingAssets/Update/init.txt and add some lines right before "CallScript( "flow" );".

**Add these lines if you are using this for chapter 1, Onikakushi. I have tested this.**
	
	// Enable voice files without a special prefix
	SetGlobalFlag(GVOther, FALSE);
	// On a scale of 0 to 100, the volume of the voices.
	SetGlobalFlag(GVoiceVolume, 75);
	CallScript( "flow" );

**Only add the second line if you're using this for chapter 5. I have not tested this.**
	
	SetGlobalFlag(GVoiceVolume, 75);
	CallScript( "flow" );


The CallScript command is only there to show you where to add the lines. It should already be in init.txt. After editing the file, **delete HigurashiEpXX_Data/StreamingAssets/CompiledUpdateScripts.**

Setting the GVOther flag to FALSE tells the game that normal voice lines aren't disabled. Changing GVoiceVolume lets us change the voice volume from 0 without an in-game settings menu.

**After you've done everything correctly, you will need to start the game, close it, and then open it again.**

Explination of how voice volume global loading will happen THE FIRST TIME:

	* User starts the game
	* Globals are loaded, voice volume is 0. Voice audio controller volume set to 0.
	* init.txt sets the voice volume to 75. The voice audio controller volume isn't changed.
	* The user exits the game, saving the new global.
	* User starts the game
	* Globals are loaded, voice volume is 75. Voice audio controller volume is set to 75.

---

## Chapters 2 through 4 and DLL modding

Chapters 2 through 4 could easily be modded to fix the command. Here are the two functions I changed in order to fix the command. I only partially copied these functions so they mostly just show the code I wrote. Don't forget about the code that was already in the function.
	

	// BurikoMemory.cs
	public BurikoVariable GetGlobalFlag(string flagname)
	{
		int num;
		if (!this.variableReference.TryGetValue(flagname, out num))
		{
			// This is the old line of code
			// throw new Exception("stuff here");
			// This is the new line of code
		    return new BurikoVariable(0);
		}
		// Other stuff that was already here.
	}

	// Wait.cs
	public Wait(float length, WaitTypes type, OnFinishWait onFinishDelegate)
	{
		// Add this
		if (type == WaitTypes.WaitForVoice)
		{
			type = WaitTypes.WaitForAudio;
			if (length < 0f)
			{
				length *= -1f;
			}
		}
		// Other code that was already here.
	}

I tested these modded functions with chapter 3.

GetGlobalFlag is modded to return an empty flag instead of throwing an exception when the flag it's checking doesn't exist. Now, when the flags for voice disabling are checked, the program won't crash AND will receive a FALSE disable flag. The Wait command is modded to use WaitTypes.WaitForAudio instead of WaitTypes.WaitForVoice because WaitTypes.WaitForVoice seems to be ignored. Also, for some reason, the length of the sound file is negative. I fix that in the Wait function too.

---

For reference:

	PlayVoice(channel, filename, volume)
	PlaySE(channel, filename, volume, pan)