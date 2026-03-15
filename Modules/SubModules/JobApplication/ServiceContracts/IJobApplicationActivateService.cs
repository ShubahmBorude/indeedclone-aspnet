namespace IndeedClone.Modules.SubModules.JobApplication.ServiceContracts
{
    public interface IJobApplicationActivateService
    {
        Task SubmitAsync(string applicationUid);

        public Task SoftDeleteDraftAsync(string jobUid, string refNo);

    }
}
