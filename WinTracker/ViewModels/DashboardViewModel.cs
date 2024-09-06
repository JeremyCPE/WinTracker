using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System.Windows.Input;
using WinTracker.Database;
using WinTracker.Models;
using WinTracker.Utils;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace WinTracker.ViewModels
{
    public partial class DashboardViewModel : ViewModel
    {

        private static int _index = 0;

        [ObservableProperty]
        private DateTime _toDate;

        [ObservableProperty]
        private DateTime _fromDate;

        [ObservableProperty]
        private IEnumerable<ISeries> _series;

        private IDatabaseConnection _databaseConnection;
        private ITrackingService _trackingService;

        public ICommand ApplyDateCommand { get; set; }

        public LabelVisual Title { get; set; } =
            new LabelVisual
            {
                Text = "Time Used",
            };

        public DashboardViewModel(IDatabaseConnection database, ITrackingService tracking)
        {
            _databaseConnection = database;
            _trackingService = tracking;

            ApplyDateCommand = new RelayCommand(ApplyDateOnClick);

            FromDate = DateTime.Today;
            ToDate = DateTime.Today;

            Load();


        }

        private async void Load()
        {
            ApplicationInfos appInfos = await _trackingService.LoadAsync();
            IEnumerable<double> totalSeconds = appInfos.List.AsEnumerable().Select(x => (double)x.TimeElapsed.TotalSeconds);
            string[] processNames = appInfos.List.AsEnumerable().Select(x => x.ProcessInfo.ProcessName).ToArray();

            LoadSeries(totalSeconds, processNames);
        }

        public async void ApplyDateOnClick()
        {
            List<ApplicationInfo> appInfos = await _databaseConnection.LoadManyAsync(DateOnly.FromDateTime(FromDate), DateOnly.FromDateTime(ToDate));

            IEnumerable<double> totalSeconds = appInfos.AsEnumerable().Select(x => (double)x.TimeElapsed.TotalSeconds);
            string[] processNames = appInfos.AsEnumerable().Select(x => x.ProcessInfo.ProcessName).ToArray();

            LoadSeries(totalSeconds, processNames);

        }
        private void LoadSeries(IEnumerable<double> totalSeconds, string[] processNames)
        {
            Series =

            totalSeconds.AsPieSeries((value, series) =>
            {
                series.Name = processNames[_index++ % totalSeconds.Count()];
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
            });
        }
    }
}
