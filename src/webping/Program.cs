using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace webping
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 1)
            {
                Console.WriteLine("Syntax; ");
                Console.WriteLine("webping <url> <count>");
                Console.WriteLine("-1 Ping the specified host until stopped.");

                return;
            }

          

            try
            {

                string url = args[0];
                int count = 4;

                if (args.Length>1)
                {
                    count = Convert.ToInt32(args[1]);
                }


                if (!url.ToLower().StartsWith("http"))
                    url = "http://" + url;

                if (CheckForArguments(args, url))
                    return;


                if (args.Contains("t"))
                {
                    PingUntilBreak(url);
                }
                else
                {
                    PingCount(url, count);
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
            if (count == -1)
            {
                while (true)
                {
                    DoSinglePing(url);
                }
            }


            for (int i = 0; i < count; i++)
            {
                DoSinglePing(url);
            }
        }



        private static long GetHead(string url)
        {
            var sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request =  new HttpRequestMessage(HttpMethod.Head,new Uri(url));

            HttpResponseMessage response = httpClient.SendAsync(request).Result;
            sw.Stop();
            
            string msg = $"Reply from {url}: code: {response.StatusCode} time {sw.ElapsedMilliseconds} ms";
            Console.WriteLine(msg);
            return sw.ElapsedMilliseconds;
        }



        private static void DoSinglePing(string url)
        {
            var msec = GetHead(url);

            if (msec  < 1000)
            {
                Thread.Sleep(1000 - (int)msec);
            }

        }
    }
}
