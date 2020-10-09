using IplServerSide.Core.Services;
using System.Web.Http;
using IplServerSide.Enums;

namespace IplServerSide.Controllers
{
    [RoutePrefix("api/matches")]
    [Authorize(Roles = nameof(UserRoleEnum.User) + "," + nameof(UserRoleEnum.Admin) + "," + nameof(UserRoleEnum.Tester))]
    public class MatchController : ApiController
    {
        private readonly MatchService _matchService;

        public MatchController( MatchService matchService)
        {
            _matchService = matchService;
        }

        [Route("GetMatchDetails")]
        public IHttpActionResult Get()
        {
            var matchDetails = _matchService.GetMatchDetails();
            return Ok(matchDetails);
        }
    }
}
