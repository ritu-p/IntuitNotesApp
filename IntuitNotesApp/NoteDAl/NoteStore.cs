using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuitNotesApp.NotesModel
{
  public static   class NoteStore
    {
        private static List<Notes> lstNotes;
        private static DateTime lastUpDateTime;

        public static  List<Notes> LstNotes
        {
            get { return lstNotes; }
            set { lstNotes = value; }
        }

        public static DateTime LastUpDateTime
        {
            get { return lastUpDateTime; }
            set { lastUpDateTime = value; }
        }
    }
}
