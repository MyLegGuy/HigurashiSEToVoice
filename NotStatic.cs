/*
 * User: knoob
 * Date: 7/17/2017
 * Time: 9:51 PM
 * 
 * PlaySE(channel, path, volume, pan?)
 * PlayVoice(channel, filenme, volume)
 * channel is 1-8?
 * path is relative to StreamingAssets/SE/ for SE and relative to StreamingAssets/voice/ for Voices
 * volume is 0-256? 128 is usually used for SE
 * pan is usually 64.
 * 
 * For voices, the filename must be at least 4 characters long. Why? Because of whoever programmed this game was apathetic when fixing the PlayVoice command.
 * 
 * Whoever put this together doesn't put a space before the first argument. 
 * PlaySE( 3, "wa_021", 128, 64 );
 * PlaySE(4, "s05/13/101300502", 128, 64);
 */
using System;
// For File.ReadAllLines, File.WriteAllLines, and Directory.Exists
using System.IO;

namespace HigurashiSEToVoice
{
	/// <summary>
	/// It's not static
	/// </summary>
	public class NotStatic
	{
		public NotStatic()
		{
			if (Directory.Exists(Options.folderName)==false){
				Console.Out.WriteLine("{0} was not found! Please create that folder and put your script files in it.",Options.folderName);
				return;
			}
			string[] fileEntries = Directory.GetFiles(Options.folderName, "*.*", SearchOption.AllDirectories);
			for (int i = 0; i < fileEntries.Length; i++) {
				FixScriptFile(fileEntries[i]);
			}
		}
		
		// Give it a PlaySE line. Returns true if it's a voice line and not a normal SE line. Returns false otherwise.
		// This is where you add exceptions for sound effects. You may want to make one that returns false if the channel is 3, or something. Not that you would really need to because this will catch every default sound effect.
		//
		// This will return false if...
		// The string "wa_" is found in the sting.
		bool IsVoiceLine(string line){
			// If "wa_" is found in the line, return false.
			// As of chapter 4, all sound effect files start with "wa_".
			if (line.IndexOf("wa_")!=-1){
				return false;
			}
			return true;
		}
		
		// Removes the second string from the end of the first string if the second string is at the end of the first string.
		// If the second string is not at the end of the first string, the first string is returned.
		string TrimEndString(string toTrim, string toLookFor){
			if (toTrim.Length>=toLookFor.Length){
				if (toTrim.Substring(toTrim.Length-toLookFor.Length)==toLookFor){
					return toTrim.Substring(0,toTrim.Length-toLookFor.Length);
				}
			}
			return toTrim;
		}
		
		// Returns an array of the arguments of a passed PlaySE command
		// Returned arguments are in order
		// Passed PlaySE string should already have the indentation trimmed
		// Results are like
		// 4
		// "somefile"
		// 128
		// 64
		string[] GetPlaySEArguments(string line){
			// If the line is too short, and probably not a PlaySE command, return.
			if (line.Length<7){
				return null;
			}
			// Split the command up. Results are like
			// PlaySE( 3
			//  "aaa"
			//  128
			//  64 );
			string[] _splitSECommand = line.Split(',');
			// Remove "PlaySE(" from the start of the first one.
			_splitSECommand[0] = _splitSECommand[0].Substring(7);
			// Remove the parentheses at the end of the last one.
			_splitSECommand[_splitSECommand.Length-1] = TrimEndString(_splitSECommand[_splitSECommand.Length-1]," );");
			_splitSECommand[_splitSECommand.Length-1] = TrimEndString(_splitSECommand[_splitSECommand.Length-1],");");
			for (int i=0;i<_splitSECommand.Length;i++){
				// Remove spaces from the beginning and end.
				_splitSECommand[i] = _splitSECommand[i].TrimStart((char)32);
				_splitSECommand[i] = _splitSECommand[i].TrimEnd((char)32);
			}
			
			// Override the volume and channel if the options file says to.
			if (Options.overrideChannel!=-1){
				_splitSECommand[0]=Options.overrideChannel.ToString();
			}
			if (Options.overrideVolume!=-1){
				_splitSECommand[2]=Options.overrideVolume.ToString();
			}
			
			return _splitSECommand;
		}
		
		// The first argument is an entire PlaySE command.
		// Returns a string that's the same as the one you gave it if it's a normal PlaySE command.
		// Returns a PlayVoice command if you passed it a voice PlaySE command.
		// Example usage:
		// DoSELine("PlaySE(4, \"s05/13/101300502\", 128, 64);");
		string DoSELine(string line){
			// If this isn't a voice line PlaySE command, return.
			if (IsVoiceLine(line)==false){
				return line;
			}
			// Get the arguments
			string[] _playSEArguments = GetPlaySEArguments(line);
			// Make a PlayVoice command with the arguments from the PlaySE command.
			string _playVoiceResult = String.Format("PlayVoice( {0}, {1}, {2} );",_playSEArguments[0],_playSEArguments[1],_playSEArguments[2]);
			return _playVoiceResult;
		}

		// Fixes the script file at this location. Filepath is the path to the file.
		void FixScriptFile(string filepath){
			Console.Out.WriteLine("Doing {0}",filepath);
			// An array of all the lines in the current script
			string[] lines = File.ReadAllLines(filepath);
			// The current line we're dealing with
			string line;
			// Changed at the start of the for loop. Is true if trimming character 09 from the start actually changed the length of the string.
			bool DidRemoveIndentation;
			// Loop through every line to find one that's a PlaySE line
			for (int i = 0; i < lines.Length; i++) {
				// Remove indentation
				line = lines[i].TrimStart((char)09);
				if (line.Length<lines[i].Length){
					DidRemoveIndentation=true;
				}else{
					DidRemoveIndentation=false;
				}
				if (line.Length>=6){
					if (line.Substring(0,6)=="PlaySE"){
						if (Options.WriteLines==true){
							Console.Out.Write(line);
						}
						line = DoSELine(line);
						if (Options.WriteLines==true){
							Console.Out.Write(" -> {0}\n",line);
						}
						// If we removed the indentation before, add it back.
						if (DidRemoveIndentation==true){
							line = (Char)09+line;
						}
						lines[i]=line;
					}
				}
			}
			File.WriteAllLines(filepath,lines);
		}
	}
}
