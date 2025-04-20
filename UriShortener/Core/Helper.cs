namespace UriShortener.Core;

internal static class Helper
{
  public static bool HasEmptyFields(params List<string> data){

    foreach(var d in data)
      if (string.IsNullOrEmpty(d.Trim()))
        return true;

    return false;
  }
}