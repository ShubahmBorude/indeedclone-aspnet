namespace IndeedClone.Modules.IndeedClone.DTO
{
    public class IndeedCloneJobSearchFilterDTO
    {
        public string? Keyword { get; set; }  
        public string? Location { get; set; }  

        public string? WorkArrangement { get; set; } 
        public string? DatePosted { get; set; } 
        public decimal? Salary { get; set; } 

        public List<string>? JobTypes { get; set; }   
        public List<string>? EducationLevels { get; set; }  

        public string? Company { get; set; }
        public string? Language { get; set; }

        public int Page { get; set; } = 1;
    }
}
