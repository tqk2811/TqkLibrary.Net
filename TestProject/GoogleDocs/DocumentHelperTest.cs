using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqkLibrary.Net.GoogleDocs;

namespace TestProject.GoogleDocs
{
    [TestClass]
    public class DocumentHelperTest
    {
        readonly DocumentHelper _documentHelper = new();
        public static IEnumerable<object[]> Ids
        {
            get
            {
                yield return new object[] { "1uzdnz6gfo6V4QUSHBPgOtZkJ2u4yJXMhhkDeb7GkxcY" };
            }
        }


        [TestMethod(), DynamicData(nameof(Ids))]
        public async Task ExportTest(string id)
        {
            using var stream = await _documentHelper.ExportAsync(DocumentHelper.ExportFormat.ziphtml, id);
            using MemoryStream memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
        }
    }
}
