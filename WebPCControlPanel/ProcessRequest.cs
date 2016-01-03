using Newtonsoft.Json;
using System;

namespace WebPCControlPanel
{
    class CProcessRequest
    {
        public static bool doesClientExist(int clientID)
        {
            foreach(var client in Program.ClientUsage)
            {
                if(client.pcId == clientID)
                {
                    return true;
                }
            }
            return false;
        }
        public static int heighestClientID()
        {
            int HighestID = 1;
            foreach (var client in Program.ClientUsage)
            {
                if(client.pcId > HighestID)
                {
                    HighestID = client.pcId;
                }
            }
            return HighestID;
        }
        public static void ProcessAutoRequest(string json)
        {
            try
            {
                HeartBeat beat = JsonConvert.DeserializeObject<HeartBeat>(json);
                if(!doesClientExist(beat.pcId))
                {
                    Program.ClientUsage.Add(new NetworkUsage
                    {
                        pcId = beat.pcId,
                        uploadSpeed = beat.uploadSpeed,
                        downloadSpeed = beat.downloadSpeed
                    });
                }
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
        public static void ProcessRequest(string json)
        {
            if (json.Contains("heartbeat"))
            {
                HeartBeat beat = JsonConvert.DeserializeObject<HeartBeat>(json);
                int HighestID = 0;
                foreach (var beats in Program.Clients)
                {
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
            if (json.Contains("restart"))
            {
                ConsoleView.AddLog("Restarting remote computer");
            }
            ConsoleView.Update();

        }

    }
}
