using Microsoft.EntityFrameworkCore;
using Voting.Common;
using Voting.DB;
using Voting.DB.Entities;
using Voting.DTO;

namespace Voting.Logic {
    public class VoterLogic {

        private readonly IVotingDBContext _dbContext;
        public VoterLogic(IVotingDBContext votingDBContext) { 
            _dbContext = votingDBContext;
        }

        public async Task<Voter> GetVoter(int voterId) {
            try {
                var result = await _dbContext.Voters.SingleAsync(x => x.VoterId == voterId);
                if (ExceptionHelper.IsEmptyOrNull(result)) return new Voter();
                return MapEntityToDto(result);
            } catch (Exception ex) {
                throw ExceptionHelper.ProcessException(ex);
            }
        }

        public async Task<VoterInfoList> GetVoterNames() {
            try {
                var result = await _dbContext.Voters.Where(x => x.HasVoted != 'v').ToListAsync();
                if (ExceptionHelper.IsEmptyOrNull(result)) return new VoterInfoList();
                return MapEntityToDto(result);
            } catch (Exception ex) { 
                throw ExceptionHelper.ProcessException(ex);
            }
        }

        public async Task<VoterListInfoList> GetVoters(int pageNumber, int pageSize) {
            try {
                int itemsToSkip = (pageNumber - 1) * pageSize;
                var result = await _dbContext.Voters.OrderBy(v => v.VoterId).Skip(itemsToSkip).Take(pageSize).ToListAsync();
                if (ExceptionHelper.IsEmptyOrNull(result)) return new VoterListInfoList();
                return MapEntityToDtoList(result);
            } catch (Exception ex) {
                throw ExceptionHelper.ProcessException(ex);
            }
        }

        public async Task<Voter> AddVoter(string voterName) {
            try {

                //Get voter entity object
                var voter = GetVoterEntity(voterName);

                //Validate for any existing voter with same name.
                var doesVoterExist =
                    await _dbContext.Voters.AnyAsync(x => x.FirstName == voter.FirstName && x.LastName == voter.LastName);

                //Stop execution and report to user if voter with same name is being added. 
                if (doesVoterExist) return new Voter { Result = new Result { Success = false, Message = string.Format(Constants.NameExistsMessage, Constants.Voter) } };

                //Add the new voter
                var result = await _dbContext.Voters.AddAsync(voter);
                await _dbContext.SaveChangesAsync();
                var voterDto = MapEntityToDto(voter);
                voterDto.Result.Message = string.Format(Constants.UserAddedSuccessfullyMessage, Constants.Voter);
                return voterDto;
            } catch (Exception ex) {
                throw ExceptionHelper.ProcessException(ex);
            }
        }

        public async Task UpdateVoteStatus(int voterId) {
            try {
                var voter = await _dbContext.Voters.SingleAsync(x => x.VoterId == voterId);
                voter.HasVoted = 'v';
                await _dbContext.SaveChangesAsync();
            } catch (Exception ex) {
                throw ExceptionHelper.ProcessException(ex);
            }
        }

        private VoterInfoList MapEntityToDto(List<VotersEntity> entities) {
            var list = new VoterInfoList();
            foreach (var entity in entities) {
                list.VoterInfos.Add(new VoterInfo {
                    VoterId = entity.VoterId,
                    VoterName = HelperUtility.GetCombinedName(entity.FirstName, entity.LastName),
                });
            }

            return list;
        }

        private VoterListInfoList MapEntityToDtoList(List<VotersEntity> entities) {
            var list = new VoterListInfoList();
            foreach (var entity in entities) {
                list.VoterListInfos.Add(new VoterListInfo {
                    VoterId = entity.VoterId,
                    VoterName = HelperUtility.GetCombinedName(entity.FirstName, entity.LastName),
                    HasVoted = entity.HasVoted,
                    RowVersion = entity.Row_Version
                });
            }

            return list;
        }

        private Voter MapEntityToDto(VotersEntity entity) {
            return new Voter {
                VoterId = entity.VoterId,
                VotedOn = entity.VotedOn,
                HasVoted = entity.HasVoted,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
            };
        }

        

        private VotersEntity GetVoterEntity(string voterName) {
            var index = voterName.Trim().IndexOf(" ");
            var firstNameEndIndex = index > 0 ? index : voterName.Length;
            var firstName = voterName[..firstNameEndIndex].Trim();
            var lastName = firstNameEndIndex < voterName.Length ? voterName.Substring(firstNameEndIndex).Trim() : null;
            return new VotersEntity { FirstName = firstName, LastName = lastName };
        }
    }
}
