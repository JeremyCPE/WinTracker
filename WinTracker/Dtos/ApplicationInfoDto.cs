using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WinTracker.Models;

namespace WinTracker.Dtos
{
    public class ApplicationInfoDto
    {
        public Guid Guid { get; set; }
        public DateOnly DateOnly { get; set; }

        public ProcessInfo ProcessInfo { get; set; }

        public CategoryDto CategoryDto { get; set; }

        public TimeSpan TimeElapsed { get; set; }

        public ImageSource? Image { get; set; }


        internal static List<ApplicationInfo> ToInfoList(List<ApplicationInfoDto> appInfoDtos)
        {
            var appInfos = new List<ApplicationInfo>();
            appInfoDtos.ForEach(appInfoDto => appInfos.Add(To(appInfoDto)));
            return appInfos;
        }

        private static ApplicationInfo To(ApplicationInfoDto appInfoDto)
        {
            return new ApplicationInfo(appInfoDto.ProcessInfo, appInfoDto.CategoryDto.To())
            {
                Guid = appInfoDto.Guid,
                DateOnly = appInfoDto.DateOnly,
                TimeElapsed = appInfoDto.TimeElapsed,
                State = State.Stopped,
                Image = appInfoDto.Image
            };

        }
    }
}
