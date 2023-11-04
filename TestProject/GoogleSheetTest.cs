using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqkLibrary.Net.Other.Others;
namespace TestProject
{
    [TestClass]
    public class GoogleSheetTest
    {
        readonly GoogleSheet _googleSheet = new GoogleSheet();
        public static IEnumerable<object[]> Ids
        {
            get
            {
                yield return new object[] { "1olq8d_cPt6BlLSnql3zSVmNsMa8uJCD-9qpRzpSHR6U" };
            }
        }


        [TestMethod(), DynamicData(nameof(Ids))]
        public async Task ExportTest(string id)
        {
            using var stream = await _googleSheet.ExportAsync(id);
            using MemoryStream memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
        }
    }
}
