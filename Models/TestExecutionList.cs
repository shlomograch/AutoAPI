namespace AutomationAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TestExecutionList")]
    public partial class TestExecutionList
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ApplicationKey { get; set; }

        [StringLength(50)]
        public string ApplicationName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string TestName { get; set; }

        [StringLength(200)]
        public string TestDescription { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(12)]
        public string Status { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string TesterName { get; set; }

        [StringLength(50)]
        public string Browser { get; set; }

        [StringLength(20)]
        public string Environment { get; set; }

        public DateTime? LastRun { get; set; }

        [StringLength(20)]
        public string TestRunStatus { get; set; }

        public int? ManualTime { get; set; }
    }
}
