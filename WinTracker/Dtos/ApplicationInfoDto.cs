﻿using WinTracker.Models;

namespace WinTracker.Dtos
{
    public class ApplicationInfoDto
    {
        public Guid Guid { get; }
        public DateOnly DateOnly { get; }

        public ProcessInfo ProcessInfo { get; }


        public string FileName { get; }

        public CategoryDto CategoryDto { get; }

        public TimeSpan TimeElapsed { get; }

        public ApplicationInfoDto(Guid guid, DateOnly dateOnly, ProcessInfo processInfo, string fileName, CategoryDto categoryDto, TimeSpan timeElapsed)
        {
            Guid = guid;
            DateOnly = dateOnly;
            ProcessInfo = processInfo;
            FileName = fileName;
            CategoryDto = categoryDto;
            TimeElapsed = timeElapsed;
        }

        public static List<ApplicationInfo> ToInfoList(List<ApplicationInfoDto> appInfoDtos)
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
