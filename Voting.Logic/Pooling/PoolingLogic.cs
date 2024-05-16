using System;
using Voting.Common;
using Voting.DB;
using Voting.DTO;

namespace Voting.Logic {
    public class PoolingLogic {
        private readonly IVotingDBContext _dbContext;
        private readonly VoterLogic _voterLogic;
        private readonly CandidateLogic _candidateLogic;
        public PoolingLogic(IVotingDBContext dbContext) { 
            _dbContext = dbContext;
            _candidateLogic = new CandidateLogic(dbContext);
            _voterLogic = new VoterLogic(dbContext);
        }

        public async Task<ResultDto> SaveVote(Pooling pooling) {
            try {

                var voterRecord = await _voterLogic.GetVoter(pooling.VoterId);
                if (voterRecord.VoterId <= 0) return new ResultDto { Result = new Result { Success = false, Message = string.Format(Constants.UserNotFoundMessage, Constants.Voter) } };
                if (voterRecord.HasVoted == 'v') return new ResultDto { Result = new Result { Success = false, Message = Constants.PoolingNotValidMessage } };

                var candidateRecord = await _candidateLogic.GetCandidate(pooling.CandidateId);
                if (candidateRecord.CandidateId <= 0) return new ResultDto { Result = new Result { Success = false, Message = string.Format(Constants.UserNotFoundMessage, Constants.Candidate) } };

                using (var transaction = _dbContext.BeginTransaction()) {
                    await _voterLogic.UpdateVoteStatus(voterRecord.VoterId);
                    await _candidateLogic.UpdateVoteCount(pooling.CandidateId);
                }

                await _dbContext.CommitTransaction();
                return new ResultDto { Result = new Result { Message = Constants.PoolingUpdatedSuccessMessage } };
            } catch (Exception ex) {
                throw ExceptionHelper.ProcessException(ex);
            }
        }
    }
}
