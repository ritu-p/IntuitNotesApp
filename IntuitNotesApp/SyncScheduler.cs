using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using IntuitNotesBL.NoteDAl;
using IntuitNotesBL.NotesModel;
using Timer = System.Timers.Timer;

namespace IntuitNotesApp
{
    public static class SyncScheduler
    {
        private static BackgroundWorker worker;
        private static readonly Timer objTimer = new Timer();
        private static DbWrapper dbClient = new DbWrapper("notes.db");
        private static readonly string clientId = dbClient.GetClientId();
        private static long syncInterval = Convert.ToInt64( ConfigurationManager.AppSettings["Client.SyncInterval"]);

        public static void StartSyncTimer() 
        {
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
      //      worker.RunWorkerCompleted += bw_RunWorkerCompleted;
       

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

         Dictionary<string,Notes>  dictNotes= new NotesSync(new HttpClientUtil()).Sync(clientId).Result;

       
        }
     

        
    }
}