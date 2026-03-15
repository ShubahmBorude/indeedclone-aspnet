using IndeedClone.Modules.Shared.Enums;

namespace IndeedClone.Modules.SubModules.JobPost.ModelContracts
{
    public interface IJobPostModel
    {
        string JobUid { get; set; }
        JobPostStatus Status { get; set; }
    }
}
