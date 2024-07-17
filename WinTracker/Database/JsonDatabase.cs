using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WinTracker.Models;

namespace WinTracker.Database
{
    public class JsonDatabase
    {
        private static readonly string DatabaseFilePath = "database.json";


        public static async Task Save(ApplicationInfo applicationInfo)
        {
            var json = JsonSerializer.Serialize(applicationInfo);
            await File.AppendAllTextAsync(DatabaseFilePath, json).ConfigureAwait(false);
        }

        public List<ApplicationInfo> Load()
        {
            if (!File.Exists(DatabaseFilePath))
            {
                return new List<ApplicationInfo>();
            }

            var json = File.ReadAllText(DatabaseFilePath);
            var ret = JsonSerializer.Deserialize<List<ApplicationInfo>>(json);
            if(ret != null) { return ret; }

            return new List<ApplicationInfo>();
        }


    }
}
