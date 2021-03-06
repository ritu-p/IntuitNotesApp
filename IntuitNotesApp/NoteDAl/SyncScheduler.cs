﻿using System;
using System.ComponentModel;
using System.Timers;

namespace IntuitNotesApp.NoteDAl
{
    public static class SyncScheduler
    {
        private static BackgroundWorker worker;
        private static readonly Timer objTimer = new Timer();

        public static void StartSyncTimer()
        {
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;


            var iTimerInterval = Convert.ToInt32(10000);
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
            //whatever You want the background thread to do...
        }
    }
}