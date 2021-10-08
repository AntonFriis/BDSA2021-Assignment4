using System;
using System.Linq;
using Assignment4.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using static Assignment4.Core.Response;

namespace Assignment4.Entities.Tests
{
    public class TagRepositoryTests : IDisposable
    {
        private readonly KanbanContext _context;
        private readonly TagRepository _repo;
        public TagRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();
            
            _context = context;
            DbContextFactory.DataFiller(_context);
            
            _repo = new TagRepository(_context);
        }

        [Fact]
        public void TagCreate_Returns_CreatedResponse()
        {
            // Arrange
            var testTag = new TagCreateDTO
            {
                Name = "HumanlyPossible"
            };
            
            // Act
            var result = _repo.Create(testTag);
            
            // Assert
            Assert.Equal(Created, result.Item1);
        }

        [Fact]
        public void TagCreate_Returns_Table_Length_4()
        {
            // Arrange
            var testTag = new TagCreateDTO
            {
                Name = "HumanlyPossible"
            };
            
            // Act
            var exp = _context.Tags.Count();
            var result = _repo.Create(testTag);
            
            // Assert
            Assert.Equal(exp+1, _context.Tags.Count());
        }
        
        [Fact]
        public void TagCreate_Returns_Id4()
        {
            // Arrange
            var testTag = new TagCreateDTO
            {
                Name = "HumanlyPossible"
            };
            
            // Act
            var result = _repo.Create(testTag);

            // Assert
            Assert.Equal(4, result.Item2);
        }
        
        [Fact]
        public void TagCreate_Returns_Conflict()
        {
            // Arrange
            var testTag = new TagCreateDTO
            {
                Name = "Coding"
            };
            
            // Act
            var result = _repo.Create(testTag);

            // Assert
            Assert.Equal(Conflict, result.Item1);
        }
        
        [Fact]
        public void TagRead_Returns_TagDTO_Coding()
        {
            // Arrange
            var actual = _repo.Read(1);

            // Assert
            Assert.Equal(new TagDTO(1, "Coding"), actual);
        }
        

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
