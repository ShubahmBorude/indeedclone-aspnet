using IndeedClone.Modules.IndeedClone.DTO;

namespace IndeedClone.Modules.IndeedClone.Models
{
    public class IndeedCloneHomeViewModel
    {
        public IEnumerable<IndeedCloneLeftJobCardsDTO> LeftJobCards { get; set; } = Enumerable.Empty<IndeedCloneLeftJobCardsDTO>();
        public IndeedCloneJobDescriptionDTO? RightJobDetails { get; set; }
        public IndeedCloneJobSearchFilterDTO? SearchFilter { get; set; }
        public IndeedClonePaginationDTO Pagination { get; set; } = new();
    }
}
