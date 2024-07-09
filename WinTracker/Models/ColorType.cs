using System.ComponentModel;
using System.Drawing;

namespace WinTracker.Models
{
    public static class ColorType
    {
        public static Color Color { get; set; }

        public static Color DefaultColor { get; set; } = Color.Black;
        public static IDictionary<Category, Color> ColorList { get; set; } = new Dictionary<Category, Color>();

        public static void AssignDefault()
        {
            ColorList.Clear();
            ColorList.Add(Category.Office, Color.Black);
            ColorList.Add(Category.Windows, Color.Blue);
            ColorList.Add(Category.VideoGames, Color.Red);
            ColorList.Add(Category.Chatting, Color.Green);
            ColorList.Add(Category.Developpement, Color.Purple);
            ColorList.Add(Category.Browser, Color.Orange);
        }

        public static void AssignColor(Category category, Color color)
        {
            ColorList[category] = color;
        }

        public static Color GetColor(Category category)
        {
            return ColorList.TryGetValue(category, out var color) ? color : DefaultColor;
        }
    }
}
