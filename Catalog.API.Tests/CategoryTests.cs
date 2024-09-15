using Catalog.API.Categories.CreateCategory;
using GameVault.Common.Interfaces.Helpers;
using Catalog.API.Categories.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Catalog.API.Models;
using Catalog.API.Tests.FakeData;
using Catalog.API.Categories.GetAllCategories;

namespace Catalog.API.Tests
{
    public class CategoryTests
    {
        private readonly Mock<DbSet<Category>> _categorySetMock;
        private readonly Mock<ApplicationDbContext> _contextMock;
        private readonly Mock<IImageHandler> _imageHandlerMock;

        public CategoryTests()
        {
            _categorySetMock = new Mock<DbSet<Category>>();
            _contextMock = new Mock<ApplicationDbContext>();
            _imageHandlerMock = new Mock<IImageHandler>();
        }

        [Fact]
        public async Task CreateCategory_Handler_Should_ThrowCategoryAlreadyExistsException_WhenNameIsNotUnique()
        {
            // Arrange
            var command = new CreateCategoryCommand("Consoles", "Test", null, true, null);
            SetupMock("Consoles");


            var handler = new CreateCategoryHandler(
                _contextMock.Object,
                _imageHandlerMock.Object);

            // Assert
            await Assert.ThrowsAsync<CategoryExceptions.CategoryAlreadyExistsException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task CreateCategory_Handler_Should_ReturnCategoryGuid_WhenNameIsUnique()
        {
            // Arrange
            var command = new CreateCategoryCommand("Games", "Test", null, true, null);

            SetupMock("Consoles");

            var handler = new CreateCategoryHandler(
                _contextMock.Object,
                _imageHandlerMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.True(result?.Id != null);
        }

        [Fact]
        public async Task CreateCategory_Handler_Should_AddCategoryToDatabase()
        {
            // Arrange
            var command = new CreateCategoryCommand("Games", "Test", null, true, null);

            var handler = new CreateCategoryHandler(
                _contextMock.Object,
                _imageHandlerMock.Object);

            SetupMock("Consoles");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            _categorySetMock.Verify(m => m.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once());
            _contextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        private void SetupMock(string name = "", bool isFakeData = false, int count = 0)
        {
            IQueryable<Category> existingCategories = null!;
            if (isFakeData)
            {
                var fakeData = CategoriesGenerator.GetFakeCategories(count);
                existingCategories = fakeData.AsQueryable();
            } 
            else
            {
                existingCategories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = name, Description = "Just Description",  } // This name will trigger the exception
                }.AsQueryable();
            }
            

            _categorySetMock.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(existingCategories.Provider);
            _categorySetMock.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(existingCategories.Expression);
            _categorySetMock.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(existingCategories.ElementType);
            _categorySetMock.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(existingCategories.GetEnumerator());
            _contextMock.Setup(c => c.Set<Category>()).Returns(_categorySetMock.Object);
        }
    }
}
