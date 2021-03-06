using System;
using System.IO;
using Assignment4.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using static Assignment4.Core.State;

namespace Assignment4
{
    public class DbContextFactory : IDesignTimeDbContextFactory<KanbanContext>
    {
        public KanbanContext CreateDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>()
                .Build();

            var connectionString = configuration.GetConnectionString("kanban");
            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseSqlServer(connectionString);
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Assignment4"));

            return new KanbanContext(optionsBuilder.Options);
        }

        public KanbanContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }

        public static void Seed(KanbanContext context)
        {
            context.Database.ExecuteSqlRaw("DELETE dbo.Tasks"); // Remove existing. WARNING: This Should not be run in production environment.
            context.Database.ExecuteSqlRaw("DELETE dbo.Tags"); // Remove existing. WARNING: This Should not be run in production environment.
            context.Database.ExecuteSqlRaw("DELETE dbo.Users"); // Remove existing. WARNING: This Should not be run in production environment.
            
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tasks', RESEED, 0)"); // Reset Index to 0.
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tags', RESEED, 0)"); // Reset Index to 0.
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Users', RESEED, 0)"); // Reset Index to 0.

            DataFiller(context);
        }

        public static void DataFiller(KanbanContext context)
        {
            var taskStateNew = new Task {Title = "Implement ITaskRepository", Description = "SomeThing", State = New, Tags = new List<Tag>{}};
            var taskStateActive = new Task {Title = "Make UML", Description = "An Active", Created = new DateTime(2021, 10, 7), State = Active, StateUpdated = new DateTime(2021, 10 , 10), Tags = new List<Tag>{}};
            var taskStateResolved = new Task {Title = "Cookies", Description = "We Did it", State = Resolved, Tags = new List<Tag>{}};
            var taskStateRemoved = new Task {Title = "Clean up Stuff", Description = "These are not the rubbish you are looking for", State = Removed, Tags = new List<Tag>{}};

            var documentation = new Tag {Name = "Documentation", Tasks = new List<Task>{}};
            var coding = new Tag {Name = "Coding", Tasks = new List<Task>{}};
            var other = new Tag {Name = "Other", Tasks = new List<Task>{}};
            var removed = new Tag {Name = "Removed", Tasks = new List<Task>{}};

            documentation.Tasks.Add(taskStateActive);
            coding.Tasks.Add(taskStateNew);
            other.Tasks.Add(taskStateResolved);
            removed.Tasks.Add(taskStateRemoved);

            taskStateNew.Tags.Add(coding);
            taskStateActive.Tags.Add(documentation);
            taskStateResolved.Tags.Add(other);
            taskStateRemoved.Tags.Add(removed);


            var mads = new User {Name = "Mads", Email = "mads@itu.dk", Tasks = new List<Task>{ taskStateNew}};
            var kristian = new User {Name = "Kristian", Email = "kris@itu.dk", Tasks = new List<Task>{ taskStateActive}};
            var anton = new User {Name = "Anton", Email = "anto@itu.dk", Tasks = new List<Task>{ taskStateResolved}};
            var codemonkey = new User {Name = "Code Monkey", Email = "Code@Monkey.dk", Tasks = new List<Task>{ taskStateRemoved}};

            taskStateNew.AssignedTo = mads;
            taskStateActive.AssignedTo = kristian;
            taskStateResolved.AssignedTo = anton;
            taskStateRemoved.AssignedTo = codemonkey;

            context.Users.AddRange(
                mads,
                kristian,
                anton,
                codemonkey
            );

            context.Tags.AddRange(
                documentation,
                coding,
                other,
                removed
            );

            context.Tasks.AddRange(
                taskStateNew,
                taskStateActive,
                taskStateResolved,
                taskStateRemoved
            );

            context.SaveChanges();
        }
    }
}