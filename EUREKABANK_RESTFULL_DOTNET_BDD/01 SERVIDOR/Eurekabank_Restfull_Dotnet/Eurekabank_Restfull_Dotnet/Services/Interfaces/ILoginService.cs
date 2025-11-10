namespace Eurekabank_Restfull_Dotnet.Services.Interfaces
{
    public interface ILoginService
    {
        bool Login(string username, string password);
    }
}
