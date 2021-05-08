using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.CloudStorage.GoogleDrive
{
  public static class DriveApiNonLogin
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="save"></param>
    /// <exception cref="HttpRequestException"></exception>
    /// <returns></returns>
    public static async Task Download(string fileId, Dictionary<string, string> cookies, Stream save) => await Download(fileId, cookies, save, CancellationToken.None).ConfigureAwait(false);

    /// <summary>
    ///
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="save"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="HttpRequestException"></exception>
    /// <returns></returns>
    public static async Task Download(string fileId, Dictionary<string, string> cookies, Stream save, CancellationToken cancellationToken)
    {
      using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://docs.google.com/uc?id={fileId}");
      httpRequestMessage.Headers.Add("Cookie", cookies.GetCookiesString());
      using HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
      if (httpResponseMessage.EnsureSuccessStatusCode().Content.Headers.ContentType.MediaType.Contains("application"))
      {
        await (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false)).CopyToAsync(save, 81920, cancellationToken).ConfigureAwait(false);
        return;
      }

      using HttpRequestMessage httpRequestMessage2 = new HttpRequestMessage(HttpMethod.Get, $"https://docs.google.com/uc?confirm=abeQ?id={fileId}");
      httpRequestMessage2.Headers.Add("Cookie", cookies.GetCookiesString());
      using HttpResponseMessage httpResponseMessage2 = await NetExtensions.httpClient.SendAsync(httpRequestMessage2, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
      if (httpResponseMessage2.EnsureSuccessStatusCode().Content.Headers.ContentType.MediaType.Equals("application/octet-stream"))
      {
        await (await httpResponseMessage2.Content.ReadAsStreamAsync().ConfigureAwait(false)).CopyToAsync(save, 81920, cancellationToken).ConfigureAwait(false);
        return;
      }

      throw new Exception("Download Failed");
    }

    //https://clients6.google.com/drive/v2beta/files?openDrive=false&reason=102&syncType=0&errorRecovery=false&q=trashed%20%3D%20false%20and%20
    //'0Bx154iMNwuyWMVp0TTJzYjFOYzg'%20in%20parents&
    //fields=kind%2CnextPageToken%2Citems(kind%2CmodifiedDate%2CmodifiedByMeDate%2ClastViewedByMeDate%2CfileSize%2Cowners(kind%2CpermissionId%2CdisplayName%2Cpicture)%2ClastModifyingUser(kind%2CpermissionId%2CdisplayName%2Cpicture)%2ChasThumbnail%2CthumbnailVersion%2Ctitle%2Cid%2Cshared%2CsharedWithMeDate%2CuserPermission(role)%2CexplicitlyTrashed%2CmimeType%2CquotaBytesUsed%2Ccopyable%2CfileExtension%2CsharingUser(kind%2CpermissionId%2CdisplayName%2Cpicture)%2Cspaces%2Cversion%2CteamDriveId%2ChasAugmentedPermissions%2CcreatedDate%2CtrashingUser(kind%2CpermissionId%2CdisplayName%2Cpicture)%2CtrashedDate%2Cparents(id)%2CshortcutDetails(targetId%2CtargetMimeType%2CtargetLookupStatus)%2Ccapabilities(canCopy%2CcanDownload%2CcanEdit%2CcanAddChildren%2CcanDelete%2CcanRemoveChildren%2CcanShare%2CcanTrash%2CcanRename%2CcanReadTeamDrive%2CcanMoveTeamDriveItem)%2Clabels(starred%2Ctrashed%2Crestricted%2Cviewed))%2CincompleteSearch
    //&appDataFilter=NO_APP_DATA&spaces=drive&maxResults=50&supportsTeamDrives=true&includeTeamDriveItems=true&corpora=default
    //&orderBy=folder%2Ctitle_natural%20asc&retryCount=0&key=AIzaSyC1qbk75NzWBvSaDh6KnsjjA9pIrP4lYIE

    /// <summary>
    ///
    /// </summary>
    /// <param name="folderId"></param>
    /// <exception cref="HttpRequestException"></exception>
    /// <returns></returns>
    public static async Task<string> ListPublicFolder(string folderId)
    {
      string urlRequest = "https://clients6.google.com/drive/v2beta/files?openDrive=false&reason=102&syncType=0&errorRecovery=false" +
        "&appDataFilter=NO_APP_DATA&spaces=drive&maxResults=1000&supportsTeamDrives=true" +
        "&includeTeamDriveItems=true&corpora=default&retryCount=0&key=AIzaSyC1qbk75NzWBvSaDh6KnsjjA9pIrP4lYIE" +
        $"&q=trashed%20%3D%20false%20and%20'{folderId}'%20in%20parents" +
        "&fields=*" + "&orderBy=folder%2Ctitle_natural%20asc";

      using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, urlRequest);
      httpRequestMessage.Headers.Referrer = new Uri("https://drive.google.com");
      using HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
      return await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="format">xlsx, ods, csv, tsv, zip (html zip)</param>
    /// <exception cref="HttpRequestException"></exception>
    /// <returns></returns>
    public static async Task ExportExcel(string fileId, Stream copy, CancellationToken cancellationToken, ExportExcelType format = ExportExcelType.xlsx)
    {
      string url = $"https://docs.google.com/spreadsheets/d/{fileId}/export?format={format}&id={fileId}";
      using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
      httpRequestMessage.Headers.Referrer = new Uri("https://docs.google.com");
      using HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
      await (await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStreamAsync().ConfigureAwait(false)).CopyToAsync(copy, 81920, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="copy"></param>
    /// <param name="format"></param>
    /// <exception cref="HttpRequestException"></exception>
    /// <returns></returns>
    public static async Task ExportExcel(string fileId, Stream copy, ExportExcelType format = ExportExcelType.xlsx)
      => await ExportExcel(fileId, copy, CancellationToken.None, format).ConfigureAwait(false);

    public enum ExportExcelType
    {
      xlsx,
      ods,
      csv,
      tsv,
      zip
    }

    //pdf is post https://docs.google.com/spreadsheets/d/17tQ4em4gVHcceSUL2a0oMlWG1aajTNFp/pdf?id=17tQ4em4gVHcceSUL2a0oMlWG1aajTNFp
    //a=true&pc=%5Bnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2C0%2C%5B%5B%221989784426%22%5D%5D%2C10000000%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2Cnull%2C44088.422909560184%2Cnull%2Cnull%2C%5B0%2Cnull%2C1%2C0%2C0%2C0%2C0%2C0%2C1%2C1%2C1%2C1%2Cnull%2Cnull%2C1%2C1%5D%2C%5B%22letter%22%2C0%2C5%2C1%2C%5B0.75%2C0.75%2C0.7%2C0.7%5D%5D%2Cnull%2C0%5D&gf=%5B%5D
  }
}