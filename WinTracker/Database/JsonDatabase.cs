using System.IO;
using System.Text.Json;
using WinTracker.Dtos;
using WinTracker.Models;

namespace WinTracker.Database
{
    public class JsonDatabase
    {
        private static readonly string DatabaseFilePath = "database.json";


        public static void Save(List<ApplicationInfoDto> applicationInfoDto)
        {
            try
            {
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
            try
            {
                if (!File.Exists(DatabaseFilePath))
                {
                    return new List<ApplicationInfo>();
                }

                string json = File.ReadAllText(DatabaseFilePath);
                List<ApplicationInfoDto>? appInfoDtos = JsonSerializer.Deserialize<List<ApplicationInfoDto>>(json);
                if (appInfoDtos is not null) { return ApplicationInfoDto.ToInfoList(appInfoDtos); }

                return new List<ApplicationInfo>();
            }
            catch
            {
                return new List<ApplicationInfo>();
            }
        }


    }
}
