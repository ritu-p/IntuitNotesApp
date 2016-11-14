using System.Net;

namespace IntuitNotesBL.NoteDAl
{
    public class HttpUtilityOutput
    {
        public HttpStatusCode Status { get; set; }
        public string Content { get; set; }
    }
}