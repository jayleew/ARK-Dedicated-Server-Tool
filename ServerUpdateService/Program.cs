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
                service.Event += service_Event;               
                service.Start();

                Console.Write(">");

                string command = Console.ReadLine();
                while (command.ToLower() != "exit" && command.ToLower() != "quit")
                {
                    if (command.StartsWith("/".ToLower()))
                    {
                        command.TrimStart("/".ToCharArray());
                        
                    }
                    else
                    {
                        try
                        {
                            switch (command)
                            {
                                case "getversion" :
                                    var manager = new ServerUpdateService.BusinessLogic.APIARKBARManager();
                                    var serverinfo = manager.GetServerInfo<JSONDataContracts.ServerInfoRoot>("http://api.ark.bar/server/173.26.48.221/27015");
                                    var gameinfo = manager.GetServerInfo<JSONDataContracts.GameVersionInfo>("http://api.ark.bar/version");
                                    break;
                                default:
                                    Console.WriteLine(service.SendRCONCommand(command));
                                    break;
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    Console.Write(">");
                    command = Console.ReadLine();
                }
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

        static void service_Event(object sender, string message)
        {
            Console.WriteLine(message);
            Console.Write(">");
        }

        static void service_ErrorEvent(object sender, BusinessLogic.ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.ToString());
        }
    }
}
