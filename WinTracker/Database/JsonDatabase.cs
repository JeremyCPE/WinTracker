using System.IO;
using System.Text.Json;
using WinTracker.Dtos;
using WinTracker.Models;

namespace WinTracker.Database
{
    public class JsonDatabase
    {
        private static readonly string DatabaseFileFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + """\WinTracker\logs\""";
        private static readonly string DatabaseFileName = "database.json";
        private static readonly string DatabaseFilePath = DatabaseFileFolder + DateTime.Today.ToString("yyyyMMdd") + DatabaseFileName;


        public static void Save(List<ApplicationInfoDto> applicationInfoDto)
        {
            try
            {
                if (!Directory.Exists(DatabaseFileFolder)) Directory.CreateDirectory(DatabaseFileFolder);

                string json = JsonSerializer.Serialize(applicationInfoDto);
                File.WriteAllTextAsync(DatabaseFilePath, json);
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static List<ApplicationInfo> Load()
        {
            string path = DatabaseFilePath;
            try
            {
                if (!File.Exists(path))
                {
                    return new List<ApplicationInfo>();
                }

                string json = File.ReadAllText(path);
                List<ApplicationInfoDto>? appInfoDtos = JsonSerializer.Deserialize<List<ApplicationInfoDto>>(json);
                if (appInfoDtos is not null) { return ApplicationInfoDto.ToInfoList(appInfoDtos); }

                return new List<ApplicationInfo>();
            }
            catch
            {
                return new List<ApplicationInfo>();
            }
        }

        /// <summary>
        /// Delete file olderThan the given days (default 30)
        /// </summary>
        /// <param name="olderThan"></param>
        public static void DeleteOldFile(int olderThan = 30)
        {
            try
            {
                // Get the current date
                DateTime currentDate = DateTime.Now;

                // Get all files in the log folder
                string[] files = Directory.GetFiles(DatabaseFileFolder);

                foreach (string file in files)
                {
                    if (!file.Contains(DatabaseFileName))
                    {
                        continue;
                    }
                    // Get the creation time of the file
                    DateTime creationTime = File.GetCreationTime(file);

                    // Calculate the age of the file
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
