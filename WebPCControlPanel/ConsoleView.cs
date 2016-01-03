using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPCControlPanel
{
    /// <summary>
    /// Static console data that gets re-rendered to the screen  every update()
    /// </summary>
    public static class ConsoleData
    {
        public static string AppName { get; set; }
        public static string AuthorName { get; set; }
        public static int CurrentConnectedClients { get; set; }
        public static int ListNum { get; set; }
        public static long DownloadTotal { get; set; }
        public static long UploadTotal { get; set; }
    }
    /// <summary>
    /// Class that draws data for the server application to the console window
    /// </summary>
    public static class ConsoleView
    {
        /// <summary>
        /// Where the typing pointer is currently on update
        /// </summary>
        public static int CurrentConsoleLeft;

        /// <summary>
        /// 
        /// </summary>
        public static void ClearConsole()
        {
            for(int i = 0; i < 19; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }
        /// <summary>
        /// clears user input
        /// </summary>
        public static void ClearInput()
        {
            Console.SetCursorPosition(0, 22);
            Console.Write("║                                                                              ║");
            Console.SetCursorPosition(1, 22);
        }
        /// <summary>
        /// Add a 78 char log to line 20
        /// </summary>
        /// <param name="logLine">String you want to show as a log</param>
        public static void AddLog(string logLine)
        {
            //Clear last log
            Console.SetCursorPosition(0, 20);
            Console.Write("║                                                                              ║");
            //Write new log
            Console.SetCursorPosition(1, 20);
            Console.Write(logLine);
        }
        /// <summary>
        /// Show the top 5 currently connected clients
        /// </summary>
        /// <param name="listNum">Defaulted to 1, shows the number e.g 1 = 0-4 client list, 2 = 5-10 client list</param>

        public static void WriteClients(int listNum)
        {
            ConsoleData.ListNum = listNum;
            if (ConsoleData.ListNum == 0)
            {
                ConsoleData.ListNum = 1;
            }
            List<string> clients = new List<string>();
            foreach (var client in Program.Clients)
            {
                foreach (var networkClient in Program.ClientUsage)
                {
                    if (networkClient.pcId == client.pcId)
                    {
                        string clientString = client.pcId.ToString() + " " + client.pcName + " " + client.ipSocket + " " + networkClient.downloadSpeed.ToString() + " " + networkClient.uploadSpeed.ToString();
                        clients.Add(clientString);
                    }
                }
            }
            int j = (1 * ConsoleData.ListNum) -1;
            for (int i = 5; i < 10; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║                                                                              ║");
                if (clients.Count > 0)
                {
                    if((j+1) > clients.Count)
                    {
                        continue;
                    }

                    string[] array = clients[j].Split(null);
                    Console.SetCursorPosition(5, i);
                    Console.Write(array[0]);
                    Console.SetCursorPosition(15, i);
                    Console.Write(array[1]);
                    Console.SetCursorPosition(36, i);
                    Console.Write(array[2]);
                    Console.SetCursorPosition(65, i);
                    Console.Write(array[3] + "/" + array[4]);
                }
                j++;
            }
        }
        /// <summary>
        /// Writes the network speed total and overall usage to the console window
        /// </summary>
        public static void WriteNetworkSpeed()
        {
            // Current Looped network speed over 1 second
            long uploadSpeed = 0;
            long downloadSpeed = 0;
            string uploadUnit = "Kbps";
            string downloadUnit = "Kbps";
            foreach(var client in Program.ClientUsage)
            {
                uploadSpeed += client.uploadSpeed;
                downloadSpeed += client.downloadSpeed;
            }
            ConsoleData.UploadTotal += uploadSpeed;
            ConsoleData.DownloadTotal += downloadSpeed;
            Console.SetCursorPosition(0, 10);
            Console.Write("║                                                                              ║");
            Console.SetCursorPosition(45, 10);
            Console.Write("CURRENT TOTAL:");
            Console.SetCursorPosition(65, 10);
            //Convert to mbps
            if(uploadSpeed > 1024)
            {
                uploadSpeed = uploadSpeed / 1024;
                uploadUnit = "Mbps";
            }
            if(downloadSpeed > 1024)
            {
                downloadSpeed = downloadSpeed / 1024;
                downloadUnit = "Mbps";
            }
            Console.Write(downloadSpeed.ToString() + downloadUnit + "/" + uploadSpeed.ToString() + uploadUnit);
            //Total Network Usage Over Course of Program Live Time
            Console.SetCursorPosition(1, 10);
            Console.Write("USAGE TOTAL(Mbit):");
            Console.SetCursorPosition(20, 10);
            Console.Write("D" + (ConsoleData.DownloadTotal / 1024L).ToString() + "/U" + (ConsoleData.UploadTotal / 1024L).ToString());

            Console.SetCursorPosition(0, 11);
            Console.Write("║                                                                              ║");
        }
        /// <summary>
        /// Refreshes all data in the console window
        /// </summary>
        public static void Update()
        {
            if(Console.CursorLeft == 0)
            {
                Console.CursorLeft = 1;
            }
            CurrentConsoleLeft = Console.CursorLeft;
            ClearConsole();
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.SetCursorPosition(0, 0);
            Console.Write("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.SetCursorPosition(0, 1);
            Console.Write("║                                                                              ║");
            Console.SetCursorPosition(0, 2);
            Console.Write("╠══════════════════════════════════════════════════════════════════════════════╣");
            Console.SetCursorPosition(0, 3);
            Console.Write("║                                                                              ║");
            Console.SetCursorPosition(0, 4);
            Console.Write("║    ID        PC NAME              SOCKET                       DOWN/UP       ║");
            WriteClients(ConsoleData.ListNum);
            WriteNetworkSpeed();
            for (int i = 12; i < 20; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║                                                                              ║");
            }

            Console.SetCursorPosition(0, 20);
            Console.Write("║");
            Console.SetCursorPosition(79, 20);
            Console.Write("║"); 
            Console.SetCursorPosition(0, 21);
            Console.Write("╠══════════════════════════════════════════════════════════════════════════════╣");
            Console.SetCursorPosition(0, 22);
            Console.Write("║");
            Console.SetCursorPosition(79, 22);
            Console.Write("║");
            Console.SetCursorPosition(0, 23);
            Console.Write("╚══════════════════════════════════════════════════════════════════════════════╝");
            UpdateText();
            Console.SetCursorPosition(CurrentConsoleLeft, 22);
        }
        /// <summary>
        /// Shows header information in the console window
        /// </summary>
        public static void UpdateText()
        {
            ConsoleData.AppName = "Web Control Panel by";
            ConsoleData.AuthorName = "Nathan Heinrich";
            Console.SetCursorPosition(20, 1);
            Console.Write(ConsoleData.AppName + " " + ConsoleData.AuthorName);
            Console.SetCursorPosition(1, 3);
            Console.Write("Current Connected Client Count: " + ConsoleData.CurrentConnectedClients.ToString());
        }
    }
}
