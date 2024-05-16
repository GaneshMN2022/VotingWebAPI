using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Voting.Common;
using Voting.DB;
using Voting.DTO;
using Voting.Logic;

namespace Voting.Controllers {

    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAllHeaders")]
    public class CandidateController : ControllerBase {
        private readonly CandidateLogic _candidateLogic;

        public CandidateController(IVotingDBContext votingDBContext) {
            _candidateLogic = new CandidateLogic(votingDBContext);
        }

        [HttpGet("{candidateId}")]
        public async Task<Candidate> GetCandidate(int candidateId) {
            if (candidateId <= 0) return new Candidate { Result = new Result { Success = false, Message = string.Format(Constants.InvalidIdMessage, nameof(candidateId)) } };
            return await _candidateLogic.GetCandidate(candidateId);
        }

        [HttpGet()]
        public async Task<CandidateInfoList> GetCandidateNames() {
            return await _candidateLogic.GetCandidateNames();
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<CandidateListInfoList> GetCandidates(int pageNumber, int pageSize) {
            if (pageNumber <= 0) return new CandidateListInfoList { Result = new Result { Success = false, Message = string.Format(Constants.InvalidIdMessage, nameof(pageNumber)) } };
            if (pageSize <= 0) return new CandidateListInfoList { Result = new Result { Success = false, Message = string.Format(Constants.InvalidIdMessage, nameof(pageSize)) } };
            return await _candidateLogic.GetCandidates(pageNumber, pageSize);
        }

        [HttpPost()]
        public async Task<Candidate> AddCandidate(string voterName) {
            if (string.IsNullOrWhiteSpace(voterName)) return new Candidate { Result = new Result { Success = false, Message = string.Format(Constants.InvalidIdMessage, nameof(voterName)) } };
            return await _candidateLogic.AddCandidate(voterName);
        }
    }
}
