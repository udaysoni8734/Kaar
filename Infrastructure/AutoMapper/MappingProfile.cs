using Api.Models.Requests;
using Api.Models.Responses;
using AutoMapper;
using Kaar.Domain.Entities;

namespace Infrastructure.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StockPrice, StockPriceResponse>()
            .ForMember(
                dest => dest.Symbol,
                opt => opt.MapFrom(src => src.StockSymbol))
            .ForMember(
                dest => dest.PriceChange,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.PercentageChange,
                opt => opt.Ignore());

        CreateMap<UserPreference, UserPreferenceResponse>()
            .ForMember(
                dest => dest.Symbol,
                opt => opt.MapFrom(src => src.StockSymbol));

        //CreateMap<CreateUserPreferenceRequest, UserPreference>()
        //    .ForMember(
        //        dest => dest.StockSymbol,
        //        opt => opt.MapFrom(src => src.Symbol));
    }
}
