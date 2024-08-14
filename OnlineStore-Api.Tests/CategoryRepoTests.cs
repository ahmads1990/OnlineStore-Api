using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OnlineStore_Api.Models;
using OnlineStore_Api.Services;
using OnlineStore_Api.Services.Interfaces;

namespace OnlineStore_Api.Tests
{
    public class CategoryRepoTests
    {
        private AppDbContext appDbContext;
        private ICategoryRepo categoryRepo;
        private SqliteConnection connection;
        private static int categorySeedDataCount = 3;
        private static byte nonExistingId = 100;

        public IEnumerable<Category> GetCategorySeedData()
        {
            return new List<Category>()
            {
                new Category { CategoryID = 1, Title = "Category1", Description = "CategoryDesc1" },
                new Category { CategoryID = 2, Title = "Category2", Description = "CategoryDesc2" },
                new Category { CategoryID = 3, Title = "Category3", Description = "CategoryDesc3" }
            };
        }

        [SetUp]
        public void Setup()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseSqlite(connection)
               .Options;

            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Categories.AddRange(GetCategorySeedData());
                context.SaveChanges();
            }

            appDbContext = new AppDbContext(options);
            categoryRepo = new CategoryRepo(appDbContext);
        }

        [TearDown]
        public void TearDown()
        {
            appDbContext.Dispose();
            connection.Close();
        }

        [Test]
        public async Task GetAllCategoriesAsync_ReturnsAllCategories()
        {
            var result = await categoryRepo.GetAllCategoriesAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(categorySeedDataCount));
        }

        [Test]
        public async Task GetCategoryWithIDAsync_ValidId_ReturnsCategory()
        {
            int testId = 1;
            var result = await categoryRepo.GetCategoryWithIDAsync(testId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.CategoryID, Is.EqualTo(testId));
        }

        [Test]
        public async Task GetCategoryWithIDAsync_InvalidId_ReturnsNull()
        {
            var result = await categoryRepo.GetCategoryWithIDAsync(nonExistingId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCategoryWithNameAsync_ValidName_ReturnsCategory()
        {
            string testName = "Category1";
            var result = await categoryRepo.GetCategoryWithNameAsync(testName);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo(testName));
        }

        [Test]
        public async Task GetCategoryWithNameAsync_InvalidName_ReturnsNull()
        {
            var result = await categoryRepo.GetCategoryWithNameAsync("NonExistingCategory");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CheckCategoryExistAsync_ExistingCategory_ReturnsTrue()
        {
            int testId = 1;
            var result = await categoryRepo.CheckCategoryExistAsync(testId);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task CheckCategoryExistAsync_NonExistingCategory_ReturnsFalse()
        {
            var result = await categoryRepo.CheckCategoryExistAsync(nonExistingId);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task AddNewCategoryAsync_ValidCategory_ReturnsNewCategory()
        {
            var newCategory = new Category { Title = "NewCategory", Description = "NewCategoryDesc" };

            var result = await categoryRepo.AddNewCategoryAsync(newCategory);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo(newCategory.Title));
        }

        [Test]
        public void AddNewCategoryAsync_NullCategory_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => categoryRepo.AddNewCategoryAsync(null));
        }

        [Test]
        public async Task UpdateCategoryAsync_ValidCategory_ReturnsUpdatedCategory()
        {
            var categoryToUpdate = GetCategorySeedData().First();
            categoryToUpdate.Title = "UpdatedCategory";

            var result = await categoryRepo.UpdateCategoryAsync(categoryToUpdate);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("UpdatedCategory"));
        }
        [Test]
        public async Task DeleteCategoryAsync_ExistingCategory_ReturnsTrue()
        {
            int categoryIdToDelete = 1;

            var result = await categoryRepo.DeleteCategoryAsync(categoryIdToDelete);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteCategoryAsync_NonExistingCategory_ReturnsNull()
        {
            var result = await categoryRepo.DeleteCategoryAsync(nonExistingId);

            Assert.That(result, Is.Null);
        }
    }
}