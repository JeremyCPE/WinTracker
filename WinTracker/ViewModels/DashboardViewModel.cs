using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace WinTracker.ViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<ISeries> Series { get; set; } = new[] { 2, 4, 1, 4, 3 }.AsPieSeries();

        public LabelVisual Title { get; set; } =
            new LabelVisual
            {
                Text = "Time Used",
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };
    }
}
