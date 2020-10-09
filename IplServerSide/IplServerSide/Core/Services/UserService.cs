using System;
using AutoMapper;
using IplServerSide.Dtos;
using IplServerSide.Enums;
using IplServerSide.Models;
using System.Linq;
using IplServerSide.Core.Repositories;

namespace IplServerSide.Core.Services
{
    public class UserService
    {
        private readonly BettingContext _bettingContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _autoMapper;

        public UserService(BettingContext bettingContext, IMapper autoMapper, IUnitOfWork unitOfWork)
        {
            _bettingContext = bettingContext;
            _autoMapper = autoMapper;
            _unitOfWork = unitOfWork;
        }

        public UserDto GetUserDetails(int userId)
        {
            var userBets = _bettingContext.Bets
                .Where(bet => bet.UserId == userId && bet.WinningTeamId == null).Select(x => x.MatchId)
                .ToList();
            var userDetails = _bettingContext.Users.FirstOrDefault(user => user.UserId == userId);
            var userDetailsDto = _autoMapper.Map<User, UserDto>(userDetails);
            userDetailsDto.UsersFutureBets = userBets;

            return userDetailsDto;
        }

        public void AddUser(UserDto user)
        {
            var userData = new User()
            {
                DisplayName = user.DisplayName,
                UserGroup = user.UserGroup,
                PassKey = user.SecretKey,
                UserAmount = user.UserAmount,
                UserRole = (UserRoleEnum) user.UserRole,
                UserName = user.UserName
            };

            _bettingContext.Users.Add(userData);
            _bettingContext.SaveChanges();
        }

        public void ChangePassword(dynamic userData, int userId)
        {
            var currentUser = _unitOfWork.Users.Get(userId);
            if (currentUser.PassKey != userData.CurrentPassword.ToString())
                throw new Exception("Current password is not matching !!");

            currentUser.PassKey = userData.NewPassword;
            _unitOfWork.Complete();
        }
    }
}