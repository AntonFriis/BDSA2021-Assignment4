﻿using System;
using Assignment4.Entities;
using Assignment4.Core;
using System.Collections.Generic;

namespace Assignment4
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var context = new DbContextFactory().CreateDbContext();
            DbContextFactory.Seed(context);

            var workOnWorkTask = new TaskCreateDTO {
                Title = "workOnWorkTask",
                AssignedToId = 1,
                Description = "A Work Task",
                Tags = new List<string>{"coding"}
            };

            var ret = new TaskRepository(context).Create(workOnWorkTask);
        }
    }
}
