using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ServerUpdateService
{
    public partial class ARKServerUpdateService : ServiceBase
    {
        ServerUpdateService.BusinessLogic.ARKUpdate updateService = null;

        public ARKServerUpdateService()
        {
            InitializeComponent();
            updateService = new BusinessLogic.ARKUpdate();
            updateService.ErrorEvent += updateService_ErrorEvent;
        }

        void updateService_ErrorEvent(object sender, BusinessLogic.ErrorEventArgs e)
        {
            EventLog.WriteEntry(e.Exception.Message, EventLogEntryType.Error);
            throw e.Exception; //bubble the error for windows service manager
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            updateService.Start();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            updateService.Stop();
        }
    }
}
