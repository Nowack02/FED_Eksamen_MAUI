// ExamAppMAUI/ApplicationDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ExamAppMAUI.Data;
using System.IO;

namespace ExamAppMAUI
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var projectDirectory = Directory.GetCurrentDirectory();
            var databasePath = Path.Combine(projectDirectory, "Data", "ExamAppDesign.db"); // Et unikt navn for at undgå konflikter

            // Sørg for, at Data-mappen eksisterer
            Directory.CreateDirectory(Path.GetDirectoryName(databasePath));

            optionsBuilder.UseSqlite($"Filename={databasePath}");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}