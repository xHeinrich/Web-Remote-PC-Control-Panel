using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;
using System.Windows;

namespace WebPCControlPanel
{
    class Program
    {
        public static List<HeartBeat> Clients = new List<HeartBeat>();
        public static List<NetworkUsage> ClientUsage = new List<NetworkUsage>();
        static Timer ClientHeartbeatTimer;
        static int HeartbeatInterval = 2;

        static void Main(string[] args)
        {
            ConsoleView.Update();
            ClientHeartbeatTimer = new Timer(HeartbeatInterval * 1000);
            ClientHeartbeatTimer.Elapsed += new ElapsedEventHandler(ClientHeartbeatTimer_Elapsed);
            ClientHeartbeatTimer.Enabled = true;

            while (true)
            {
                string ConsoleString = Console.ReadLine();
                String[] ConsoleCommand = ConsoleString.Split(null);
                switch (ConsoleCommand[0])
                {
                    case "add":
                        if(!WebPost.SendRequest(ConsoleCommand[1]))
                        {
                            ConsoleView.AddLog("Failed to add client on socket: " + ConsoleCommand[1]);
                        }
                        else
                        {
                            ConsoleData.CurrentConnectedClients += 1;
                        }
                        break;
                    case "update":
                        HeartbeatInterval = Convert.ToInt32(ConsoleCommand[1]);
                        ClientHeartbeatTimer.Interval = (HeartbeatInterval * 1000);
                        ConsoleView.AddLog("Update rate modifier: " + ConsoleCommand[1]);
                        break;
                    case "restart":
                    case "reboot":
                        if (!WebPost.SendRequest(ConsoleCommand[1], false, "{\"restartPc\" : true}"))
                        {
                            ConsoleView.AddLog("Failed to reboot remote computer");
                        }
                        else
                        {
                            ConsoleView.AddLog("Restarting remote computer " + ConsoleCommand[1]);
                        }
                        break;
                    case "list":
                        ConsoleView.WriteClients(Convert.ToInt32(ConsoleCommand[1]));
                        break;
                    //Basic commands
                    case "help":
                    case "?":
                        Console.WriteLine("Commands Available:");
                        Console.WriteLine("add <socket>, clear");
                        break;
                }
                ConsoleView.ClearInput();
                ConsoleView.Update();

            }
        }
        static void ClientHeartbeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int clientIndex = 0;
            int clientId = 0;
            if (Clients != null)
            {

                try {
                    foreach (var client in Clients)
                    {
                        if (!WebPost.SendRequest(client.ipSocket, true, "{\"heartbeat\":true, \"pcId\":" + client.pcId.ToString() + "}"))
                        {
                            //Remove client from Client
                            clientId = client.pcId;
                            Clients.RemoveAt(clientIndex);

                            //Remove client from ClientUsage
                            clientIndex = 0;
                            foreach (var clientU in ClientUsage)
                            {
                                if (clientU.pcId == clientId)
                                {
                                    ClientUsage.RemoveAt(clientIndex);
                                }
                                clientIndex++;
                            }
                            ConsoleView.AddLog("Heartbeat failed, removing client: " + client.ipSocket);
                            return;
                        }
                        clientIndex++;
                    }
                }catch(Exception)
                {

                }
            }
            ConsoleData.CurrentConnectedClients = clientIndex;
            ConsoleView.Update();
        }
        public static int getHighestClientNum()
        {
            int HeighestNum = 0;
            foreach (var client in Clients )
            {
                if(client.pcId > HeighestNum)
                {
                    HeighestNum = client.pcId;
                }
            }
            return HeighestNum;
        }
    }
}
