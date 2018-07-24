namespace AutomationAPI.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class vApplicationEnvironment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ApplicationEnvironmentsKey { get; set; }

        [StringLength(50)]
        public string ApplicationName { get; set; }

        [StringLength(20)]
        public string Environment { get; set; }

        public string Url { get; set; }

        public bool Active { get; set; }
    }
}
