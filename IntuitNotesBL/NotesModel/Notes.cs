using System;
using System.Text;

namespace IntuitNotesBL.NotesModel
{
    public class Notes
    {
        private StringBuilder body = new StringBuilder("");
        private string noteGUID;

        public Notes()
        {
            IsDeleted = false;
        }

        public StringBuilder Body
        {
            get { return body; }
            set { body = value; }
        }

        public string Title { get; set; }

        public string NoteGuid
        {
            get
            {
                if (string.IsNullOrEmpty(noteGUID))
                    noteGUID = Guid.NewGuid().ToString();
                return noteGUID;
            }
            set { noteGUID = value; }
        }

        public bool IsDeleted { get; set; }

        public DateTime ModifiedDate { get; set; }

    }
}