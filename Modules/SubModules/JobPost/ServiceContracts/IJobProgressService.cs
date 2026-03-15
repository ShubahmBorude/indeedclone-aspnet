namespace IndeedClone.Modules.SubModules.JobPost.ServiceContracts
{
    public interface IJobProgressService
    {
        Task<int> GetLastSavedPageAsync(string jobUid, string refNo);

    }
}
