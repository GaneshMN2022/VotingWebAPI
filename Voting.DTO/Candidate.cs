
namespace Voting.DTO {
    public class Candidate : BaseResultDto {
        public int CandidateId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class CandidateInfo {
        public int CandidateId { get; set;}
        public string? CandidateName { get; set;}
    }

    public class CandidateInfoList : ResultDto {
        public List<CandidateInfo> CandidateInfos { get; set; } = new List<CandidateInfo> { };
    }

    public class CandidateListInfo : BaseDto {
        public int CandidateId { get; set; }
        public string? CandidateName { get; set; }
        public int? Votes { get; set; }
    }

    public class CandidateListInfoList : ResultDto {
        public List<CandidateListInfo> CandidateListInfos { get; set; } = new List<CandidateListInfo> { };
    }
}
