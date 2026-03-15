using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.ModelContracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.JobPost.Models
{
    [Table("job_basics")]
    public class JobBasicsModel : IJobPostModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("job_uid")]
        public string JobUid { get; set; }

     // # Textarea
        [Column("company_description")]
        public string CompanyDescription { get; set; }

        [Column("job_title")]
        public string JobTitle { get; set; }

        [Column("job_location")]
        public string JobLocation { get; set; }

        [Column("work_arrangement")]
        public string WorkArrangement { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("area")]
        public string Area { get; set; }

        [Column("street_address")]
        public string StreetAddress { get; set; }

     // # Future use
        [Column("field1")]
        public string? Field1 { get; set; } = string.Empty;

        // # Future use
        [Column("field2")]
        public string? Field2 { get; set; } = string.Empty;

        [Column("current_page")]
        public int? CurrentPage { get; set; }

        [Column("status")]
        public JobPostStatus Status { get; set; } = JobPostStatus.DRAFT;
    }
}
