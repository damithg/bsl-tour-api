using System.Threading.Tasks;

namespace BSLTours.API.Services
{
    public interface ITurnstileService
    {
        Task<bool> VerifyTokenAsync(string token, string? remoteIp = null);
    }
}