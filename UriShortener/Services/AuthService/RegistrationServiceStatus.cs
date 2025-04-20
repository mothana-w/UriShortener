namespace UriShortener.Services;

public enum RegistrationServiceStatus 
{
  Success,

  UserAlreadyExists,

  EmptyUsername,
  IncorrectUsernameFormat,

  EmptyEmail,
  IncorrectEmailFormat,

  EmptyPassword,
  ShortPassword,
  LongPassword,
  WeakPassword,
}