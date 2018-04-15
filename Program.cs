using System;
using MMITest;
using System.IO;
using System.Text;
using System.Threading;
using System.Reflection;

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

            String file = String.Empty;
			#if __MonoCS__
                        file = "/home/hanna/Desktop/Graph1.txt";

#else
            file = @"C:\Users\Hanna\\MMI\Graph2.txt";
#endif


            // string file = @"..\Graphen\Graph2.txt";
            if (File.Exists(file))
            {
                string text = File.ReadAllText(file);
                foreach (var line in text)
                {
                    Console.WriteLine(line);
                }
            }

        }
	}
}
