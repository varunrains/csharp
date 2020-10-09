using IplServerSide.Core.Services;
using IplServerSide.Dtos;
using IplServerSide.Enums;
using IplServerSide.Helpers;
using System.Web.Http;

namespace IplServerSide.Controllers
{
    [RoutePrefix("api/bets")]
    [Authorize(Roles = nameof(UserRoleEnum.User) + "," + nameof(UserRoleEnum.Admin) + "," + nameof(UserRoleEnum.Tester))]
    public class BetController : ApiController
    {
        private readonly BettingService _bettingService;

        public BetController(BettingService bettingService)
        {
            _bettingService = bettingService;
        }

        [Route("AddBet")]
        [HttpPost]
        public IHttpActionResult AddBet([FromBody] BetDto betDetails)
        {
             var userId = UserHelper.GetUserId(User.Identity);
            var matchId =_bettingService.AddUserBet(betDetails, userId);
            return Ok(matchId);
        }

        [Route("UpdateBet")]
        [HttpPut]
        public IHttpActionResult UpdateBet([FromBody] DisplayBetsDto betDetails)
        {
            var userId = UserHelper.GetUserId(User.Identity);
           var updatedBet = _bettingService.UpdateUserBet(betDetails, userId);
            return Ok(updatedBet);
        }

        [Route("GetBettingHistory")]
        public IHttpActionResult GetBettingHistory()
        {
            var userId = UserHelper.GetUserId(User.Identity);
            var bettingHistoryDetails = _bettingService.GetBettingHistory(userId);
            return Ok(bettingHistoryDetails);
        }

        [Route("GetCurrentBets")]
        public IHttpActionResult GetCurrentBets()
        {   
            var userId = UserHelper.GetUserId(User.Identity);
            var currentBets = _bettingService.GetCurrentBets(userId);
            return Ok(currentBets);
        }
    }
}
