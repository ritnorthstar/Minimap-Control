using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Hosting
{
    public class WebAPIServer
    {
        private IDisposable instance;

        public int Port
        {
            get
            {
                return Port;
            }
            set
            {
                port = value;
                if (IsRunning())
                {
                    Restart();
                }
            }
        }
        protected int port = 9000;

        public void Start()
        {
            if (!IsRunning())
            {
                Console.WriteLine("STARTING SERVER");
                instance = WebApp.Start<Startup>(url: "http://*:" + Port);
            }
        }

        public void Stop()
        {
            if (IsRunning())
            {
                Console.WriteLine("STOPPING SERVER");
                instance.Dispose();
                instance = null;
            }
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public bool IsRunning()
        {
            return instance != null;
        }
    }
}