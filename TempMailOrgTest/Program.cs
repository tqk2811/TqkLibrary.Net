// See https://aka.ms/new-console-template for more information

using TqkLibrary.Net.Mails.TempMails;

int count = 500;
Console.WriteLine("Nhập số lượng:");
while (true)
{
    string val = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(val)) break;
    if (int.TryParse(val, out count)) break;
    Console.WriteLine("Nhập sai, nhập lại:");
}

TempMailOrg tempMailOrg = new TempMailOrg();
try
{
    for (int i = 0; i < count; i++)
    {
        var token = await tempMailOrg.InitToken();
        var message = await tempMailOrg.Messages(token);
        Console.WriteLine($"{(i + 1):0000} {token.MailBox} | {token.Token}");
    }
    Console.WriteLine("Hoàn tất");
    Console.ReadLine();
}
catch (Exception ex)
{
    if (ex is AggregateException ae) ex = ae.InnerException;
    Console.WriteLine($"{ex.GetType().FullName}: {ex.Message}, {ex.StackTrace}");
    Console.ReadLine();
}