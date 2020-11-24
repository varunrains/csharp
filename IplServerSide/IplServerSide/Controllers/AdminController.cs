using IplServerSide.Core.Services;
using IplServerSide.Dtos;
using IplServerSide.Enums;
using System.Web.Http;

namespace IplServerSide.Controllers
{
    [RoutePrefix("api/admin")]
    [Authorize(Roles = nameof(UserRoleEnum.Admin))]
    public class AdminController : ApiController
    {
        private readonly AdminService _adminService;
        private readonly MatchService _matchService;
        private readonly UserService _userService;

        public AdminController(AdminService adminService, MatchService matchService, UserService userService)
        {
            _adminService = adminService;
            _matchService = matchService;
            _userService = userService;
        }

        [Route("UpdateResult")]
        [HttpPut]
        public IHttpActionResult UpdateResult([FromBody] MatchDto matchDetails)
        {
            _adminService.UpdateResult(matchDetails);
            return Ok();
        }

        [Route("GetMatchDetails")]
        [HttpGet]
        public IHttpActionResult GetMatchDetails()
        {
            var matchDetails =_matchService.GetMatchesToUpdateResult();
            return Ok(matchDetails);
        }

        [Route("AddUser")]
        [HttpPost]
        public IHttpActionResult AddUser([FromBody] UserDto userData)
        {
            _userService.AddUser(userData);
            return Ok();
        }


        [Route("RemoveAllBets")]
        [HttpGet]
        public IHttpActionResult RemoveAllBets()
        {
             _adminService.RemoveAllBets();
            return Ok();
        }

        [Route("SendNotification")]
        [HttpPost]
        public IHttpActionResult SendNotification([FromBody] NotificationChild notificationData)
        {
            _adminService.SendNotifications(notificationData);
            return Ok();
        }
    }
}
