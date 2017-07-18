/*
 * User: knoob
 * Date: 7/17/2017
 * Time: 10:50 PM
 */
using System;

namespace HigurashiSEToVoice
{
	/// <summary>
	/// Description of Options.
	/// </summary>
	public static class Options
	{
		// False won't write anything to the console. Makes stuff more speedy.
		public static bool WriteLines = true;
		// Change this from -1 to override the PlaySE channel argument and set it to this instead
		public static int overrideChannel = -1;
		// Same as overrideChannel, but for the volume
		public static int overrideVolume = -1;
		// Folder with scripts in it.
		public static string folderName = "./PlzFixThese";
	}
}
