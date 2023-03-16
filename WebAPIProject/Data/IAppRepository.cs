using WebAPIProject.Models;

namespace WebAPIProject.Data
{
    public interface IAppRepository
    {
        
        void add<T>(T entity) where T : class;
        void remove<T>(T entity) where T:class;        
        bool SaveAll();

        List<City> GetCities();
        List<Photo> GetPhotosByCity(int cityId);
        City GetCityById(int cityId);
        Photo GetPhoto(int id);

    }
}
