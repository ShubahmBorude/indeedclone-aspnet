using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.ModelContracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.JobPost.Models
{
    [Table("job_description")]
    public class JobDescriptionModel : IJobPostModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("job_uid")]
        public string JobUid { get; set; } 

        [Column("description")]
        public string Description { get; set; } 

     // # Future use
        public string? Field1 { get; set; }

     // # Future use
        public string? Field2 { get; set; }

        [Column("current_page")]
        public int? CurrentPage { get; set; }

        [Column("status")]
        public JobPostStatus Status { get; set; } = JobPostStatus.DRAFT;

    }
}
