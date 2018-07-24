namespace AutomationAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TestListModel : DbContext
    {
        public TestListModel()
            : base("name=TestListModel")
        {
        }

        public virtual DbSet<TestList> TestLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestList>()
                .Property(e => e.TestName)
                .IsUnicode(false);

            modelBuilder.Entity<TestList>()
                .Property(e => e.TestDescription)
                .IsUnicode(false);
        }
    }
}
