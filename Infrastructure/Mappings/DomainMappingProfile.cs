using AutoMapper;
using Kaar.Domain.Entities;
using Kaar.Domain.Models;
using Api.Models.Requests;
using Api.Models.Responses;

namespace Infrastructure.Mappings
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            CreateMap<StockPrice, StockPriceDto>()
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.StockSymbol))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp));

            CreateMap<StockPriceDto, StockPrice>()
                .ForMember(dest => dest.StockSymbol, opt => opt.MapFrom(src => src.Symbol));

            CreateMap<StockPriceUpdateDto, StockPrice>()
                .ForMember(dest => dest.StockSymbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.NewPrice));

            CreateMap<StockPriceUpdateRequest, StockPriceUpdateDto>();
            CreateMap<StockPriceDto, StockPriceResponse>();
            
            CreateMap<UserPreference, UserPreferenceDto>();
            CreateMap<UserPreferenceDto, UserPreference>();
            CreateMap<UserPreferenceDto, UserPreferenceResponse>();
            CreateMap<CreateUserPreferenceRequest, CreateUserPreferenceDto>();
            CreateMap<UpdateUserPreferenceRequest, UpdateUserPreferenceDto>();
        }
    }
} 