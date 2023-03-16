using WebAPIProject.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;

namespace WebAPIProject.Data
{
    public class AppRepository : IAppRepository
    {
        private readonly DataContext _context; 

        public AppRepository(DataContext context)
        {
            _context = context;
        }

        public void add<T>(T entity) where T : class
        {
           _context.Add(entity);
        }

        public List<City> GetCities()
        {
            var cities = _context.Cities.Include(c=> c.Photos).ToList();
            return cities;
        }

        public City GetCityById(int cityId)
        {
            var cityById = _context.Cities.Include(c=>c.Photos).FirstOrDefault(c => c.Id == cityId);
            return cityById;
        }

        public Photo GetPhoto(int id)
        {
           var photo =  _context.Photos.FirstOrDefault(p => p.Id == id);
            return photo;
        }

        public List<Photo> GetPhotosByCity(int cityId)
        {
            var photos = _context.Photos.Where(p => p.CityId == cityId).ToList();
            return photos;
        }

        public void remove<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
