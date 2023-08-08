using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile() 
        {
            CreateMap<Models.Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestForCreationDto, Models.Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestForUpdateDto, Models.Entities.PointOfInterest>();
            CreateMap<Models.Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
        }

    }
}
