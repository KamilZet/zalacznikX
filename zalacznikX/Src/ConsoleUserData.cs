using System;
using System.Security;
using Microsoft.Exchange.WebServices.Data;

namespace zalacznikX
{
  public class ConsoleUserData : IUserData
  {
    public static ConsoleUserData UserData;

    public static IUserData GetUserData()
    {
      if (UserData == null)
      {
        GetUserDataFromConsole();
      }

      return UserData;
    }

    internal static void GetUserDataFromConsoleCredUI(ref ExchangeService service)
    {
        CredentialHelper.AppLogin(ref service);
    }

    private static void GetUserDataFromConsole()
    {
      UserData = new ConsoleUserData();

      Console.Write("Podaj adres email subskrybowanego konta: ");
      UserData.EmailAddress = Console.ReadLine();

      UserData.Password = new SecureString();

      Console.Write("Wprowadź hasło subskrybowanego konta: ");

      while (true)
      {
          ConsoleKeyInfo userInput = Console.ReadKey(true);
          if (userInput.Key == ConsoleKey.Enter)
          {
              break;
          }
          else if (userInput.Key == ConsoleKey.Escape)
          {
              return;
          }
          else if (userInput.Key == ConsoleKey.Backspace)
          {
              if (UserData.Password.Length != 0)
              {
                  UserData.Password.RemoveAt(UserData.Password.Length - 1);
              }
          }
          else
          {
              UserData.Password.AppendChar(userInput.KeyChar);
              Console.Write("*");
          }
      }

      Console.WriteLine();

      UserData.Password.MakeReadOnly();
    }

    public ExchangeVersion Version { get { return ExchangeVersion.Exchange2013; } }

    public string EmailAddress
    {
        get;
        set;
    }

    public SecureString Password
    {
        get;
        set;
    }

    public Uri AutodiscoverUrl
    {
        get;
        set;
    }
  }
}
