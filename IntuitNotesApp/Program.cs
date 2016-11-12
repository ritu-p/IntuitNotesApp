using System;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Windows.Forms;

namespace IntuitNotesApp
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
           

            var config = new HttpSelfHostConfiguration("http://localhost:9090");
            IntuitCloudService.WebAPIConfigcs.Register(config);

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{action}",
                  defaults: new { controller = "Sync", action = "SyncData" });


            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new IntuitNotes());
            }
        }
    }
}