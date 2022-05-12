using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TqkLibrary.Net.Mails.TempMails;

namespace TqkLibrary.Net.Test.TempMail
{
    [TestClass]
    public class TempMailOrgTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            TempMailOrg tempMailOrg = new TempMailOrg();
            using StreamWriter streamWriter = new StreamWriter("test.txt", true);
            for (int i = 0; i < 50; i++)
            {
                var token = tempMailOrg.InitToken().Result;
                streamWriter.WriteLine($"{token.MailBox}|{token.Token}");
            }
        }
    }
}
