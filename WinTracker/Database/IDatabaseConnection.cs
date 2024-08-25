using WinTracker.Dtos;
using WinTracker.Models;

namespace WinTracker.Database
{
    public interface IDatabaseConnection
    {
        void DeleteOldFile(int olderThan = 30);
        List<ApplicationInfo> Load(string? path = null);
        List<ApplicationInfo> LoadMany(DateOnly from, DateOnly to);
        void Save(List<ApplicationInfoDto> applicationInfoDto);
    }
}