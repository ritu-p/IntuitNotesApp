using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntuitNotesApp.NotesModel;
using Newtonsoft.Json;

namespace IntuitNotesApp.NoteDAl
{
    public class NotesSync
    {
        private static DateTime lastSyncTime;
        private static List<Notes> notes = DbWrapper.GetNotesForSync();
        private static List<Notes> noteToSync;

        private static void syncFromCloud()
        {
           
        }

        private static void syncToCloud()
        {
 if (noteToSync != null && noteToSync.Count > 0)
 {
     string json = JsonConvert.SerializeObject(noteToSync);


 }
        }

        public static void Sync()
        {
            try
            {

           
            lastSyncTime = DbWrapper.GetLastSyncTimestamp();
            var lstSync=notes.Select(n=>n).Where(f=>f.ModifiedDate>lastSyncTime);
            if (lstSync!=null && lstSync.Count() > 0)
            {
                noteToSync = lstSync.ToList<Notes>();
            }
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
