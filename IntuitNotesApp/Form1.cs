using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using IntuitNotesApp.NoteDAl;
using IntuitNotesBL.NoteDAl;
using IntuitNotesBL.NotesModel;

namespace IntuitNotesApp
{
    public partial class IntuitNotes : Form
    {
        public static string selectedNote = "";
        private Dictionary<string, Notes> dicNotes;
        public bool isEdited;
        private static readonly string clientId = DbWrapper.GetClientId();
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

        private void SaveNote()
        {
            Notes newNote;
            if (!dicNotes.TryGetValue(selectedNote, out newNote))
                newNote = new Notes();
            newNote.Title = txtTitle.Text;
            newNote.Body.Append(NotesBody.Text);
            DbWrapper.UpsertNotes(newNote);
            isEdited = false;
            if (dicNotes.ContainsKey(newNote.NoteGuid))
                dicNotes[newNote.NoteGuid] = newNote;
            else
                dicNotes.Add(newNote.NoteGuid, newNote);

            UpdateGridView();
        }

        private void UpdateGridView()
        {
            var NotesList = new BindingList<Notes>(dicNotes.Values.ToList());
            dvNotes.DataSource = NotesList;
            dvNotes.Refresh();
        }

        private void IntuitNotes_Load(object sender, EventArgs e)
        {
          DbWrapper.Connect("notes.db");

               SyncScheduler.StartSyncTimer(dvNotes);
            dicNotes = DbWrapper.GetNotesForDisplay();
            UpdateGridView();
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

        /*  private void dvNotes_RowEnter(object sender,
        DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dvNotes.Rows[e.RowIndex].Cells.Count; i++)
            {
                dvNotes[i, e.RowIndex].Style.BackColor = Color.Yellow;
            }
        }*/

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
            string subject = "Intuit Notes App shares " + txtTitle.Text;
            string body = NotesBody.Text;
            string command = "mailto:?subject="+subject+"&body="+body;
            Process.Start(command); 
        }

        private void Sync_Click(object sender, EventArgs e)
        {
            HttpClientUtil httpClientUtil =new HttpClientUtil();
            dicNotes = new NotesSync(httpClientUtil).Sync(clientId);
        UpdateGridView();  
        }
    }
}