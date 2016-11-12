using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntuitNotesBL.NoteDAl;
using IntuitNotesBL.NotesModel;

namespace IntuitCloudService.SyncBL
{
    public class SyncServer
    {
        public static DateTime lastClinetSyncTimestamp;
        public static List<Notes> Sync(NoteStore noteStore)
        {
            var noteToSyncFromServer = GetDataFromServer(noteStore);
            var notesToSyncFromClient = noteStore.LstNotes;
            foreach (var note in notesToSyncFromClient)
            {
                DbWrapper.UpsertNotes(note);
            }
            return GetServerDatatoSend(noteToSyncFromServer, notesToSyncFromClient);
        }

        private static List<Notes> GetDataFromServer(NoteStore noteStore)
        {
            List<Notes> notesFromStore = DbWrapper.GetNotesForSync();
            List<Notes> noteToSync = new List<Notes>();
            lastClinetSyncTimestamp = DbWrapper.GetLastSyncTimestamp(noteStore.ClientId);
            var changedListFromServer = notesFromStore.Select(n => n).Where(f => f.ModifiedDate > lastClinetSyncTimestamp);
            if ((changedListFromServer != null) && (changedListFromServer.Count() > 0))
                noteToSync = changedListFromServer.ToList();
            return noteToSync;
        }

        private  static List<Notes> GetServerDatatoSend(List<Notes> noteToSyncFromServer, List<Notes> notesToSyncFromClient)
        {
            foreach (var notetoRemove in notesToSyncFromClient)
            {
                var item = noteToSyncFromServer.SingleOrDefault(x => x.NoteGuid == notetoRemove.NoteGuid);
                noteToSyncFromServer.Remove(item);
            }
            return noteToSyncFromServer;
        }
    }
}