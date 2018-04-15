using System;
using MMITest;

namespace MMITest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// Searching search = new Searching ();
			// search.Breitensuche ();
			// search.Tiefensuche ();
			// read file
			// String fileWindows = "C:\\Users\\Public\\TestFolder\\WriteText.txt\"";
			String fileUbuntu = "/home/hanna/Desktop/Graph1.txt";
			string text = System.IO.File.ReadAllText(fileUbuntu);
			foreach (var line in text) {
				Console.WriteLine (line);
			}
		}
	}
}
