using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqkLibrary.Net.Others;

namespace TqkLibrary.Net.Test
{
    [TestClass]
    public class CloneMailSieuReApiTest
    {
        [TestMethod]
        public void Test()
        {
            CloneMailSieuReApi cloneMail = new CloneMailSieuReApi("tqk2811", "3ygvz7zTUJSa6VD");
            var ListResource = cloneMail.ListResource().Result;
            var InfoResource = cloneMail.InfoResource(ListResource.Categories.First().Accounts.First()).Result;
            //var ListResource = cloneMail.ListResource().Result;
        }
    }
}
