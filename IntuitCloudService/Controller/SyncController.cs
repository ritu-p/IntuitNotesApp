using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IntuitCloudService.SyncBL;
using IntuitNotesBL.NoteDAl;
using IntuitNotesBL.NotesModel;

namespace IntuitCloudService.Controller
{
    public class SyncController : ApiController
    {

        // POST api/<controller>
        public async Task<HttpResponseMessage> SyncData([FromBody] NoteStore noteStore)
        {
            try
            {
                List<Notes> notesForClient = SyncServer.Sync(noteStore);

                return Request.CreateResponse(HttpStatusCode.OK, notesForClient);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

    }
}