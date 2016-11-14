using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Text;

using IntuitNotesBL.NotesModel;

namespace IntuitNotesBL.NoteDAl
{
    public  class DbWrapper
    {
        private static string dbName;
        private  SQLiteConnection sqlite_conn;
        private  SQLiteDataReader sqlite_datareader;

        public DbWrapper(String Dbname)
        {
            dbName = Dbname;
            Connect();
        }
     /*   private static string GetDbName()
        {
            if (ConfigurationManager.AppSettings["isServer"] != null &&
                Convert.ToBoolean(ConfigurationManager.AppSettings["isServer"]))
            {
                return !String.IsNullOrEmpty(ConfigurationManager.AppSettings["Server.dbName"].ToString()) ? ConfigurationManager.AppSettings["Server.dbName"].ToString() : "|DataDirectory|servernotes.db";
            }
            return !String.IsNullOrEmpty(ConfigurationManager.AppSettings["Client.dbName"].ToString()) ? ConfigurationManager.AppSettings["Client.dbName"].ToString() : "notes.db";
        }*/
        public  void Connect()
        {
            sqlite_conn = new SQLiteConnection("Data Source=" + dbName + ";Version=3;New=True;Compress=True;");

            // open the connection:
            sqlite_conn.Open();

            CreateNoteSchema();
            CreateSyncTimeStampStore();

        }

        private  void CreateNoteSchema()
        {
            InitialzeDb();
            using (
                var mCmd =
                    new SQLiteCommand(
                        "CREATE TABLE IF NOT EXISTS [Notes] ('notes_id' TEXT ,'title' TEXT, 'body' TEXT,'is_deleted' INTEGER DEFAULT 0,'modified_dt' DATETIME DEFAULT current_timestamp);",
                        sqlite_conn))
            {
                mCmd.ExecuteNonQuery();
            }
        }

        private  void InitialzeDb()
        {
            if (sqlite_conn == null)
            {
                Connect();
            }
        }

        private  void CreateSyncTimeStampStore()
        {
            InitialzeDb();
            using (
                var mCmd =
                    new SQLiteCommand(
                        "CREATE TABLE IF NOT EXISTS [SyncTimeStamp] (client_id TEXT,last_synctimestamp DATETIME );",
                        sqlite_conn))
            {
                mCmd.ExecuteNonQuery();
            }
        }

        public  void UpsertNotes(Notes note)
        {
            try
            {
                InitialzeDb();
                var com = new SQLiteCommand(sqlite_conn);
                com.CommandText = "Update [Notes] set 'title'='" + note.Title + "',body='" + note.Body +
                                  "','is_deleted'=" + Convert.ToInt64(note.IsDeleted) +
                                  ", 'modified_dt'= DATETIME('NOW') where notes_id='" +
                                  note.NoteGuid + "'"; // Add the first entry into our database 
                var updated = com.ExecuteNonQuery(); // Execute the query
                if (updated != 1)
                {
                    com.CommandText =
                        "INSERT INTO [Notes] ('notes_id'  ,'title', 'body','is_deleted' ) Values (@notes_id,@title,@body,@deleted)";
                    // Add another entry into our database 
                    com.Parameters.AddWithValue("@notes_id", note.NoteGuid);
                    com.Parameters.AddWithValue("@title", note.Title);
                    com.Parameters.AddWithValue("@body", note.Body);
                    com.Parameters.AddWithValue("@deleted", Convert.ToInt64(note.IsDeleted));

                    //     com.Parameters.AddWithValue("@modified_date", DateTime.Now.ToUniversalTime());
                    com.ExecuteNonQuery(); // Execute the query
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  Dictionary<string, Notes> GetNotesForDisplay()
        {
            InitialzeDb();
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

        public  List<Notes> GetNotesForSync()
        {
            InitialzeDb();
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

        public  DateTime GetLastSyncTimestamp(string clientId)
        {
            InitialzeDb();
            using (var fmd = sqlite_conn.CreateCommand())
            {
                fmd.CommandText =
                    @"SELECT  last_synctimestamp from  [SyncTimeStamp] where client_id='" + clientId + "'";
                fmd.CommandType = CommandType.Text;
                var dataReader = fmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                        return dataReader.GetDateTime(0).ToUniversalTime();
                }
                else
                {
                    dataReader.Close();
                    var insertCMD = sqlite_conn.CreateCommand();
                    insertCMD.CommandText =
                        "INSERT INTO [SyncTimeStamp] ('client_id'  ,'last_synctimestamp' ) Values (@clientid,@lastsyncdate)";
                    // Add another entry into our database 
                    insertCMD.Parameters.AddWithValue("@clientid", clientId);
                    insertCMD.Parameters.AddWithValue("@lastsyncdate", DateTime.Now.ToUniversalTime());

                    //     com.Parameters.AddWithValue("@modified_date", DateTime.Now.ToUniversalTime());
                    insertCMD.ExecuteNonQuery(); // Execute the query
                }


            }
            return DateTime.Now.ToUniversalTime();
        }

        public  string GetClientId()
        {
            InitialzeDb();
            var client_id = Guid.NewGuid().ToString();
            try
            {

                using (var fmd = sqlite_conn.CreateCommand())
                {
                    fmd.CommandText =
                        @"SELECT client_id from  [SyncTimeStamp] LIMIT 1";
                    fmd.CommandType = CommandType.Text;
                    var dataReader = fmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                            client_id = dataReader.GetString(0);
                    }
                    else
                    {
                        dataReader.Close();
                        var insertCMD = sqlite_conn.CreateCommand();
                        insertCMD.CommandText =
                            "INSERT INTO [SyncTimeStamp] ('client_id'  ,'last_synctimestamp' ) Values (@clientid,@lastsyncdate)";
                        // Add another entry into our database 
                        insertCMD.Parameters.AddWithValue("@clientid", client_id);
                        insertCMD.Parameters.AddWithValue("@lastsyncdate", DateTime.Now.ToUniversalTime());

                        //     com.Parameters.AddWithValue("@modified_date", DateTime.Now.ToUniversalTime());
                        insertCMD.ExecuteNonQuery(); // Execute the query
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return client_id;
        }

        public  void UpdateSyncTimeStamp(NoteStore noteStore)
        {
            InitialzeDb();
            try
            {
                var com = new SQLiteCommand(sqlite_conn);
                com.CommandText = "Update [SyncTimeStamp] set last_synctimestamp=DATETIME('NOW'),client_id='" +
                                  noteStore.ClientId + "'";
                ;
                // Add the first entry into our database 
                var updated = com.ExecuteNonQuery(); // Execute the query
            }
            catch (Exception ex)
            {
            }
        }

        public  void ClearTables()
        {
            InitialzeDb();
            try
            {
                var com = new SQLiteCommand(sqlite_conn);
                com.CommandText = "delete from Notes";
                ;
                // Add the first entry into our database 
                var updated = com.ExecuteNonQuery(); // Execute the query
                var com2 = new SQLiteCommand(sqlite_conn);
                com2.CommandText = "delete from [SyncTimeStamp]";
                ;

                var updated1 = com2.ExecuteNonQuery(); // Execute the query
            }
            catch (Exception ex)
            {
            }
        }

        public void Close()
        {
            sqlite_conn.Close();
            GC.Collect();
        }
    }
}