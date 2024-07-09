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
            var categoryName = "TestCategory";
            
            Category.AddCategory(categoryName);

            // Act
            var result = Category.RemoveCategory(categoryName);

            // Assert
            Assert.True(result);
            Assert.Null(Category.GetCategoryByName(categoryName));
        }

        [Fact]
        public void RemoveCategory_CategoryDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var categoryName = "NonExistentCategory";

            // Act
            var result = Category.RemoveCategory(categoryName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveCategory_CategoryNotRemovable_ReturnsFalse()
        {
            // Arrange
            var categoryName = "Windows";
            Category.AddCategory(categoryName);

            // Act
            var result = Category.RemoveCategory(categoryName);

            // Assert
            Assert.False(result);
            Assert.NotNull(Category.GetCategoryByName(categoryName));
        }

        [Fact]
        public void RemoveCategory_EmptyCategoryList_ReturnsFalse()
        {
            // Arrange
            var categoryName = "AnyCategory";

            // Act
            var result = Category.RemoveCategory(categoryName);

            // Assert
            Assert.False(result);
        }
    }
}