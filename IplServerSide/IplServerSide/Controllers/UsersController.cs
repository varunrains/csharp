using IplServerSide.Core.Services;
using IplServerSide.Enums;
using IplServerSide.Helpers;
using System.Web.Http;

namespace IplServerSide.Controllers
{
    [RoutePrefix("api/users")]
    [Authorize(Roles = nameof(UserRoleEnum.User) + "," + nameof(UserRoleEnum.Admin) + "," + nameof(UserRoleEnum.Tester))]
    public class UsersController : ApiController
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [Route("GetUserDetails")]
        public IHttpActionResult Get()
        {
            var userId = UserHelper.GetUserId(User.Identity);
            var userObject = _userService.GetUserDetails(userId);
            return Ok(userObject);
        }

        [Route("ChangePassword")]
        [HttpPut]
        public IHttpActionResult ChangePassword([FromBody] dynamic userData)
        {
            var userId = UserHelper.GetUserId(User.Identity);
            _userService.ChangePassword(userData, userId);
            return Ok();
        }

        [Route("SaveUserSubscription")]
        [HttpPost]
        public IHttpActionResult SaveUserSubscription([FromBody] dynamic subscriptionData)
        {
            var userId = UserHelper.GetUserId(User.Identity);
            _userService.SaveUserSubscription(subscriptionData.subscriptionObject?.ToString(), userId);
            return Ok();
        }

        [Route("GetUserWinningPercentage")]
        public IHttpActionResult GetUserWinningPercentage()
        {
           var userDetails = _userService.GetUserWinningPercentage();
            return Ok(userDetails);
        }

    }
}
