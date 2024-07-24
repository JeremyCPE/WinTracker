using WinTracker.Models;

namespace WinTracker.Dtos
{
    public class ApplicationInfoDto
    {
        public Guid Guid { get; set; }
        public DateOnly DateOnly { get; set; }

        public ProcessInfo ProcessInfo { get; set; }


        public string FileName { get; set; }

        public CategoryDto CategoryDto { get; set; }

        public TimeSpan TimeElapsed { get; set; }



        internal static List<ApplicationInfo> ToInfoList(List<ApplicationInfoDto> appInfoDtos)
        {
            List<ApplicationInfo> appInfos = new();
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
                FileName = appInfoDto.FileName,
                State = State.Stopped,
            };

        }
    }
}
