namespace AutomationAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vUserPreference")]
    public partial class vUserPreference
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string TesterName { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TesterKey { get; set; }

        [StringLength(50)]
        public string ApplicationName { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ApplicationKey { get; set; }

        [StringLength(50)]
        public string EnvironmentDescription { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(20)]
        public string Environment { get; set; }

        [StringLength(50)]
        public string BrowserName { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BrowserId { get; set; }

        public bool? StackTrace { get; set; }

        public bool? Utilization { get; set; }

        public bool RemoteDriver { get; set; }

        public bool? BrowserConsoleLog { get; set; }

        public DateTime? OccurrenceDate { get; set; }

        public string EnvironmentUrl { get; set; }

        public bool? WriteToDatabase { get; set; }

    }
}
