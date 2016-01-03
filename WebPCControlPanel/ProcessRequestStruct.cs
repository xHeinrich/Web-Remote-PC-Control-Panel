using System;
namespace WebPCControlPanel
{
    public class HeartBeat
    {
        public int pcId { get; set; }
        public bool heartbeat { get; set; }
        public DateTime lastUpdate { get; set; }
        public string ipSocket { get; set; }
        public string pcName { get; set; }
        public long uploadSpeed { get; set; } // dont write to this, only use it to decrypt the datas
        public long downloadSpeed { get; set; } // dont write to this, only use it to decrypt the datas
    }
    public class NetworkUsage
    {
        public int pcId { get; set; }
        public long uploadSpeed { get; set; }
        public long downloadSpeed { get; set; }
    }
    public class Requests
    {
        public bool restartClient { get; set; }
    }
    public class ProcessRequestStruct
    {
        public HeartBeat HeartBeat { get; set; }
        public Requests Requests { get; set; }
    }
}
