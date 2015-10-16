using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;

namespace webping
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string url = args[0];
                if (!url.ToLower().StartsWith("http"))
                    url = "http://" + url;

                if (CheckForArguments(args, url))
                    return;


                if (args.Contains("-t"))
                {
                    PingUntilBreak(url);
                }
                else
                {
                    PingCount(url, 4);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine("Error " + ex.Message);
                Console.WriteLine("");
            }

        }

        private static bool CheckForArguments(IEnumerable<string> args, string url)
        {
            if (!args.Any())
            {
                Console.WriteLine("usage eg: webping www.website.com");
                Console.WriteLine("-t Ping the specified host until stopped.");
                return true;
            }
            Console.WriteLine("webping " + url);
            return false;
        }

        private static void PingUntilBreak(string url)
        {

            Console.WriteLine("Press ESC to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                    DoSinglePing(url);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        }

        private static void PingCount(string url, int count)
        {
            for (int i = 0; i < count; i++)
            {
                DoSinglePing(url);
            }
        }

        private static void DoSinglePing(string url)
        {
            var sw = new Stopwatch();
            var client = new WebClient();

            sw.Reset();
            sw.Start();
            string content = client.DownloadString(url);
            sw.Stop();

            string msg = string.Format("Reply from {0}: bytes {1} time {2} ms", url, content.Length,sw.ElapsedMilliseconds);
            Console.WriteLine(msg);

            if (sw.ElapsedMilliseconds < 1000)
            {
                Thread.Sleep(1000 - (int)sw.ElapsedMilliseconds);
            }

        }
    }
}
