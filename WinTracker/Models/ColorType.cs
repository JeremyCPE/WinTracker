using System.Drawing;
using WinTracker.Utils;


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
            // Not supposed to be null but just to be sure....
            ColorList.AddIfNotNull(Category.GetCategoryByName("Office"), Color.Black);
            ColorList.AddIfNotNull(Category.GetCategoryByName("Windows"), Color.Blue);
            ColorList.AddIfNotNull(Category.GetCategoryByName("VideoGames"), Color.Red);
            ColorList.AddIfNotNull(Category.GetCategoryByName("Chatting"), Color.Green);
            ColorList.AddIfNotNull(Category.GetCategoryByName("Developpement"), Color.Purple);
            ColorList.AddIfNotNull(Category.GetCategoryByName("Browser"), Color.Orange);
        }

        public static void AssignColor(Category category, Color color)
        {
            ColorList[category] = color;
        }

        public static Color GetColor(Category category)
        {
            return ColorList.TryGetValue(category, out Color color) ? color : DefaultColor;
        }
    }
}
