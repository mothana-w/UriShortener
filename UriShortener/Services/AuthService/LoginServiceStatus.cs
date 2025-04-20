namespace UriShortener.Services;

public enum LoginServiceStatus 
{
  Success,

  UsernameEmailOrPasswordIncorrect,
  UsernameOrEmailEmpty,
  PasswordEmpty,
  EmailNotVerified
}