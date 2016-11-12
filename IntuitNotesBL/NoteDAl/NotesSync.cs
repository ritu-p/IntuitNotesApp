using System;
using System.Collections.Generic;
using System.Linq;
using IntuitNotesApp.NotesModel;
using IntuitNotesBL.NoteDAl;
using Newtonsoft.Json;

namespace IntuitNotesApp.NoteDAl
{
    public class NotesSync
    {
        private static DateTime lastSyncTime;
        private static readonly List<Notes> notes = DbWrapper.GetNotesForSync();
        private static List<Notes> noteToSync;
        private static readonly NoteStore noteStore = new NoteStore();

        private static void syncFromCloud()
        {
        }

        private static async void syncToCloud()
        {
            if ((noteToSync != null) && (noteToSync.Count > 0))
            {
                var json = JsonConvert.SerializeObject(note);
                var url = new Uri("http://localhost:8080/Sync");
                var result = await HttpClientUtil.PostHttpAsync(url, json).ConfigureAwait(false);
            }
        }

        public static void Sync(string clientId)
        {
            try
            {
                lastSyncTime = DbWrapper.GetLastSyncTimestamp(clientId);
                var lstSync = notes.Select(n => n).Where(f => f.ModifiedDate > lastSyncTime);
                if ((lstSync != null) && (lstSync.Count() > 0))
                    noteToSync = lstSync.ToList();
                noteStore.LastUpDateTime = lastSyncTime;
                noteStore.LstNotes = noteToSync;
                noteStore.ClientId = clientId;
                syncToCloud();
                syncFromCloud();
                DbWrapper.UpdateSyncTimeStamp();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}