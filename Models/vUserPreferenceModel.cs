namespace AutomationAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class vUserPreferenceModel : DbContext
    {
        public vUserPreferenceModel()
            : base("name=vUserPreferenceModel")
        {
        }

        public virtual DbSet<vUserPreference> vUserPreferences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vUserPreference>()
                .Property(e => e.TesterName)
                .IsUnicode(false);

            modelBuilder.Entity<vUserPreference>()
                .Property(e => e.ApplicationName)
                .IsUnicode(false);

            modelBuilder.Entity<vUserPreference>()
                .Property(e => e.EnvironmentDescription)
                .IsUnicode(false);

            modelBuilder.Entity<vUserPreference>()
                .Property(e => e.Environment)
                .IsUnicode(false);

            modelBuilder.Entity<vUserPreference>()
                .Property(e => e.BrowserName)
                .IsUnicode(false);

            modelBuilder.Entity<vUserPreference>()
                .Property(e => e.EnvironmentUrl)
                .IsUnicode(false);
        }
    }
}
