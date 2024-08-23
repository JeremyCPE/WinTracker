using WinTracker.Models;

namespace WinTracker.Dtos
{
    public class CategoryDto
    {
        public string Name { get; private set; } = "Others";
        public static CategoryDto From(Category category)
        {
            return new() { Name = category.Name ?? "Others" };
        }

        public Category To()
        {
            return new(Name);
        }
    }
}