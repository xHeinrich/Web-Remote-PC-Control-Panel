using System;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace WebPCControlPanelClient.Lib
{
    static class NetworkInfo
    {
        //network stats
        public static long bytesSentSpeed = 0;
        public static long bytesReceivedSpeed = 0;
        public static DateTime byteSentLast;
        public static DateTime byteReceivedLast;

        /// <summary>
        /// Shows the networktraffic using ipv4int class.
        /// alot faster than ShowNetworkTraffic as it doesnt have to wait for perf things and thread.sleep
        /// int type 0 = sent(upload), 1 = recieved(download)
        /// </summary>
        /// 
        public static long ShowNetworkTraffic(int type)
        {
            IPv4InterfaceStatistics interfaceStats = NetworkInterface.GetAllNetworkInterfaces()[0].GetIPv4Statistics();
            switch (type)
            {
                case 0:
                    long sendSpeed = interfaceStats.BytesSent - bytesSentSpeed;
                    bytesSentSpeed = interfaceStats.BytesSent;
                    //seconds since last update
                    TimeSpan lastCheckSend = DateTime.Now - byteSentLast;
                    byteSentLast = DateTime.Now;
                    Console.WriteLine(lastCheckSend.TotalSeconds.ToString() + " -: Timer upRate" );
                    return Convert.ToInt64(((double)(sendSpeed / 1024L)/ lastCheckSend.TotalSeconds)*8L);
                case 1:
                    long recieveSpeed = interfaceStats.BytesReceived - bytesReceivedSpeed;
                    bytesReceivedSpeed = interfaceStats.BytesReceived;
                    TimeSpan lastCheckRec = DateTime.Now - byteSentLast;
                    byteReceivedLast = DateTime.Now;
                    Console.WriteLine(lastCheckRec.TotalSeconds.ToString() + " -: Timer downRate");
                    return Convert.ToInt64(((double)(recieveSpeed / 1024L) / lastCheckRec.TotalSeconds)*8L);
                default:
                    return 0;
            }
        }
        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
    }
}

