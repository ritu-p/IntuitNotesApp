using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using IntuitNotesBL.NoteDAl;


namespace IntuitCloudService
{
    public class WebAPIConfigcs
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        // DbWrapper.Connect("|DataDirectory|servernotes.db");
        }
    }
}