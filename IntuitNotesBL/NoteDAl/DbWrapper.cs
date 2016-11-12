using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using IntuitNotesApp.NotesModel;

namespace IntuitNotesApp.NoteDAl
{
    public static class DbWrapper
    {
        private static SQLiteConnection sqlite_conn;
        private static SQLiteDataReader sqlite_datareader;

        public static void Connect()
        {
            sqlite_conn = new SQLiteConnection("Data Source=notes.db;Version=3;New=True;Compress=True;");

            // open the connection:
            sqlite_conn.Open();

            CreateNoteSchema();
            CreateSyncTimeStampStore();
        }

        private static void CreateNoteSchema()
        {
            using (
                var mCmd =
                    new SQLiteCommand(
                        "CREATE TABLE IF NOT EXISTS [Notes] ('notes_id' TEXT ,'title' TEXT, 'body' TEXT,'is_deleted' INTEGER DEFAULT 0,'modified_dt' DATETIME DEFAULT current_timestamp);",
                        sqlite_conn))
            {
                mCmd.ExecuteNonQuery();
            }
        }

        private static void CreateSyncTimeStampStore()
        {
            using (
                var mCmd =
                    new SQLiteCommand(
                        "CREATE TABLE IF NOT EXISTS [SyncTimeStamp] (client_id TEXT,last_synctimestamp DATETIME );",
                        sqlite_conn))
            {
                mCmd.ExecuteNonQuery();
            }
        }

        public static void UpsertNotes(Notes note)
        {
            try
            {
                var com = new SQLiteCommand(sqlite_conn);
                com.CommandText = "Update [Notes] set 'title'='" + note.Title + "',body='" + note.Body +
                                  "','is_deleted'=" + Convert.ToInt64(note.IsDeleted) +
                                  ", 'modified_dt'= DATETIME('NOW') where notes_id='" +
                                  note.NoteGuid + "'"; // Add the first entry into our database 
                var updated = com.ExecuteNonQuery(); // Execute the query
                if (updated != 1)
                {
                    com.CommandText =
                        "INSERT INTO [Notes] ('notes_id'  ,'title', 'body' ) Values (@notes_id,@title,@body)";
                    // Add another entry into our database 
                    com.Parameters.AddWithValue("@notes_id", note.NoteGuid);
                    com.Parameters.AddWithValue("@title", note.Title);
                    com.Parameters.AddWithValue("@body", note.Body);

                    //     com.Parameters.AddWithValue("@modified_date", DateTime.Now.ToUniversalTime());
                    com.ExecuteNonQuery(); // Execute the query
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<string, Notes> GetNotesForDisplay()
        {
            var dicNotes = new Dictionary<string, Notes>();
            using (var fmd = sqlite_conn.CreateCommand())
            {
                fmd.CommandText =
                    @"SELECT  notes_id  ,title, body ,modified_dt FROM [Notes] where is_deleted=0";
                fmd.CommandType = CommandType.Text;
                var dataReader = fmd.ExecuteReader();
                while (dataReader.Read())
                {
                    var note = new Notes();
                    note.NoteGuid = dataReader["notes_id"] is DBNull ? string.Empty : dataReader["notes_id"].ToString();
                    note.Title = dataReader["title"] is DBNull ? null : dataReader["title"].ToString();
                    note.Body = new StringBuilder(dataReader["body"] is DBNull ? null : dataReader["body"].ToString());

                    note.ModifiedDate = dataReader["modified_dt"] is DBNull
                        ? DateTime.Now
                        : DateTime.Parse(dataReader["modified_dt"].ToString());
                    dicNotes.Add(note.NoteGuid, note);
                }
            }
            return dicNotes;
        }

        public static List<Notes> GetNotesForSync()
        {
            var lstNotes = new List<Notes>();
            using (var fmd = sqlite_conn.CreateCommand())
            {
                fmd.CommandText =
                    @"SELECT  notes_id  ,title, body ,modified_dt FROM [Notes]";
                fmd.CommandType = CommandType.Text;
                var dataReader = fmd.ExecuteReader();
                while (dataReader.Read())
                {
                    var note = new Notes();
                    note.NoteGuid = dataReader["notes_id"] is DBNull ? string.Empty : dataReader["notes_id"].ToString();
                    note.Title = dataReader["title"] is DBNull ? null : dataReader["title"].ToString();
                    note.Body = new StringBuilder(dataReader["body"] is DBNull ? null : dataReader["body"].ToString());

                    note.ModifiedDate = dataReader["modified_dt"] is DBNull
                        ? DateTime.Now
                        : DateTime.Parse(dataReader["modified_dt"].ToString());
                    lstNotes.Add(note);
                }
            }
            return lstNotes;
        }

        public static DateTime GetLastSyncTimestamp(string clientId)
        {
            using (var fmd = sqlite_conn.CreateCommand())
            {
                fmd.CommandText =
                    @"SELECT  last_synctimestamp from  [SyncTimeStamp] where client_id='" + clientId + "'";
                fmd.CommandType = CommandType.Text;
                var dataReader = fmd.ExecuteReader();
                while (dataReader.Read())
                    return dataReader.GetDateTime(0);
            }
            return DateTime.Now;
        }

        public static string GetClientId()
        {
            var client_id = Guid.NewGuid().ToString();
            using (var fmd = sqlite_conn.CreateCommand())
            {
                fmd.CommandText =
                    @"SELECT top 1  client_id from  [SyncTimeStamp]";
                fmd.CommandType = CommandType.Text;
                var dataReader = fmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                        client_id = dataReader.GetString(0);
                }
                else
                {
                    fmd.CommandText =
                        "INSERT INTO [SyncTimeStamp] ('client_id'  ,'last_synctimestamp' ) Values (@clientid,@lastsyncdate)";
                    // Add another entry into our database 
                    fmd.Parameters.AddWithValue("@clientid", client_id);
                    fmd.Parameters.AddWithValue("@lastsyncdate", DateTime.Now.ToUniversalTime());

                    //     com.Parameters.AddWithValue("@modified_date", DateTime.Now.ToUniversalTime());
                    fmd.ExecuteNonQuery(); // Execute the query
                }
            }
            return client_id;
        }

        public static void UpdateSyncTimeStamp()
        {
            try
            {
                var com = new SQLiteCommand(sqlite_conn);
                com.CommandText = "Update [SyncTimeStamp] set last_synctimestamp=DATETIME('NOW'),clinet_id='" +
                                  NoteStore.ClientId;
                // Add the first entry into our database 
                var updated = com.ExecuteNonQuery(); // Execute the query
            }
            catch (Exception ex)
            {
            }
        }
    }
}