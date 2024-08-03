using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using WinTracker.Utils;

namespace WinTracker.ViewModels
{
    public class DashboardViewModel
    {

        private static int _index = 0;

        public IEnumerable<ISeries> Series { get; set; }
        public LabelVisual Title { get; set; } =
            new LabelVisual
            {
                Text = "Time Used",
            };

        public DashboardViewModel()
        {
            IEnumerable<double> totalSeconds = TrackingServices._applicationInfos.AsEnumerable().Select(x => (double)x.TimeElapsed.TotalSeconds);
            string[] processNames = TrackingServices._applicationInfos.AsEnumerable().Select(x => x.ProcessInfo.ProcessName).ToArray();


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
