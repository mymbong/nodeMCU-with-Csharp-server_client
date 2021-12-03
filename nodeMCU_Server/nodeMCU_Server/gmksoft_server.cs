using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nodeCU_Server
{
    class gmksoft_server
    {
        public event MessageEventHandler Message;

        public delegate void MessageEventHandler(gmksoft_server sender, string Data);

        //Server Control
        public IPAddress ServerIP = IPAddress.Parse("192.168.1.9");
        public int ServerPort = 1000;
        public TcpListener myserver;

        public Thread Comthread;
        public bool IsLiserning = true;

        //Clients
        private TcpClient client;
        private StreamReader clientdata;

        public gmksoft_server()
        {
            myserver = new TcpListener(ServerIP, ServerPort);
            myserver.Start();

            Comthread = new Thread(new ThreadStart(Hearing));
            Comthread.Start();

        }

        private void Hearing()
        {
            while (!IsLiserning == false)
            {
                if (myserver.Pending() == true)
                {
                    client = myserver.AcceptTcpClient();
                    clientdata = new StreamReader(client.GetStream());
                }

                try
                {
                    Message?.Invoke(this, clientdata.ReadLine());
                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(10);

            }
        }
    }
}
