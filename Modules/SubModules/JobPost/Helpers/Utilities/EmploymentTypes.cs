namespace IndeedClone.Modules.SubModules.JobPost.Helpers.Utilities
{
    public static class EmploymentTypes
    {
        public static string[] EmploymentType { get; } =
        {
            "Full Time", "Permanent", "Fresher", "Internship", "Contractual", "Part Time",
            "Temporary", "Freelance", "Volunteer", "Student Job", "Seasonal", "Other"
        };

        public static int[] NumberOfPeopleToHire { get; } =
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
            21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
            41, 42, 43, 44, 45, 46, 47, 48, 49, 50
        };

        public static string[] EmployeeSupplementedBenefits { get; } =
        {
            "Performance Bonus", "Yearly Bonus", "Commission Pay", "Overtime Pay",
            "Quarterly Bonus", "Shift Allowance", "Joining Bonus","Overtime Hours",
            "Health Insurance" 
        };

        public static string[] Benefits { get; } =
        {
            "Health Insurance", "Provident Fund", "Phone Reimbursement", "Paid Sick Leave", 
            "Paid Time Off", "Food Provided",  "Internet Reimbursement", "Computer Assistance",
            "Leave Encashment", "Life Insurance", "Flexible Schedule", "Other"
        };

    }
}
