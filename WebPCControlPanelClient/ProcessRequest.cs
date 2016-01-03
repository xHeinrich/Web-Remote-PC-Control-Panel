using Newtonsoft.Json;
using System;
namespace WebPCControlPanelClient
{
    static class CProcessRequest
    {
        public static string ProcessReq(string json)
        {
            if (json.Contains("heartbeat"))
            {
                HeartBeat beat = JsonConvert.DeserializeObject<HeartBeat>(json);
                if(beat.heartbeat == true)
                {
                    beat.heartbeat = true;
                    beat.ipSocket = Program.ipSocket;
                    beat.lastUpdate = DateTime.Now;
                    beat.downloadSpeed = Lib.NetworkInfo.ShowNetworkTraffic(1);
                    beat.uploadSpeed = Lib.NetworkInfo.ShowNetworkTraffic(0);
                }
                Program.clientID = beat.pcId;
                Console.WriteLine(JsonConvert.SerializeObject(beat));
                return JsonConvert.SerializeObject(beat);
             }
            if(json.Contains("restart"))
            {
                Console.WriteLine("Rebooting pc");
            }
            return "";
        }
    }
}
