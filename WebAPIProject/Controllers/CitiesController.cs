using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Data;
using WebAPIProject.Dtos;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IAppRepository _appRepository;
        private readonly IMapper _mapper;

        public CitiesController(IAppRepository appRepository,IMapper mapper)
        {
            _appRepository = appRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = _appRepository.GetCities();
            var returnToCities = _mapper.Map<List<CityForListDto>>(cities);
            return Ok(returnToCities);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(City city)
        {
            _appRepository.add(city);
            _appRepository.SaveAll();
            return Ok(city);
        }
        [HttpGet]
        [Route("detail")]
        public IActionResult GetCityDetail(int id)
        {
            var city = _appRepository.GetCityById(id);
            var returnToCity = _mapper.Map<CityForDetailDto>(city);
            return Ok(returnToCity);
        }

        [HttpGet]
        [Route("photos")]

        public IActionResult GetPhotosByCityId(int cityId)
        {
            var photos = _appRepository.GetPhotosByCity(cityId);
            return Ok(photos);
        }
    }
}
