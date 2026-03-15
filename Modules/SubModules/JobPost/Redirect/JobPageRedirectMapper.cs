using IndeedClone.Modules.SubModules.JobPost.Enums;

namespace IndeedClone.Modules.SubModules.JobPost.Redirect
{
    public static class JobPageRedirectMapper
    {
        private static readonly Dictionary<JobPages, (string Controller, string Action)> PageRoutes =
           new()
           {
                { JobPages.JobOrganization, ("JobPost", "Retrieve") },
                { JobPages.JobBasics, ("JobPost", "JobBasics") },
                { JobPages.JobDetails, ("JobPost", "JobDeatils") },
                { JobPages.JobPayBenefits, ("JobPost", "PayBenefits") },
                { JobPages.JobDescription, ("JobPost", "JobDescription") },
                { JobPages.JobPreferences, ("JobPost", "JobPreferences") },
                { JobPages.Review, ("JobReview", "Review") },
                { JobPages.JobQualification, ("JobPost", "Qualifications") }
           };

     // # Returns the controller/action for the given page
        public static (string Controller, string Action) GetRoute(JobPages page)
        {
            if (PageRoutes.TryGetValue(page, out var route))
                return route;

            // Default fallback
            return ("JobPost", "Organization");
        }
    }
}
