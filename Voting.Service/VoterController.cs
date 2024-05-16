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
    public class VoterController : ControllerBase {
        private readonly VoterLogic _voterLogic;
        public VoterController(IVotingDBContext votingDBContext) { 
            _voterLogic = new VoterLogic(votingDBContext);
        }

        [HttpGet("{voterId}")]
        public async Task<Voter> GetVoter(int voterId) {
            if (voterId <= 0) return new Voter { Result = new Result { Success = false, Message = string.Format(Constants.InvalidIdMessage, nameof(voterId)) } };
            return await _voterLogic.GetVoter(voterId);
        }

        [HttpGet()]
        public async Task<VoterInfoList> GetVoterNames() {
            return await _voterLogic.GetVoterNames();
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<VoterListInfoList> GetVoters(int pageNumber, int pageSize) {
            if (pageNumber <= 0) return new VoterListInfoList { Result = new Result { Success = false, Message = string.Format(Constants.InvalidIdMessage, nameof(pageNumber)) } };
            if (pageSize <= 0) return new VoterListInfoList { Result = new Result { Success = false, Message = string.Format(Constants.InvalidIdMessage, nameof(pageSize)) } };
            return await _voterLogic.GetVoters(pageNumber, pageSize);
        }

        [HttpPost()]
        public async Task<Voter> AddVoter(string voterName) {
            if (string.IsNullOrWhiteSpace(voterName)) return new Voter { Result = new Result { Success = false, Message = string.Format(Constants.InvalidIdMessage, nameof(voterName)) } };
            return await _voterLogic.AddVoter(voterName);
        }
    }
}
