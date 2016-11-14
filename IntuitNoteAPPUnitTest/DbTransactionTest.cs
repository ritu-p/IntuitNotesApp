using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using IntuitNotesBL.NoteDAl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntuitNoteAPPUnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DbTransactionTest
    {

        protected TransactionScope TransactionScope;
        DbWrapper dbClient;
        DbWrapper dbServer;

        [TestInitialize]
        public void TestSetup()
        {
            dbClient = new DbWrapper("notes.db");
            dbClient.ClearTables();
            TransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
       

        }


  
        [TestCleanup]
        public void TestCleanup()
        {
            dbClient.ClearTables();
            dbClient.Close();
            TransactionScope.Dispose();
         
        }

        [TestMethod]
        public void InsertNewClient()
        {
            string newClientId = dbClient.GetClientId();
            DateTime lastsync = dbClient.GetLastSyncTimestamp(newClientId);
            Assert.IsTrue(DateTime.UtcNow > lastsync);

        }

        [TestMethod]
        public void GetExistingClient()
        {
            dbClient.ClearTables();
            string newClientId = dbClient.GetClientId();
            string updatedClientid = dbClient.GetClientId();
            Assert.AreEqual(newClientId, updatedClientid);
        }

        [TestMethod]
        public void GetLastSyncForNewClient()
        {

            DateTime dateTime = dbClient.GetLastSyncTimestamp("New-ClientId");
            string newClientId = dbClient.GetClientId();
            Assert.AreEqual(newClientId, "New-ClientId");
        }
    }
}
