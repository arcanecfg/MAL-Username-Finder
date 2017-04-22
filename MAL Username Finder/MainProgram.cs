/*
 *        d8888                                                .d888         
      d88888                                               d88P"          
     d88P888                                               888            
    d88P 888888d888 .d8888b 8888b. 88888b.  .d88b.  .d8888b888888 .d88b.  
   d88P  888888P"  d88P"       "88b888 "88bd8P  Y8bd88P"   888   d88P"88b 
  d88P   888888    888     .d888888888  88888888888888     888   888  888 
 d8888888888888    Y88b.   888  888888  888Y8b.    Y88b.   888   Y88b 888 
d88P     888888     "Y8888P"Y888888888  888 "Y8888  "Y8888P888    "Y88888 
                                                                      888 
                                                                 Y8b d88P 
                                                                  "Y88P"  
MAL Username Finder v1.0
 * www.WastedWolf.com
 * www.YouTube.com/Arcanecfg
 * 20/04/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MAL_Username_Finder
{
    class MainProgram
    {
        public static int count;
        public static bool reset = false;
        static void Main(string[] args)

        {
            string[] usernameArray = { null };
            List<string> usernameList = new List<string>();
            DisplayLoadInfo();
            Console.Write("Username List: ");
            string usernamePath = Console.ReadLine();
            if (System.IO.File.Exists(usernamePath))
            {
                usernameArray = System.IO.File.ReadAllLines(usernamePath);
                usernameList = usernameArray.ToList();
            }

            Console.Write("Thread Count (default 25): ");
            string threadCount = Console.ReadLine();
            int threads = (threadCount == string.Empty ? 25 : Convert.ToInt32(threadCount));
            System.Net.ServicePointManager.DefaultConnectionLimit = threads;
            System.Net.ServicePointManager.Expect100Continue = false;
            ThreadPool.SetMaxThreads(threads, threads);

            count = usernameList.Count();
            MALCheck Checker = new MALCheck();
            foreach (string username in usernameList)
            {
                //Checker.CheckUsername(username);
                System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(Checker.CheckUsername), username);
            }
            var inputKey = Console.ReadKey();
            switch (inputKey.Key)
            {
                case ConsoleKey.P:
                    reset = true;
                    OutputUsernames();
                    break;
            }
            Console.ReadKey();
            
        }

        static void CenterPrintString(string msg)
        {
            Console.SetCursorPosition((Console.WindowWidth - msg.Length) / 2, Console.CursorTop);
            Console.WriteLine(msg);
        }

        static void DisplayLoadInfo()
        {
            string title = "MyAnimeList Username Finder 1.0";
            string author = "Written by Arcanecfg - www.WastedWolf.com";
            string separator = "+-------------------------------------------------+";
            Console.Title = title;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Environment.NewLine);
            CenterPrintString(title);
            Console.ForegroundColor = ConsoleColor.White;
            CenterPrintString(author);
            CenterPrintString("Release: 4/20/2017");
            CenterPrintString(separator);
            Console.WriteLine();
            Console.ResetColor();
        }
        public static void OutputUsernames()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Environment.NewLine + "Wrote available names to file: MAL_AVAILABLE.TXT");
            System.IO.File.AppendAllLines("MAL_AVAILABLE.txt", MALCheck.availableList);
            Console.ResetColor();
        }
    }
}
