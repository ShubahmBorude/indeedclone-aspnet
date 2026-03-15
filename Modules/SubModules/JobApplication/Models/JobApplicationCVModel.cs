using IndeedClone.Modules.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.JobApplication.Models
{
    [Table("jobapplication_cv")]
    public class JobApplicationCVModel
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("application_uid")]
        public string ApplicationUid { get; set; }

        [Column("file_name")]
        public string FileName { get; set; }

        [Column("file_path")]
        public string FilePath { get; set; }

        [Column("file_size")]
        public int FileSize { get; set; }

        [Column("status")]
        public JobApplicationStatus Status { get; set; } = JobApplicationStatus.DRAFT;

        [Column("field1")]
        public string? Field1 { get; set; }

        [Column("field2")]
        public string? Field2 { get; set; }

    }
}
