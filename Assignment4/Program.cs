using System;
using Assignment4.Entities;

namespace Assignment4
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var context = new DbContextFactory().CreateDbContext();
            DbContextFactory.Seed(context);

           var ret = new TaskRepository(context).ReadAllByUser(1);
        }
    }
}
