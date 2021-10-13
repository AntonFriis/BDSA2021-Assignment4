using System;
using System.Collections.Generic;
using System.Linq;
using Assignment4.Core;
using Assignment4;
using Microsoft.EntityFrameworkCore;
using Xunit;
//using Microsoft.EntityFrameworkCore.Sqlite;
//using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Data.Sqlite.Core;
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
        public void TaskCreate_Returns_Created_And_A_Number_Larger_Than_2()
        {
            // Arrange
            var workOnWorkTask = new TaskCreateDTO {
                Title = "workOnWorkTask",
                AssignedToId = 1,
                Description = "A Work Task",
                Tags = new List<string>{"coding"}
            };

            // Act
            var result = _Repo.Create(workOnWorkTask);

            // Assert
            Assert.Equal(Created, result.Item1);
        }

        [Fact]
        public void TaskReadAll_Returns_List_With6_Elements()
        {
            // Act
            var result = _Repo.ReadAll();

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(6, result.Count);

        }

        [Fact]
        public void TaskReadAllRemoved_Returns_1Item()
        {
            // Act
            var result = _Repo.ReadAllRemoved();

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TaskReadAllByTag_Returns1Item()
        {
            // Act
            var result = _Repo.ReadAllByTag("Removed");

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(1, result.Count);

        }

        [Fact]
        public void TaskReadAllByUser_Returns1Item()
        {
            var result = _Repo.ReadAllByUser(4);

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TaskReadAllByState_Returns1Item()
        {
            var result = _Repo.ReadAllByState(Removed);

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TaskRead_ReturnsTaskDetailsDTO_From_Given_Id()
        {
            var result = _Repo.Read(2);


            Assert.Equal(2,result.Id);
            Assert.Equal("Make UML", result.Title);
            Assert.Equal("An Active", result.Description);
            Assert.Equal(DateTime.ParseExact("07-10-2021", "dd-MM-yyyy", null), result.Created);
            Assert.Equal("Kristian", result.AssignedToName);
            Assert.Equal(new List<string> {"Documentation"}.AsReadOnly(), result.Tags);
            Assert.Equal(Active, result.State);
            Assert.Equal(DateTime.ParseExact("10-10-2021", "dd-MM-yyyy", null), result.StateUpdated);
        }
    }
}
