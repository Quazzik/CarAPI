using CarAPI.Services;
using CarAPI.DTOs;
using CarAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrimLevelsController : EntityController<TrimLevel, EntityDto, CreateEntityDto>
    {
        public TrimLevelsController(EntityService<TrimLevel, EntityDto, CreateEntityDto> service) 
            : base(service)
        {
        }
    }
}