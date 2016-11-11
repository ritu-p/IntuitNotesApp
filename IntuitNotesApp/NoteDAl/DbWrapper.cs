using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntuitNotesApp.NotesModel;

namespace IntuitNotesApp.NoteDAl
{
    public static class DbWrapper
    {
        static SQLiteConnection sqlite_conn;
        static SQLiteDataReader sqlite_datareader;

        public static void Connect()
        {
            sqlite_conn = new SQLiteConnection("Data Source=notes.db;Version=3;New=True;Compress=True;");

            // open the connection:
            sqlite_conn.Open();

            using (
                SQLiteCommand mCmd =
                    new SQLiteCommand(
                        "CREATE TABLE IF NOT EXISTS [Notes] ('notes_id' TEXT ,'title' TEXT, 'body' TEXT,'is_server_synced' BOOL,'modified_dt' DATETIME DEFAULT current_timestamp);",
                        sqlite_conn))
            {
                mCmd.ExecuteNonQuery();
            }

        }

        public static void UpsertNotes(Notes note)
        {
            try
            {


                SQLiteCommand com = new SQLiteCommand(sqlite_conn);
                com.CommandText = "Update [Notes] set 'title'='" + note.Title + "',body='" + note.Body +
                                  "','modified_dt'= DATETIME('NOW') where notes_id='" +
                                  note.NoteGuid + "'"; // Add the first entry into our database 
                int updated = com.ExecuteNonQuery(); // Execute the query
                if (updated != 1)
                {
                    com.CommandText =
                        "INSERT INTO [Notes] ('notes_id'  ,'title', 'body' ,'is_server_synced' ,'modified_dt') Values (@notes_id,@title,@body,@is_server_synced,@modified_date)";
                    // Add another entry into our database 
                    com.Parameters.AddWithValue("@notes_id", note.NoteGuid);
                    com.Parameters.AddWithValue("@title", note.Title);
                    com.Parameters.AddWithValue("@body", note.Body);
                    com.Parameters.AddWithValue("@is_server_synced", note.IsCloudSynced);
                    com.Parameters.AddWithValue("@modified_date", DateTime.Now.ToUniversalTime());
                    com.ExecuteNonQuery(); // Execute the query
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<string,Notes> GetNotes()
        {
            Dictionary<string, Notes> dicNotes = new Dictionary<string, Notes>();
            using (SQLiteCommand fmd = sqlite_conn.CreateCommand())
            {
                fmd.CommandText =
                    @"SELECT  notes_id  ,title, body ,is_server_synced ,modified_dt FROM [Notes]";
                fmd.CommandType = CommandType.Text;
                SQLiteDataReader dataReader = fmd.ExecuteReader();
                       while (dataReader.Read())
            {
                           Notes note = new Notes();
                           note.NoteGuid = dataReader["notes_id"] is DBNull ? String.Empty : dataReader["notes_id"].ToString();
                note.Title = dataReader["title"] is DBNull ? null : dataReader["title"].ToString();
                note.Body = new StringBuilder(dataReader["body"] is DBNull ? null : dataReader["body"].ToString());
                note.IsCloudSynced = dataReader["is_server_synced"] is DBNull
                    ? false
                    : bool.Parse(dataReader["is_server_synced"].ToString());
                note.ModifiedDate = dataReader["modified_dt"] is DBNull
                    ? DateTime.Now
                    : DateTime.Parse(dataReader["modified_dt"].ToString());
                           dicNotes.Add(note.NoteGuid,note);
            }
            }
            return dicNotes;
        }
    }
}
