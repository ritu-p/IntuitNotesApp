using System;
using System.Text;

namespace IntuitNotesApp.NotesModel
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

        /*  public  List<Notes> GetFromReader(SQLiteDataReader dataReader)
        {
            while (dataReader.Read())
            {
                this.noteGUID = dataReader["notes_id"] is DBNull ? null : dataReader["notes_id"].ToString(),
                Title = dataReader["title"] is DBNull ? null : r["title"].ToString(),
                Body = dataReader["body"] is DBNull ? null : r["body"].ToString(),
                IsDeleted = dataReader["is_server_synced"] is DBNull ? false : bool.Parse( r["is_server_synced"].ToString()),
                modifiedDate = dataReader["modified_dt"] is DBNull ? DateTime.Now : DateTime.Parse(r["title"].ToString()) 
            }
            List<Notes> notes = dataReader..Select(r => new Notes
            {
                noteGUID = dataReader["notes_id"] is DBNull ? null : dataReader["notes_id"].ToString(),
                Title = dataReader["title"] is DBNull ? null : r["title"].ToString(),
                Body = dataReader["body"] is DBNull ? null : r["body"].ToString(),
                IsDeleted = dataReader["is_server_synced"] is DBNull ? false : bool.Parse( r["is_server_synced"].ToString()),
                modifiedDate = dataReader["modified_dt"] is DBNull ? DateTime.Now : DateTime.Parse(r["title"].ToString()) 
            }).ToList();
            return notes;
        }*/

        /*   public void Save()
        {
           JsonSerializer serializer= new JsonSerializer();
            using (StreamWriter sw = new StreamWriter("../AppData/"+this.noteGUID+".json"))
 using (JsonWriter writer = new JsonTextWriter(sw))
{
    serializer.Serialize(writer, this);

}
        }*/
    }
}