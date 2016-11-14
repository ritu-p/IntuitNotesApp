using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Http.SelfHost;
using IntuitNotesBL.NoteDAl;
using IntuitNotesBL.NotesModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Moq;

namespace IntuitNoteAPPUnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class IntuitNoteApp
    {


        protected TransactionScope TransactionScope;

        [TestInitialize]
        public void TestSetup()
        {

            TransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);

        }

        [TestCleanup]
        public void TestCleanup()
        {
            TransactionScope.Dispose();

        }
        private void RegisterSelfhostServer()
        {
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration("http://localhost:9091");
            IntuitCloudService.WebAPIConfigcs.Register(config);
            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{action}",
                defaults: new { controller = "Sync", action = "SyncData" });
            HttpSelfHostServer server = new HttpSelfHostServer(config);

            server.OpenAsync().Wait();
        }

        [TestMethod]
        public void SyncNewRecodsTOCloud()
        {

            RegisterSelfhostServer();
            ConfigurationManager.AppSettings["isServer"] = "true";
            ConfigurationManager.RefreshSection("AppSettings");
            //Insert records to server for client1
            NoteStore client1Store = new NoteStore();
            List<Notes> lstNotes = new List<Notes>();
            Notes client1Note = new Notes();
            client1Note.NoteGuid = "b68f9b83-667c-43f3-98ca-422b31ad37a6";
            client1Note.Title = "Title1";
            client1Note.Body = new StringBuilder("TestBody");
            client1Note.IsDeleted = false;
            lstNotes.Add(client1Note);
            client1Store.ClientId = "TestClient1";
            client1Store.LstNotes = lstNotes;
            client1Store.LastUpDateTime = DateTime.UtcNow;
            var json = JsonConvert.SerializeObject(client1Store);
            var url = new Uri("http://localhost:9091/api/Sync/SyncData");
            var result = new HttpClientUtil().PostHttpAsync(url, json).Result;
            if (result.Status == HttpStatusCode.OK)
            {
                List<Notes> notesFromServer = JsonConvert.DeserializeObject<List<Notes>>(result.Content);

                var itemServerReturned = notesFromServer.SingleOrDefault(x => x.NoteGuid == "b68f9b83-667c-43f3-98ca-422b31ad37a6");
                Assert.IsNull(itemServerReturned);
            }



        }

        [TestMethod]
        public async Task SyncNewRecordsfromCloud()
        {
                  

            var mockhttpclientUtil = new Mock<IHttpClientUtil>();
            HttpUtilityOutput httpUtility = new HttpUtilityOutput();
            List<Notes> lstNotes = new List<Notes>();
            Notes client1Note = new Notes();
            client1Note.NoteGuid = "b68f9b83-667c-43f3-98ca-422b31ad37b5";
            client1Note.Title = "Title1";
            client1Note.Body = new StringBuilder("TestBody");
            client1Note.IsDeleted = false;
            lstNotes.Add(client1Note);
            var json = JsonConvert.SerializeObject(lstNotes);
            httpUtility.Status = HttpStatusCode.OK;
            httpUtility.Content = json;
            mockhttpclientUtil.Setup(m => m.PostHttpAsync(It.IsAny<Uri>(), It.IsAny<String>())).ReturnsAsync(httpUtility);

            NotesSync notesSync = new NotesSync(mockhttpclientUtil.Object);
            Dictionary<string, Notes> resultDic = await notesSync.Sync("TestClient1").ConfigureAwait(false);
            Assert.IsTrue(resultDic.ContainsKey(client1Note.NoteGuid));

        }
        [TestMethod]
        public void SyncNewDeletedRecordsfromCloud()
        {

            var mockhttpclientUtil = new Mock<IHttpClientUtil>();
            HttpUtilityOutput httpUtility = new HttpUtilityOutput();
            List<Notes> lstNotes = new List<Notes>();
            Notes client1Note = new Notes();
            client1Note.NoteGuid = "b68f9b83-667c-43f3-98ca-422b31ad33qw";
            client1Note.Title = "Title1";
            client1Note.Body = new StringBuilder("TestBody");
            client1Note.IsDeleted = true;
            lstNotes.Add(client1Note);
            var json = JsonConvert.SerializeObject(lstNotes);
            httpUtility.Status = HttpStatusCode.OK;
            httpUtility.Content = json;
            mockhttpclientUtil.Setup(m => m.PostHttpAsync(It.IsAny<Uri>(), It.IsAny<String>())).ReturnsAsync(httpUtility);

            NotesSync notesSync = new NotesSync(mockhttpclientUtil.Object);
            Dictionary<string, Notes> resultDic = notesSync.Sync("TestClient1").Result;
            Assert.IsTrue(!resultDic.ContainsKey(client1Note.NoteGuid));


        }

    }
}
