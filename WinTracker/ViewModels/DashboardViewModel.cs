using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.VisualElements;
using WinTracker.Utils;

namespace WinTracker.ViewModels
{
    public class DashboardViewModel
    {

        private static int _index = 0;
        private static string[] _names = new[] { "Maria", "Susan", "Charles", "Fiona", "George" };

        public IEnumerable<ISeries> Series { get; set; }
        public LabelVisual Title { get; set; } =
            new LabelVisual
            {
                Text = "Time Used",
            };

        public DashboardViewModel()
        {
            IEnumerable<int> totalSeconds = TrackingServices._applicationInfos.AsEnumerable().Select(x => (int)x.TimeElapsed.TotalMinutes);
            IEnumerable<string> processNames = TrackingServices._applicationInfos.AsEnumerable().Select(x => x.ProcessInfo.ProcessName);

            /*
            Series =

            new PieSeries((totalSeconds)
            totalSeconds.AsPieSeries((value, series) =>
            {
                series.Values = result.Tuple.item2;
                series.Name = _names[_index++ % _names.Length];
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
            });
            */

        }
    }
}
