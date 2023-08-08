using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile() 
        { 
            CreateMap<Models.Entities.City, Models.CityWithoutPointOfInterestDto>();
            CreateMap<Models.Entities.City, Models.CityDto>();
        }
    }
}
