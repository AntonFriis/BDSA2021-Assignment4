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
    public class TaskRepositoryTests
    {
        TaskRepository _Repo = new TaskRepository(new DbContextFactory().CreateDbContext());



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
        public void TaskReadAll_Returns_List_With4_Elements()
        {
            // Act
            var result = _Repo.ReadAll();

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(4, result.Count);

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
            var result = _Repo.ReadAllByTag("Coding");

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(1, result.Count);

        }

        [Fact]
        public void TaskReadAllByUser_Returns1Item()
        {
            var result = _Repo.ReadAllByUser(1);

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TaskReadAllByState_Returns1Item()
        {
            var result = _Repo.ReadAllByState(New);

            // Assert // Should Deffinitely also test that Refs to User and List of Tags have been associated.
            Assert.Equal(1, result.Count);
        }
    }
}
