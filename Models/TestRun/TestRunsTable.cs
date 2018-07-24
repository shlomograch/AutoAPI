namespace AutomationAPI.Models.TestRun
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TestRunsTable")]
    public partial class TestRunsTable
    {
        [Key]
        public int TestRunKey { get; set; }

        public string TestRunGuid { get; set; }

        public int TestKey { get; set; }

        public int TesterKey { get; set; }

        public int ApplicationKey { get; set; }

        [Required]
        public string Environment { get; set; }

        public string Browser { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public string TestRunStatus { get; set; }
    }
}
