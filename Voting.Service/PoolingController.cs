using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Voting.DB;
using Voting.DTO;
using Voting.Logic;
using WebSocketManager = Voting.Logic.Socket.WebSocketManager;

namespace Voting.Controllers {
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAllHeaders")]
    public class PoolingController : ControllerBase {
        private readonly PoolingLogic _poolingLogic;
        public PoolingController(IVotingDBContext votingDBContext, WebSocketManager webSocketManager) 
        {
            _poolingLogic = new PoolingLogic(votingDBContext, webSocketManager);
        }

        [HttpPost]
        public async Task<ResultDto> SaveVote(Pooling voting) {
            return await _poolingLogic.SaveVote(voting);
        }
    }
}
