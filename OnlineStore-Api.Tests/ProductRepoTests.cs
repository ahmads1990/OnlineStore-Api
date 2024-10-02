using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OnlineStore_Api.Models;
using OnlineStore_Api.Repositories;
using OnlineStore_Api.Repositories.Interfaces;

namespace OnlineStore_Api.Tests
{
    public class ProductRepoTests
    {
        private AppDbContext appDbContext;
        private IProductRepo productRepo;
        private SqliteConnection connection;
        private static int productSeedDataCount = 5;
        private static int categorySeedDataCount = 3;
        private static int nonExistingId = 1000;

        public IEnumerable<Product> GetProductSeedData()
        {
            return new List<Product>()
            {
                new Product { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.99f, CategoryId = 1 },
                new Product { Id = 2, Name = "Product2", Description = "Desc2", Price = 20.99f, CategoryId = 1 },
                new Product { Id = 3, Name = "Product3", Description = "Desc3", Price = 30.99f, CategoryId = 2 },
                new Product { Id = 4, Name = "Product4", Description = "Desc4", Price = 40.99f, CategoryId = 2 },
                new Product { Id = 5, Name = "Product5", Description = "Desc5", Price = 50.99f, CategoryId = 3 }
            };
        }

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
                context.Products.AddRange(GetProductSeedData());
                context.SaveChanges();
            }

            appDbContext = new AppDbContext(options);
            productRepo = new ProductRepo(appDbContext);
        }

        [TearDown]
        public void TearDown()
        {
            appDbContext.Dispose();
            connection.Close();
        }

        [Test]
        public async Task GetAllProductsAsync_NoLimit_ReturnsAllProducts()
        {
            var result = await productRepo.GetAllProductsAsync(null);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(productSeedDataCount));
        }

        [Test]
        public async Task GetAllProductsAsync_WithLimit_ReturnsLimitedProducts()
        {
            int limit = 3;
            var result = await productRepo.GetAllProductsAsync(limit);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(limit));
        }

        //[Test]
        //public async Task GetAllProductsAsync_ZeroLimit_ReturnsEmptyList()
        //{
        //    var result = await productRepo.GetAllProductsAsync(0);

        //    Assert.That(result, Is.Empty);
        //}

        [Test]
        public async Task GetAllProductsAsync_NegativeLimit_ReturnsAllProducts()
        {
            var result = await productRepo.GetAllProductsAsync(-1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(productSeedDataCount));
        }

        [Test]
        public async Task GetFullProductByIDAsync_ValidId_ReturnsProductWithCategory()
        {
            int testId = 1;
            var result = await productRepo.GetFullProductByIDAsync(testId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(testId));
            Assert.That(result.Category, Is.Not.Null);
            Assert.That(result.Category.CategoryID, Is.EqualTo(result.CategoryId));
        }

        [Test]
        public async Task GetFullProductByIDAsync_InvalidId_ReturnsNull()
        {
            var result = await productRepo.GetFullProductByIDAsync(nonExistingId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AddNewProductAsync_ValidProduct_ReturnsNewProduct()
        {
            var newProduct = new Product { Name = "NewProduct", Description = "NewDesc", Price = 60.99f, CategoryId = 1 };

            var result = await productRepo.AddNewProductAsync(newProduct);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(newProduct.Name));
            Assert.That(result.Id, Is.GreaterThan(0));
        }

        [Test]
        public void AddNewProductAsync_NullProduct_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => productRepo.AddNewProductAsync(null));
        }

        [Test]
        public async Task UpdateProductAsync_ValidProduct_ReturnsUpdatedProduct()
        {
            var productToUpdate = GetProductSeedData().First();
            productToUpdate.Name = "UpdatedProduct";

            var result = await productRepo.UpdateProductAsync(productToUpdate);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("UpdatedProduct"));
        }

        //[Test]
        //public void UpdateProductAsync_NullProduct_ThrowsException()
        //{
        //    Assert.ThrowsAsync<ArgumentNullException>(() => productRepo.UpdateProductAsync(null));
        //}

        //[Test]
        //public async Task UpdateProductAsync_NonExistingProduct_ReturnsNull()
        //{
        //    var nonExistingProduct = new Product { Id = nonExistingId, Name = "NonExisting", CategoryId = 1 };

        //    var result = await productRepo.UpdateProductAsync(nonExistingProduct);

        //    Assert.That(result, Is.Null);
        //}

        [Test]
        public async Task DeleteProductAsync_ExistingProduct_ReturnsTrue()
        {
            int productIdToDelete = 1;

            var result = await productRepo.DeleteProductAsync(productIdToDelete);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteProductAsync_NonExistingProduct_ReturnsNull()
        {
            var result = await productRepo.DeleteProductAsync(nonExistingId);

            Assert.That(result, Is.Null);
        }

        //[Test]
        //public async Task DeleteProductAsync_NegativeId_ReturnsFalse()
        //{
        //    var result = await productRepo.DeleteProductAsync(-1);

        //    Assert.That(result, Is.False);
        //}
    }
}