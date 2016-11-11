using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IntuitNotesApp.NoteDAl;
using IntuitNotesApp.NotesModel;

namespace IntuitNotesApp
{
    public partial class IntuitNotes : Form
    {
        private Dictionary<string, Notes> dicNotes;
        BindingSource srcNotes= new BindingSource();
        public static string selectedNote = "";
        public bool isEdited = false;
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
            {
                newNote = new Notes();
            }
            newNote.Title = txtTitle.Text;
            newNote.Body.Append(NotesBody.Text);
            DbWrapper.UpsertNotes(newNote);
            isEdited = false;
            if (dicNotes.ContainsKey(newNote.NoteGuid))
            {
                dicNotes[newNote.NoteGuid] = newNote;
            }
            else
            {
                dicNotes.Add(newNote.NoteGuid, newNote);
            }

            var NotesList = new BindingList<Notes>(dicNotes.Values.ToList());
            dvNotes.DataSource = NotesList;
            dvNotes.Refresh();
        }

        private void IntuitNotes_Load(object sender, EventArgs e)
        {
            DbWrapper.Connect();

            dicNotes = DbWrapper.GetNotes();
            var NotesList = new BindingList<Notes>(dicNotes.Values.ToList());
            dvNotes.DataSource = NotesList;
            dvNotes.Refresh();
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

        
      
    }
}
