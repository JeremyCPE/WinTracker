using WinTracker.Models;

namespace WinTracker.Dtos
{
    public class CategoryDto
    {
        public string Name { get; }

        public CategoryDto(string name)
        {
            Name = name;
        }

        public static CategoryDto From(Category category)
        {
            return new(category.Name);
        }

        public Category To()
        {
            return new(Name);
        }
    }
}