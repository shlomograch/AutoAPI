using Newtonsoft.Json;

namespace AutomationAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class QueuedTestExtra
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationName { get; set; }

        [Required]
        public string TestName { get; set; }

        public string UserName { get; set; }

        public DateTime? QueuedDateTime { get; set; }

        public bool? StackTrace { get; set; }

        public bool? Utilization { get; set; }

        public bool? ConsoleLog { get; set; }

        public string Environment { get; set; }

        public bool RunAllApi { get; set; }

        public bool RunAllGui { get; set; }
    }
}
