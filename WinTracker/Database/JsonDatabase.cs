using System.IO;
using System.Text.Json;
using WinTracker.Dtos;
using WinTracker.Models;
using WinTracker.Utils;

namespace WinTracker.Database
{
    public class JsonDatabase : IDatabaseConnection
    {
        private readonly static string DatabaseFileFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + """\WinTracker\logs\""";
        private readonly static string SettingsFileFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + """\WinTracker\settings\""";
        private readonly static string SettingsFileName = "settings.json";
        private readonly static string DatabaseFileName = "database.json";
        private readonly static string DatabaseFilePath = DatabaseFileFolder + DateTime.Today.ToString("yyyyMMdd") + DatabaseFileName;


        public async void SaveAsync(List<ApplicationInfoDto> applicationInfoDto)
        {
            try
            {
                if (!Directory.Exists(DatabaseFileFolder)) Directory.CreateDirectory(DatabaseFileFolder);

                string json = JsonSerializer.Serialize(applicationInfoDto);
                await File.WriteAllTextAsync(DatabaseFilePath, json);
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void SaveAsync(UserSettings userSettings)
        {
            try
            {
                if (!Directory.Exists(SettingsFileFolder)) Directory.CreateDirectory(SettingsFileFolder);

                string json = JsonSerializer.Serialize(userSettings);
                await File.WriteAllTextAsync(SettingsFileFolder + SettingsFileName, json);
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<ApplicationInfo>> LoadAsync(string? path = null)
        {
            if (string.IsNullOrEmpty(path)) path = DatabaseFilePath;
            try
            {
                if (!File.Exists(path))
                {
                    return [];
                }

                string json = await File.ReadAllTextAsync(path);
                List<ApplicationInfoDto>? appInfoDtos = JsonSerializer.Deserialize<List<ApplicationInfoDto>>(json);
                if (appInfoDtos is not null) { return ApplicationInfoDto.ToInfoList(appInfoDtos); }

                return [];
            }
            catch
            {
                return [];
            }
        }

        public async Task<UserSettings?> LoadAsyncUserSettings(string? path = null)
        {
            if (string.IsNullOrEmpty(path)) path = SettingsFileFolder + SettingsFileName;
            try
            {
                if (!File.Exists(path))
                {
                    return null;
                }

                string json = await File.ReadAllTextAsync(path);
                UserSettings? userSettingsDto = JsonSerializer.Deserialize<UserSettings>(json);
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Load many file included in the Date range
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        /// <exception cref="GetManyFilesException"></exception>
        public async Task<List<ApplicationInfo>> LoadManyAsync(DateOnly from, DateOnly to)
        {
            List<ApplicationInfo> appInfos = new();
            try
            {
                foreach (string file in Directory.GetFiles(DatabaseFileFolder))
                {
                    DateOnly creationTime = DateOnly.FromDateTime(File.GetCreationTime(file));
                    if (creationTime < from || creationTime > to)
                    {
                        continue;
                    }
                    List<ApplicationInfo> tempAppInfos = await LoadAsync(file);

                    foreach (ApplicationInfo tempApp in tempAppInfos)
                    {
                        ApplicationInfo? exist = appInfos.FirstOrDefault(d => d.Match(tempApp));
                        if (exist is null)
                        {
                            appInfos.Add(tempApp);
                        }
                        else
                        {
                            exist.UpdateTime(tempApp.TimeElapsed);
                        }
                    }
                }
                return appInfos;
            }
            catch (Exception ex)
            {
                throw new GetManyFilesException(ex.Message);
            }
        }

        /// <summary>
        /// Delete file olderThan the given days (default 30)
        /// </summary>
        /// <param name="olderThan"></param>
        public void DeleteOldFile(int olderThan = 30)
        {
            try
            {
                // Get the current date
                DateTime currentDate = DateTime.Now;

                string[] files = Directory.GetFiles(DatabaseFileFolder);

                foreach (string file in files)
                {
                    if (!file.Contains(DatabaseFileName))
                    {
                        continue;
                    }
                    DateTime creationTime = File.GetCreationTime(file);

                    TimeSpan fileAge = currentDate - creationTime;

                    // Check if the file is older than 30 days
                    if (fileAge.TotalDays > olderThan)
                    {
                        // Delete the file
                        File.Delete(file);
                        Console.WriteLine($"Deleted {file}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }


    }
}
