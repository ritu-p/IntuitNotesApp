using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntuitNotesApp.NotesModel
{
    public class Notes
    {
        private string title;
        private string noteGUID;
        private bool isCloudSynced = false;
        private DateTime modifiedDate;
      
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

        public string NoteGuid
        {
            get
            {
                if (String.IsNullOrEmpty(noteGUID))
                {
                    noteGUID = Guid.NewGuid().ToString();
                }
                    return noteGUID;
            }
             set { noteGUID = value; }
        }

        public bool IsCloudSynced
        {
            get { return isCloudSynced; }
            set { isCloudSynced = value; }
        }

        public DateTime ModifiedDate
        {
            get { return modifiedDate; }
            set { modifiedDate = value; }
        }

        private StringBuilder body=new StringBuilder("");

     /*   public void Save()
        {
           JsonSerializer serializer= new JsonSerializer();
            using (StreamWriter sw = new StreamWriter("../AppData/"+this.noteGUID+".json"))
 using (JsonWriter writer = new JsonTextWriter(sw))
{
    serializer.Serialize(writer, this);

}
        }*/

      /*  public  List<Notes> GetFromReader(SQLiteDataReader dataReader)
        {
            while (dataReader.Read())
            {
                this.noteGUID = dataReader["notes_id"] is DBNull ? null : dataReader["notes_id"].ToString(),
                Title = dataReader["title"] is DBNull ? null : r["title"].ToString(),
                Body = dataReader["body"] is DBNull ? null : r["body"].ToString(),
                IsCloudSynced = dataReader["is_server_synced"] is DBNull ? false : bool.Parse( r["is_server_synced"].ToString()),
                modifiedDate = dataReader["modified_dt"] is DBNull ? DateTime.Now : DateTime.Parse(r["title"].ToString()) 
            }
            List<Notes> notes = dataReader..Select(r => new Notes
            {
                noteGUID = dataReader["notes_id"] is DBNull ? null : dataReader["notes_id"].ToString(),
                Title = dataReader["title"] is DBNull ? null : r["title"].ToString(),
                Body = dataReader["body"] is DBNull ? null : r["body"].ToString(),
                IsCloudSynced = dataReader["is_server_synced"] is DBNull ? false : bool.Parse( r["is_server_synced"].ToString()),
                modifiedDate = dataReader["modified_dt"] is DBNull ? DateTime.Now : DateTime.Parse(r["title"].ToString()) 
            }).ToList();
            return notes;
        }*/
    }
}
