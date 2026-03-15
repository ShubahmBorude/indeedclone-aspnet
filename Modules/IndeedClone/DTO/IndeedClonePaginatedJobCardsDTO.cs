namespace IndeedClone.Modules.IndeedClone.DTO
{
    public class IndeedClonePaginatedJobCardsDTO
    {
        public IEnumerable<IndeedCloneLeftJobCardsDTO> Jobs { get; set; } = Enumerable.Empty<IndeedCloneLeftJobCardsDTO>();
        public IndeedClonePaginationDTO Pagination { get; set; } = new();
        public IndeedCloneJobSearchFilterDTO JobSearchFilter { get; set; }
    }
}
