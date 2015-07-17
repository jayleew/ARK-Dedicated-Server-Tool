using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace ServerUpdateService
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> Params = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains("=")) //this is a key-value pair argument
                {
                    Params.Add(args[i].Split('=')[0].ToUpper(), args[i].Split('=')[1]);
                }
                else //this is just a flag argument
                {
                    Params.Add(args[i].ToUpper(), "");
                }
            }
            //System.Diagnostics.Debugger.Launch();
            if (Params.ContainsKey("/CONSOLE"))
            {
                var service = new BusinessLogic.ARKUpdate();
                service.ErrorEvent += service_ErrorEvent;
                service.Start();
                Console.ReadKey();
                service.Stop();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new ServerUpdateService.ARKServerUpdateService() 
                };
                ServiceBase.Run(ServicesToRun);
            }
        }

        static void service_ErrorEvent(object sender, BusinessLogic.ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.Message + "\n" + e.Exception.StackTrace);
        }
    }
}
