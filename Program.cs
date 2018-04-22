using System;
using MMITest;
using System.IO;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;

namespace MMITest
{
	class MainClass
	{

        public static void Main ()
		{            
            String file = String.Empty;
			#if __MonoCS__
                        file = "/home/hanna/Desktop/Graph1.txt";

#else
            file = @"C:\Users\Hanna\\MMI\Graph2.txt";
#endif
            Graphenbibiliothek einleser = new Graphenbibiliothek();
            IList<Node> test = einleser.KantenListeEinlesen(file, false);



            // Breitensuche
            Node testen = new Node(0);
            Searching search = new Searching();
            search.Tiefensuche();
            int b = 0;
        }
    }
}
