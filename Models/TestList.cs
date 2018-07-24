namespace AutomationAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TestList")]
    public partial class TestList
    {
        [Key]
        public int TestListKey { get; set; }

        [Required]
        [StringLength(50)]
        public string TestName { get; set; }

        [StringLength(200)]
        public string TestDescription { get; set; }

        public int ApplicationKey { get; set; }

        public bool? Active { get; set; }

        public int? TotalIterations { get; set; }

        public int? ManualTime { get; set; }

        public int? AverageAutomatedTime { get; set; }
    }
}
