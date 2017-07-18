/*
 * User: knoob
 * Date: 7/17/2017
 * Time: 9:51 PM
 * 
 * Please put a directory named "PlzFixThese" with script files in it in the same directory as the exe file.
 */
using System;

namespace HigurashiSEToVoice
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			
			// TODO: Implement Functionality Here
			
			NotStatic mynotstaticthing = new NotStatic();
			
			Console.Out.WriteLine("===========================================\n== DONE!\n===========================================");
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}