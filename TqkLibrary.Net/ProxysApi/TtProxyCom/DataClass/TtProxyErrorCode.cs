namespace TqkLibrary.Net.ProxysApi.TtProxyCom
{
  public enum TtProxyErrorCode
  {
    OK = 0,
    Failed = 1000,
    UnknownError = 1001,
    ServerInternalError = 1002,
    UnsupportedOperations = 1003,
    SendMailError = 1005,
    WrongArguments = 1010,
    LackArguments = 1011,
    InvalidData = 1012,
    AlreadyCompleted = 1100,
    AlreadyExpired = 1102,
    AlreadyExisted = 1103,
    NoModification = 1104,
    TooManyItems = 1105,
    InvalidRequest = 1400,
    AuthFailed = 1401,
    Forbidden = 1403,
    NotFound = 1404,
    InvalidCaptcha = 1405,
    NoActivation = 1406,
    UserForbidden = 1407,
    WrongPassword = 1408,
    TooManyErrorsWithIncorrectPassword = 1409,
    TooManyRequests = 1429,
    AbnormalOrderStatus = 1601,
    PaymentFailure = 1610,
    AuthFailed2 = 2000,
    ReachesTheUpperLimit = 2001,
    TemporarilyExhausted = 2003,
    WrongArguments2 = 2004,
    CredentialExpires = 2005,
    Forbidden2 = 2006,
    TooManyWhitelists = 2010
  }
}
