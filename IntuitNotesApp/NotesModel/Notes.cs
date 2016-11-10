using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuitNotesApp.NotesModel
{
    class Notes
    {
        public string title;

        public StringBuilder Body
        {
            get { return body; }
            set { body = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private StringBuilder body=new StringBuilder("");

    }
}
