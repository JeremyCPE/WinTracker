using WinTracker.Models;

namespace WinTracker.Dtos
{
    public class CategoryDto
    {
        public string Name { get; private set; } = "Others";
        internal static CategoryDto From(Category category)
        {
          return new() { Name = category.Name ?? "Others" };   
        }

        internal Category To()
        {
            return new(Name);
        }
    }
}