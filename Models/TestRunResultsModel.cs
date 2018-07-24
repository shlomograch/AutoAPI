namespace AutomationAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TestRunResultsModel : DbContext
    {
        public TestRunResultsModel()
            : base("name=TestRunResultsModel")
        {
        }

        public virtual DbSet<TestRunResult> TestRunResult { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestRunResult>()
                .Property(e => e.ApplicationName)
                .IsUnicode(false);

            modelBuilder.Entity<TestRunResult>()
                .Property(e => e.TestName)
                .IsUnicode(false);

            modelBuilder.Entity<TestRunResult>()
                .Property(e => e.TesterName)
                .IsUnicode(false);

            modelBuilder.Entity<TestRunResult>()
                .Property(e => e.Environment)
                .IsUnicode(false);

            modelBuilder.Entity<TestRunResult>()
                .Property(e => e.Browser)
                .IsUnicode(false);

            modelBuilder.Entity<TestRunResult>()
                .Property(e => e.TestRunStatus)
                .IsUnicode(false);
        }
    }
}
