using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas() 
        {
            _logger.LogInformation("Obtener las villas");
            IEnumerable<Villa> villaList = await _context.Villas.ToListAsync();
            var villaMapped = _mapper.Map<IEnumerable<VillaDto>>(villaList);
            return Ok(villaMapped);
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Error al traer villa con id: " + id);
                return BadRequest();
            }
            var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            var villaMapped = _mapper.Map<VillaDto>(villa);
            return Ok(villaMapped);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existeNombre = await _context.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower());
            if (existeNombre != null)
            {
                ModelState.AddModelError("NombreExiste", "La Villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }
            if (createDto == null)
            {
                return BadRequest(createDto);
            }
            var modelo = _mapper.Map<Villa>(createDto);
            await _context.Villas.AddAsync(modelo);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new {id = modelo.Id}, modelo);
        }
        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null) 
            {
                return NotFound();
            }
            _context.Villas.Remove(villa);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if(updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }
            var modelo = _mapper.Map<Villa>(updateDto);
            _context.Villas.Update(modelo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id <= 0)
            {
                return BadRequest();
            }
            var villa = await _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            if (villa == null) return BadRequest();
            patchDto.ApplyTo(villaDto, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa modelo = _mapper.Map<Villa>(villaDto);
            _context.Villas.Update(modelo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
