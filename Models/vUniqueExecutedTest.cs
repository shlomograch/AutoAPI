namespace AutomationAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vUniqueExecutedTest
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Environment { get; set; }

        public int? TestRunKey { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string TestName { get; set; }

        [StringLength(50)]
        public string ApplicationName { get; set; }

        [StringLength(50)]
        public string TesterName { get; set; }

        [StringLength(50)]
        public string Browser { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        [StringLength(20)]
        public string TestRunStatus { get; set; }
    }
}
