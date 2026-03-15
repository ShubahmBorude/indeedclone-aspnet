using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.JobApplication.Models
{
    [Table("jobapplications_core")]
    public class JobApplicationModel
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("ref_no")]
        public string RefNo { get; set; }

        [Column("application_uid")]
        public string ApplicationUid { get; set; }

        [Column("job_uid")]
        public string JobUid { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("edited")]
        public DateTime Edited { get; set; }

        [Column("deleted")]
        public DateTime? Deleted { get; set; }

        [Column("current_page")]
        public JobApplicationPages? CurrentPage { get; set; } 

        [Column("status")]
        public JobApplicationStatus Status { get; set; } = JobApplicationStatus.DRAFT;

        [Column("field1")]
        public string? Field1 { get; set; }

        [Column("field2")]
        public string? Field2 { get; set; }

    }
}
