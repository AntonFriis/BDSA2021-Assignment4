using System;
using System.Linq;
using Assignment4.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using static Assignment4.Core.Response;

namespace Assignment4.Entities.Tests
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly KanbanContext _context;
        private readonly UserRepository _repo;
        public UserRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();
            
            _context = context;
            DbContextFactory.DataFiller(_context);
            
            _repo = new UserRepository(_context);
        }
        
        [Fact]
        public void UserCreate_Returns_CreatedResponse()
        {
            // Arrange
            var testUser = new UserCreateDTO()
            {
                Name = "Rasmus",
                Email = "rasm@itu.dk"
            };
            
            // Act
            var result = _repo.Create(testUser);
            
            // Assert
            Assert.Equal(Created, result.Item1);
        }

        [Fact]
        public void UserCreate_Returns_Table_Length_4()
        {
            // Arrange
            var testUser = new UserCreateDTO()
            {
                Name = "Rasmus",
                Email = "rasm@itu.dk"
            };
            var exp = _context.Users.Count();
            
            // Act
            _repo.Create(testUser);
            
            // Assert
            Assert.Equal(exp+1, _context.Users.Count());
        }
        
        [Fact]
        public void UserCreate_Returns_Id4()
        {
            // Arrange
            var testUser = new UserCreateDTO()
            {
                Name = "Rasmus",
                Email = "rasm@itu.dk"
            };
            
            // Act
            var result = _repo.Create(testUser);

            // Assert
            Assert.Equal(5, result.Item2);
        }
        
        [Fact]
        public void UserCreate_Returns_Conflict()
        {
            // Arrange
            var testUser = new UserCreateDTO()
            {
                Name = "Friis",
                Email = "anto@itu.dk"
            };
            
            // Act
            var result = _repo.Create(testUser);

            // Assert
            Assert.Equal(Conflict, result.Item1);
        }
        
        [Fact]
        public void UserRead_Returns_UserDTO_Mads()
        {
            // Arrange
            var actual = _repo.Read(1);
        
            // Assert
            Assert.Equal(new UserDTO(1, "Mads", "mads@itu.dk"), actual);
        }
        
        [Fact]
        public void UserRead_Returns_null()
        {
            // Arrange
            var actual = _repo.Read(_context.Users.Count()+1);
        
            // Assert
            Assert.Null(actual);
        }
        
        [Fact]
        public void UserReadAll_returns_all_users()
        {
            var users = _repo.ReadAll();
        
            Assert.Collection(users,
                u => Assert.Equal(new UserDTO(1, "Mads", "mads@itu.dk"), u),
                u => Assert.Equal(new UserDTO(2, "Kristian", "kris@itu.dk"), u),
                u => Assert.Equal(new UserDTO(3, "Anton", "anto@itu.dk"), u),
                u => Assert.Equal(new UserDTO(4, "Code Monkey", "Code@Monkey.dk"), u)
            );
        }
        
        [Fact]
        public void UserUpdate_given_non_existing_id_returns_NotFound()
        {
            var user = new UserUpdateDTO()
            {
                Id = 42,
                Name = "Markus",
                Email = "mark@itu.dk"
            };
        
            var updated = _repo.Update(user);
        
            Assert.Equal(NotFound, updated);
        }
        
        [Fact]
        public void UserUpdate_updates_existing_tag()
        {
            var tag = new UserUpdateDTO()
            {
                Id = 3,
                Name = "Friis",
                Email = "frii@itu.dk"
            };
        
            var updated = _repo.Update(tag);
        
            Assert.Equal(Updated, updated);
        
            var user = _repo.Read(3);
            Assert.Equal(3, user.Id);
            Assert.Equal("Friis", user.Name);
            Assert.Equal("frii@itu.dk", user.Email);
        }
        
        [Fact]
        public void UserDelete_given_non_existing_id_returns_NotFound()
        {
            var deleted = _repo.Delete(42);
        
            Assert.Equal(NotFound, deleted);
        }
        
        [Fact]
        public void UserDelete_given_existing_id_deletes_with_force()
        {
            var deleted = _repo.Delete(3, true);
        
            Assert.Equal(Deleted, deleted);
            Assert.Null(_context.Users.Find(3));
        }
        
        [Fact]
        public void UserDelete_given_existing_id_returns_conflict()
        {
            var deleted = _repo.Delete(3);
        
            Assert.Equal(Conflict, deleted);
            Assert.NotNull(_context.Users.Find(3));
        }
        
        

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
