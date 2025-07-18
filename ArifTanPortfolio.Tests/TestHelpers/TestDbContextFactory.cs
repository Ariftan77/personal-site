using Microsoft.EntityFrameworkCore;
using ArifTanPortfolio.Data;

namespace ArifTanPortfolio.Tests.TestHelpers
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext CreateInMemoryDbContext(string databaseName = "TestDb")
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            return new ApplicationDbContext(options);
        }

        public static ApplicationDbContext CreateInMemoryDbContextWithData(string databaseName = "TestDb")
        {
            var context = CreateInMemoryDbContext(databaseName);
            
            // Add test data
            context.Skills.AddRange(TestDataBuilder.CreateTestSkills());
            context.Projects.AddRange(TestDataBuilder.CreateTestProjects());
            context.BlogPosts.AddRange(TestDataBuilder.CreateTestBlogPosts());
            
            context.SaveChanges();
            return context;
        }
    }
}