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
        private string serverUrl = "http://localhost:9000/";

        public void Start()
        {
            if (instance == null)
            {
                instance = WebApp.Start<Startup>(url: serverUrl);
                Console.WriteLine("Starting web server");
            }
        }

        public void Stop()
        {
            if (instance != null)
            {
                instance.Dispose();
                instance = null;
                Console.WriteLine("Stopping web server");
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