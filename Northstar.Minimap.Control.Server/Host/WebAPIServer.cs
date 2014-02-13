using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Northstar.Minimap.Control.Server.Host
{
    public class WebAPIServer
    {
        private IDisposable instance;

        public void Start()
        {
            if (instance == null)
            {
                instance = WebApp.Start<Startup>(url: "http://*:9000/");
            }
        }

        public void Stop()
        {
            if (instance != null)
            {
                instance.Dispose();
                instance = null;
            }
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public bool isRunning()
        {
            return instance != null;
        }
    }
}