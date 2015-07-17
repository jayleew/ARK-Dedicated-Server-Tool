using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerUpdateService.BusinessLogic
{
    public delegate void OnErrorEventHandler(object sender, ErrorEventArgs e);
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
        public event OnErrorEventHandler ErrorEvent;
        public void Start()
        {
            try
            {

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
    }
}
