using CarAPI.Services;
using CarAPI.DTOs;
using CarAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarBrandsController : EntityController<CarBrand, EntityDto, CreateEntityDto>
    {
        public CarBrandsController(EntityService<CarBrand, EntityDto, CreateEntityDto> service) 
            : base(service)
        {
        }
    }
}