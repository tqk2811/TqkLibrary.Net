namespace TqkLibrary.Net.PhoneNumberApi.ChoThueSimCodeCom
{
  public enum ResponseCode
  {
    Success = 0,
    Error = 1,
  }

  public enum ResponseCodeGetPhoneNumber
  {
    Success = 0,
    WalletNotEnough = 1,
    AppNotExist = 2,
    PhoneNumberIsTemporarilyRunningOut = 3
  }

  public enum ResponseCodeMessage
  {
    Success = 0,
    Waitting = 1,
    Timeout = 2,
    InputIsCorrect = 3
  }

  public enum ResponseCodeCancelMessage
  {
    Success = 0,
    IdNotFound = 1,
    WasCanceled = 2
  }
}