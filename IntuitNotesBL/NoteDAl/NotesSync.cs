using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IntuitNotesBL.NoteDAl;
using IntuitNotesBL.NotesModel;
using Newtonsoft.Json;

namespace IntuitNotesBL.NoteDAl
{
    public class NotesSync
    {
        private static DbWrapper dbClient = new DbWrapper("notes.db");
        private static DateTime lastSyncTime;
        private static readonly List<Notes> notes = dbClient.GetNotesForSync();
        private static List<Notes> noteToSync;
        private static readonly NoteStore noteStore = new NoteStore();
        private IHttpClientUtil httpClientUtil;
        

        public NotesSync(IHttpClientUtil httpClient)
        {
            httpClientUtil = httpClient;
        }
        private static void syncFromCloud(List<Notes> notesFromServer)
        {
            foreach (var note in notesFromServer)
            {
                dbClient.UpsertNotes(note);
            }
        }

      public async Task<Dictionary<string, Notes>> Sync(string clientId)
        {
            try
            {
                lastSyncTime = dbClient.GetLastSyncTimestamp(clientId);
                var lstSync = notes.Select(n => n).Where(f => f.ModifiedDate > lastSyncTime);
                if ((lstSync != null) && (lstSync.Count() > 0))
                {
                    noteToSync = lstSync.ToList();
                }
                else
                {
                    noteToSync = new List<Notes>();
                }

                noteStore.LastUpDateTime = lastSyncTime;
                noteStore.LstNotes = noteToSync;
                noteStore.ClientId = clientId;
                var json = JsonConvert.SerializeObject(noteStore);
                var url = new Uri("http://localhost:9090/api/Sync/SyncData");
                var result = await httpClientUtil.PostHttpAsync(url, json).ConfigureAwait(false);
                if (result.Status == HttpStatusCode.OK)
                {
                    List<Notes> notesFromServer = JsonConvert.DeserializeObject<List<Notes>>(result.Content);
                    syncFromCloud(notesFromServer);

                    dbClient.UpdateSyncTimeStamp(noteStore);
                }


              return dbClient.GetNotesForDisplay();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}