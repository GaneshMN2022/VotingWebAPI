using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voting.DTO {
    public class Voter : BaseResultDto {
        public int VoterId { get; set; }
        public DateTime? VotedOn { get; set; }
        public char? HasVoted { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class VoterInfo {
        public int VoterId { get; set;}
        public string? VoterName { get; set; }
    }

    public class VoterInfoList : ResultDto {
        public List<VoterInfo> VoterInfos { get; set; } = new List<VoterInfo> { };
    }

    public class VoterListInfo : BaseDto {
        public int VoterId { get; set; }
        public string? VoterName { get; set; }
        public char HasVoted { get; set; }
    }

    public class VoterListInfoList : ResultDto {
        public List<VoterListInfo> VoterListInfos { get; set; } = new List<VoterListInfo> { };
    }
}
