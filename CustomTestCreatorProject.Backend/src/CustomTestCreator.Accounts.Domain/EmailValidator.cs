using System.Text.RegularExpressions;

namespace CustomTestCreator.Accounts.Domain;

public struct EmailValidator
{
    public static bool IsVaild(string email)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);

        return match.Success;
    }
}