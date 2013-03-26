using ServiceStack.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using tracksStackd;

namespace tracksService
{
    public partial class TracksService : ServiceBase
    {
        private const string _eventLogSource = "TracksRest";
        public AppHost Host { get; set; }
        public TracksService()
        {
            InitializeComponent();

            if (!System.Diagnostics.EventLog.SourceExists(_eventLogSource))
            {
                System.Diagnostics.EventLog.CreateEventSource(_eventLogSource, "TracksRestAPI");
            }
            eventLog.Source = _eventLogSource;
            eventLog.Log = "Application";
        }

        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("Tracks Rest API host starting");

            var settings = new AppSettings();

            Host = new AppHost();
            Host.Init();
            Host.InitDb();
            Host.Start(settings.GetString("restBaseAddress"));
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("Tracks Rest API host stopping");
            Host.Stop();
        }

        protected override void OnShutdown()
        {
            Host.Stop();
            base.OnShutdown();
        }
    }
}
