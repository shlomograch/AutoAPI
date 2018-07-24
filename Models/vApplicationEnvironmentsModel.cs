namespace AutomationAPI.Models
{
    using System.Data.Entity;

    public partial class vApplicationEnvironmentsModel : DbContext
    {
        public vApplicationEnvironmentsModel()
            : base("name=vApplicationEnvironmentsModel")
        {
        }

        public virtual DbSet<vApplicationEnvironment> vApplicationEnvironments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vApplicationEnvironment>()
                .Property(e => e.ApplicationName)
                .IsUnicode(false);

            modelBuilder.Entity<vApplicationEnvironment>()
                .Property(e => e.Environment)
                .IsUnicode(false);

            modelBuilder.Entity<vApplicationEnvironment>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<vApplicationEnvironment>()
                .Property(e => e.Active);
        }
    }
}
