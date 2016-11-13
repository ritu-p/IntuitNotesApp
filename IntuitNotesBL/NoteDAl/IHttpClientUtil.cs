using System;
using System.Threading.Tasks;

namespace IntuitNotesBL.NoteDAl
{
    public interface IHttpClientUtil
    {
        Task<HttpUtilityOutput> PostHttpAsync(Uri url, string jsonContent);
    }
}