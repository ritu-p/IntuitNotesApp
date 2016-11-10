using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IntuitNotesApp.NotesModel;

namespace IntuitNotesApp
{
    public partial class IntuitNotes : Form
    {
        private List<Notes> lstNotes;
        BindingSource srcNotes= new BindingSource();
        public IntuitNotes()
        {
            InitializeComponent();
            dvNotes.AutoGenerateColumns = false;
        }

        private void Add_Click(object sender, EventArgs e)
        {
           txtTitle.Clear();
            NotesBody.Clear();

        }

        private void Save_Click(object sender, EventArgs e)
        {
           Notes newNote= new Notes();
            newNote.Title = txtTitle.Text;
            newNote.Body.Append(NotesBody.Text);
            lstNotes.Add(newNote);
            var NotesList = new BindingList<Notes>(lstNotes);
            dvNotes.DataSource = NotesList;
            dvNotes.Refresh();
        }

        private void IntuitNotes_Load(object sender, EventArgs e)
        {
            lstNotes= new List<Notes>();

        }
        
    

      
    }
}
