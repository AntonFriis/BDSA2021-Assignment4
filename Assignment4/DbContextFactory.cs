using System.IO;
using Assignment4.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Assignment4
{
    public class DbContextFactory : IDesignTimeDbContextFactory<KanbanContext>
    {
        public KanbanContext CreateDbContext(string[] args)
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

        public static void Seed(KanbanContext context)
        {
           context.Users.AddRange(
                new User {Name = "Mads", Email = "mads@itu.dk", Tasks = { }},
                new User {Name = "Kristian", Email = "kris@itu.dk", Tasks = { }},
                new User {Name = "Anton", Email = "anto@itu.dk", Tasks = { }}
            );
            context.Tags.Add(new Tag {Name = "Create", Tasks = { }});
            context.SaveChanges();
        }
    }
}