using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TqkLibrary.Net.Mail.TempMail;

namespace TqkLibrary.Net.Test.TempMail
{
    [TestClass]
    public class MailTmTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            MailTm.Test();
            var mail = MailTm.NewInstanceAsync().Result;
            mail.StartListen().Wait();
        }
    }
}
