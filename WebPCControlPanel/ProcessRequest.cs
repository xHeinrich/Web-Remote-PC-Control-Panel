using Newtonsoft.Json;
using System;

namespace WebPCControlPanel
{
    class CProcessRequest
    {

        /// <summary>
        /// Process request from main threads timer with no user input.
        /// </summary>
        /// <param name="json">Json recieved form the external client shown here as a string.</param>
        public static void ProcessAutoRequest(string json)
        {
            try
            {
                //try to deserialize json string into the HeartBeat class
                HeartBeat beat = JsonConvert.DeserializeObject<HeartBeat>(json);
                //check if the client exists in the NetworkUsage class, if it does not add it to the NetworkUsage class
                if(!NetworkUsage.doesClientExist(beat.pcId, Program.ClientUsage))
                {
                    Program.ClientUsage.Add(new NetworkUsage
                    {
                        pcId = beat.pcId,
                        uploadSpeed = beat.uploadSpeed,
                        downloadSpeed = beat.downloadSpeed
                    });
                }
                //if the client exists update its current network usage in the NetworkUsage class
                else
                {
                    foreach(var client in Program.ClientUsage)
                    {
                        if(client.pcId == beat.pcId)
                        {
                            client.downloadSpeed = beat.downloadSpeed;
                            client.uploadSpeed = beat.uploadSpeed;
                        }
                    }
                }

            }
            catch (Exception)
            {
                ConsoleView.AddLog("Could not deserialize object ProcessAutoRequest()");
            }
        }
        /// <summary>
        /// Process user driven requests
        /// </summary>
        /// <param name="json">Json recieved form the external client shown here as a string.</param>
        public static void ProcessRequest(string json)
        {
            //Check if the json is in the HeartBeat class
            if (json.Contains("heartbeat"))
            {
                //Deserialize the json to a HeatBeat class
                HeartBeat beat = JsonConvert.DeserializeObject<HeartBeat>(json);
                int HighestID = 0;
                foreach (var beats in Program.Clients)
                {
                    //Check if the client is already in the HeatBeat list, if it is not add it to the list
                    if (beats.ipSocket == beat.ipSocket)
                    {
                        ConsoleView.AddLog("Client already in database");
                        return;
                    }
                    if (beats.pcId > HighestID)
                    {
                        HighestID = beats.pcId;
                    }
                }
                Program.Clients.Add(
                    new HeartBeat
                    {
                        pcId = HighestID + 1,
                        heartbeat = beat.heartbeat,
                        ipSocket = beat.ipSocket,
                        lastUpdate = beat.lastUpdate,
                        pcName = beat.pcName
                    }
                );
                ConsoleView.AddLog("Added client on " + beat.ipSocket + " with ID:" + (HighestID + 1).ToString());
            }
            //If the json is a restart command tell the server application that the client is restarting
            if (json.Contains("restart"))
            {
                ConsoleView.AddLog("Restarting remote computer");
            }
            ConsoleView.Update();

        }

    }
}
