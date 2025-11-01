using CarAPI.Services;
using CarAPI.DTOs;
using CarAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TrimLevelsController : EntityController<TrimLevel, EntityDto, CreateEntityDto>
    {
        public TrimLevelsController(EntityService<TrimLevel, EntityDto, CreateEntityDto> service) 
            : base(service)
        {
        }
    }
}