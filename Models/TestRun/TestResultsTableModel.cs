namespace AutomationAPI.Models.TestRun
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TestResultsTableModel : DbContext
    {
        public TestResultsTableModel()
            : base("name=TestResultsTableModel")
        {
        }

        public virtual DbSet<TestResultsTable> TestResultsTables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestResultsTable>()
                .Property(e => e.ValidationKey)
                .IsUnicode(false);

            modelBuilder.Entity<TestResultsTable>()
                .Property(e => e.ValidationValue)
                .IsUnicode(false);

            modelBuilder.Entity<TestResultsTable>()
                .Property(e => e.ResultStatus)
                .IsUnicode(false);

            modelBuilder.Entity<TestResultsTable>()
                .Property(e => e.ExtraData)
                .IsUnicode(false);
        }
    }
}
