using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System.ComponentModel;
using System.Windows.Input;
using WinTracker.Database;
using WinTracker.Models;
using WinTracker.Utils;

namespace WinTracker.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {

        private static int _index = 0;

        private DateTime _toDate;
        private DateTime _fromDate;
        private IEnumerable<ISeries> _series;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ApplyDateCommand { get; set; }

        public DateTime FromDate
        {
            get
            {
                return _fromDate;
            }
            set
            {
                _fromDate = value;
                OnPropertyChanged(nameof(FromDate));
            }
        }
        public DateTime ToDate
        {
            get { return _toDate; }

            set
            {
                _toDate = value;
                OnPropertyChanged(nameof(ToDate));
            }
        }

        public IEnumerable<ISeries> Series { get { return _series; } set { _series = value; OnPropertyChanged(nameof(Series)); } }
        public LabelVisual Title { get; set; } =
            new LabelVisual
            {
                Text = "Time Used",
            };

        public DashboardViewModel()
        {
            ApplyDateCommand = new RelayCommand(o => ApplyDateOnClick());

            IEnumerable<double> totalSeconds = TrackingServices._applicationInfos.AsEnumerable().Select(x => (double)x.TimeElapsed.TotalSeconds);
            string[] processNames = TrackingServices._applicationInfos.AsEnumerable().Select(x => x.ProcessInfo.ProcessName).ToArray();

            LoadSeries(totalSeconds, processNames);

            FromDate = DateTime.Today;
            ToDate = DateTime.Today;

        }
        public void ApplyDateOnClick()
        {
            List<ApplicationInfo> appInfos = JsonDatabase.LoadMany(DateOnly.FromDateTime(FromDate), DateOnly.FromDateTime(ToDate));

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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
