using CityInfo.API.Models.Entities;

namespace CityInfo.API.Models.Services
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery);
        Task<City?> GetCityAsync(int cityId, bool pointOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);
        Task<PointOfInterest> GetPointOfInterestAsync(int? cityId, int? pointOfInterestId);
        Task<bool> CityExistsAsync(int cityId);
        Task AddPointOfInterestToCityAsync(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterestFromCity(PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
    }
}
