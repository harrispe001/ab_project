using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ab_project.Data;
using ab_project.Models;

namespace ab_project.Models
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any existing categories
            if (context.Categories.Any())
            {
                return;   // DB has been seeded
            }

            var categories = new Category[]
            {
                new Category { Name = "Programming" },
                new Category { Name = "Data Science" },
                new Category { Name = "Web Development" }
            };
            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var trainings = new Training[]
            {
                new Training { Name = "C# Fundamentals", Author = "John Doe", CategoryId = categories[0].CategoryId },
                new Training { Name = "Python for Data Analysis", Author = "Jane Smith", CategoryId = categories[1].CategoryId },
                new Training { Name = "JavaScript Essentials", Author = "Bob Johnson", CategoryId = categories[2].CategoryId }
            };
            foreach (Training t in trainings)
            {
                context.Trainings.Add(t);
            }
            context.SaveChanges();
        }
    }
}
