//The count is still broken, need to fix the threadpool synchronization.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace MAL_Username_Finder
{
    class MALCheck
    {
        public static List<string> availableList = new List<string>();
        List<string> unavailableList = new List<string>();
        int totalCount = MainProgram.count;
        int count = 0;
        //private static Object _countLock = new object();
        public void CheckUsername(Object Username)
        {
            if (!MainProgram.reset)
            {
                if (!(availableList.Contains(Username.ToString()) || unavailableList.Contains(Username.ToString())))
                {

                    try
                    {
                        string username = Username.ToString();
                        string url = "https://myanimelist.net/rss.php?type=rw&u=" + username;
                        HttpWebRequest pageRequest = (HttpWebRequest)WebRequest.Create(url);
                        pageRequest.KeepAlive = true;
                        HttpWebResponse pageResponse = (HttpWebResponse)pageRequest.GetResponse();
                        if (pageResponse.StatusCode == HttpStatusCode.NotFound)
                        {
                            DoneChecking(username, true);
                        }
                        else
                        {
                            DoneChecking(username, false);
                        }
                        pageResponse.Dispose();
                    }

                    catch (WebException wEx)
                    {
                        if (wEx.Response != null)
                        {
                            HttpWebResponse errResp = (HttpWebResponse)wEx.Response;
                            if (errResp.StatusCode == HttpStatusCode.NotFound)
                            {
                                DoneChecking(Username.ToString(), true);
                            }
                            errResp.Dispose();
                        }
                    }
                }
            }
        }

        private void DoneChecking(string username, bool available)
        {

            System.Threading.Interlocked.Increment(ref count);
            //Console.WriteLine(count.ToString() + "/" + totalCount.ToString());
            switch (available)
            {
                case true:
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(String.Format("[{0}/{1}] Available username -- {2}",
                            count.ToString(),totalCount.ToString(),username.ToString()));
                        availableList.Add(username.ToString());
                        break;
                    }
                case false:
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(String.Format("[{0}/{1}] Unavailable username -- {2}",
                            count.ToString(),totalCount.ToString(),username.ToString()));
                        unavailableList.Add(username.ToString());
                        break;
                    }
            }

            if (count >= totalCount)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(String.Format("{0}Done checking.{1}Found {2} available usernames.",
                        Environment.NewLine, Environment.NewLine, availableList.Count()));
                MainProgram.OutputUsernames();
            }
        }
    }
}
