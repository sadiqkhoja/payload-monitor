namespace PayloadMonitor
{
    public interface IAuthenticationService
    {
        bool CheckCredentials(string username, string password);
    }
}