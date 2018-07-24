namespace AutomationAPI.Models.TestRun
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TestResultsTable")]
    public partial class TestResultsTable
    {
        [Key]
        public int TestResultKey { get; set; }

        public int TestRunKey { get; set; }

        public string ValidationKey { get; set; }

        public string ValidationValue { get; set; }

        public string ResultStatus { get; set; }

        public string ExtraData { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
