using IndeedClone.Modules.Shared.Enums;

namespace IndeedClone.Modules.JobDashboard.DTO
{
    public class ApplicationActivityLogDTO
    {
        public string ApplicationUid { get; set; }
        public JobApplicationStatus OldStatus { get; set; }
        public JobApplicationStatus NewStatus { get; set; }
        public string ActionBy { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
