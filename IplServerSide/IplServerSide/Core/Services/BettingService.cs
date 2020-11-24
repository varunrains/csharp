using AutoMapper;
using IplServerSide.Core.Repositories;
using IplServerSide.Dtos;
using IplServerSide.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IplServerSide.Core.Services
{
    public class BettingService
    {
        private readonly BettingContext _bettingContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BettingService(BettingContext bettingContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _bettingContext = bettingContext;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BetDto AddUserBet(BetDto betDetails, int userId)
        {
            var userDetail = _unitOfWork.Users.Get(userId);
            var displayBetDto = _mapper.Map<BetDto, DisplayBetsDto>(betDetails);
            ValidateTheBet(displayBetDto, userId, userDetail.UserAmount);
            var bet = new Bet()
            {
                BettingTeamId = betDetails.BettingTeamIdOrTeamAId,
                BetAmount = betDetails.BetAmount,
                UserId = userId,
                BettingDate = DateTimeOffset.UtcNow,
                MatchId = betDetails.MatchId
            };
            _unitOfWork.Bets.Add(bet);
            userDetail.UserAmount -= betDetails.BetAmount;

            _unitOfWork.Complete();

            return betDetails;
        }

        public BetDto UpdateUserBet(DisplayBetsDto betDetails, int userId)
        {
            var userDetail = _unitOfWork.Users.Get(userId);
            ValidateTheBet(betDetails, userId, userDetail.UserAmount);
            var bet = _unitOfWork.Bets.Get(betDetails.BetId);
            userDetail.UserAmount = (userDetail.UserAmount + bet.BetAmount) - betDetails.BetAmount;
            bet.BettingTeamId = betDetails.BettingTeamIdOrTeamAId;
            bet.BetAmount = betDetails.BetAmount;
            bet.BettingDate = DateTimeOffset.UtcNow;
            if (betDetails.IsBetDeleted)
            {
                _unitOfWork.Bets.Remove(bet);
            }

            _unitOfWork.Complete();

            return new BetDto()
            {
                BettingTeamIdOrTeamAId = betDetails.BettingTeamIdOrTeamAId,
                BetAmount = betDetails.BetAmount,
                MatchId = betDetails.MatchId,
                BetId = betDetails.BetId
            };
        }

        public List<DisplayBetsDto> GetBettingHistory(int userId)
        {
            var bets = from bet in _bettingContext.Bets
                join match in _bettingContext.Matches on bet.MatchId equals match.MatchId
                join teamA in _bettingContext.Teams on match.TeamIdA equals teamA.TeamId
                join teamB in _bettingContext.Teams on match.TeamIdB equals teamB.TeamId
                       where bet.WinningTeamId != null && bet.NetAmountWon != null && bet.UserId == userId
                select new DisplayBetsDto()
                {
                    BetAmount = bet.BetAmount,
                    BettingTeamNameOrTeamA = teamA.TeamShortName,
                    WinningTeamNameOrTeamB = teamB.TeamShortName,
                    TeamAId = teamA.TeamId,
                    TeamBId = teamB.TeamId,
                    BettingTeamIdOrTeamAId = bet.BettingTeamId,
                    WinningTeamIdOrTeamBId = bet.WinningTeamId.Value,
                    MatchDate = match.MatchDateTime,
                    NetAmountWon = bet.NetAmountWon,
                    MatchId = bet.MatchId

                };
          return bets.OrderByDescending(x => x.MatchDate).ToList();
        }

        public List<DisplayBetsDto> GetCurrentBets(int userId)
        {
            var bets = from bet in _bettingContext.Bets
                join match in _bettingContext.Matches on bet.MatchId equals match.MatchId
                join teamA in _bettingContext.Teams on match.TeamIdA equals teamA.TeamId
                join teamB in _bettingContext.Teams on match.TeamIdB equals teamB.TeamId
                where bet.WinningTeamId == null && bet.NetAmountWon == null && bet.UserId == userId
                select new DisplayBetsDto()
                {
                    BetAmount = bet.BetAmount,
                    BettingTeamNameOrTeamA = teamA.TeamShortName,
                    WinningTeamNameOrTeamB = teamB.TeamShortName,
                    TeamAId = teamA.TeamId,
                    TeamBId = teamB.TeamId,
                    BettingTeamIdOrTeamAId = bet.BettingTeamId,
                    MatchDate = match.MatchDateTime,
                    BetId = bet.BetId,
                    MatchId = bet.MatchId
                };
            return bets.OrderBy(x => x.MatchDate).ToList();
        }

        public List<DisplayBetsDto> GetOtherUsersBets(int matchId)
        {
            var bets = from bet in _bettingContext.Bets
                       join user in _bettingContext.Users on bet.UserId equals user.UserId
                       join teamA in _bettingContext.Teams on bet.BettingTeamId equals teamA.TeamId
                       where bet.MatchId == matchId && bet.WinningTeamId == null && bet.NetAmountWon == null 
                       orderby bet.BettingDate descending
                       select new DisplayBetsDto()
                       {
                           BetAmount = bet.BetAmount,
                           BettingTeamNameOrTeamA = teamA.TeamShortName,
                           UserName = user.DisplayName
                       };
            return bets.ToList();
        }

        public List<DisplayBetsDto> GetAmountOwnByUsers(int matchId)
        {
            var bets = from bet in _bettingContext.Bets
                       join user in _bettingContext.Users on bet.UserId equals user.UserId
                       join teamA in _bettingContext.Teams on bet.BettingTeamId equals teamA.TeamId
                       where bet.MatchId == matchId && bet.WinningTeamId != null && bet.NetAmountWon != null
                       orderby bet.NetAmountWon descending
                       select new DisplayBetsDto()
                       {
                           NetAmountWon = bet.NetAmountWon,
                           BettingTeamNameOrTeamA = teamA.TeamShortName,
                           UserName = user.DisplayName
                       };
            return bets.ToList();
        }

        private void ValidateTheBet(DisplayBetsDto betDetails, int userId, decimal walletAmount)
        {
            var matchDate = _unitOfWork.Matches.Get(betDetails.MatchId).MatchDateTime;

            if (walletAmount < betDetails.BetAmount)
            {
                throw new Exception("You do not have sufficient funds to Bet!!");
            }

            if (matchDate < DateTimeOffset.UtcNow.AddMinutes(30))
            {
                throw new Exception("You are not allowed to bet for this match. Time elapsed!");
            }
        }

    }
}