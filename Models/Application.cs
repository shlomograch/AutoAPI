namespace AutomationAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Application
    {
        [Key]
        public int ApplicationKey { get; set; }

        [StringLength(50)]
        public string ApplicationName { get; set; }

        public string ApplicationDescription { get; set; }

        public bool? Active { get; set; }
    }
}
