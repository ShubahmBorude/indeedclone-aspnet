using IndeedClone.Modules.Shared.Enums;

namespace IndeedClone.Modules.JobDashboard.DTO
{
    public class ApplicantDTO
    {
        public int ApplicantId { get; set; }
        public string ApplicationUid { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Initials { get; set; } = string.Empty;
        public DateTime AppliedDate { get; set; }
        public decimal Experience { get; set; }
        public decimal CurrentCTC { get; set; }
        public decimal ExpectedCTC { get; set; }
        public JobApplicationStatus Status { get; set; }
    }
}
