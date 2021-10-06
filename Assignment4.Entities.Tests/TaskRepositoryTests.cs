using System;
using System.Collections.Generic;
using System.Linq;
using Assignment4.Core;
using Assignment4;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;
using static Assignment4.Core.State;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {
        //KanbanContext context = new DbContextFactory().CreateDbContext();

        [Fact]
        public void TaskAll_Returns3Tasks()
        {
            using var context = new DbContextFactory().CreateDbContext();
            var tasks = new TaskRepository(context).All();
            Assert.Equal(3, tasks.Count);
        }
        
        [Fact]
        public void TaskCreate_ReturnsId4()
        {
            using var context = new DbContextFactory().CreateDbContext();
            var task = new TaskDTO {
                Title = "Test task",
                Description = "Task for testing",
                State = Closed};
            Assert.Equal(4, new TaskRepository(context).Create(task));
        }
        
        [Fact]
        public void TaskDelete_ReturnsCountOf2()
        {
            using var context = new DbContextFactory().CreateDbContext();
            new TaskRepository(context).Delete(2);
            Assert.Equal(2, context.Tasks.Count());
        }
        
        [Fact]
        public void TaskFindById_ReturnsTaskTitleImplement_FromTaskId1()
        {
            using var context = new DbContextFactory().CreateDbContext();
            Assert.Equal("Implement ITaskRepository", new TaskRepository(context).FindById(1).Title);
        }
        
        
        
        // public void Update(TaskDTO task)
        // {
        // }

    }
}
