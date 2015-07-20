using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerUpdateService.BusinessLogic;
using ServerUpdateService.JSONDataContracts;

namespace ServerUpdateService.BusinessLogic
{
    public delegate void OnErrorEventHandler(object sender, ErrorEventArgs e);
    public delegate void OnEventHandler(object sender, string message);

    public class ErrorEventArgs : EventArgs
    {
        public Exception Exception;
        public Exception InnerMost;
        
        public ErrorEventArgs(Exception ex, Exception innermost)
        {
            this.Exception = ex;
            this.InnerMost = innermost;
        }

    }
    public class ARKUpdate
    {
        public ARKUpdate()
        {

        }
        public event OnErrorEventHandler ErrorEvent;
        public event OnEventHandler Event;

        public void Start()
        {
            try
            {
                InitARKServer();   
            }
            catch (Exception ex)
            {
                ErrorEvent(this, new ErrorEventArgs(ex, null));
            }
        }

        public void Stop()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ErrorEvent(this, new ErrorEventArgs(ex, null));
            }
        }

        QueryMaster.Server server = null;
        SteamKit2.WebAPI.Interface steam = null;

        System.Timers.Timer timer = null;
        System.Timers.Timer countDownTimer = null;
        System.Threading.Mutex timerMutex = null;

        string password = "";
        string apiARKBARURL = "";
        string serverIP = "";
        ushort RCONPort = 0;

        int countDown = 0;

        /// <summary>
        /// Use for testing.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public void InitARKServer()
        {
            this.password = System.Configuration.ConfigurationManager.AppSettings["AdminPassword"];
            this.apiARKBARURL = System.Configuration.ConfigurationManager.AppSettings["APIARKBARURL"];
            this.serverIP = System.Configuration.ConfigurationManager.AppSettings["ServerIP"];
            this.RCONPort = ushort.Parse(System.Configuration.ConfigurationManager.AppSettings["RCONPort"]);

            timer = new System.Timers.Timer(int.Parse(System.Configuration.ConfigurationManager.AppSettings["VersionCheckInterval"])); //1 hour = 360000
            countDownTimer = new System.Timers.Timer(int.Parse(System.Configuration.ConfigurationManager.AppSettings["CountDownInterval"])); //1 minute = 60000

            server = QueryMaster.ServerQuery.GetServerInstance(QueryMaster.EngineType.Source, serverIP, RCONPort);
            
            timerMutex = new System.Threading.Mutex();

            countDownTimer.Elapsed += countDown_Elapsed;
            timer.Elapsed += timer_Elapsed;

            timer.Start();
            //use api.ark.bar to get the current version and the server version            
        }

        void countDown_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {            
            //SendRCONCommand("broadcast A new major version was released and will be installed in " + countDown + " minutes.");
            countDown -= 1;           
            if (countDown == -1)
            {
                countDownTimer.Stop();
                Event(this, "Stopping server...");
                //SendRCONCommand("quit");

                Event(this, "Waiting for close..");
                //System.Threading.Thread.Sleep(120000); //2 minutes

                //+login anonymous +force_install_dir "{0}"  "+app_update 376030 {1}" +quit

                Event(this, "Updating ARK...");
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = @"c:\ark\updateark.bat"
                    }
                };
                //process.Start();
                //process.WaitForExit();

                Event(this, "Starting ARK...");
                process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = @"c:\ark\startark.bat"
                    }
                };
                //process.Start();
                timer.Start();
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (timerMutex.WaitOne(0))
            {
                Event(this, "Checking version...");
                var manager = new APIARKBARManager();
                var gameinfo = manager.GetServerInfo<GameVersionInfo>(string.Format("{0}/version",apiARKBARURL));
                var serverinfo = manager.GetServerInfo<ServerInfoRoot>(string.Format("{0}/server/{1}/{2}", apiARKBARURL, serverIP, 27015));

                if (Math.Truncate(gameinfo.Version) > Math.Truncate(serverinfo.ServerInfo.Version))
                {
                    Event(this, "New version found preparing to update...");
                    //update the server
                    countDown = 10;
                    countDownTimer.Start();
                    //SendRCONCommand("saveworld");
                    timer.Stop();
                }
                timerMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Use for testing.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string SendRCONCommand(string command)
        {
            Event(this,"Sent RCON command: " + command);
            var rconclient = server.GetControl(password);
            return rconclient.SendCommand(command);
        }

        /// <summary>
        /// Use for testing.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string SendSteamCMD(string command)
        {
            var args = new Dictionary<string, string>();
            args.Add("appid", "346110");
            args.Add("key","");

            var results = steam.Call("GetSchemaForGame", 2, args);
            return "";
        }
    }
}
