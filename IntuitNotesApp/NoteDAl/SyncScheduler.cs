using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace IntuitNotesApp.NoteDAl
{

    public static class SyncScheduler
    {
    private static BackgroundWorker worker;
    private static System.Timers.Timer objTimer = new System.Timers.Timer();
        public static void StartSyncTimer()
        {
    worker = new BackgroundWorker();
        worker.DoWork += worker_DoWork;

    
            int iTimerInterval = Convert.ToInt32(10000);
            objTimer.Interval = iTimerInterval;
            objTimer.Elapsed += objTimer_Elapsed;
            objTimer.Start();

        }

      private static  void objTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        private static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //whatever You want the background thread to do...
        }
   
    }
}
