using System;
using System.Net;
namespace WebPCControlPanelClient
{
    public class HeartBeat
    {
        public int pcId { get; set; }
        public bool heartbeat { get; set; }
        public DateTime lastUpdate { get; set; }
        public string ipSocket { get; set; }
        public string pcName = Environment.MachineName;
        public long uploadSpeed;
        public long downloadSpeed;
    }
    public class Requests
    {
        public bool restartClient { get; set; }
        public string addClient { get; set; }
    }
    public class ProcessRequestStruct
    {
        public HeartBeat HeartBeat { get; set; }
        public Requests Requests { get; set; }
    }
}
