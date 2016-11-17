using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

using IntuitNotesBL.NoteDAl;
using IntuitNotesBL.NotesModel;
using Timer = System.Timers.Timer;

namespace IntuitNotesApp
{
    public partial class IntuitNotes : Form
    {

        public static string selectedNote = "";
        private Dictionary<string, Notes> dicNotes;
        public bool isEdited;
        delegate void SetGridCallback(Dictionary<string, Notes> dicNotes);
        private static BackgroundWorker worker;
        private static readonly Timer objTimer = new Timer();
        private static DbWrapper dbClient = new DbWrapper("notes.db");
        private static readonly string clientId = dbClient.GetClientId();
        private static long syncInterval = Convert.ToInt64(ConfigurationManager.AppSettings["Client.SyncInterval"]);


        #region Form Events
        public IntuitNotes()
        {
            InitializeComponent();
            dvNotes.AutoGenerateColumns = false;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            txtTitle.Clear();
            NotesBody.Clear();
            selectedNote = "";
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveNote();
        }

        private void IntuitNotes_Load(object sender, EventArgs e)
        {
            //  clientId= dbClient.GetClientId();

            StartSyncTimer();
            dicNotes = dbClient.GetNotesForDisplay();
            UpdateGridView(dicNotes);
        }

        //Click cell to save notes
        private void dvNotes_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dvNotes.SelectedRows)
            {
                selectedNote = row.Cells[1].Value.ToString();
                Notes displayNote;
                dicNotes.TryGetValue(selectedNote, out displayNote);
                txtTitle.Text = displayNote.Title;
                NotesBody.Text = displayNote.Body.ToString();
                //...
            }
        }


        private void dvNotes_RowLeave(object sender,
            DataGridViewCellEventArgs e)
        {
            if (isEdited)
            {
                selectedNote = dvNotes.Rows[e.RowIndex].Cells[1].Value.ToString();
                SaveNote();
            }
        }


        private void NotesBody_KeyPress(object sender, KeyPressEventArgs e)
        {
            isEdited = true;
        }

        private void txtTitle_KeyPress(object sender, KeyPressEventArgs e)
        {
            isEdited = true;
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            var subject = "Intuit Notes App shares " + txtTitle.Text;
            var body = NotesBody.Text;
            var command = "mailto:?subject=" + subject + "&body=" + body;
            Process.Start(command);
        }

        private void Sync_Click(object sender, EventArgs e)
        {
            var httpClientUtil = new HttpClientUtil();
            dicNotes = new NotesSync(httpClientUtil).Sync(clientId).Result;
            UpdateGridView(dicNotes);
        }
        #endregion


        #region Sync Scheduler
        public void StartSyncTimer()
        {
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += bw_RunWorkerCompleted;


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

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {


            dicNotes = new NotesSync(new HttpClientUtil()).Sync(clientId).Result;


        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            UpdateGridView(dicNotes);
        }
        #endregion


        #region Private Methods
        private void SaveNote()
        {
            Notes newNote;
            if (!dicNotes.TryGetValue(selectedNote, out newNote))
                newNote = new Notes();
            newNote.Title = txtTitle.Text;
            newNote.Body.Append(NotesBody.Text);
            dbClient.UpsertNotes(newNote);
            isEdited = false;
            if (dicNotes.ContainsKey(newNote.NoteGuid))
                dicNotes[newNote.NoteGuid] = newNote;
            else
                dicNotes.Add(newNote.NoteGuid, newNote);

            UpdateGridView(dicNotes);
        }

        private void UpdateGridView(Dictionary<string, Notes> dicNotes)
        {
            if (this.dvNotes.InvokeRequired)
            {
                SetGridCallback callback = new SetGridCallback(UpdateGridView);
                this.Invoke(callback, new object[] { dicNotes });
            }
            else
            {
                var NotesList = new BindingList<Notes>(dicNotes.Values.ToList());
                dvNotes.DataSource = NotesList;
                dvNotes.Refresh();
            }
        } 
        #endregion
    }
}