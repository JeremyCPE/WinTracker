using WinTracker.Models;

namespace WinTracker.Dtos
{
    public class ApplicationInfoDto
    {
        public Guid Guid { get; private set; }
        public DateOnly DateOnly { get; private set; }

        public ProcessInfo ProcessInfo { get; private set; }


        public string FileName { get; private set; }

        public CategoryDto CategoryDto { get; private set; }

        public TimeSpan TimeElapsed { get; private set; }

        public ApplicationInfoDto(Guid guid, DateOnly dateOnly, ProcessInfo processInfo, string fileName, CategoryDto categoryDto, TimeSpan timeElapsed)
        {
            Guid = guid;
            DateOnly = dateOnly;
            ProcessInfo = processInfo;
            FileName = fileName;
            CategoryDto = categoryDto;
            TimeElapsed = timeElapsed;
        }

        internal static List<ApplicationInfo> ToInfoList(List<ApplicationInfoDto> appInfoDtos)
        {
            List<ApplicationInfo> appInfos = new();
            appInfoDtos.ForEach(appInfoDto => appInfos.Add(To(appInfoDto)));
            return appInfos;
        }

        private static ApplicationInfo To(ApplicationInfoDto appInfoDto)
        {
            return new ApplicationInfo
                (appInfoDto.ProcessInfo, appInfoDto.CategoryDto.To(), appInfoDto.Guid, appInfoDto.DateOnly, appInfoDto.TimeElapsed, appInfoDto.FileName);

        }
    }
}
