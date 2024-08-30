using WinTracker.Dtos;
using WinTracker.Models;

namespace WinTracker.Database
{
    public interface IDatabaseConnection
    {
        void DeleteOldFile(int olderThan = 30);
        Task<List<ApplicationInfo>> LoadAsync(string? path = null);
        Task<List<ApplicationInfo>> LoadManyAsync(DateOnly from, DateOnly to);
        void SaveAsync(List<ApplicationInfoDto> applicationInfoDto);
    }
}