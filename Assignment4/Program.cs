﻿using System;

namespace Assignment4
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var context = new DbContextFactory().CreateDbContext();
            DbContextFactory.Seed(context);
        }
    }
}
