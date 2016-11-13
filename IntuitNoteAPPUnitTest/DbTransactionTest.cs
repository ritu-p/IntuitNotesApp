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


        [TestInitialize]
        public void TestSetup()
        {
            DbWrapper.ClearTables();
            TransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TransactionScope.Dispose();
            DbWrapper.ClearTables();
        }

        [TestMethod]
        public void InsertNewClient()
        {
            string newClientId = DbWrapper.GetClientId();
            DateTime lastsync = DbWrapper.GetLastSyncTimestamp(newClientId);
            Assert.IsTrue(DateTime.UtcNow > lastsync);

        }

        [TestMethod]
        public void GetExistingClient()
        {
            string newClientId = DbWrapper.GetClientId();
            string updatedClientid = DbWrapper.GetClientId();
            Assert.AreEqual(newClientId, updatedClientid);
        }

        [TestMethod]
        public void GetLastSyncForNewClient()
        {

            DateTime dateTime = DbWrapper.GetLastSyncTimestamp("New-ClientId");
            string newClientId = DbWrapper.GetClientId();
            Assert.AreEqual(newClientId, "New-ClientId");
        }
    }
}
