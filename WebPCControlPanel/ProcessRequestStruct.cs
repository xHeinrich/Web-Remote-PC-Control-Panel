using System;
using System.Collections.Generic;
namespace WebPCControlPanel
{
    /// <summary>
    /// The class the external client system echos to the server.
    /// </summary>
    public class HeartBeat
    {
        /// <summary>
        /// Id of the computer being referenced.
        /// </summary>
        public int pcId { get; set; }
        /// <summary>
        /// if the external system is active or not.
        /// </summary>
        public bool heartbeat { get; set; }
        /// <summary>
        /// The time of the last update the server recieved from the client.
        /// </summary>
        public DateTime lastUpdate { get; set; }
        /// <summary>
        /// The socket to write to for this client.
        /// </summary>
        public string ipSocket { get; set; }
        /// <summary>
        /// The netBIOS name for the external client.
        /// </summary>
        public string pcName { get; set; }
        /// <summary>
        /// Upload speed for the external client, do not use this to find the upload speed but use the NetworkUsage class
        /// </summary>
        public long uploadSpeed { get; set; }
        /// <summary>
        /// Download speed for the external client, do not use this to find the download speed but use the NetworkUsage class
        /// </summary>
        public long downloadSpeed { get; set; }
        /// <summary>
        /// Check if the client exists in the ClientUsage list.
        /// </summary>
        /// <param name="ClientID">The client ID you are checking if it exists or not.</param>
        /// <param name="Client">A list of NetworkUsage.</param>
        /// <returns>Returns true of the client exists and false if it does not.</returns>
        public static bool doesClientExist(int ClientID, List<HeartBeat> Client)
        {
            foreach (var client in Client)
            {
                if (client.pcId == ClientID)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Get the highest client ID in the ClientUsage list 
        /// </summary>
        /// <param name="Client">A list of NetworkUsage</param>
        /// <returns>Returns an int of the highest client ID</returns>
        public static int heighestClientID(List<HeartBeat> Client)
        {
            int HighestID = 1;
            foreach (var client in Client)
            {
                if (client.pcId > HighestID)
                {
                    HighestID = client.pcId;
                }
            }
            return HighestID;
        }
    }
    /// <summary>
    /// A class to determine the upload and download speed of a client system.
    /// </summary>
    public class NetworkUsage
    {
        /// <summary>
        /// ID of the computer being referenced, links to the Client or HeartBeat class.
        /// </summary>
        public int pcId { get; set; }
        /// <summary>
        /// Upload speed of the client.
        /// </summary>
        public long uploadSpeed { get; set; }
        /// <summary>
        /// Download speed of the client.
        /// </summary>
        public long downloadSpeed { get; set; }
        /// <summary>
        /// Check if the client exists in the ClientUsage list.
        /// </summary>
        /// <param name="ClientID">The client ID you are checking if it exists or not.</param>
        /// <param name="Client">A list of NetworkUsage.</param>
        /// <returns>Returns true of the client exists and false if it does not.</returns>
        public static bool doesClientExist(int ClientID, List<NetworkUsage> Client)
        {
            foreach (var client in Client)
            {
                if (client.pcId == ClientID)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Get the highest client ID in the ClientUsage list 
        /// </summary>
        /// <param name="Client">A list of NetworkUsage</param>
        /// <returns>Returns an int of the highest client ID</returns>
        public static int heighestClientID(List<NetworkUsage> Client)
        {
            int HighestID = 1;
            foreach (var client in Client)
            {
                if (client.pcId > HighestID)
                {
                    HighestID = client.pcId;
                }
            }
            return HighestID;
        }
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
