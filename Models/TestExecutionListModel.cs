namespace AutomationAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TestExecutionListModel : DbContext
    {
        public TestExecutionListModel()
            : base("name=TestExecutionListModel")
        {
        }

        public virtual DbSet<TestExecutionList> TestExecutionLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestExecutionList>()
                .Property(e => e.ApplicationName)
                .IsUnicode(false);

            modelBuilder.Entity<TestExecutionList>()
                .Property(e => e.TestName)
                .IsUnicode(false);

            modelBuilder.Entity<TestExecutionList>()
                .Property(e => e.TestDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TestExecutionList>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<TestExecutionList>()
                .Property(e => e.TesterName)
                .IsUnicode(false);

            modelBuilder.Entity<TestExecutionList>()
                .Property(e => e.Browser)
                .IsUnicode(false);

            modelBuilder.Entity<TestExecutionList>()
                .Property(e => e.Environment)
                .IsUnicode(false);

            modelBuilder.Entity<TestExecutionList>()
                .Property(e => e.TestRunStatus)
                .IsUnicode(false);
        }
    }
}
