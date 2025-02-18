using AutoMapper;
using Kaar.Domain.Models;
using Api.Models.Responses;

namespace Api.Profiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            // Map domain models to API response models
            CreateMap<StockPriceDto, StockPriceResponse>()
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp));

            CreateMap<UserPreferenceDto, UserPreferenceResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ThresholdPrice, opt => opt.MapFrom(src => src.ThresholdPrice));
        }
    }
} 