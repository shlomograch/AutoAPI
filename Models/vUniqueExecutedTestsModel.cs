namespace AutomationAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class vUniqueExecutedTestsModel : DbContext
    {
        public vUniqueExecutedTestsModel()
            : base("name=vUniqueExecutedTestsModel")
        {
        }

        public virtual DbSet<vUniqueExecutedTest> vUniqueExecutedTests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vUniqueExecutedTest>()
                .Property(e => e.Environment)
                .IsUnicode(false);

            modelBuilder.Entity<vUniqueExecutedTest>()
                .Property(e => e.TestName)
                .IsUnicode(false);

            modelBuilder.Entity<vUniqueExecutedTest>()
                .Property(e => e.ApplicationName)
                .IsUnicode(false);

            modelBuilder.Entity<vUniqueExecutedTest>()
                .Property(e => e.TesterName)
                .IsUnicode(false);

            modelBuilder.Entity<vUniqueExecutedTest>()
                .Property(e => e.Browser)
                .IsUnicode(false);

            modelBuilder.Entity<vUniqueExecutedTest>()
                .Property(e => e.TestRunStatus)
                .IsUnicode(false);
        }
    }
}
