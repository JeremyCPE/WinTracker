using WinTracker.Models;
using Xunit;
namespace WinTracker.Tests.Models
{
    public class CategoryTests
    {
        [Fact]
        public void RemoveCategory_CategoryExistsAndIsRemovable_ReturnsTrue()
        {
            // Arrange
            string categoryName = "TestCategory";

            Category.AddCategory(categoryName);

            // Act
            bool result = Category.RemoveCategory(categoryName);

            // Assert
            Assert.True(result);
            Assert.Null(Category.GetCategoryByName(categoryName));
        }

        [Fact]
        public void RemoveCategory_CategoryDoesNotExist_ReturnsFalse()
        {
            // Arrange
            string categoryName = "NonExistentCategory";

            // Act
            bool result = Category.RemoveCategory(categoryName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveCategory_CategoryNotRemovable_ReturnsFalse()
        {
            // Arrange
            string categoryName = "Windows";
            Category.AddCategory(categoryName);

            // Act
            bool result = Category.RemoveCategory(categoryName);

            // Assert
            Assert.False(result);
            Assert.NotNull(Category.GetCategoryByName(categoryName));
        }

        [Fact]
        public void RemoveCategory_EmptyCategoryList_ReturnsFalse()
        {
            // Arrange
            string categoryName = "AnyCategory";

            // Act
            bool result = Category.RemoveCategory(categoryName);

            // Assert
            Assert.False(result);
        }
    }
}