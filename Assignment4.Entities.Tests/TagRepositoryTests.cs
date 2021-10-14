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
            var exp = _context.Tags.Count();
            
            // Act
            _repo.Create(testTag);
            
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
        
        [Fact]
        public void TagRead_Returns_null()
        {
            // Arrange
            var actual = _repo.Read(_context.Tags.Count()+1);

            // Assert
            Assert.Null(actual);
        }
        
        [Fact]
        public void TagReadAll_returns_all_tags()
        {
            var tags = _repo.ReadAll();

            Assert.Collection(tags,
                t => Assert.Equal(new TagDTO(1, "Coding"), t),
                t => Assert.Equal(new TagDTO(2, "Documentation"), t),
                t => Assert.Equal(new TagDTO(3, "Other"), t)
            );
        }
        
        [Fact]
        public void TagUpdate_given_non_existing_id_returns_NotFound()
        {
            var tag = new TagUpdateDTO()
            {
                Id = 42,
                Name = "Break",
            };

            var updated = _repo.Update(tag);

            Assert.Equal(NotFound, updated);
        }

        [Fact]
        public void TagUpdate_updates_existing_tag()
        {
            var tag = new TagUpdateDTO()
            {
                Id = 2,
                Name = "Documenting"
            };

            var updated = _repo.Update(tag);

            Assert.Equal(Updated, updated);

            var documenting = _repo.Read(2);
            Assert.Equal(2, documenting.Id);
            Assert.Equal("Documenting", documenting.Name);
        }
        
        [Fact]
        public void TagDelete_given_non_existing_id_returns_NotFound()
        {
            var deleted = _repo.Delete(42);

            Assert.Equal(NotFound, deleted);
        }

        [Fact]
        public void TagDelete_given_existing_id_deletes_with_force()
        {
            var deleted = _repo.Delete(3, true);

            Assert.Equal(Deleted, deleted);
            Assert.Null(_context.Tags.Find(3));
        }
        
        [Fact]
        public void TagDelete_given_existing_id_returns_conflict()
        {
            var deleted = _repo.Delete(3);

            Assert.Equal(Conflict, deleted);
            Assert.NotNull(_context.Tags.Find(3));
        }
        

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
