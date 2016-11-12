using System;
using System.ComponentModel;
using System.Configuration;
using System.Timers;
using IntuitNotesBL.NoteDAl;

namespace IntuitNotesBL.NoteDAl
{
    public static class SyncScheduler
    {
        private static BackgroundWorker worker;
        private static readonly Timer objTimer = new Timer();
        private static readonly string clientId = DbWrapper.GetClientId();
        private static long syncInterval = Convert.ToInt64( ConfigurationManager.AppSettings["Client.SyncInterval"]);
        
        public static void StartSyncTimer()
        {
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;


            var iTimerInterval = Convert.ToInt32(syncInterval); //TODO:configurable
            objTimer.Interval = iTimerInterval;
            objTimer.Elapsed += objTimer_Elapsed;
            objTimer.Start();
        }

        private static void objTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!worker.IsBusy)
                
                worker.RunWorkerAsync();
        }

        private static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            NotesSync.Sync(clientId);
           
        }
    }
}