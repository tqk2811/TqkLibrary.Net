using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqkLibrary.Net.Mails.BuyMailApi;

namespace TqkLibrary.Net.Test.TempMail
{
    [TestClass]
    public class DongVanFbTest
    {
        [TestMethod]
        public void Test()
        {
            var dongvan = new DongVanFbApi("lPaoSt1BWveOdaCKidv7rU6NT");
            var info = dongvan.AccountType().Result;
            //var email = dongvan.BuyAccount();
            Assert.IsNotNull(info);
        }
    }
}
