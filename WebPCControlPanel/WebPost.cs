using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.IO;
namespace WebPCControlPanel
{
    class WebPost
    {

        /// <summary>
        /// Send data to a http post server with data
        /// </summary>
        /// <param name="socket">The socket you want to send it to, defaulted to 127.0.0.1:3000</param>
        /// <param name="json">Json you want to send over the socket, defaulted to a heartbeat of variable true</param>
        /// <param name="autoRequest">If the request comes from code or from user input(true if auto, false if user input)</param>

        public static bool SendRequest(string socket = "http://127.0.0.1:3000/",bool autoRequest = false, string json = "{\"heartbeat\":true}")
        {
            try {
                foreach(var client in Program.Clients)
                {
                    if(client.ipSocket == socket || client.pcId.ToString() == socket || client.pcName == socket)
                    {
                        socket = client.ipSocket ;
                        break;
                    }
                }
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(socket);
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    if (autoRequest == false)
                    {
                        CProcessRequest.ProcessRequest(result);
                    }
                    else
                    {
                        CProcessRequest.ProcessAutoRequest(result);
                        //Do automated stuff like heartbeat every 60 seconds here
                    }
                    Debug.Print(result);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
