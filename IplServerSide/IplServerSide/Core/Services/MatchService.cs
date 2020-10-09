using AutoMapper;
using IplServerSide.Dtos;
using IplServerSide.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IplServerSide.Core.Services
{
    public class MatchService
    {
        private readonly BettingContext _bettingContext;
        private readonly IMapper _autoMapper;

        public MatchService(BettingContext bettingContext, IMapper autoMapper)
        {
            _bettingContext = bettingContext;
            _autoMapper = autoMapper;

        }

        public List<MatchDto> GetMatchDetails()
        {

            var currentTime = DateTimeOffset.UtcNow.AddMinutes(30);
            var matchDetails = _bettingContext.Matches
                .Where(x => x.MatchDateTime > currentTime)
                .OrderBy(x => x.MatchDateTime).Select(x => x).ToList();
            var teams = _bettingContext.Teams.ToDictionary(x => x.TeamId, y => y.TeamShortName);
            var matchDetailsDto = _autoMapper.Map<List<Match>, List<MatchDto>>(matchDetails);
            matchDetailsDto.ForEach(match =>
            {
                match.TeamAShortName = teams.First(x => x.Key == match.TeamIdA).Value;
                match.TeamBShortName = teams.First(x => x.Key == match.TeamIdB).Value;
            });

            return matchDetailsDto;
        }

        public List<MatchDto> GetMatchesToUpdateResult()
        {
            var currentDate = DateTimeOffset.UtcNow;
            var matchDetails = _bettingContext.Matches.Where(x => x.Result == null && x.MatchDateTime < currentDate).OrderBy(x => x.MatchDateTime).Select(x => x).ToList();
            var teams = _bettingContext.Teams.ToDictionary(x => x.TeamId, y => y.TeamShortName);
            var matchDetailsDto = _autoMapper.Map<List<Match>, List<MatchDto>>(matchDetails);
            matchDetailsDto.ForEach(match =>
            {
                match.TeamAShortName = teams.First(x => x.Key == match.TeamIdA).Value;
                match.TeamBShortName = teams.First(x => x.Key == match.TeamIdB).Value;
            });

            return matchDetailsDto;
        }
    }
}