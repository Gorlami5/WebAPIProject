using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Security.Principal;
using WebAPIProject.Data;
using WebAPIProject.Dtos;
using WebAPIProject.Helpers;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers
{
    [Route("api/cities/{cityId}[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IAppRepository _appRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private Cloudinary _cloudinary;

        public PhotosController(IAuthRepository authRepository, IMapper mapper, IOptions<CloudinarySettings> cloudinarySettings,IAppRepository appRepository)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _cloudinarySettings = cloudinarySettings;
            _appRepository = appRepository;

            Account account = new Account(
                _cloudinarySettings.Value.CloudName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(account);
        }
        public IActionResult AddPhotoForCity(int cityId ,[FromBody] PhotoForCreationDto photoForCreationDto)
        {
            var city = _appRepository.GetCityById(cityId);
            if(city == null)
            {
                return BadRequest();
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if(currentUserId != city.UserId)
            {
                return Unauthorized();
            }

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }

            }
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);
            photo.City = city;

            if(!city.Photos.Any(p=> p.IsMain))
            {
                photo.IsMain = true;
            }

            city.Photos.Add(photo);

            if (_appRepository.SaveAll())
            {
                var photoForReturnDto = _mapper.Map<PhotoForReturnDto>(photo);
                return Ok(photoForReturnDto);
            }
            return BadRequest("Failed");
        }

        [HttpGet]

        public IActionResult GetPhoto(int id)
        {
            var photofromDb = _appRepository.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photofromDb);

            return Ok(photo);
        }


       
        

    }
}
