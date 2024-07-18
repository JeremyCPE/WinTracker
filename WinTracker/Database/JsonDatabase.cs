using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WinTracker.Dtos;
using WinTracker.Models;

namespace WinTracker.Database
{
    public class JsonDatabase
    {
        private static readonly string DatabaseFilePath = "database.json";


        public static async Task Save(ApplicationInfoDto applicationInfoDto)
        {
            var json = JsonSerializer.Serialize(applicationInfoDto);
            await File.AppendAllTextAsync(DatabaseFilePath, json).ConfigureAwait(false);
        }

        public static List<ApplicationInfo> Load()
        {
            if (!File.Exists(DatabaseFilePath))
            {
                return new List<ApplicationInfo>();
            }

            var json = File.ReadAllText(DatabaseFilePath);
            var appInfoDtos = JsonSerializer.Deserialize<List<ApplicationInfoDto>>(json);
            if(appInfoDtos is not null) { return ApplicationInfoDto.ToInfoList(appInfoDtos); }

            return new List<ApplicationInfo>();
        }


    }
}
