using System;
using System.Collections.Generic;
using System.Linq;
using Assignment4.Core;
using Assignment4;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;
using static Assignment4.Core.State;
using static Assignment4.Core.Response;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests : IDisposable
    {
        private readonly KanbanContext _context;
        private readonly TaskRepository _repo;
        public TaskRepositoryTests()
        {
            _context = new DbContextFactory().CreateDbContext(); // Will be changed to In Mem Context.
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
            var result = _repo.Create(workOnWorkTask);

            // Assert
            Assert.Equal(Created, result.Item1);
        }

        [Fact]
        public void TaskCreate_ReturnsId4()
        {

        }

        [Fact]
        public void TaskDelete_ReturnsCountOf2()
        {

        }

        [Fact]
        public void TaskFindById_ReturnsTaskTitleImplement_FromTaskId1()
        {

        }



        // public void Update(TaskDTO task)
        // {
        // }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
