using Microsoft.AspNetCore.Mvc;
using CarAPI.DTOs;
using CarAPI.Models;
using CarAPI.Services;

namespace CarAPI.Controllers
{
    [ApiController]
    public class EntityController<TEntity, TDto, TCreateDto> : ControllerBase
        where TEntity : Entity
    {
        protected readonly EntityService<TEntity, TDto, TCreateDto> _service;

        public EntityController(EntityService<TEntity, TDto, TCreateDto> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
        {
            var entities = await _service.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<ActionResult<TDto>> Get(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateDto createDto)
        {
            try
            {
                var createdEntity = await _service.CreateAsync(createDto);
                return CreatedAtAction(nameof(Get), new { id = GetEntityId(createdEntity) }, createdEntity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<ActionResult<TDto>> Update(int id, [FromBody] TCreateDto updateDto)
        {
            try
            {
                var updatedEntity = await _service.UpdateAsync(id, updateDto);
                if (updatedEntity == null)
                {
                    return NotFound();
                }
                return Ok(updatedEntity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        private static int GetEntityId(TDto entity)
        {
            var property = typeof(TDto).GetProperty("Id");
            return (int)(property?.GetValue(entity) ?? 0);
        }
    }
}