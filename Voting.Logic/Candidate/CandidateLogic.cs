using Microsoft.EntityFrameworkCore;
using Voting.Common;
using Voting.DB;
using Voting.DB.Entities;
using Voting.DTO;

namespace Voting.Logic {
    public class CandidateLogic {

        private readonly IVotingDBContext _dbContext;
        public CandidateLogic(IVotingDBContext votingDBContext) 
        {
            _dbContext = votingDBContext; 
        }

        public async Task<Candidate> GetCandidate(int candidateId) {
            try {
                var result = await _dbContext.Candidate.SingleAsync(x => x.CandidateId == candidateId);
                if (ExceptionHelper.IsEmptyOrNull(result)) return new Candidate();
                return MapEntityToDto(result);
            } catch (Exception ex) {
                throw ExceptionHelper.ProcessException(ex);
            }
        }

        public async Task<CandidateInfoList> GetCandidateNames() {
            try {
                var result = await _dbContext.Candidate.ToListAsync();
                if (ExceptionHelper.IsEmptyOrNull(result)) return new CandidateInfoList();
                return MapEntityToDto(result);
            } catch (Exception ex) {
                throw ExceptionHelper.ProcessException(ex);
            }
        }

        public async Task<CandidateListInfoList> GetCandidates(int pageNumber, int pageSize) {
            try {
                int itemsToSkip = (pageNumber - 1) * pageSize;
                var result = await _dbContext.Candidate.OrderBy(c => c.CandidateId).Skip(itemsToSkip).Take(pageSize).ToListAsync();
                if (ExceptionHelper.IsEmptyOrNull(result)) return new CandidateListInfoList();
                return MapEntityToDtoList(result);
            } catch (Exception ex) {
                throw ExceptionHelper.ProcessException(ex);
            }
        }

        public async Task<Candidate> AddCandidate(string voterName) {
            try {

                //Get candidate entity object
                var candidate = GetVoterEntity(voterName);

                //Validate for any existing candidate with same name.
                var doesCandidateExist =
                    await _dbContext.Candidate.AnyAsync(x => x.FirstName == candidate.FirstName && x.LastName == candidate.LastName);

                //Stop execution and report to user if candidate with same name is being added. 
                if (doesCandidateExist) return new Candidate { Result = new Result { Success = false, Message = string.Format(Constants.NameExistsMessage, Constants.Candidate) } };
                
                //Add the new candidate
                var result = await _dbContext.Candidate.AddAsync(candidate);
                await _dbContext.SaveChangesAsync();
                if (ExceptionHelper.IsEmptyOrNull(result)) return new Candidate();
                var candidateDto = MapEntityToDto(candidate);
                candidateDto.Result.Message = string.Format(Constants.UserAddedSuccessfullyMessage, Constants.Candidate);
                return candidateDto;
            } catch (Exception ex) {
                throw ExceptionHelper.ProcessException(ex);
            }
        }

        public async Task UpdateVoteCount(int candidateId) {
            try {
                var candidate = await _dbContext.Candidate.SingleAsync(x => x.CandidateId == candidateId);
                candidate.Votes = candidate.Votes == null ? 1 : (((int)candidate.Votes) + 1);
                await _dbContext.SaveChangesAsync();
            } catch (Exception ex) {
                throw ExceptionHelper.ProcessException(ex);
            }
        }

        private CandidateInfoList MapEntityToDto(List<CandidateEntity> entities) {
            var list = new CandidateInfoList();
            foreach (var entity in entities) {
                list.CandidateInfos.Add(new CandidateInfo {
                    CandidateId = entity.CandidateId,
                    CandidateName = HelperUtility.GetCombinedName(entity.FirstName, entity.LastName),
                });
            }

            return list;
        }

        private CandidateListInfoList MapEntityToDtoList(List<CandidateEntity> entities) {
            var list = new CandidateListInfoList();
            foreach (var entity in entities) {
                list.CandidateListInfos.Add(new CandidateListInfo {
                    CandidateId = entity.CandidateId,
                    CandidateName = HelperUtility.GetCombinedName(entity.FirstName, entity.LastName),
                    Votes = entity.Votes,
                    RowVersion = entity.Row_Version
                });
            }

            return list;
        }

        private Candidate MapEntityToDto(CandidateEntity entity) {
            return new Candidate {
                CandidateId = entity.CandidateId,
                CreatedOn = entity.CreatedOn,
                UpdatedOn = entity.ModifiedOn,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy,
                IsDeleted = entity.IsDeleted,
                RowVersion = entity.Row_Version,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
            };
        }

        private CandidateEntity GetVoterEntity(string candidateName) {
            var index = candidateName.Trim().IndexOf(" ");
            var firstNameEndIndex = index > 0 ? index : candidateName.Length;
            var firstName = candidateName[..firstNameEndIndex].Trim();
            var lastName = firstNameEndIndex < candidateName.Length ? candidateName.Substring(firstNameEndIndex).Trim() : null;
            return new CandidateEntity { FirstName = firstName, LastName = lastName };
        }
    }
}
