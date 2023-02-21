using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqkLibrary.Net.CloudStorage.GoogleDrive;

namespace ConsoleTest.CloudStorage
{
    internal class GoogleDriveTest
    {
        public static async Task Test()
        {
            DriveApiNonLogin driveApiNonLogin = new DriveApiNonLogin();
            int count = 0;
            while (true)
            {
                using Stream stream = await driveApiNonLogin.DownloadFileAsync("1Yz7FGBDdH3sStSd1YGhnw6Xc3BPYmYj5");
                byte[] buffer = new byte[1024 * 1024 * 8];
                long total = 0;
                while (true)
                {
                    int byte_read = stream.Read(buffer, 0, buffer.Length);
                    total += byte_read;
                    if (byte_read == 0) break;
                    Console.WriteLine($"Byte Read: {byte_read}({total}) Bytes; Done {count}");
                }
                count++;
                Console.WriteLine($"Done {count}");
            }
        }
    }
}
