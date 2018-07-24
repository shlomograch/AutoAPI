namespace AutomationAPI.Models.TestRun
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TestRunsTableModel : DbContext
    {
        public TestRunsTableModel()
            : base("name=TestRunsTableModel")
        {
        }

        public virtual DbSet<TestRunsTable> TestRunsTables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestRunsTable>()
                .Property(e => e.TestRunGuid)
                .IsUnicode(false);

            modelBuilder.Entity<TestRunsTable>()
                .Property(e => e.Environment)
                .IsUnicode(false);

            modelBuilder.Entity<TestRunsTable>()
                .Property(e => e.Browser)
                .IsUnicode(false);

            modelBuilder.Entity<TestRunsTable>()
                .Property(e => e.TestRunStatus)
                .IsUnicode(false);
        }
    }
}
