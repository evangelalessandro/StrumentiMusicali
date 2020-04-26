using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Service
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        static void Main()
        {
            //#if (!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SchedulerService()
            };
            ServiceBase.Run(ServicesToRun);
            //#else
            //System.Threading.Thread.Sleep(500);
            //var service = new SchedulerService();
            //service.InitScheduler();
            // Put a breakpoint on the following line to always catch
            // your service when it has finished its work
            //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            //#endif
        }
    }
}
