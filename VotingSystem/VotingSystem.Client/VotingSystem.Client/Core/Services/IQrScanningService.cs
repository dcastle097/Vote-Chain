using System.Threading.Tasks;

namespace VotingSystem.Client.Core.Services
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}