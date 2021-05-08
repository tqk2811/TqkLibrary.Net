namespace TqkLibrary.Net.PhoneNumberApi.OtpSimCom
{
  public enum StatusCode
  {
    Success = 200,
    NotEnoughWallet = 201,
    ApplicationNotFoundOrPaused = 202,
    PhoneNumberIsTemporarilyRunningOut = 203,
    UnAuthenticated = 401,
    Error = -1
  }
}