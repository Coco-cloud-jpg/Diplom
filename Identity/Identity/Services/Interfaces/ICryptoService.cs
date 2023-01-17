namespace Identity.Services.Interfaces
{
    public interface ICryptoService
    {
        string ComputeSHA256(string message);
    }
}
