using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using IntuitNotesBL.NoteDAl;
using IntuitNotesBL.NotesModel;
using Newtonsoft.Json;

namespace IntuitNotesBL.NoteDAl
{
    public class NotesSync
    {
        private static DateTime lastSyncTime;
        private static readonly List<Notes> notes = DbWrapper.GetNotesForSync();
        private static List<Notes> noteToSync;
        private static readonly NoteStore noteStore = new NoteStore();

        private static void syncFromCloud(List<Notes> notesFromServer)
        {
            foreach (var note in notesFromServer)
            {
                DbWrapper.UpsertNotes(note);
            }
        }

        private static async void syncToCloud()
        {
            if ((noteToSync != null))
            {
                var json = JsonConvert.SerializeObject(noteStore);
                var url = new Uri("http://localhost:72/api/Sync/SyncData");
                var result = await HttpClientUtil.PostHttpAsync(url, json).ConfigureAwait(false);
                if (result.Status == HttpStatusCode.OK)
                {
                 List<Notes> notesFromServer  =JsonConvert.DeserializeObject<List<Notes>>(result.Content);
                    syncFromCloud(notesFromServer);
                }
            }
        }

        public static Dictionary<string, Notes> Sync(string clientId)
        {
            try
            {
                lastSyncTime = DbWrapper.GetLastSyncTimestamp(clientId);
                var lstSync = notes.Select(n => n).Where(f => f.ModifiedDate > lastSyncTime);
                if ((lstSync != null) && (lstSync.Count() > 0))
                {
                    noteToSync = lstSync.ToList();
                }
                else
                {
                    noteToSync=new List<Notes>();
                }

                noteStore.LastUpDateTime = lastSyncTime;
                    noteStore.LstNotes = noteToSync;
                    noteStore.ClientId = clientId;
                    syncToCloud();
                
                DbWrapper.UpdateSyncTimeStamp(noteStore);
            
            return  DbWrapper.GetNotesForDisplay();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}