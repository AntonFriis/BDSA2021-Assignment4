using System;
using System.Collections.Generic;
using System.Linq;
using Assignment4.Core;
using Assignment4;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.Data.Sqlite;
using static Assignment4.Core.State;
using static Assignment4.Core.Response;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {

        private readonly KanbanContext _context;
        private readonly TaskRepository _repo;

        public TaskRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();

            _context = context;
            DbContextFactory.DataFiller(_context);

            _repo = new TaskRepository(_context);
        }

        [Fact]
        public void TaskCreate_Returns_Created()
        {
            // Arrange
            var workOnWorkTask = new TaskCreateDTO {
                Title = "workOnWorkTask",
                AssignedToId = 1,
                Description = "A Work Task",
                Tags = new List<string>() // 13-10-21 Something is not handled correctly on tags. will error on runtime.
            };

            // Act
            var result = _repo.Create(workOnWorkTask);

            // Assert
            Assert.Equal(Created, result.Item1);
        }

        [Fact]
        public void TaskCreate_WithNoUserSet_Returns_Created()
        {
                        // Arrange
            var workOnWorkTask = new TaskCreateDTO {
                Title = "workOnWorkTask",
                Description = "A Work Task",
                Tags = new List<string>() // 13-10-21 Something is not handled correctly on tags. will error on runtime.
            };                            // works when run normally not through test.

            // Act
            var result = _repo.Create(workOnWorkTask);

            // Assert
            Assert.Equal(Created, result.Item1);
        }

        [Fact]
        public void TaskReadAll_Returns_List_With4_Elements()
        {
            // Act
            var result = _repo.ReadAll();

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(4, result.Count);

        }

        [Fact]
        public void TaskReadAllRemoved_Returns_1Item()
        {
            // Act
            var result = _repo.ReadAllRemoved();

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TaskReadAllByTag_Returns1Item()
        {
            // Act
            var result = _repo.ReadAllByTag("Removed");

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(1, result.Count);

        }

        [Fact]
        public void TaskReadAllByUser_Returns1Item()
        {
            var result = _repo.ReadAllByUser(4);

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TaskReadAllByState_Returns1Item()
        {
            var result = _repo.ReadAllByState(Removed);

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TaskRead_ReturnsTaskDetailsDTO_From_Given_Id()
        {
            var result = _repo.Read(2);


            Assert.Equal(2,result.Id);
            Assert.Equal("Make UML", result.Title);
            Assert.Equal("An Active", result.Description);
            Assert.Equal(DateTime.ParseExact("07-10-2021", "dd-MM-yyyy", null), result.Created);
            Assert.Equal("Kristian", result.AssignedToName);
            Assert.Equal(new List<string> {"Documentation"}.AsReadOnly(), result.Tags);
            Assert.Equal(Active, result.State);
            Assert.Equal(DateTime.ParseExact("10-10-2021", "dd-MM-yyyy", null), result.StateUpdated);
        }

        [Fact]
        public void TaskUpdate_Returns_Updated()
        {
            var toUpdate = new TaskUpdateDTO
            {
                Id = 1,
                State = Resolved
            };

            var result = _repo.Update(toUpdate);

            Assert.Equal(Updated, result);
            Assert.Equal(Resolved, _context.Tasks.Find(1).State);
        }

        [Fact]
        public void TaskDelete_Returns_Deleted()
        {
            var result = _repo.Delete(1);

            Assert.Equal(Deleted, result);
        }
    }
}
