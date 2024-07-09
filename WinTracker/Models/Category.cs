namespace WinTracker.Models
{
    public class Category
    {
        public static List<Category> Categories { get; set; } = Default();
        private Guid Guid { get; set; }
        public string Name { get; private set; }

        private Category(string categoryName)
        {
            this.Guid = Guid.NewGuid();
            this.Name = categoryName;
        }

        private static List<Category> Default()
        {
            return new List<Category>()
            {
                new("Office"),
                new("Windows"),
                new("VideoGames"),
                new("Chatting"),
                new("Developpement"),
                new("Browser")
            };
        }

        /// <summary>
        /// The Category can be deleted if it's not one by default
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        private static bool IsRemovable(string categoryName)
        {
            return !Default().Any(c => c.Name == categoryName); 
        }


        /// <summary>
        /// Add a new category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public static bool AddCategory(string categoryName)
        {
            if(Categories.Any(c => string.Equals(c.Name,categoryName,StringComparison.OrdinalIgnoreCase))) return false; // False if name already exists.
            Categories.Add(new(categoryName));
            return true;
        }

        /// <summary>
        /// Remove a Category if it's not one created by default
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public static bool RemoveCategory(string categoryName)
        {
            if(!IsRemovable(categoryName)) return false;  
            var category = Categories.FirstOrDefault(c => c.Name == categoryName);
            if (category != null)
            {
                Categories.Remove(category);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the CategoryByName
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public static Category? GetCategoryByName(string categoryName)
        {
            return Categories.FirstOrDefault(c => string.Equals(c.Name, categoryName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
