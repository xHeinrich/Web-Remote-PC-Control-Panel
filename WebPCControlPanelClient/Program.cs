using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Diagnostics; 
using System.Runtime.InteropServices;
namespace WebPCControlPanelClient
{
    class Program
    {

        public static string ipSocket;
        public static int clientID;
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Listener("http://" + Lib.NetworkInfo.LocalIPAddress() + ":3000/");

            }
            else
            {
                foreach(var arg in args)
                {
                    Console.WriteLine(arg);
                }
            }
            string Command = Console.ReadLine();
        }
        public static void Listener(string socket = "http://127.0.0.1:3000/")
        {
            ipSocket = socket;
            var prefix = ipSocket;
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            try
            {
                listener.Start();
                Console.WriteLine("Listening on " + ipSocket);
            }
            catch (HttpListenerException)
            {
                return;
            }
            while (listener.IsListening)
            {
                var context = listener.GetContext();
                ProcessRequest(context);
            }
            listener.Close();
        }
        public static void ProcessRequest(HttpListenerContext context)
        {
            // Get the data from the HTTP stream
            var body = new StreamReader(context.Request.InputStream).ReadToEnd();
            //Data to send back
            byte[] b = Encoding.UTF8.GetBytes(CProcessRequest.ProcessReq(body));
            context.Response.StatusCode = 200;
            context.Response.KeepAlive = false;
            context.Response.ContentLength64 = b.Length;

            var output = context.Response.OutputStream;
            output.Write(b, 0, b.Length);
            context.Response.Close();

            Console.WriteLine(body);
        }

    }
}
