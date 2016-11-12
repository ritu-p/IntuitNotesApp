using System;
using System.Collections.Generic;

namespace IntuitNotesBL.NotesModel
{
    public class NoteStore
    {
        private  string clientID;

        public List<Notes> LstNotes { get; set; }

        public DateTime LastUpDateTime { get; set; }

        public  string ClientId
        {
            get { return clientID; }
            set { clientID = value; }
        }
    }
}