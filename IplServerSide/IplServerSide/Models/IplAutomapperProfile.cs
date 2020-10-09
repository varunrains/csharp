using AutoMapper;
using IplServerSide.Dtos;
using IplServerSide.Enums;

namespace IplServerSide.Models
{
    public class IplAutomapperProfile: Profile
    {
        public IplAutomapperProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest =>dest.IsAdmin, src => src.MapFrom(usr => usr.UserRole == UserRoleEnum.Admin))
                .ReverseMap();
            CreateMap<Match, MatchDto>().ReverseMap();
            //CreateMap<Bet, BetDto>().ForMember(dest => dest.MatchDate,
            //    src => src.MapFrom(x => x.Match.MatchDateTime));
            CreateMap<Bet, BetDto>();
            CreateMap<BetDto, DisplayBetsDto>();
        }
    }
}