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
        private static DbWrapper dbServer = new DbWrapper("|DataDirectory|servernotes.db");
        public static DateTime lastClinetSyncTimestamp;

        public static List<Notes> Sync(NoteStore noteStore)
        {
            var noteToSyncFromServer = GetDataFromServer(noteStore);
            var notesToSyncFromClient = noteStore.LstNotes;
            foreach (var note in notesToSyncFromClient)
            {
                dbServer.UpsertNotes(note);
            }
            List<Notes> returnList = GetServerDatatoSend(noteToSyncFromServer, notesToSyncFromClient);
            dbServer.UpdateSyncTimeStamp(noteStore);
            return returnList;
        }

        private static List<Notes> GetDataFromServer(NoteStore noteStore)
        {
            List<Notes> notesFromStore = dbServer.GetNotesForSync();
            List<Notes> noteToSync = new List<Notes>();
            lastClinetSyncTimestamp = dbServer.GetLastSyncTimestamp(noteStore.ClientId);
            var changedListFromServer = notesFromStore.Select(n => n).Where(f => f.ModifiedDate > lastClinetSyncTimestamp);
            if ((changedListFromServer != null) && (changedListFromServer.Count() > 0))
                noteToSync = changedListFromServer.ToList();
            return noteToSync;
        }

        private static List<Notes> GetServerDatatoSend(List<Notes> noteToSyncFromServer, List<Notes> notesToSyncFromClient)
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