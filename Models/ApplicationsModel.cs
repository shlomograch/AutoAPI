namespace AutomationAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ApplicationsModel : DbContext
    {
        public ApplicationsModel()
            : base("name=ApplicationsModel")
        {
        }

        public virtual DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>()
                .Property(e => e.ApplicationName)
                .IsUnicode(false);

            modelBuilder.Entity<Application>()
                .Property(e => e.ApplicationDescription)
                .IsUnicode(false);
        }
    }
}
