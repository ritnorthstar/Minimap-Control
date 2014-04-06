﻿using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Hosting
{
    public class WebAPIServer
    {
        private IDisposable instance;

        public void Start(int port = 9000)
        {
            if (instance == null)
            {
                Console.WriteLine("STARTING SERVER");
                instance = WebApp.Start<Startup>(url: "http://*:" + port);
            }
        }

        public void Stop()
        {
            if (instance != null)
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