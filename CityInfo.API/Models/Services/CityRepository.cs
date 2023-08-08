using CityInfo.API.DbContexts;
using CityInfo.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Models.Services
{
    public class CityRepository : ICityRepository
    {
        private readonly CityInfoContext _context;

        public CityRepository(CityInfoContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(string? name, 
            string? searchQuery)
        {
            

            if (String.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetCitiesAsync();
            }

            var Collection = _context.Cities as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                Collection = Collection.Where(c => c.Name == name);
            }

            if(!String.IsNullOrWhiteSpace(searchQuery)) 
            {
                searchQuery = searchQuery.Trim();
                Collection = Collection.Where(c => c.Name.Contains(searchQuery)
                 || ((c.Description != null) && c.Description.Contains(searchQuery)));
                    
            }
            return Collection.OrderBy(c => c.Name);
        }

            public async Task<City?> GetCityAsync(int cityId, bool pointOfInterest)
        {
            if(pointOfInterest)
            {
                return await _context.Cities.Include(p => p.PointsOfInterest).
                    Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }
            return await _context.Cities.
                    Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<PointOfInterest> GetPointOfInterestAsync(int? cityId, int? pointOfInterestId)
        {
            if(cityId != 0 & pointOfInterestId != 0)
            {
                Console.WriteLine("abc");
            }
            return await _context.PointsOfInterest.
                Where(p => p.CityId == cityId && p.Id == pointOfInterestId).
                FirstOrDefaultAsync();
                }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId)
        {
            return await _context.PointsOfInterest.
                Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task AddPointOfInterestToCityAsync(int cityId, 
            PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if (city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        } 

        public void DeletePointOfInterestFromCity(PointOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
        }

        public async Task<bool> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync() >= 0;
        }
        
    }
}
