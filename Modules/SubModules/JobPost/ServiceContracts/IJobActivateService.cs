namespace IndeedClone.Modules.SubModules.JobPost.ServiceContracts
{
    public interface IJobActivateService
    {
        public Task ActivateJobPostAsync(string jobUid);
        public Task SoftDeleteDraftAsync(string jobUid, string refNo);

    }
}
