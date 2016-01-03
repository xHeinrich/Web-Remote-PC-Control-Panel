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
        /// <summary>
        /// Client class containing all heatbeat data in a list.
        /// </summary>
        public static List<HeartBeat> Clients = new List<HeartBeat>();
        /// <summary>
        /// Client network usage class with all network usage data in it.
        /// </summary>
        public static List<NetworkUsage> ClientUsage = new List<NetworkUsage>();
        /// <summary>
        /// The timer to process all automatic requests.
        /// </summary>
        static Timer ClientHeartbeatTimer;
        /// <summary>
        /// How long between heatbeat requests in seconds.
        /// </summary>
        static int HeartbeatInterval = 2;

        static void Main(string[] args)
        {
            //First update of the console to draw UI
            ConsoleView.Update();
            //Set timer up initially and activate it
            ClientHeartbeatTimer = new Timer(HeartbeatInterval * 1000);
            ClientHeartbeatTimer.Elapsed += new ElapsedEventHandler(ClientHeartbeatTimer_Elapsed);
            ClientHeartbeatTimer.Enabled = true;

            while (true)
            {
                //Wait for user input
                string ConsoleString = Console.ReadLine();
                //Split console commands via a space or " " represented as a null
                String[] ConsoleCommand = ConsoleString.Split(null);
                switch (ConsoleCommand[0])
                {
                    //The add command to add a new client to the Clients list
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
                    //Set the interval between 
                    case "update":
                        HeartbeatInterval = Convert.ToInt32(ConsoleCommand[1]);
                        ClientHeartbeatTimer.Interval = (HeartbeatInterval * 1000);
                        ConsoleView.AddLog("Update rate modifier: " + ConsoleCommand[1]);
                        break;
                    //Restart the remote computer
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
                    //show the next 5 clients in the list on the console ui
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
        /// <summary>
        /// 
        /// </summary>
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
    }
}
